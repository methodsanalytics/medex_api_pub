﻿using AutoMapper;
using MedicalExaminer.API.Authorization;
using MedicalExaminer.Common.Loggers;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;
using Microsoft.AspNetCore.Authorization;
using Permission = MedicalExaminer.Common.Authorization.Permission;

namespace MedicalExaminer.API.Controllers
{
    /// <summary>
    /// Base Authorization Controller.
    /// </summary>
    public abstract class AuthorizedBaseController : AuthenticatedBaseController
    {
        /// <summary>
        /// Authorization Based Controller.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="mapper">Mapper.</param>
        /// <param name="usersRetrievalByEmailService">Users Retrieval By Email Service.</param>
        /// <param name="authorizationService">Authorization Service.</param>
        protected AuthorizedBaseController(
            IMELogger logger,
            IMapper mapper,
            IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser> usersRetrievalByEmailService,
            IAuthorizationService authorizationService)
            : base(logger, mapper, usersRetrievalByEmailService)
        {
            AuthorizationService = authorizationService;
        }

        /// <summary>
        /// Authorization Service.
        /// </summary>
        protected IAuthorizationService AuthorizationService { get; }

        /// <summary>
        /// Can do Permission somewhere against something.
        /// </summary>
        /// <param name="permission">The Permission.</param>
        /// <returns>True if can.</returns>
        protected bool CanAsync(Permission permission)
        {
            var authorizationResult = AuthorizationService
                .AuthorizeAsync(User, null, new PermissionRequirement(permission)).Result;

            return authorizationResult.Succeeded;
        }

        /// <summary>
        /// Can do Permission to Document
        /// </summary>
        /// <param name="permission">The Permission.</param>
        /// <param name="document">The Document.</param>
        /// <returns>True if can.</returns>
        protected bool CanAsync(Permission permission, ILocationBasedDocument document)
        {
            var authorizationResult = AuthorizationService
                .AuthorizeAsync(User, document, new PermissionRequirement(permission)).Result;

            return authorizationResult.Succeeded;
        }
    }
}
