﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MedicalExaminer.API.Authorization;
using MedicalExaminer.API.Filters;
using MedicalExaminer.API.Models.v1.Permissions;
using MedicalExaminer.API.Services;
using MedicalExaminer.Common.Extensions.Models;
using MedicalExaminer.Common.Loggers;
using MedicalExaminer.Common.Queries.Location;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;

namespace MedicalExaminer.API.Controllers
{
    /// <inheritdoc />
    /// <summary>
    ///     Permissions Controller
    /// </summary>
    [ApiVersion("1.0")]
    [Route("/v{api-version:apiVersion}/users/{meUserId}/permissions")]
    [ApiController]
    [Authorize]
    public class PermissionsController : AuthorizedBaseController
    {
        /// <summary>
        /// User Retrieval By Id Service.
        /// </summary>
        private readonly IAsyncQueryHandler<UserRetrievalByIdQuery, MeUser> _userRetrievalByIdService;

        /// <summary>
        /// User Update Service.
        /// </summary>
        private readonly IAsyncQueryHandler<UserUpdateQuery, MeUser> _userUpdateService;

        /// <summary>
        /// Locations Parents Service.
        /// </summary>
        private readonly
            IAsyncQueryHandler<LocationsParentsQuery, IDictionary<string, IEnumerable<Location>>>
            _locationsParentsService;

        /// <summary>
        /// Location Parents Service.
        /// </summary>
        private readonly IAsyncQueryHandler<LocationParentsQuery, IEnumerable<Location>> _locationParentsService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="PermissionsController" /> class.
        /// </summary>
        /// <param name="logger">The Logger.</param>
        /// <param name="mapper">The Mapper.</param>
        /// <param name="usersRetrievalByOktaIdService">User Retrieval By Okta Id Service.</param>
        /// <param name="authorizationService">Authorization Service.</param>
        /// <param name="permissionService">Permission Service.</param>
        /// <param name="userRetrievalByIdService">User retrieval by id service.</param>
        /// <param name="userUpdateService">User update service.</param>
        /// <param name="locationParentsService">Location Parents Service.</param>
        /// <param name="locationsParentsService">Locations Parents Service.</param>
        public PermissionsController(
            IMELogger logger,
            IMapper mapper,
            IAsyncQueryHandler<UserRetrievalByOktaIdQuery, MeUser> usersRetrievalByOktaIdService,
            IAuthorizationService authorizationService,
            IPermissionService permissionService,
            IAsyncQueryHandler<UserRetrievalByIdQuery, MeUser> userRetrievalByIdService,
            IAsyncQueryHandler<UserUpdateQuery, MeUser> userUpdateService,
            IAsyncQueryHandler<LocationParentsQuery, IEnumerable<Location>> locationParentsService,
            IAsyncQueryHandler<LocationsParentsQuery, IDictionary<string, IEnumerable<Location>>> locationsParentsService)
            : base(logger, mapper, usersRetrievalByOktaIdService, authorizationService, permissionService)
        {
            _userRetrievalByIdService = userRetrievalByIdService;
            _userUpdateService = userUpdateService;
            _locationParentsService = locationParentsService;
            _locationsParentsService = locationsParentsService;
        }

        /// <summary>
        ///     Get all Permissions for a User ID.
        /// </summary>
        /// <param name="meUserId">The User Identifier.</param>
        /// <returns>A GetPermissionsResponse.</returns>
        [HttpGet]
        [AuthorizePermission(Common.Authorization.Permission.GetUserPermissions)]
        public async Task<ActionResult<GetPermissionsResponse>> GetPermissions(string meUserId)
        {
            try
            {
                // Get all the permission location ids on the user in the request.
                var meUser = await _userRetrievalByIdService.Handle(new UserRetrievalByIdQuery(meUserId));

                if (meUser == null)
                {
                    return NotFound(new GetPermissionsResponse());
                }

                var permissionLocations = meUser.Permissions.Select(p => p.LocationId).ToList();

                // Get all the location paths for all those locations.
                var locationPaths =
                    await _locationsParentsService.Handle(new LocationsParentsQuery(permissionLocations));

                // The locations the user making the request has direct access to.
                var permissedLocations = (await LocationsWithPermission(Common.Authorization.Permission.GetUserPermissions)).ToList();

                // Select only the permissions that the user making the request has access to from the user in question.
                var permissions = meUser
                    .Permissions
                    .Where(p => locationPaths[p.LocationId]
                        .Any(l => permissedLocations.Contains(l.LocationId)));

                var mappedPermissions = permissions.Select(p => Mapper.Map<PermissionItem>(p)).ToArray();

                foreach(var mp in mappedPermissions)
                {
                    mp.UserId = meUserId;
                }

                return Ok(new GetPermissionsResponse
                {
                    Permissions = mappedPermissions
                });
            }
            catch (DocumentClientException)
            {
                return NotFound(new GetPermissionsResponse());
            }
            catch (ArgumentException)
            {
                return NotFound(new GetPermissionsResponse());
            }
        }

