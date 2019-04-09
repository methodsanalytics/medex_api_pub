﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MedicalExaminer.API.Authorization;
using MedicalExaminer.API.Services;
using MedicalExaminer.Common.Loggers;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Permission = MedicalExaminer.Common.Authorization.Permission;

namespace MedicalExaminer.API.Controllers
{
    /// <summary>
    /// Test Feature Controller.
    /// </summary>
    [ApiVersion("1.0")]
    [Route("/v{api-version:apiVersion}/test")]
    [ApiController]
    [Authorize]
    public class TestFeatureController : AuthorizedBaseController
    {
        /// <summary>
        /// Initialise a new instance of the <see cref="TestFeatureController"/>.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="mapper">Mapper.</param>
        /// <param name="authorizationService">Authorization Service.</param>
        /// <param name="permissionService">Permission Service.</param>
        public TestFeatureController(
            IMELogger logger,
            IMapper mapper,
            IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser> usersRetrievalByEmailService,
            IAuthorizationService authorizationService,
            IPermissionService permissionService)
            : base(logger, mapper, usersRetrievalByEmailService, authorizationService, permissionService)
        {
        }

        [HttpGet("action")]
        [AuthorizePermission(Permission.GetLocation)]
        public bool Action()
        {
            return true;
        }

        [HttpGet("actionInside")]
        public bool ActionInside()
        {
            var document = new Examination()
            {

            };

            if(CanAsync(Permission.GetLocation))
            {
                return true;
            }

            return false;
        }

        [HttpGet("actionInside2")]
        public bool ActionInside2()
        {
            var document = new Examination()
            {

            };

            if (CanAsync(Permission.CreateExamination, document))
            {
                return true;
            }

            return false;
        }
    }
}
