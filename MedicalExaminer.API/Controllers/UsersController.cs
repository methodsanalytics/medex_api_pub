using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MedicalExaminer.API.Authorization;
using MedicalExaminer.API.Models.v1.Users;
using MedicalExaminer.API.Services;
using MedicalExaminer.Common.Loggers;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Permission = MedicalExaminer.Common.Authorization.Permission;

namespace MedicalExaminer.API.Controllers
{
    /// <summary>
    ///     Users Controller
    /// </summary>
    /// <inheritdoc />
    [ApiVersion("1.0")]
    [Route("/v{api-version:apiVersion}/users")]
    [ApiController]
    [Authorize]
    public class UsersController : AuthorizedBaseController
    {
        /// <summary>
        ///     The User Persistence Layer
        /// </summary>
        private readonly IAsyncQueryHandler<CreateUserQuery, MeUser> _userCreationService;

        private readonly IAsyncQueryHandler<UserRetrievalByIdQuery, MeUser> _userRetrievalByIdService;
        private readonly IAsyncQueryHandler<UsersRetrievalQuery, IEnumerable<MeUser>> _usersRetrievalService;
        private readonly IAsyncQueryHandler<UserUpdateQuery, MeUser> _userUpdateService;
        private readonly IAsyncQueryHandler<UserSuspendQuery, MeUser> _userSuspendService;
        private readonly IAsyncQueryHandler<UserDeleteQuery, MeUser> _userDeleteService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="logger">The Logger.</param>
        /// <param name="mapper">The Mapper.</param>
        /// <param name="usersRetrievalByOktaIdService">User Retrieval By Okta Id Service.</param>
        /// <param name="authorizationService">Authorization Service.</param>
        /// <param name="permissionService">Permission Service.</param>
        /// <param name="userCreationService">User creation service.</param>
        /// <param name="userRetrievalByIdService">User retrieval service.</param>
        /// <param name="usersRetrievalService">Users retrieval service.</param>
        /// <param name="userUpdateService">The userToCreate update service</param>
        /// <param name="userSuspendService">The user suspend service.</param>
        /// <param name="userDeleteService">The user delete service.</param>
        public UsersController(
            IMELogger logger,
            IMapper mapper,
            IAsyncQueryHandler<UserRetrievalByOktaIdQuery, MeUser> usersRetrievalByOktaIdService,
            IAuthorizationService authorizationService,
            IPermissionService permissionService,
            IAsyncQueryHandler<CreateUserQuery, MeUser> userCreationService,
            IAsyncQueryHandler<UserRetrievalByIdQuery, MeUser> userRetrievalByIdService,
            IAsyncQueryHandler<UsersRetrievalQuery, IEnumerable<MeUser>> usersRetrievalService,
            IAsyncQueryHandler<UserUpdateQuery, MeUser> userUpdateService,
            IAsyncQueryHandler<UserSuspendQuery, MeUser> userSuspendService,
            IAsyncQueryHandler<UserDeleteQuery, MeUser> userDeleteService)
            : base(logger, mapper, usersRetrievalByOktaIdService, authorizationService, permissionService)
        {
            _userCreationService = userCreationService;
            _userRetrievalByIdService = userRetrievalByIdService;
            _usersRetrievalService = usersRetrievalService;
            _userUpdateService = userUpdateService;
            _userSuspendService = userSuspendService;
            _userDeleteService = userDeleteService;
        }

        /// <summary>
        /// Get all Users.
        /// </summary>
        /// <returns>A GetUsersResponse.</returns>
        [HttpGet]
        [AuthorizePermission(Permission.GetUsers)]
        public async Task<ActionResult<GetUsersResponse>> GetUsers()
        {
            try
            {
                var users = await _usersRetrievalService.Handle(new UsersRetrievalQuery(null));
                return Ok(new GetUsersResponse
                {
                    Users = users.Select(u => Mapper.Map<UserItem>(u)),
                });
            }
            catch (DocumentClientException)
            {
                return NotFound(new GetUsersResponse());
            }
            catch (ArgumentException)
            {
                return NotFound(new GetUsersResponse());
            }
        }

        /// <summary>
        ///     Get a User by its Identifier.
        /// </summary>
        /// <param name="meUserId">The User Identifier.</param>
        /// <returns>A GetUserResponse.</returns>
        [HttpGet("{meUserId}")]
        [AuthorizePermission(Permission.GetUser)]
        public async Task<ActionResult<GetUserResponse>> GetUser(string meUserId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new GetUserResponse());
            }

