﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MedicalExaminer.API.Authorization;
using MedicalExaminer.API.Models;
using MedicalExaminer.API.Models.v1.Permissions;
using MedicalExaminer.API.Services;
using MedicalExaminer.Common.Extensions.Models;
using MedicalExaminer.Common.Extensions.Permission;
using MedicalExaminer.Common.Queries.Location;
using MedicalExaminer.Common.Queries.MELogger;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Permission = MedicalExaminer.Common.Authorization.Permission;

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

        private readonly IAsyncQueryHandler<LocationRetrievalByIdQuery, Location> _locationRetrievalService;

        private readonly IAsyncQueryHandler<LocationsRetrievalByIdQuery, IEnumerable<Location>> _locationsRetrievalByIdService;

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
        /// <param name="locationRetrievalService">Location Retrieval Service.</param>
        /// <param name="locationsRetrievalByIdService">Locations Retrieval By Id Service.</param>
        public PermissionsController(
            IAsyncQueryHandler<CreateMELoggerQuery, LogMessageActionDefault> logger,
            IMapper mapper,
            IAsyncQueryHandler<UserRetrievalByOktaIdQuery, MeUser> usersRetrievalByOktaIdService,
            IAuthorizationService authorizationService,
            IPermissionService permissionService,
            IAsyncQueryHandler<UserRetrievalByIdQuery, MeUser> userRetrievalByIdService,
            IAsyncQueryHandler<UserUpdateQuery, MeUser> userUpdateService,
            IAsyncQueryHandler<LocationParentsQuery, IEnumerable<Location>> locationParentsService,
            IAsyncQueryHandler<LocationsParentsQuery, IDictionary<string, IEnumerable<Location>>> locationsParentsService,
            IAsyncQueryHandler<LocationRetrievalByIdQuery, Location> locationRetrievalService,
            IAsyncQueryHandler<LocationsRetrievalByIdQuery, IEnumerable<Location>> locationsRetrievalByIdService)
            : base(logger, mapper, usersRetrievalByOktaIdService, authorizationService, permissionService)
        {
            _userRetrievalByIdService = userRetrievalByIdService;
            _userUpdateService = userUpdateService;
            _locationParentsService = locationParentsService;
            _locationsParentsService = locationsParentsService;
            _locationRetrievalService = locationRetrievalService;
            _locationsRetrievalByIdService = locationsRetrievalByIdService;
        }

        /// <summary>
        ///     Get all Permissions for a User ID.
        /// </summary>
        /// <param name="meUserId">The User Identifier.</param>
        /// <returns>A GetPermissionsResponse.</returns>
        [HttpGet]
        [AuthorizePermission(Permission.GetUserPermissions)]
        public async Task<ActionResult<GetPermissionsResponse>> GetPermissions(string meUserId)
        {
            try
            {
                var user = await _userRetrievalByIdService.Handle(new UserRetrievalByIdQuery(meUserId));
                if (user == null)
                {
                    return NotFound(new GetPermissionsResponse());
                }

                var usersPermissionLocationsIds = user.Permissions == null
                    ? new List<string>()
                    : user.Permissions
                        .Where(p => p.LocationId != null)
                        .Select(p => p.LocationId)
                        .ToList();

                // Get all the location paths for all those locations.
                var locationPaths =
                    await _locationsParentsService.Handle(new LocationsParentsQuery(usersPermissionLocationsIds));

                // The locations the user making the request has direct access to.
                var permissedLocations = (await LocationsWithPermission(Permission.GetUserPermissions)).ToList();

                // Select only the permissions that the user making the request has access to from the user in question.
                var permissions = user
                    .Permissions?.Where(p => p.LocationId != null)
                    .Where(p => locationPaths[p.LocationId]
                        .Any(l => permissedLocations.Contains(l.LocationId)))
                    .ToList() ?? new List<MEUserPermission>();

                // To avoid getting names for locations we cant see; get a list of just the ones we're going to return.
                var locationIdsToGetNamesFor = permissions
                    .Select(p => p.LocationId)
                    .ToList();

                // Get the lookup location model for the ones we want names for.
                var locations = (await
                    _locationsRetrievalByIdService.Handle(new LocationsRetrievalByIdQuery(
                        true,
                        locationIdsToGetNamesFor))).ToList();

                // Now create the permission list incorporating the location name.
                var permissionItems = permissions
                    .Select(permission => Mapper.Map<PermissionItem>(new PermissionLocation(
                        permission,
                        locations.SingleOrDefault(l => l.LocationId == permission.LocationId),
                        meUserId)))
                    .ToList();

                return Ok(new GetPermissionsResponse
                {
                    Permissions = permissionItems
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
        [AuthorizePermission(Permission.GetUserPermission)]
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

                if (!CanAsync(Permission.GetUserPermission, locationDocument))
                {
                    return Forbid();
                }

                var location = await permission.GetLocationName(_locationsRetrievalByIdService);

                var permissionLocations = new PermissionLocation(permission, location, meUserId);

                var result = Mapper.Map<GetPermissionResponse>(permissionLocations);
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
        /// <param name="meUserId">ME User ID</param>
        /// <param name="postPermission">The PostPermissionRequest.</param>
        /// <returns>A PostPermissionResponse.</returns>
        [HttpPost]
        [AuthorizePermission(Permission.CreateUserPermission)]
        public async Task<ActionResult<PostPermissionResponse>> CreatePermission(
            [Required] string meUserId,
            [FromBody]
            PostPermissionRequest postPermission)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new PostPermissionResponse());
            }

            var currentUser = await CurrentUser();

            if (currentUser.UserId.Equals(meUserId))
            {
                return Forbid();
            }

            try
            {
                var permission = Mapper.Map<MEUserPermission>(postPermission);
                permission.PermissionId = Guid.NewGuid().ToString();

                var locationDocument = (await
                        _locationParentsService.Handle(
                            new LocationParentsQuery(permission.LocationId)))
                    .ToLocationPath();

                if (!CanAsync(Permission.CreateUserPermission, locationDocument))
                {
                    return Forbid();
                }

                var user = await _userRetrievalByIdService.Handle(new UserRetrievalByIdQuery(meUserId));

                if (user == null)
                {
                    return NotFound(new PostPermissionResponse());
                }

                var existingPermissions = user.Permissions != null ? user.Permissions.ToList() : new List<MEUserPermission>();

                if (user.Permissions == null)
                {
                    user.Permissions = new List<MEUserPermission>();
                }

                if (user.Permissions.Any(usersPermissions => usersPermissions.IsEquivalent(permission)))
                {
                    return Conflict();
                }

                existingPermissions.Add(permission);

                user.Permissions = existingPermissions;

                var updateUser = new UserUpdatePermissions
                {
                    UserId = user.UserId,
                    Permissions = user.Permissions,
                };

                await _userUpdateService.Handle(new UserUpdateQuery(updateUser, currentUser));

                var location = _locationRetrievalService.Handle(new LocationRetrievalByIdQuery(permission.LocationId)).Result;

                var permissionLocation = new PermissionLocation(permission, location, meUserId);

                var result = Mapper.Map<PostPermissionResponse>(permissionLocation);
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
        [AuthorizePermission(Permission.UpdateUserPermission)]
        public async Task<ActionResult<PutPermissionResponse>> UpdatePermission(
            [Required] string meUserId,
            [Required] string permissionId,
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

                if (currentUser.UserId.Equals(meUserId))
                {
                    return Forbid();
                }

                var locationDocument = (await
                        _locationParentsService.Handle(
                            new LocationParentsQuery(putPermission.LocationId)))
                    .ToLocationPath();

                if (!CanAsync(Permission.UpdateUserPermission, locationDocument))
                {
                    return Forbid();
                }

                var user = await _userRetrievalByIdService.Handle(new UserRetrievalByIdQuery(meUserId));

                if (user == null)
                {
                    return NotFound(new PutPermissionResponse());
                }

                var permissionToUpdate = user.Permissions.SingleOrDefault(p => p.PermissionId == permissionId);

                if (permissionToUpdate == null)
                {
                    return NotFound(new PutPermissionResponse());
                }

                if (user.Permissions.Any(usersPermission => usersPermission.LocationId == putPermission.LocationId && usersPermission.UserRole == putPermission.UserRole))
                {
                    return Conflict();
                }

                permissionToUpdate = Mapper.Map(putPermission, permissionToUpdate);

                var updateUser = new UserUpdatePermissions
                {
                    UserId = user.UserId,
                    Permissions = user.Permissions,
                };

                await _userUpdateService.Handle(new UserUpdateQuery(updateUser, currentUser));

                var locationOfPermission =
                    _locationRetrievalService.Handle(new LocationRetrievalByIdQuery(permissionToUpdate.LocationId)).Result;

                var permissionLocation = new PermissionLocation(permissionToUpdate, locationOfPermission, meUserId);

                var result = Mapper.Map<PutPermissionResponse>(permissionLocation);

                return Ok(result);
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
        [AuthorizePermission(Permission.DeleteUserPermission)]
        public async Task<ActionResult> DeletePermission(
            [Required] string meUserId,
            [Required] string permissionId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                var currentUser = await CurrentUser();

                if (currentUser.UserId.Equals(meUserId))
                {
                    return Forbid();
                }

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

                var locationDocument = (await
                        _locationParentsService.Handle(
                            new LocationParentsQuery(permissionToDelete.LocationId)))
                    .ToLocationPath();

                if (!CanAsync(Permission.DeleteUserPermission, locationDocument))
                {
                    return Forbid();
                }

                var temp = user.Permissions.ToList();
                temp.Remove(permissionToDelete);

                var userUpdate = new UserUpdatePermissions()
                {
                    UserId = user.UserId,
                    Permissions = temp,
                };

                user.Permissions = temp;
                await _userUpdateService.Handle(new UserUpdateQuery(userUpdate, currentUser));
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