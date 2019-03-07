﻿using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using MedicalExaminer.API.Filters;
using MedicalExaminer.API.Models.v1.Users;
using MedicalExaminer.Common;
using MedicalExaminer.Common.Loggers;
using MedicalExaminer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;

namespace MedicalExaminer.API.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// Users Controller
    /// </summary>
    [Route("users")]
    [ApiController]
    [Authorize]
    public class UsersController : BaseController
    {
        /// <summary>
        /// The User Persistence Layer
        /// </summary>
        private readonly IUserPersistence _userPersistence;

        /// <summary>
        /// Initialise a new instance of the Users controller.
        /// </summary>
        /// <param name="userPersistence">The User Persistance.</param>
        /// <param name="logger">The Logger.</param>
        /// <param name="mapper">The Mapper.</param>
        public UsersController(IUserPersistence userPersistence, IMELogger logger, IMapper mapper)
            : base(logger, mapper)
        {
            _userPersistence = userPersistence;
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
                var users = await _userPersistence.GetUsersAsync();
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
        /// Get all Users that are of type Medical Examiner.
        /// </summary>
        /// <returns>A GetUsersResponse.</returns>
        [HttpGet("/MedicalExaminers")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<GetUsersResponse>> GetMedicalExaminers()
        {
            try
            {
                var users = await _userPersistence.GetUsersAsync();
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
        /// Get a User by its Identifier.
        /// </summary>
        /// <param name="meUserId">The User Identifier.</param>
        /// <returns>A GetUserResponse.</returns>
        // GET api/users/{user_id}
        [HttpGet("{meUserId}")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<GetUserResponse>> GetUser(string meUserId)
        {
            if (!ModelState.IsValid) return BadRequest(new GetUserResponse());

            try
            {
                var user = await _userPersistence.GetUserAsync(meUserId);
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

            return BadRequest(new GetUserResponse());
        }

        /// <summary>
        /// Create a new User.
        /// </summary>
        /// <param name="postUser">The PostUserRequest.</param>
        /// <returns>A PostUserResponse.</returns>
       // POST api/users
        [HttpPost]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<PostUserResponse>> CreateUser([FromBody] PostUserRequest postUser)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new PostUserResponse());

                var user = Mapper.Map<MeUser>(postUser);
                var createdUser = await _userPersistence.CreateUserAsync(user);
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
        /// Create a new User.
        /// </summary>
        /// <param name="putUser">The PutUserRequest.</param>
        /// <returns>A PutUserResponse.</returns>
        // POST api/users
        [HttpPut("{id}")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<PutUserResponse>> UpdateUser([FromBody] PutUserRequest putUser)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new PutUserResponse());
                var user = Mapper.Map<MeUser>(putUser);
                var updatedUser = await _userPersistence.UpdateUserAsync(user);
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