        /// <summary>
        ///     Get a Users permission by its Identifier.
        /// </summary>
        /// <param name="meUserId">The User Id.</param>
        /// <param name="permissionId">The Permission Id.</param>
        /// <returns>A GetPermissionResponse.</returns>
        [HttpGet("{permissionId}")]
        [AuthorizePermission(Common.Authorization.Permission.GetUserPermission)]
        public async Task<ActionResult<GetPermissionResponse>> GetPermission(string meUserId, string permissionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new GetPermissionResponse());
            }

            try
            {
                var user = await _userRetrievalByIdService.Handle(new UserRetrievalByIdQuery(meUserId));

                if (user == null)
                {
                    return NotFound(new GetPermissionResponse());
                }

                var permission = user.Permissions.FirstOrDefault(p => p.PermissionId == permissionId);

                if (permission == null)
                {
                    return NotFound(new GetPermissionResponse());
                }

                var locationDocument = (await
                        _locationParentsService.Handle(
                            new LocationParentsQuery(permission.LocationId)))
                    .ToLocationPath();

                if (!CanAsync(Common.Authorization.Permission.GetUserPermission, locationDocument))
                {
                    return Forbid();
                }

                var result = Mapper.Map<GetPermissionResponse>(permission);
                result.UserId = meUserId;
                return Ok(result);
            }
            catch (ArgumentException)
            {
                return NotFound(new GetPermissionResponse());
            }
            catch (NullReferenceException)
            {
                return NotFound(new GetPermissionResponse());
            }
        }

        /// <summary>
        ///     Create a new Permission.
        /// </summary>
        /// <param name="postPermission">The PostPermissionRequest.</param>
        /// <returns>A PostPermissionResponse.</returns>
        [HttpPost]
        [AuthorizePermission(Common.Authorization.Permission.CreateUserPermission)]
        public async Task<ActionResult<PostPermissionResponse>> CreatePermission(string meUserId,
            [FromBody]
            PostPermissionRequest postPermission)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new PostPermissionResponse());
            }

            try
            {
                var permission = Mapper.Map<MEUserPermission>(postPermission);
                permission.PermissionId = Guid.NewGuid().ToString();

                var locationDocument = (await
                        _locationParentsService.Handle(
                            new LocationParentsQuery(permission.LocationId)))
                    .ToLocationPath();

                if (!CanAsync(Common.Authorization.Permission.CreateUserPermission, locationDocument))
                {
                    return Forbid();
                }

                var currentUser = await CurrentUser();

                var user = await _userRetrievalByIdService.Handle(new UserRetrievalByIdQuery(meUserId));

                if (user == null)
                {
                    return NotFound(new PostPermissionResponse());
                }

                var existingPermissions = user.Permissions != null ? user.Permissions.ToList() : new List<MEUserPermission>();

                var possiblePermission = existingPermissions.SingleOrDefault(ep => ep.LocationId == postPermission.LocationId
                && ep.UserRole == postPermission.UserRole);

                foreach (var usersPermissions in user.Permissions)
                {
                    if (usersPermissions.IsSame(permission))
                    {
                        return Conflict();
                    }
                }

                PostPermissionResponse result = null;

                if (possiblePermission == null)
                {
                    existingPermissions.Add(permission);

                    user.Permissions = existingPermissions;

                    await _userUpdateService.Handle(new UserUpdateQuery(user, currentUser));
                    result = Mapper.Map<MEUserPermission, PostPermissionResponse>(
                    permission,
                    opts => opts.AfterMap((src, dest) => { dest.UserId = user.UserId; }));
                }
                else
                {
                    result = Mapper.Map<MEUserPermission, PostPermissionResponse>(
                    possiblePermission,
                    opts => opts.AfterMap((src, dest) => { dest.UserId = user.UserId; }));
                }

                return Ok(result);
            }
            catch (DocumentClientException)
            {
                return NotFound(new PostPermissionResponse());
            }
            catch (ArgumentException)
            {
                return NotFound(new PostPermissionResponse());
            }
        }

        /// <summary>
        /// Updates a Permission.
        /// </summary>
        /// <param name="meUserId"> ME User Id </param>
        /// <param name="permissionId"> permission Id </param>
        /// <param name="putPermission">The PutPermissionRequest.</param>
        /// <returns>A PutPermissionResponse.</returns>
        [HttpPut("{permissionId}")]
        [AuthorizePermission(Common.Authorization.Permission.UpdateUserPermission)]
        public async Task<ActionResult<PutPermissionResponse>> UpdatePermission(
            string meUserId,
            string permissionId,
            [FromBody]
            PutPermissionRequest putPermission)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new PutPermissionResponse());
                }

                var currentUser = await CurrentUser();

                var locationDocument = (await
                        _locationParentsService.Handle(
                            new LocationParentsQuery(putPermission.LocationId)))
                    .ToLocationPath();

                if (!CanAsync(Common.Authorization.Permission.UpdateUserPermission, locationDocument))
                {
                    return Forbid();
                }

                var user = await _userRetrievalByIdService.Handle(new UserRetrievalByIdQuery(meUserId));

                if (user == null)
                {
                    return NotFound(new PutPermissionResponse());
                }

                //var permission = Mapper.Map(putPermission, permissionToUpdate);

                var permissionToUpdate = user.Permissions.SingleOrDefault(p => p.PermissionId == permissionId);

                if (permissionToUpdate == null)
                {
                    return NotFound(new PutPermissionResponse());
                }

                permissionToUpdate = Mapper.Map(putPermission, permissionToUpdate);

                foreach (var usersPermissions in user.Permissions)
                {
                    if (usersPermissions.IsSame(permissionToUpdate))  // should be permission???
                    {
                        return Conflict();
                    }
                }

                await _userUpdateService.Handle(new UserUpdateQuery(user, currentUser));

                return Ok(Mapper.Map<MEUserPermission, PutPermissionResponse>(
                    permissionToUpdate,
                    opts => opts.AfterMap((src, dest) => { dest.UserId = user.UserId; })));
            }
            catch (DocumentClientException)
            {
                return NotFound(new PutPermissionResponse());
            }
            catch (ArgumentException)
            {
                return NotFound(new PutPermissionResponse());
            }
        }

        /// <summary>
        /// Deletes a Permission.
        /// </summary>
        /// <param name="meUserId"> ME User Id </param>
        /// <param name="permissionId"> permission Id </param>
        /// <returns>A Response.</returns>
        [HttpDelete("{permissionId}")]
        [AuthorizePermission(Common.Authorization.Permission.DeleteUserPermission)]
        public async Task<ActionResult> DeletePermission(
            string meUserId,
            string permissionId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                var currentUser = await CurrentUser();

                var user = await _userRetrievalByIdService.Handle(new UserRetrievalByIdQuery(meUserId));

                if (user == null)
                {
                    return NotFound();
                }

                var permissionToDelete = user.Permissions.FirstOrDefault(p => p.PermissionId == permissionId);

                if (permissionToDelete == null)
                {
                    return NotFound();
                }

                var temp = user.Permissions.ToList();
                temp.Remove(permissionToDelete);

                user.Permissions = temp;

                await _userUpdateService.Handle(new UserUpdateQuery(user, currentUser));
                return Ok();
            }
            catch (DocumentClientException)
            {
                return NotFound();
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
        }
    }
}