﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using MedicalExaminer.API.Controllers;
using MedicalExaminer.API.Models.v1.Permissions;
using MedicalExaminer.Common;
using MedicalExaminer.Common.Loggers;
using MedicalExaminer.Common.Queries.Location;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace MedicalExaminer.API.Tests.Controllers
{
    /// <summary>
    ///     Tests the Users Controller
    /// </summary>
    public class TestPermissionsController : AuthorizedControllerTestsBase<PermissionsController>
    {
        private readonly Mock<IAsyncQueryHandler<UserRetrievalByIdQuery, MeUser>> _userRetrievalByIdServiceMock;
        private readonly Mock<IAsyncQueryHandler<UserUpdateQuery, MeUser>> _userUpdateServiceMock;
        private readonly Mock<IAsyncQueryHandler<LocationParentsQuery, IEnumerable<Location>>> _locationParentsServiceMock;
        private readonly Mock<IPermissionPersistence> _permissionPersistenceMock;

        /// <summary>
        ///     Initializes a new instance of the <see cref="TestPermissionsController" /> class.
        /// </summary>
        public TestPermissionsController()
        {
            _permissionPersistenceMock = new Mock<IPermissionPersistence>();
            _userRetrievalByIdServiceMock = new Mock<IAsyncQueryHandler<UserRetrievalByIdQuery, MeUser>>();
            _userUpdateServiceMock = new Mock<IAsyncQueryHandler<UserUpdateQuery, MeUser>>();
            _locationParentsServiceMock = new Mock<IAsyncQueryHandler<LocationParentsQuery, IEnumerable<Location>>>();

            Controller = new PermissionsController(
                LoggerMock.Object,
                Mapper,
                UsersRetrievalByEmailServiceMock.Object,
                AuthorizationServiceMock.Object,
                PermissionServiceMock.Object,
                _userRetrievalByIdServiceMock.Object,
                _userUpdateServiceMock.Object,
                _locationParentsServiceMock.Object,
                _permissionPersistenceMock.Object
            );
        }

        /// <summary>
        ///     Test returning an empty list
        /// </summary>
        /// <returns>Async Task</returns>
        [Fact]
        public async Task TestGetEmptyResponse()
        {
            // Arrange
            var userId = "fake_id_01";

            _permissionPersistenceMock.Setup(pp => pp.GetPermissionsAsync(userId)).Returns(
                Task.FromResult<IEnumerable<Permission>>(
                    new List<Permission>()));

            // Act
            var response = await Controller.GetPermissions(userId);

            // Assert
            response.Result.Should().BeAssignableTo<OkObjectResult>();
            var result = (OkObjectResult)response.Result;
            result.Value.Should().BeAssignableTo<GetPermissionsResponse>();
            var model = (GetPermissionsResponse)result.Value;
            model.Errors.Count.Should().Be(0);
            model.Success.Should().BeTrue();

            model.Permissions.Count().Should().Be(0);
        }

        /// <summary>
        ///     Test get a list of users
        /// </summary>
        /// <returns>Async Task</returns>
        [Fact]
        public async Task TestGetGoodResponse()
        {
            // Arrange
            const string expectedPermissionId = "fake_id_02";
            const string userId = "fake_id_01";

            _permissionPersistenceMock.Setup(pp => pp.GetPermissionsAsync("fake_id_01")).Returns(
                Task.FromResult<IEnumerable<Permission>>(
                    new List<Permission>
                        { new Permission { UserId = "fake_id_01", PermissionId = expectedPermissionId } }));

            // Act
            var response = await Controller.GetPermissions(userId);

            // Assert
            response.Result.Should().BeAssignableTo<OkObjectResult>();
            var result = (OkObjectResult)response.Result;
            result.Value.Should().BeAssignableTo<GetPermissionsResponse>();
            var model = (GetPermissionsResponse)result.Value;
            model.Errors.Count.Should().Be(0);
            model.Success.Should().BeTrue();

            model.Permissions.Count().Should().Be(1);
            model.Permissions.First().PermissionId.Should().Be(expectedPermissionId);
        }

        /// <summary>
        ///     Test that a good response is returned in full
        /// </summary>
        /// <returns>Async Task</returns>
        [Fact]
        public async Task TestGetIdGoodResponse()
        {
            // Arrange
            const string expectedPermissionId = "expectedPermissionId";
            const string expectedUserId = "expectedUserId";

            var expectedPermission = new Permission { PermissionId = expectedPermissionId, UserId = expectedUserId };

            _permissionPersistenceMock.Setup(pp => pp.GetPermissionAsync(expectedUserId, expectedPermissionId)).Returns(
                Task.FromResult(expectedPermission));

            // Act
            var response = await Controller.GetPermission(expectedUserId, expectedPermissionId);

            // Assert
            response.Result.Should().BeAssignableTo<OkObjectResult>();
            var result = (OkObjectResult)response.Result;
            result.Value.Should().BeAssignableTo<GetPermissionResponse>();
            var model = (GetPermissionResponse)result.Value;
            model.Errors.Count.Should().Be(0);
            model.Success.Should().BeTrue();
            model.UserId.Should().Be(expectedUserId);
            model.PermissionId.Should().Be(expectedPermissionId);
        }

        /// <summary>
        ///     Test when no user is found
        /// </summary>
        /// <returns>Async Task</returns>
        [Fact]
        public async Task TestGetIdNotFoundResponse()
        {
            // Arrange
            const string expectedUserId = "expectedUserId";

            _permissionPersistenceMock.Setup(up => up.GetPermissionAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Throws<NullReferenceException>();

            // Act
            var response = await Controller.GetPermission(expectedUserId, "Something_that_does_not_exist");

            // Assert
            response.Result.Should().BeAssignableTo<NotFoundObjectResult>();
            var result = (NotFoundObjectResult)response.Result;
            result.Value.Should().BeAssignableTo<GetPermissionResponse>();
            var model = (GetPermissionResponse)result.Value;
            model.Errors.Count.Should().Be(0);
            model.Success.Should().BeTrue();

            model.UserId.Should().Be(null);
        }

        /// <summary>
        ///     Test that model validation error causes validation failure
        /// </summary>
        /// <returns>Async Task</returns>
        [Fact]
        public async Task TestGetIdValidationFailure()
        {
            // Arrange
            Controller.ModelState.AddModelError("An", "Error");

            // Act
            var response = await Controller.GetPermission(string.Empty, string.Empty);

            // Assert
            response.Result.Should().BeAssignableTo<BadRequestObjectResult>();
            var result = (BadRequestObjectResult)response.Result;
            result.Value.Should().BeAssignableTo<GetPermissionResponse>();
            var model = (GetPermissionResponse)result.Value;
            model.Errors.Count.Should().Be(1);
            model.Success.Should().BeFalse();
        }
    }
}