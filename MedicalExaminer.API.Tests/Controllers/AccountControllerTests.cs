﻿using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using MedicalExaminer.API.Authorization;
using MedicalExaminer.API.Controllers;
using MedicalExaminer.API.Models;
using MedicalExaminer.API.Models.v1.Account;
using MedicalExaminer.Common.Authorization;
using MedicalExaminer.Common.Loggers;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using Okta.Sdk;
using Okta.Sdk.Configuration;
using Xunit;

namespace MedicalExaminer.API.Tests.Controllers
{
    /// <summary>
    /// Account Controller Tests.
    /// </summary>
    public class AccountControllerTests : ControllerTestsBase<AccountController>
    {
        private readonly Mock<IAsyncQueryHandler<UserRetrievalByOktaIdQuery, MeUser>> _mockUserRetrievalByOktaIdService;

        private readonly Mock<IRolePermissions> _mockRolePermissions;
        public AccountControllerTests()
        {
            var mockLogger = new Mock<IMELogger>(MockBehavior.Strict);
            var mockMapper = new Mock<IMapper>(MockBehavior.Strict);
            _mockUserRetrievalByOktaIdService = new Mock<IAsyncQueryHandler<UserRetrievalByOktaIdQuery, MeUser>>(MockBehavior.Strict);
            _mockRolePermissions = new Mock<IRolePermissions>();

            Controller = new AccountController(
                mockLogger.Object,
                mockMapper.Object,
                _mockUserRetrievalByOktaIdService.Object,
                _mockRolePermissions.Object)
            {
                ControllerContext = GetControllerContext()
            };
        }

        [Fact]
        public async void ValidateSession_ReturnsSession_WhenSessionValid()
        {
            var user = new MeUser()
            {
                Permissions = new List<MEUserPermission>()
                {
                    new MEUserPermission()
                    {
                        UserRole = UserRoles.MedicalExaminer
                    }
                }
            };

            var permissions = new Dictionary<Permission, bool>();

            _mockRolePermissions
                .Setup(rp => rp.PermissionsForRoles(It.IsAny<IEnumerable<UserRoles>>()))
                .Returns(permissions)
                .Verifiable();

            _mockUserRetrievalByOktaIdService
                .Setup(s => s.Handle(It.IsAny<UserRetrievalByOktaIdQuery>()))
                .Returns(Task.FromResult(user));

            var response = await Controller.ValidateSession();
            var taskResult = response.Should().BeOfType<ActionResult<PostValidateSessionResponse>>().Subject;
            var okRequestResult = taskResult.Result.Should().BeAssignableTo<OkObjectResult>().Subject;
            var result = okRequestResult.Value.Should().BeAssignableTo<PostValidateSessionResponse>().Subject;
            result.Role.Contains(UserRoles.MedicalExaminer).Should().BeTrue();

            _mockRolePermissions
                .Verify(rp => rp.PermissionsForRoles(It.IsAny<IEnumerable<UserRoles>>()));
        }

        [Fact]
        public async void ValidateSession_ReturnsBadRequest_WhenNoCurrentUser()
        {
            const MeUser user = null;

            _mockUserRetrievalByOktaIdService
                .Setup(s => s.Handle(It.IsAny<UserRetrievalByOktaIdQuery>()))
                .Returns(Task.FromResult(user));

            var response = await Controller.ValidateSession();
            var taskResult = response.Should().BeOfType<ActionResult<PostValidateSessionResponse>>().Subject;
            var badRequestResult = taskResult.Result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
            badRequestResult.Value.Should().BeAssignableTo<PostValidateSessionResponse>();
        }
    }
}
