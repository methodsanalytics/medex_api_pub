﻿using AutoMapper;
using FluentAssertions;
using MedicalExaminer.API.Extensions.Data;
using MedicalExaminer.API.Models.v1.Users;
using MedicalExaminer.Models;
using Xunit;

namespace MedicalExaminer.API.Tests.Mapper
{
    /// <summary>
    /// Mapper Users Tests.
    /// </summary>
    public class MapperUsersProfileTests
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MapperUsersProfileTests"/> class.
        /// </summary>
        public MapperUsersProfileTests()
        {
            var config = new MapperConfiguration(cfg => { cfg.AddProfile<UsersProfile>(); });
            _mapper = config.CreateMapper();
        }

        /// <summary>
        ///     Mapper.
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        ///     Test Mapping MeUser to GetUserResponse.
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
        ///     Test Mapping PostUserRequest to MeUser.
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
        ///     Test Mapping MeUser to PostUserResponse.
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
        ///     Test Mapping PutUserRequest to MeUser.
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
        ///     Test Mapping MeUser to PutUserResponse.
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
        ///     Test Mapping MeUser to UserItem.
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