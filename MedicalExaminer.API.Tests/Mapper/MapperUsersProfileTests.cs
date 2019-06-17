﻿using AutoMapper;
using FluentAssertions;
using MedicalExaminer.API.Extensions.Data;
using MedicalExaminer.API.Models.V1.Users;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;
using Xunit;

namespace MedicalExaminer.API.Tests.Mapper
{
    /// <summary>
    /// Mapper Users Tests.
    /// </summary>
    public class MapperUsersProfileTests
    {
        /// <summary>
        ///     Mapper.
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="MapperUsersProfileTests"/> class.
        /// </summary>
        public MapperUsersProfileTests()
        {
            var config = new MapperConfiguration(cfg => { cfg.AddProfile<UsersProfile>(); });
            _mapper = config.CreateMapper();
        }

        [Fact]
        public void MEUserPermission_To_UserPermission()
        {
            var meUserPermission = new MEUserPermission
            {
                LocationId = "LocationId",
                PermissionId = "PermissionId",
                UserRole = UserRoles.MedicalExaminer
            };

            var result = _mapper.Map<UserPermission>(meUserPermission);

            result.LocationId.Should().Be(meUserPermission.LocationId);
            result.PermissionId.Should().Be(meUserPermission.PermissionId);
            result.UserRole.Should().Be(meUserPermission.UserRole);
        }

        /// <summary>
        ///     Test Mapping UserToCreate to GetUserResponse.
        /// </summary>
        [Fact]
        public void TestGetUserResponse()
        {
            var expectedUserId = "expectedUserId";

            var examination = new MeUser
            {
                UserId = expectedUserId
            };

            var response = _mapper.Map<GetUserResponse>(examination);

            response.UserId.Should().Be(expectedUserId);
        }

        /// <summary>
        /// Test Mapping PostUserRequest to UserToCreate.
        /// </summary>
        [Fact]
        public void TestPostUserRequest()
        {
            var expectedEmail = "testing@methods.co.uk";

            var examination = new PostUserRequest
            {
                Email = expectedEmail
            };

            var response = _mapper.Map<MeUser>(examination);

            response.Email.Should().Be(expectedEmail);
        }

        /// <summary>
        ///     Test Mapping UserToCreate to PostUserResponse.
        /// </summary>
        [Fact]
        public void TestPostUserResponse()
        {
            var expectedUserId = "expectedUserId";

            var examination = new MeUser
            {
                UserId = expectedUserId
            };

            var response = _mapper.Map<PostUserResponse>(examination);

            response.UserId.Should().Be(expectedUserId);
        }

        /// <summary>
        ///     Test Mapping PutUserRequest to UserToCreate.
        /// </summary>
        [Fact]
        public void TestPutUserRequest()
        {
            var expectedUserId = "expectedUserId";
            var expectedEmail = "test@methods.co.uk";

            var user = new PutUserRequest
            {
                UserId = expectedUserId,
                Email = expectedEmail
            };

            var response = _mapper.Map<MeUser>(user);

            response.UserId.Should().Be(expectedUserId);
            response.Email.Should().Be(expectedEmail);
        }

        /// <summary>
        ///     Test Mapping UserToCreate to PutUserResponse.
        /// </summary>
        [Fact]
        public void TestPutUserResponse()
        {
            var expectedUserId = "expectedUserId";

            var examination = new MeUser
            {
                UserId = expectedUserId
            };

            var response = _mapper.Map<PutUserResponse>(examination);

            response.UserId.Should().Be(expectedUserId);
        }

        /// <summary>
        ///     Test Mapping UserToCreate to UserItem.
        /// </summary>
        [Fact]
        public void TestUserItem()
        {
            var expectedUserId = "expectedUserId";

            var examination = new MeUser
            {
                UserId = expectedUserId
            };

            var response = _mapper.Map<UserItem>(examination);

            response.UserId.Should().Be(expectedUserId);
        }
    }
}