            try
            {
                var user = await _userRetrievalByIdService.Handle(new UserRetrievalByIdQuery(meUserId));
                return Ok(Mapper.Map<GetUserResponse>(user));
            }
            catch (ArgumentException)
            {
                return NotFound(new GetUserResponse());
            }
            catch (NullReferenceException)
            {
                return NotFound(new GetUserResponse());
            }
        }

        /// <summary>
        ///     Create a new User.
        /// </summary>
        /// <param name="postUser">The PostUserRequest.</param>
        /// <returns>A PostUserResponse.</returns>
        // POST api/users
        [HttpPost]
        [AuthorizePermission(Permission.InviteUser)]
        public async Task<ActionResult<PostUserResponse>> CreateUser([FromBody] PostUserRequest postUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new PostUserResponse());
            }

            try
            {
                var userToCreate = Mapper.Map<MeUser>(postUser);
                var currentUser = await CurrentUser();
                var createdUser = await _userCreationService.Handle(new CreateUserQuery(userToCreate, currentUser));
                return Ok(Mapper.Map<PostUserResponse>(createdUser));
            }
            catch (DocumentClientException)
            {
                return NotFound(new PostUserResponse());
            }
            catch (ArgumentException)
            {
                return NotFound(new PostUserResponse());
            }
        }

        /// <summary>
        /// Update a new User.
        /// </summary>
        /// <param name="putUser">The PutUserRequest.</param>
        /// <returns>A PutUserResponse.</returns>
        [HttpPut("{meUserId}")]
        [AuthorizePermission(Permission.UpdateUser)]
        public async Task<ActionResult<PutUserResponse>> UpdateUser([FromBody] PutUserRequest putUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new PutUserResponse());
            }

            try
            {
                var userToUpdate = Mapper.Map<MeUser>(putUser);
                var currentUser = await CurrentUser();
                var updatedUser = await _userUpdateService.Handle(new UserUpdateQuery(userToUpdate, currentUser));
                return Ok(Mapper.Map<PutUserResponse>(updatedUser));
            }
            catch (DocumentClientException)
            {
                return NotFound(new PutUserResponse());
            }
            catch (ArgumentException)
            {
                return NotFound(new PutUserResponse());
            }
        }

        /// <summary>
        /// Delete a user.
        /// </summary>
        /// <param name="meUserId">The user identifier.</param>
        /// <returns></returns>
        [HttpDelete("{meUserId}")]
        [AuthorizePermission(Permission.DeleteUser)]
        public async Task<ActionResult<DeleteUserResponse>> DeleteUser(string meUserId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new DeleteUserResponse());
            }

            try
            {
                var currentUser = await CurrentUser();
                var updatedUser = await _userDeleteService.Handle(
                    new UserDeleteQuery(
                        meUserId,
                        currentUser));
                return Ok(Mapper.Map<DeleteUserResponse>(updatedUser));
            }
            catch (DocumentClientException)
            {
                return NotFound(new DeleteUserResponse());
            }
            catch (ArgumentException)
            {
                return NotFound(new DeleteUserResponse());
            }
        }

        /// <summary>
        /// Update a new User.
        /// </summary>
        /// <param name="meUserId">The User Identifier.</param>
        /// <returns>A PutUserResponse.</returns>
        [HttpPut("{meUserId}/suspend")]
        [AuthorizePermission(Permission.EnableUser)]
        public async Task<ActionResult<PutSuspendUserResponse>> EnableUser(string meUserId)
        {
            return await SuspendUser(meUserId, false);
        }

        /// <summary>
        /// Update a new User.
        /// </summary>
        /// <param name="meUserId">The User Identifier.</param>
        /// <returns>A PutUserResponse.</returns>
        [HttpPut("{meUserId}/suspend")]
        [AuthorizePermission(Permission.SuspendUser)]
        public async Task<ActionResult<PutSuspendUserResponse>> SuspendUser(string meUserId)
        {
            return await SuspendUser(meUserId, true);
        }

        /// <summary>
        /// Common method for <see cref="EnableUser"/> and <see cref="SuspendUser"/>.
        /// </summary>
        /// <param name="userId">Id of the user.</param>
        /// <param name="suspend">True to suspend the user.</param>
        /// <returns>Response.</returns>
        private async Task<ActionResult<PutSuspendUserResponse>> SuspendUser(string userId, bool suspend)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new PutSuspendUserResponse());
            }

            try
            {
                var currentUser = await CurrentUser();
                var updatedUser = await _userSuspendService.Handle(
                    new UserSuspendQuery(
                        userId,
                        suspend,
                        currentUser));
                return Ok(Mapper.Map<PutSuspendUserResponse>(updatedUser));
            }
            catch (DocumentClientException)
            {
                return NotFound(new PutSuspendUserResponse());
            }
            catch (ArgumentException)
            {
                return NotFound(new PutSuspendUserResponse());
            }
        }
    }
}