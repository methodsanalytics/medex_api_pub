﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentAssertions;
using MedicalExaminer.API.Controllers;
using MedicalExaminer.API.Models.v1.Permissions;
using MedicalExaminer.Common;
using MedicalExaminer.Common.Queries.Location;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

        private readonly Mock<IAsyncQueryHandler<LocationsParentsQuery, IDictionary<string, IEnumerable<Location>>>>
            _locationsParentsServiceMock;


        /// <summary>
        ///     Initializes a new instance of the <see cref="TestPermissionsController" /> class.
        /// </summary>
        public TestPermissionsController()
            : base(false)
        {
            _userRetrievalByIdServiceMock = new Mock<IAsyncQueryHandler<UserRetrievalByIdQuery, MeUser>>(MockBehavior.Strict);
            _userUpdateServiceMock = new Mock<IAsyncQueryHandler<UserUpdateQuery, MeUser>>(MockBehavior.Strict);
            _locationParentsServiceMock = new Mock<IAsyncQueryHandler<LocationParentsQuery, IEnumerable<Location>>>(MockBehavior.Strict);
            _locationsParentsServiceMock = new Mock<IAsyncQueryHandler<LocationsParentsQuery, IDictionary<string, IEnumerable<Location>>>>(MockBehavior.Strict);

            Controller = new PermissionsController(
                LoggerMock.Object,
                Mapper,
                UsersRetrievalByEmailServiceMock.Object,
                AuthorizationServiceMock.Object,
                PermissionServiceMock.Object,
                _userRetrievalByIdServiceMock.Object,
                _userUpdateServiceMock.Object,
                _locationParentsServiceMock.Object,
                _locationsParentsServiceMock.Object
            );

            Controller.ControllerContext = GetContollerContext();
        }

        private ControllerContext GetContollerContext()
        {
            return new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Email, "test@example.com")
                    }))
                }
            };
        }

        [Fact]
        public async Task GetPermissions_ReturnsPermission()
        {
            // Arrange
            var expectedEmail = "expectedEmail";
            var expectedCurrentUserId = "expectedCurrentUserId";
            var expectedUserId = "expectedUserId";
            var expectedSiteId = "site1";
            var expectedRegionId = "region1";
            var expectedNationalId = "national1";
            var expectedRole = UserRoles.MedicalExaminer;
            var expectedCurrentUserEmail = "test@example.com";
            var expectedUser = new MeUser()
            {
                UserId = expectedUserId,
                Permissions = new[]
                {
                    new MEUserPermission()
                    {
                        LocationId = expectedSiteId,
                        UserRole = expectedRole,
                    },
                    new MEUserPermission()
                    {
                        LocationId = expectedNationalId,
                        UserRole = expectedRole,
                    },
                }
            };
            var expectedCurrentUser = new MeUser()
            {
                UserId = expectedCurrentUserId,
                Email = expectedCurrentUserEmail,
                Permissions = new[]
                {
                    new MEUserPermission()
                    {
                        LocationId = expectedRegionId,
                        UserRole = expectedRole,
                    },
                }
            };
            var expectedPermission = Common.Authorization.Permission.GetUserPermissions;
            var expectedLocations = new[]
            {
                expectedRegionId,
            };
            IDictionary<string, IEnumerable<Location>> expectedLocationsParents =
                new Dictionary<string, IEnumerable<Location>>()
                {
                    {
                        expectedSiteId, new[]
                        {
                            new Location() {LocationId = expectedSiteId},
                            new Location() {LocationId = "trust1"},
                            new Location() {LocationId = expectedRegionId},
                            new Location() {LocationId = expectedNationalId},
                        }
                    },
                    {
                        expectedNationalId, new[]
                        {
                            new Location() {LocationId = expectedNationalId},
                        }
                    },
                };

            _userRetrievalByIdServiceMock
                .Setup(urbis => urbis.Handle(It.Is<UserRetrievalByIdQuery>(q => q.UserId == expectedUserId)))
                .Returns(Task.FromResult(expectedUser));

            UsersRetrievalByEmailServiceMock
                .Setup(urbes => urbes.Handle(It.Is<UserRetrievalByEmailQuery>(q => q.Email == expectedCurrentUserEmail)))
                .Returns(Task.FromResult(expectedCurrentUser));

            PermissionServiceMock
                .Setup(ps => ps.LocationIdsWithPermission(expectedCurrentUser, expectedPermission))
                .Returns(expectedLocations);

            _locationsParentsServiceMock
                .Setup(lps => lps.Handle(It.IsAny<LocationsParentsQuery>()))
                .Returns(Task.FromResult(expectedLocationsParents));

            // Act
            var response = await Controller.GetPermissions(expectedUserId);

            // Assert
            response.Result.Should().BeAssignableTo<OkObjectResult>();
            var result = (OkObjectResult)response.Result;
            result.Value.Should().BeAssignableTo<GetPermissionsResponse>();
            var model = (GetPermissionsResponse)result.Value;
            model.Errors.Count.Should().Be(0);
            model.Success.Should().BeTrue();

            model.Permissions.Count().Should().Be(1);
            model.Permissions.ElementAt(0).LocationId.Should().Be(expectedSiteId);
            model.Permissions.ElementAt(0).UserRole.Should().Be(expectedRole);
        }

        [Fact]
        public async Task GetPermissions_ReturnsNotFound_WhenDocumentClientExceptionOccurs()
        {
            // Arrange
            const string expectedUserId = "expectedUserId";

            _userRetrievalByIdServiceMock
                .Setup(urbis => urbis.Handle(It.Is<UserRetrievalByIdQuery>(q => q.UserId == expectedUserId)))
                .Throws(CreateDocumentClientExceptionForTesting());

            // Act
            var response = await Controller.GetPermissions(expectedUserId);

            // Assert
            response.Result.Should().BeAssignableTo<NotFoundObjectResult>();
            var result = (NotFoundObjectResult)response.Result;
            result.Value.Should().BeAssignableTo<GetPermissionsResponse>();
            var model = (GetPermissionsResponse)result.Value;
            model.Errors.Count.Should().Be(0);
            model.Success.Should().BeTrue();

            model.Permissions.Should().BeNull();
        }

        [Fact]
        public async Task GetPermissions_ReturnsNotFound_WhenArgumentExceptionnOccurs()
        {
            // Arrange
            const string expectedUserId = "expectedUserId";

            _userRetrievalByIdServiceMock
                .Setup(urbis => urbis.Handle(It.Is<UserRetrievalByIdQuery>(q => q.UserId == expectedUserId)))
                .Throws<ArgumentException>();

            // Act
            var response = await Controller.GetPermissions(expectedUserId);

            // Assert
            response.Result.Should().BeAssignableTo<NotFoundObjectResult>();
            var result = (NotFoundObjectResult)response.Result;
            result.Value.Should().BeAssignableTo<GetPermissionsResponse>();
            var model = (GetPermissionsResponse)result.Value;
            model.Errors.Count.Should().Be(0);
            model.Success.Should().BeTrue();

            model.Permissions.Should().BeNull();
        }

        [Fact]
        public async Task GetPermission_ReturnsPermission()
        {
            // Arrange
            SetupAuthorize(AuthorizationResult.Success());

            var expectedPermissionId = "expectedPermissionId";
            var expectedEmail = "expectedEmail";
            var expectedCurrentUserId = "expectedCurrentUserId";
            var expectedUserId = "expectedUserId";
            var expectedSiteId = "site1";
            var expectedRegionId = "region1";
            var expectedNationalId = "national1";
            var expectedRole = UserRoles.MedicalExaminer;
            var expectedCurrentUserEmail = "test@example.com";
            var expectedUser = new MeUser()
            {
                UserId = expectedUserId,
                Permissions = new[]
                {
                    new MEUserPermission()
                    {
                        PermissionId = expectedPermissionId,
                        LocationId = expectedSiteId,
                        UserRole = expectedRole,
                    },
                    new MEUserPermission()
                    {
                        PermissionId = "unexpectedPermissionId",
                        LocationId = expectedNationalId,
                        UserRole = expectedRole,
                    },
                }
            };
            var expectedCurrentUser = new MeUser()
            {
                UserId = expectedCurrentUserId,
                Email = expectedCurrentUserEmail,
                Permissions = new[]
                {
                    new MEUserPermission()
                    {
                        LocationId = expectedRegionId,
                        UserRole = expectedRole,
                    },
                }
            };
            var expectedPermission = Common.Authorization.Permission.GetUserPermissions;
            var expectedLocations = new[]
            {
                expectedRegionId,
            };
            IEnumerable<Location> expectedLocationParents = new[]
            {
                new Location() {LocationId = expectedSiteId},
                new Location() {LocationId = "trust1"},
                new Location() {LocationId = expectedRegionId},
                new Location() {LocationId = expectedNationalId},
            };

            _userRetrievalByIdServiceMock
                .Setup(urbis => urbis.Handle(It.Is<UserRetrievalByIdQuery>(q => q.UserId == expectedUserId)))
                .Returns(Task.FromResult(expectedUser));

            UsersRetrievalByEmailServiceMock
                .Setup(urbes => urbes.Handle(It.Is<UserRetrievalByEmailQuery>(q => q.Email == expectedCurrentUserEmail)))
                .Returns(Task.FromResult(expectedCurrentUser));

            PermissionServiceMock
                .Setup(ps => ps.LocationIdsWithPermission(expectedCurrentUser, expectedPermission))
                .Returns(expectedLocations);

            _locationParentsServiceMock
                .Setup(lps => lps.Handle(It.IsAny<LocationParentsQuery>()))
                .Returns(Task.FromResult(expectedLocationParents));

            // Act
            var response = await Controller.GetPermission(expectedUserId, expectedPermissionId);

            // Assert
            response.Result.Should().BeAssignableTo<OkObjectResult>();
            var result = (OkObjectResult)response.Result;
            result.Value.Should().BeAssignableTo<GetPermissionResponse>();
            var model = (GetPermissionResponse)result.Value;
            model.Errors.Count.Should().Be(0);
            model.Success.Should().BeTrue();

            model.PermissionId.Should().Be(expectedPermissionId);
        }

        [Fact]
        public async Task GetPermission_ReturnsNotFound_WhenUserNotFound()
        {
            // Arrange
            SetupAuthorize(AuthorizationResult.Success());

            var expectedPermissionId = "expectedPermissionId";
            var expectedEmail = "expectedEmail";
            var expectedCurrentUserId = "expectedCurrentUserId";
            var expectedUserId = "expectedUserId";
            var expectedSiteId = "site1";
            var expectedRegionId = "region1";
            var expectedNationalId = "national1";
            var expectedRole = UserRoles.MedicalExaminer;
            var expectedCurrentUserEmail = "test@example.com";
            const MeUser expectedUser = null;
            var expectedCurrentUser = new MeUser()
            {
                UserId = expectedCurrentUserId,
                Email = expectedCurrentUserEmail,
                Permissions = new[]
                {
                    new MEUserPermission()
                    {
                        LocationId = expectedRegionId,
                        UserRole = expectedRole,
                    },
                }
            };
            var expectedPermission = Common.Authorization.Permission.GetUserPermissions;
            var expectedLocations = new[]
            {
                expectedRegionId,
            };
            IEnumerable<Location> expectedLocationParents = new[]
            {
                new Location() {LocationId = expectedSiteId},
                new Location() {LocationId = "trust1"},
                new Location() {LocationId = expectedRegionId},
                new Location() {LocationId = expectedNationalId},
            };

            _userRetrievalByIdServiceMock
                .Setup(urbis => urbis.Handle(It.Is<UserRetrievalByIdQuery>(q => q.UserId == expectedUserId)))
                .Returns(Task.FromResult(expectedUser));

            UsersRetrievalByEmailServiceMock
                .Setup(urbes => urbes.Handle(It.Is<UserRetrievalByEmailQuery>(q => q.Email == expectedCurrentUserEmail)))
                .Returns(Task.FromResult(expectedCurrentUser));

            PermissionServiceMock
                .Setup(ps => ps.LocationIdsWithPermission(expectedCurrentUser, expectedPermission))
                .Returns(expectedLocations);

            _locationParentsServiceMock
                .Setup(lps => lps.Handle(It.IsAny<LocationParentsQuery>()))
                .Returns(Task.FromResult(expectedLocationParents));

            // Act
            var response = await Controller.GetPermission(expectedUserId, expectedPermissionId);

            // Assert
            response.Result.Should().BeAssignableTo<NotFoundObjectResult>();
            var result = (NotFoundObjectResult)response.Result;
            result.Value.Should().BeAssignableTo<GetPermissionResponse>();
            var model = (GetPermissionResponse)result.Value;
            model.Errors.Count.Should().Be(0);
            model.Success.Should().BeTrue();

            model.PermissionId.Should().BeNull();
        }


        [Fact]
        public async Task GetPermission_ReturnsNotFound_WhenPermissionNotFound()
        {
            // Arrange
            SetupAuthorize(AuthorizationResult.Success());

            var expectedPermissionId = "expectedPermissionId";
            var expectedEmail = "expectedEmail";
            var expectedCurrentUserId = "expectedCurrentUserId";
            var expectedUserId = "expectedUserId";
            var expectedSiteId = "site1";
            var expectedRegionId = "region1";
            var expectedNationalId = "national1";
            var expectedRole = UserRoles.MedicalExaminer;
            var expectedCurrentUserEmail = "test@example.com";
            var expectedUser = new MeUser()
            {
                UserId = expectedUserId,
                Permissions = new[]
                {
                    new MEUserPermission()
                    {
                        PermissionId = "unexpectedPermissionId",
                        LocationId = expectedNationalId,
                        UserRole = expectedRole,
                    },
                }
            };
            var expectedCurrentUser = new MeUser()
            {
                UserId = expectedCurrentUserId,
                Email = expectedCurrentUserEmail,
                Permissions = new[]
                {
                    new MEUserPermission()
                    {
                        LocationId = expectedRegionId,
                        UserRole = expectedRole,
                    },
                }
            };
            var expectedPermission = Common.Authorization.Permission.GetUserPermissions;
            var expectedLocations = new[]
            {
                expectedRegionId,
            };
            IEnumerable<Location> expectedLocationParents = new[]
            {
                new Location() {LocationId = expectedSiteId},
                new Location() {LocationId = "trust1"},
                new Location() {LocationId = expectedRegionId},
                new Location() {LocationId = expectedNationalId},
            };

            _userRetrievalByIdServiceMock
                .Setup(urbis => urbis.Handle(It.Is<UserRetrievalByIdQuery>(q => q.UserId == expectedUserId)))
                .Returns(Task.FromResult(expectedUser));

            UsersRetrievalByEmailServiceMock
                .Setup(urbes => urbes.Handle(It.Is<UserRetrievalByEmailQuery>(q => q.Email == expectedCurrentUserEmail)))
                .Returns(Task.FromResult(expectedCurrentUser));

            PermissionServiceMock
                .Setup(ps => ps.LocationIdsWithPermission(expectedCurrentUser, expectedPermission))
                .Returns(expectedLocations);

            _locationParentsServiceMock
                .Setup(lps => lps.Handle(It.IsAny<LocationParentsQuery>()))
                .Returns(Task.FromResult(expectedLocationParents));

            // Act
            var response = await Controller.GetPermission(expectedUserId, expectedPermissionId);

            // Assert
            response.Result.Should().BeAssignableTo<NotFoundObjectResult>();
            var result = (NotFoundObjectResult)response.Result;
            result.Value.Should().BeAssignableTo<GetPermissionResponse>();
            var model = (GetPermissionResponse)result.Value;
            model.Errors.Count.Should().Be(0);
            model.Success.Should().BeTrue();

            model.PermissionId.Should().BeNull();
        }

        [Fact]
        public async Task GetPermission_ReturnsForbid_WhenNotAuthorized()
        {
            // Arrange
            SetupAuthorize(AuthorizationResult.Failed());

            var expectedPermissionId = "expectedPermissionId";
            var expectedEmail = "expectedEmail";
            var expectedCurrentUserId = "expectedCurrentUserId";
            var expectedUserId = "expectedUserId";
            var expectedSiteId = "site1";
            var expectedRegionId = "region1";
            var expectedNationalId = "national1";
            var expectedRole = UserRoles.MedicalExaminer;
            var expectedCurrentUserEmail = "test@example.com";
            var expectedUser = new MeUser()
            {
                UserId = expectedUserId,
                Permissions = new[]
                {
                    new MEUserPermission()
                    {
                        PermissionId = expectedPermissionId,
                        LocationId = expectedSiteId,
                        UserRole = expectedRole,
                    },
                    new MEUserPermission()
                    {
                        PermissionId = "unexpectedPermissionId",
                        LocationId = expectedNationalId,
                        UserRole = expectedRole,
                    },
                }
            };
            var expectedCurrentUser = new MeUser()
            {
                UserId = expectedCurrentUserId,
                Email = expectedCurrentUserEmail,
                Permissions = new[]
                {
                    new MEUserPermission()
                    {
                        LocationId = expectedRegionId,
                        UserRole = expectedRole,
                    },
                }
            };
            var expectedPermission = Common.Authorization.Permission.GetUserPermissions;
            var expectedLocations = new[]
            {
                expectedRegionId,
            };
            IEnumerable<Location> expectedLocationParents = new[]
            {
                new Location() {LocationId = expectedSiteId},
                new Location() {LocationId = "trust1"},
                new Location() {LocationId = expectedRegionId},
                new Location() {LocationId = expectedNationalId},
            };

            _userRetrievalByIdServiceMock
                .Setup(urbis => urbis.Handle(It.Is<UserRetrievalByIdQuery>(q => q.UserId == expectedUserId)))
                .Returns(Task.FromResult(expectedUser));

            UsersRetrievalByEmailServiceMock
                .Setup(urbes => urbes.Handle(It.Is<UserRetrievalByEmailQuery>(q => q.Email == expectedCurrentUserEmail)))
                .Returns(Task.FromResult(expectedCurrentUser));

            PermissionServiceMock
                .Setup(ps => ps.LocationIdsWithPermission(expectedCurrentUser, expectedPermission))
                .Returns(expectedLocations);

            _locationParentsServiceMock
                .Setup(lps => lps.Handle(It.IsAny<LocationParentsQuery>()))
                .Returns(Task.FromResult(expectedLocationParents));

            // Act
            var response = await Controller.GetPermission(expectedUserId, expectedPermissionId);

            // Assert
            response.Result.Should().BeAssignableTo<ForbidResult>();
        }

        [Fact]
        public async void GetPermission_ReturnsBadRequest_WhenModelStateFails()
        {
            // Arrange
            const string expectedPermissionId = "expectedPermissionId";
            const string expectedUserId = "expectedUserId";
            Controller.ModelState.AddModelError("test", "test");

            // Act
            var response = await Controller.GetPermission(expectedUserId, expectedPermissionId);

            // Assert
            response.Result.Should().BeAssignableTo<BadRequestObjectResult>();
            var result = (BadRequestObjectResult)response.Result;
            result.Value.Should().BeAssignableTo<GetPermissionResponse>();
            var model = (GetPermissionResponse)result.Value;
            model.Errors.Count.Should().Be(1);
            model.Success.Should().BeFalse();
        }

        [Fact]
        public async Task GetPermission_ReturnsNotFound_WhenArgumentExceptionnOccurs()
        {
            // Arrange
            const string expectedUserId = "expectedUserId";
            const string expectedPermissionId = "expectedPermissionId";

            _userRetrievalByIdServiceMock
                .Setup(urbis => urbis.Handle(It.Is<UserRetrievalByIdQuery>(q => q.UserId == expectedUserId)))
                .Throws<ArgumentException>();

            // Act
            var response = await Controller.GetPermission(expectedUserId, expectedPermissionId);

            // Assert
            response.Result.Should().BeAssignableTo<NotFoundObjectResult>();
            var result = (NotFoundObjectResult)response.Result;
            result.Value.Should().BeAssignableTo<GetPermissionResponse>();
            var model = (GetPermissionResponse)result.Value;
            model.Errors.Count.Should().Be(0);
            model.Success.Should().BeTrue();

            model.PermissionId.Should().BeNull();
        }

        [Fact]
        public async Task GetPermission_ReturnsNotFound_WhenNullReferenceExceptionOccurs()
        {
            // Arrange
            const string expectedUserId = "expectedUserId";
            const string expectedPermissionId = "expectedPermissionId";

            _userRetrievalByIdServiceMock
                .Setup(urbis => urbis.Handle(It.Is<UserRetrievalByIdQuery>(q => q.UserId == expectedUserId)))
                .Throws<NullReferenceException>();

            // Act
            var response = await Controller.GetPermission(expectedUserId, expectedPermissionId);

            // Assert
            response.Result.Should().BeAssignableTo<NotFoundObjectResult>();
            var result = (NotFoundObjectResult)response.Result;
            result.Value.Should().BeAssignableTo<GetPermissionResponse>();
            var model = (GetPermissionResponse)result.Value;
            model.Errors.Count.Should().Be(0);
            model.Success.Should().BeTrue();

            model.PermissionId.Should().BeNull();
        }

        [Fact]
        public async Task GetPermissions_ReturnsNotFound_WhenUserNotFound()
        {
            // Arrange
            var expectedEmail = "expectedEmail";
            var expectedCurrentUserId = "expectedCurrentUserId";
            var expectedUserId = "expectedUserId";
            var expectedSiteId = "site1";
            var expectedRegionId = "region1";
            var expectedNationalId = "national1";
            var expectedRole = UserRoles.MedicalExaminer;
            var expectedCurrentUserEmail = "test@example.com";
            const MeUser expectedUser = null;
            var expectedCurrentUser = new MeUser()
            {
                UserId = expectedCurrentUserId,
                Email = expectedCurrentUserEmail,
                Permissions = new[]
                {
                    new MEUserPermission()
                    {
                        LocationId = expectedRegionId,
                        UserRole = expectedRole,
                    },
                }
            };
            var expectedPermission = Common.Authorization.Permission.GetUserPermissions;
            var expectedLocations = new[]
            {
                expectedRegionId,
            };
            IDictionary<string, IEnumerable<Location>> expectedLocationsParents =
                new Dictionary<string, IEnumerable<Location>>()
                {
                    {
                        expectedSiteId, new[]
                        {
                            new Location() {LocationId = expectedSiteId},
                            new Location() {LocationId = "trust1"},
                            new Location() {LocationId = expectedRegionId},
                            new Location() {LocationId = expectedNationalId},
                        }
                    },
                    {
                        expectedNationalId, new[]
                        {
                            new Location() {LocationId = expectedNationalId},
                        }
                    },
                };

            _userRetrievalByIdServiceMock
                .Setup(urbis => urbis.Handle(It.Is<UserRetrievalByIdQuery>(q => q.UserId == expectedUserId)))
                .Returns(Task.FromResult(expectedUser));

            UsersRetrievalByEmailServiceMock
                .Setup(urbes => urbes.Handle(It.Is<UserRetrievalByEmailQuery>(q => q.Email == expectedCurrentUserEmail)))
                .Returns(Task.FromResult(expectedCurrentUser));

            PermissionServiceMock
                .Setup(ps => ps.LocationIdsWithPermission(expectedCurrentUser, expectedPermission))
                .Returns(expectedLocations);

            _locationsParentsServiceMock
                .Setup(lps => lps.Handle(It.IsAny<LocationsParentsQuery>()))
                .Returns(Task.FromResult(expectedLocationsParents));

            // Act
            var response = await Controller.GetPermissions(expectedUserId);

            // Assert
            response.Result.Should().BeAssignableTo<NotFoundObjectResult>();
            var result = (NotFoundObjectResult)response.Result;
            result.Value.Should().BeAssignableTo<GetPermissionsResponse>();
            var model = (GetPermissionsResponse)result.Value;
            model.Errors.Count.Should().Be(0);
            model.Success.Should().BeTrue();
        }

        [Fact]
        public async Task CreatePermission_ReturnsPermission_WhenCanCreate()
        {
            // Arrange
            SetupAuthorize(AuthorizationResult.Success());

            var expectedPermissionId = "expectedPermissionId";
            var expectedEmail = "expectedEmail";
            var expectedCurrentUserId = "expectedCurrentUserId";
            var expectedUserId = "expectedUserId";
            var expectedSiteId = "site1";
            var expectedRegionId = "region1";
            var expectedNationalId = "national1";
            var expectedRole = UserRoles.MedicalExaminer;
            var expectedCurrentUserEmail = "test@example.com";
            var expectedUser = new MeUser()
            {
                UserId = expectedUserId,
                Permissions = new[]
                {
                    new MEUserPermission()
                    {
                        PermissionId = expectedPermissionId,
                        LocationId = expectedSiteId,
                        UserRole = expectedRole,
                    },
                    new MEUserPermission()
                    {
                        PermissionId = "unexpectedPermissionId",
                        LocationId = expectedNationalId,
                        UserRole = expectedRole,
                    },
                }
            };
            var expectedCurrentUser = new MeUser()
            {
                UserId = expectedCurrentUserId,
                Email = expectedCurrentUserEmail,
                Permissions = new[]
                {
                    new MEUserPermission()
                    {
                        LocationId = expectedRegionId,
                        UserRole = expectedRole,
                    },
                }
            };
            var expectedPermission = Common.Authorization.Permission.GetUserPermissions;
            var expectedLocations = new[]
            {
                expectedRegionId,
            };
            IEnumerable<Location> expectedLocationParents = new[]
            {
                new Location() {LocationId = expectedSiteId},
                new Location() {LocationId = "trust1"},
                new Location() {LocationId = expectedRegionId},
                new Location() {LocationId = expectedNationalId},
            };

            _userRetrievalByIdServiceMock
                .Setup(urbis => urbis.Handle(It.Is<UserRetrievalByIdQuery>(q => q.UserId == expectedUserId)))
                .Returns(Task.FromResult(expectedUser));

            UsersRetrievalByEmailServiceMock
                .Setup(urbes => urbes.Handle(It.Is<UserRetrievalByEmailQuery>(q => q.Email == expectedCurrentUserEmail)))
                .Returns(Task.FromResult(expectedCurrentUser));

            PermissionServiceMock
                .Setup(ps => ps.LocationIdsWithPermission(expectedCurrentUser, expectedPermission))
                .Returns(expectedLocations);

            _locationParentsServiceMock
                .Setup(lps => lps.Handle(It.IsAny<LocationParentsQuery>()))
                .Returns(Task.FromResult(expectedLocationParents));

            _userUpdateServiceMock
                .Setup(uus => uus.Handle(It.IsAny<UserUpdateQuery>()))
                .Returns(Task.FromResult(new MeUser()
                {
                    Permissions = new List<MEUserPermission>()
                }));

            // Act
            var response = await Controller.CreatePermission(expectedUserId, new PostPermissionRequest()
            {
                LocationId = expectedSiteId,
                //UserId = expectedUserId,
                UserRole = expectedRole,
            });

            // Assert
            response.Result.Should().BeAssignableTo<OkObjectResult>();
            var result = (OkObjectResult)response.Result;
            result.Value.Should().BeAssignableTo<PostPermissionResponse>();
            var model = (PostPermissionResponse)result.Value;
            model.Errors.Count.Should().Be(0);
            model.Success.Should().BeTrue();

            model.PermissionId.Should().NotBeNull();
        }

        [Fact]
        public async Task CreatePermission_ReturnsNotFound_WhenUserNotFound()
        {
            // Arrange
            SetupAuthorize(AuthorizationResult.Success());

            var expectedPermissionId = "expectedPermissionId";
            var expectedEmail = "expectedEmail";
            var expectedCurrentUserId = "expectedCurrentUserId";
            var expectedUserId = "expectedUserId";
            var expectedSiteId = "site1";
            var expectedRegionId = "region1";
            var expectedNationalId = "national1";
            var expectedRole = UserRoles.MedicalExaminer;
            var expectedCurrentUserEmail = "test@example.com";
            const MeUser expectedUser = null;
            var expectedCurrentUser = new MeUser()
            {
                UserId = expectedCurrentUserId,
                Email = expectedCurrentUserEmail,
                Permissions = new[]
                {
                    new MEUserPermission()
                    {
                        LocationId = expectedRegionId,
                        UserRole = expectedRole,
                    },
                }
            };
            var expectedPermission = Common.Authorization.Permission.GetUserPermissions;
            var expectedLocations = new[]
            {
                expectedRegionId,
            };
            IEnumerable<Location> expectedLocationParents = new[]
            {
                new Location() {LocationId = expectedSiteId},
                new Location() {LocationId = "trust1"},
                new Location() {LocationId = expectedRegionId},
                new Location() {LocationId = expectedNationalId},
            };

            _userRetrievalByIdServiceMock
                .Setup(urbis => urbis.Handle(It.Is<UserRetrievalByIdQuery>(q => q.UserId == expectedUserId)))
                .Returns(Task.FromResult(expectedUser));

            UsersRetrievalByEmailServiceMock
                .Setup(urbes => urbes.Handle(It.Is<UserRetrievalByEmailQuery>(q => q.Email == expectedCurrentUserEmail)))
                .Returns(Task.FromResult(expectedCurrentUser));

            PermissionServiceMock
                .Setup(ps => ps.LocationIdsWithPermission(expectedCurrentUser, expectedPermission))
                .Returns(expectedLocations);

            _locationParentsServiceMock
                .Setup(lps => lps.Handle(It.IsAny<LocationParentsQuery>()))
                .Returns(Task.FromResult(expectedLocationParents));

            _userUpdateServiceMock
                .Setup(uus => uus.Handle(It.IsAny<UserUpdateQuery>()))
                .Returns(Task.FromResult(new MeUser()
                {
                    Permissions = new List<MEUserPermission>()
                }));

            // Act
            var response = await Controller.CreatePermission(expectedUserId, new PostPermissionRequest()
            {
                LocationId = expectedSiteId,
                //UserId = expectedUserId,
                UserRole = expectedRole,
            });

            // Assert
            response.Result.Should().BeAssignableTo<NotFoundObjectResult>();
            var result = (NotFoundObjectResult)response.Result;
            result.Value.Should().BeAssignableTo<PostPermissionResponse>();
            var model = (PostPermissionResponse)result.Value;
            model.Errors.Count.Should().Be(0);
            model.Success.Should().BeTrue();

            model.PermissionId.Should().BeNull();
        }

        [Fact]
        public async Task CreatePermission_ReturnsForbid_WhenNoPermissionToCreate()
        {
            // Arrange
            SetupAuthorize(AuthorizationResult.Failed());

            var expectedPermissionId = "expectedPermissionId";
            var expectedEmail = "expectedEmail";
            var expectedCurrentUserId = "expectedCurrentUserId";
            var expectedUserId = "expectedUserId";
            var expectedSiteId = "site1";
            var expectedRegionId = "region1";
            var expectedNationalId = "national1";
            var expectedRole = UserRoles.MedicalExaminer;
            var expectedCurrentUserEmail = "test@example.com";
            var expectedUser = new MeUser()
            {
                UserId = expectedUserId,
                Permissions = new[]
                {
                    new MEUserPermission()
                    {
                        PermissionId = expectedPermissionId,
                        LocationId = expectedSiteId,
                        UserRole = expectedRole,
                    },
                    new MEUserPermission()
                    {
                        PermissionId = "unexpectedPermissionId",
                        LocationId = expectedNationalId,
                        UserRole = expectedRole,
                    },
                }
            };
            var expectedCurrentUser = new MeUser()
            {
                UserId = expectedCurrentUserId,
                Email = expectedCurrentUserEmail,
                Permissions = new[]
                {
                    new MEUserPermission()
                    {
                        LocationId = expectedRegionId,
                        UserRole = expectedRole,
                    },
                }
            };
            var expectedPermission = Common.Authorization.Permission.GetUserPermissions;
            var expectedLocations = new[]
            {
                expectedRegionId,
            };
            IEnumerable<Location> expectedLocationParents = new[]
            {
                new Location() {LocationId = expectedSiteId},
                new Location() {LocationId = "trust1"},
                new Location() {LocationId = expectedRegionId},
                new Location() {LocationId = expectedNationalId},
            };

            _userRetrievalByIdServiceMock
                .Setup(urbis => urbis.Handle(It.Is<UserRetrievalByIdQuery>(q => q.UserId == expectedUserId)))
                .Returns(Task.FromResult(expectedUser));

            UsersRetrievalByEmailServiceMock
                .Setup(urbes =>
                    urbes.Handle(It.Is<UserRetrievalByEmailQuery>(q => q.Email == expectedCurrentUserEmail)))
                .Returns(Task.FromResult(expectedCurrentUser));

            PermissionServiceMock
                .Setup(ps => ps.LocationIdsWithPermission(expectedCurrentUser, expectedPermission))
                .Returns(expectedLocations);

            _locationParentsServiceMock
                .Setup(lps => lps.Handle(It.IsAny<LocationParentsQuery>()))
                .Returns(Task.FromResult(expectedLocationParents));

            _userUpdateServiceMock
                .Setup(uus => uus.Handle(It.IsAny<UserUpdateQuery>()))
                .Returns(Task.FromResult(new MeUser()
                {
                    Permissions = new List<MEUserPermission>()
                }));

            // Act
            var response = await Controller.CreatePermission(expectedUserId, new PostPermissionRequest()
            {
                LocationId = expectedSiteId,
                //UserId = expectedUserId,
                UserRole = expectedRole,
            });

            // Assert
            response.Result.Should().BeAssignableTo<ForbidResult>();
        }

        [Fact]
        public async void CreatePermission_ReturnsBadRequest_WhenModelStateFails()
        {
            // Arrange
            Controller.ModelState.AddModelError("test", "test");

            // Act
            var response = await Controller.CreatePermission(string.Empty, new PostPermissionRequest());

            // Assert
            response.Result.Should().BeAssignableTo<BadRequestObjectResult>();
            var result = (BadRequestObjectResult)response.Result;
            result.Value.Should().BeAssignableTo<PostPermissionResponse>();
            var model = (PostPermissionResponse)result.Value;
            model.Errors.Count.Should().Be(1);
            model.Success.Should().BeFalse();
        }

        [Fact]
        public async Task CreatePermission_ReturnsNotFound_WhenArgumentExceptionnOccurs()
        {
            // Arrange
            _locationParentsServiceMock
                .Setup(urbis => urbis.Handle(It.IsAny<LocationParentsQuery>()))
                .Throws<ArgumentException>();

            // Act
            var response = await Controller.CreatePermission(string.Empty, new PostPermissionRequest());

            // Assert
            response.Result.Should().BeAssignableTo<NotFoundObjectResult>();
            var result = (NotFoundObjectResult)response.Result;
            result.Value.Should().BeAssignableTo<PostPermissionResponse>();
            var model = (PostPermissionResponse)result.Value;
            model.Errors.Count.Should().Be(0);
            model.Success.Should().BeTrue();

            model.PermissionId.Should().BeNull();
        }

        [Fact]
        public async Task CreatePermission_ReturnsNotFound_WhenDocumentClientExceptionOccurs()
        {
            // Arrange
            _locationParentsServiceMock
                .Setup(urbis => urbis.Handle(It.IsAny<LocationParentsQuery>()))
                .Throws(CreateDocumentClientExceptionForTesting());

            // Act
            var response = await Controller.CreatePermission(string.Empty, new PostPermissionRequest());

            // Assert
            response.Result.Should().BeAssignableTo<NotFoundObjectResult>();
            var result = (NotFoundObjectResult)response.Result;
            result.Value.Should().BeAssignableTo<PostPermissionResponse>();
            var model = (PostPermissionResponse)result.Value;
            model.Errors.Count.Should().Be(0);
            model.Success.Should().BeTrue();

            model.PermissionId.Should().BeNull();
        }

        [Fact]
        public async Task UpdatePermission_ReturnsPermission_WhenCanUpdate()
        {
            // Arrange
            SetupAuthorize(AuthorizationResult.Success());

            var expectedPermissionId = "expectedPermissionId";
            var expectedEmail = "expectedEmail";
            var expectedCurrentUserId = "expectedCurrentUserId";
            var expectedUserId = "expectedUserId";
            var expectedSiteId = "site1";
            var expectedRegionId = "region1";
            var expectedNationalId = "national1";
            var expectedRole = UserRoles.MedicalExaminer;
            var expectedCurrentUserEmail = "test@example.com";
            var expectedUser = new MeUser()
            {
                UserId = expectedUserId,
                Permissions = new[]
                {
                    new MEUserPermission()
                    {
                        PermissionId = expectedPermissionId,
                        LocationId = expectedSiteId,
                        UserRole = expectedRole,
                    },
                    new MEUserPermission()
                    {
                        PermissionId = "unexpectedPermissionId",
                        LocationId = expectedNationalId,
                        UserRole = expectedRole,
                    },
                }
            };
            var expectedCurrentUser = new MeUser()
            {
                UserId = expectedCurrentUserId,
                Email = expectedCurrentUserEmail,
                Permissions = new[]
                {
                    new MEUserPermission()
                    {
                        LocationId = expectedRegionId,
                        UserRole = expectedRole,
                    },
                }
            };
            var expectedPermission = Common.Authorization.Permission.GetUserPermissions;
            var expectedLocations = new[]
            {
                expectedRegionId,
            };
            IEnumerable<Location> expectedLocationParents = new[]
            {
                new Location() {LocationId = expectedSiteId},
                new Location() {LocationId = "trust1"},
                new Location() {LocationId = expectedRegionId},
                new Location() {LocationId = expectedNationalId},
            };

            _userRetrievalByIdServiceMock
                .Setup(urbis => urbis.Handle(It.Is<UserRetrievalByIdQuery>(q => q.UserId == expectedUserId)))
                .Returns(Task.FromResult(expectedUser));

            UsersRetrievalByEmailServiceMock
                .Setup(urbes => urbes.Handle(It.Is<UserRetrievalByEmailQuery>(q => q.Email == expectedCurrentUserEmail)))
                .Returns(Task.FromResult(expectedCurrentUser));

            PermissionServiceMock
                .Setup(ps => ps.LocationIdsWithPermission(expectedCurrentUser, expectedPermission))
                .Returns(expectedLocations);

            _locationParentsServiceMock
                .Setup(lps => lps.Handle(It.IsAny<LocationParentsQuery>()))
                .Returns(Task.FromResult(expectedLocationParents));

            _userUpdateServiceMock
                .Setup(uus => uus.Handle(It.IsAny<UserUpdateQuery>()))
                .Returns(Task.FromResult(new MeUser()
                {
                    Permissions = new List<MEUserPermission>()
                }));

            // Act
            var response = await Controller.UpdatePermission(expectedUserId, new PutPermissionRequest()
            {
                PermissionId = expectedPermissionId,
                LocationId = expectedSiteId,
                //UserId = expectedUserId,
                UserRole = expectedRole,
            });

            // Assert
            response.Result.Should().BeAssignableTo<OkObjectResult>();
            var result = (OkObjectResult)response.Result;
            result.Value.Should().BeAssignableTo<PutPermissionResponse>();
            var model = (PutPermissionResponse)result.Value;
            model.Errors.Count.Should().Be(0);
            model.Success.Should().BeTrue();

            model.PermissionId.Should().Be(expectedPermissionId);
            model.UserId.Should().Be(expectedUserId);
            model.UserRole.Should().Be(expectedRole);
            model.LocationId.Should().Be(expectedSiteId);
        }

        [Fact]
        public async Task UpdatePermission_ReturnsForbid_WhenCanNotUpdate()
        {
            // Arrange
            SetupAuthorize(AuthorizationResult.Failed());

            var expectedPermissionId = "expectedPermissionId";
            var expectedEmail = "expectedEmail";
            var expectedCurrentUserId = "expectedCurrentUserId";
            var expectedUserId = "expectedUserId";
            var expectedSiteId = "site1";
            var expectedRegionId = "region1";
            var expectedNationalId = "national1";
            var expectedRole = UserRoles.MedicalExaminer;
            var expectedCurrentUserEmail = "test@example.com";
            var expectedUser = new MeUser()
            {
                UserId = expectedUserId,
                Permissions = new[]
                {
                    new MEUserPermission()
                    {
                        PermissionId = expectedPermissionId,
                        LocationId = expectedSiteId,
                        UserRole = expectedRole,
                    },
                    new MEUserPermission()
                    {
                        PermissionId = "unexpectedPermissionId",
                        LocationId = expectedNationalId,
                        UserRole = expectedRole,
                    },
                }
            };
            var expectedCurrentUser = new MeUser()
            {
                UserId = expectedCurrentUserId,
                Email = expectedCurrentUserEmail,
                Permissions = new[]
                {
                    new MEUserPermission()
                    {
                        LocationId = expectedRegionId,
                        UserRole = expectedRole,
                    },
                }
            };
            var expectedPermission = Common.Authorization.Permission.GetUserPermissions;
            var expectedLocations = new[]
            {
                expectedRegionId,
            };
            IEnumerable<Location> expectedLocationParents = new[]
            {
                new Location() {LocationId = expectedSiteId},
                new Location() {LocationId = "trust1"},
                new Location() {LocationId = expectedRegionId},
                new Location() {LocationId = expectedNationalId},
            };

            _userRetrievalByIdServiceMock
                .Setup(urbis => urbis.Handle(It.Is<UserRetrievalByIdQuery>(q => q.UserId == expectedUserId)))
                .Returns(Task.FromResult(expectedUser));

            UsersRetrievalByEmailServiceMock
                .Setup(urbes => urbes.Handle(It.Is<UserRetrievalByEmailQuery>(q => q.Email == expectedCurrentUserEmail)))
                .Returns(Task.FromResult(expectedCurrentUser));

            PermissionServiceMock
                .Setup(ps => ps.LocationIdsWithPermission(expectedCurrentUser, expectedPermission))
                .Returns(expectedLocations);

            _locationParentsServiceMock
                .Setup(lps => lps.Handle(It.IsAny<LocationParentsQuery>()))
                .Returns(Task.FromResult(expectedLocationParents));

            _userUpdateServiceMock
                .Setup(uus => uus.Handle(It.IsAny<UserUpdateQuery>()))
                .Returns(Task.FromResult(new MeUser()
                {
                    Permissions = new List<MEUserPermission>()
                }));

            // Act
            var response = await Controller.UpdatePermission(expectedUserId, new PutPermissionRequest()
            {
                PermissionId = expectedPermissionId,
                LocationId = expectedSiteId,
                UserRole = expectedRole,
            });

            // Assert
            response.Result.Should().BeAssignableTo<ForbidResult>();
        }

        [Fact]
        public async Task UpdatePermission_ReturnsNotFound_WhenUserNotFound()
        {
            // Arrange
            SetupAuthorize(AuthorizationResult.Failed());

            var expectedPermissionId = "expectedPermissionId";
            var expectedUserId = "expectedUserId";
            var expectedSiteId = "site1";
            var expectedRole = UserRoles.MedicalExaminer;
            const MeUser expectedUser = null;

            _userRetrievalByIdServiceMock
                .Setup(urbis => urbis.Handle(It.Is<UserRetrievalByIdQuery>(q => q.UserId == expectedUserId)))
                .Returns(Task.FromResult(expectedUser));

            // Act
            var response = await Controller.UpdatePermission(expectedUserId, new PutPermissionRequest()
            {
                PermissionId = expectedPermissionId,
                LocationId = expectedSiteId,
                //UserId = expectedUserId,
                UserRole = expectedRole,
            });

            // Assert
            response.Result.Should().BeAssignableTo<NotFoundObjectResult>();
        }

        [Fact]
        public async Task UpdatePermission_ReturnsNotFound_WhenPermissionNotFoundOnUser()
        {
            // Arrange
            SetupAuthorize(AuthorizationResult.Failed());

            var expectedPermissionId = "expectedPermissionId";
            var expectedEmail = "expectedEmail";
            var expectedCurrentUserId = "expectedCurrentUserId";
            var expectedUserId = "expectedUserId";
            var expectedSiteId = "site1";
            var expectedRegionId = "region1";
            var expectedNationalId = "national1";
            var expectedRole = UserRoles.MedicalExaminer;
            var expectedCurrentUserEmail = "test@example.com";
            var expectedUser = new MeUser()
            {
                UserId = expectedUserId,
                Permissions = new[]
                {
                    new MEUserPermission()
                    {
                        PermissionId = "unexpectedPermissionId",
                        LocationId = expectedNationalId,
                        UserRole = expectedRole,
                    },
                }
            };
            var expectedCurrentUser = new MeUser()
            {
                UserId = expectedCurrentUserId,
                Email = expectedCurrentUserEmail,
                Permissions = new[]
                {
                    new MEUserPermission()
                    {
                        LocationId = expectedRegionId,
                        UserRole = expectedRole,
                    },
                }
            };
            var expectedPermission = Common.Authorization.Permission.GetUserPermissions;
            var expectedLocations = new[]
            {
                expectedRegionId,
            };
            IEnumerable<Location> expectedLocationParents = new[]
            {
                new Location() {LocationId = expectedSiteId},
                new Location() {LocationId = "trust1"},
                new Location() {LocationId = expectedRegionId},
                new Location() {LocationId = expectedNationalId},
            };

            _userRetrievalByIdServiceMock
                .Setup(urbis => urbis.Handle(It.Is<UserRetrievalByIdQuery>(q => q.UserId == expectedUserId)))
                .Returns(Task.FromResult(expectedUser));

            UsersRetrievalByEmailServiceMock
                .Setup(urbes => urbes.Handle(It.Is<UserRetrievalByEmailQuery>(q => q.Email == expectedCurrentUserEmail)))
                .Returns(Task.FromResult(expectedCurrentUser));

            PermissionServiceMock
                .Setup(ps => ps.LocationIdsWithPermission(expectedCurrentUser, expectedPermission))
                .Returns(expectedLocations);

            _locationParentsServiceMock
                .Setup(lps => lps.Handle(It.IsAny<LocationParentsQuery>()))
                .Returns(Task.FromResult(expectedLocationParents));

            _userUpdateServiceMock
                .Setup(uus => uus.Handle(It.IsAny<UserUpdateQuery>()))
                .Returns(Task.FromResult(new MeUser()
                {
                    Permissions = new List<MEUserPermission>()
                }));

            // Act
            var response = await Controller.UpdatePermission(expectedUserId, new PutPermissionRequest()
            {
                PermissionId = expectedPermissionId,
                LocationId = expectedSiteId,
                //UserId = expectedUserId,
                UserRole = expectedRole,
            });

            // Assert
            response.Result.Should().BeAssignableTo<NotFoundObjectResult>();
        }

        [Fact]
        public async void UpdatePermission_ReturnsBadRequest_WhenModelStateFails()
        {
            // Arrange
            Controller.ModelState.AddModelError("test", "test");

            // Act
            var response = await Controller.UpdatePermission(string.Empty, new PutPermissionRequest());

            // Assert
            response.Result.Should().BeAssignableTo<BadRequestObjectResult>();
            var result = (BadRequestObjectResult)response.Result;
            result.Value.Should().BeAssignableTo<PutPermissionResponse>();
            var model = (PutPermissionResponse)result.Value;
            model.Errors.Count.Should().Be(1);
            model.Success.Should().BeFalse();
        }

        [Fact]
        public async Task UpdatePermission_ReturnsNotFound_WhenArgumentExceptionOccurs()
        {
            // Arrange
            _userRetrievalByIdServiceMock
                .Setup(urbis => urbis.Handle(It.IsAny<UserRetrievalByIdQuery>()))
                .Throws<ArgumentException>();

            // Act
            var response = await Controller.UpdatePermission(string.Empty, new PutPermissionRequest());

            // Assert
            response.Result.Should().BeAssignableTo<NotFoundObjectResult>();
            var result = (NotFoundObjectResult)response.Result;
            result.Value.Should().BeAssignableTo<PutPermissionResponse>();
            var model = (PutPermissionResponse)result.Value;
            model.Errors.Count.Should().Be(0);
            model.Success.Should().BeTrue();

            model.PermissionId.Should().BeNull();
        }

        [Fact]
        public async Task UpdatePermission_ReturnsNotFound_WhenDocumentClientExceptionOccurs()
        {
            // Arrange
            _userRetrievalByIdServiceMock
                .Setup(urbis => urbis.Handle(It.IsAny<UserRetrievalByIdQuery>()))
                .Throws(CreateDocumentClientExceptionForTesting());

            // Act
            var response = await Controller.UpdatePermission(string.Empty, new PutPermissionRequest());

            // Assert
            response.Result.Should().BeAssignableTo<NotFoundObjectResult>();
            var result = (NotFoundObjectResult)response.Result;
            result.Value.Should().BeAssignableTo<PutPermissionResponse>();
            var model = (PutPermissionResponse)result.Value;
            model.Errors.Count.Should().Be(0);
            model.Success.Should().BeTrue();

            model.PermissionId.Should().BeNull();
        }

    }
}