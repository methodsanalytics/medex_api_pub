using System;
using System.Threading.Tasks;
using FluentAssertions;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Common.Services.User;
using MedicalExaminer.Models;
using Xunit;

namespace MedicalExaminer.API.Tests.Services.Users
{
    public class UserSuspendServiceTests : ServiceTestsBase<
        UserSuspendQuery,
        UserConnectionSettings,
        MeUser,
        MeUser,
        UserSuspendService>
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Handle(bool suspend)
        {
            // Arrange
            var expectedUserId = "expectedUserId";
            var expectedEmail = "email1";
            var expectedUser = new MeUser()
            {
                UserId = expectedUserId,
                Email = expectedEmail,
            };

            var expectedOurUserId = "userId3";
            var expectedOurEmail = "email1";
            var expectedOurUser = new MeUser()
            {
                UserId = expectedOurUserId,
                Email = expectedOurEmail,
            };

            var query = new UserSuspendQuery(expectedUser.UserId, suspend, expectedOurUser);

            // Act
            var result = await Service.Handle(query);

            // Assert
            result.Should().NotBeNull();
            result.Suspended.Should().Be(suspend);
        }

        [Fact]
        public async Task Handle_ThrowsException_WhenUserDoesntExist()
        {
            // Arrange
            var expectedUserId = "unexpectedUserId";
            var expectedEmail = "email1";
            var expectedUser = new MeUser()
            {
                UserId = expectedUserId,
                Email = expectedEmail,
            };

            var expectedOurUserId = "userId3";
            var expectedOurEmail = "email1";
            var expectedOurUser = new MeUser()
            {
                UserId = expectedOurUserId,
                Email = expectedOurEmail,
            };

            var query = new UserSuspendQuery(expectedUser.UserId, true, expectedOurUser);

            // Act
            Func<Task> act = async () => await Service.Handle(query);

            // Assert
            act.Should().Throw<InvalidOperationException>();
        }

        protected override MeUser[] GetExamples()
        {
            return new[]
            {
                new MeUser()
                {
                    UserId = "expectedUserId",
                    Email = "email1",
                },
                new MeUser()
                {
                    UserId = "userId2",
                    Email = "email2",
                },
            };
        }
    }
}
