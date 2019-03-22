﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MedicalExaminer.API.Filters;
using MedicalExaminer.API.Models.v1.Users;
using MedicalExaminer.Common;
using MedicalExaminer.Common.Loggers;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Common.Services.User;
using MedicalExaminer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;

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
    public class UsersController : BaseController
    {
        /// <summary>
        ///     The User Persistence Layer
        /// </summary>
        private readonly IAsyncQueryHandler<CreateUserQuery, MeUser> _userCreationService;
        private readonly IAsyncQueryHandler<UserRetrievalByIdQuery, MeUser> _userRetrievalByIdService;
        private readonly IAsyncQueryHandler<UsersRetrievalQuery, IEnumerable<MeUser>> _usersRetrievalService;
        private readonly IAsyncQueryHandler<UserUpdateQuery, MeUser> _usersUpdateService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="userCreationService">User creation service.</param>
        /// <param name="userRetrievalByIdService">User retrieval service.</param>
        /// <param name="usersRetrievalService">Users retrieval service.</param>
        /// <param name="logger">The Logger.</param>
        /// <param name="mapper">The Mapper.</param>
        public UsersController(
            IMELogger logger,
            IMapper mapper,
            IAsyncQueryHandler<CreateUserQuery, MeUser> userCreationService,
            IAsyncQueryHandler<UserRetrievalByIdQuery, MeUser> userRetrievalByIdService,
            IAsyncQueryHandler<UsersRetrievalQuery, IEnumerable<MeUser>> usersRetrievalService)
            : base(logger, mapper)
        {
            _userCreationService = userCreationService;
            _userRetrievalByIdService = userRetrievalByIdService;
            _usersRetrievalService = usersRetrievalService;
        }

        /// <summary>
        /// Get all Users.
        /// </summary>
        /// <returns>A GetUsersResponse.</returns>
        [HttpGet]
        [ServiceFilter(typeof(ControllerActionFilter))]
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
        [ServiceFilter(typeof(ControllerActionFilter))]
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
        ///     Get all Users that are in the role of Medical Examiner.
        /// </summary>
        /// <returns>A GetUsersResponse.</returns>
        [HttpGet("medical_examiners")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<GetUsersResponse>> GetMedicalExaminers()
        {
            try
            {
                var users = await _usersRetrievalService.Handle(new UsersRetrievalQuery(null));

                return Ok(new GetUsersResponse
                {
                    Users = users.Select(u => Mapper.Map<UserItem>(u))
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
        ///     Get all Users that are in the role of Medical Examiner Officer.
        /// </summary>
        /// <returns>A GetUsersResponse.</returns>
        [HttpGet("medical_examiner_officers")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<GetUsersResponse>> GetMedicalExaminerOfficers()
        {
            try
            {
                var users = await _usersRetrievalService.Handle(new UsersRetrievalQuery(null));
                return Ok(new GetUsersResponse
                {
                    Users = users.Select(u => Mapper.Map<UserItem>(u))
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
        ///     Create a new User.
        /// </summary>
        /// <param name="postUser">The PostUserRequest.</param>
        /// <returns>A PostUserResponse.</returns>
        // POST api/users
        [HttpPost]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<PostUserResponse>> CreateUser([FromBody] PostUserRequest postUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new PostUserResponse());
            }

            try
            {
                var user = Mapper.Map<MeUser>(postUser);
                var createdUser = await _userCreationService.Handle(new CreateUserQuery(user));
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
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<PutUserResponse>> UpdateUser([FromBody] PutUserRequest putUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new PutUserResponse());
            }

            try
            {
                var user = Mapper.Map<MeUser>(putUser);
                var updatedUser = await _usersUpdateService.Handle(new UserUpdateQuery(user));
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
    }
}