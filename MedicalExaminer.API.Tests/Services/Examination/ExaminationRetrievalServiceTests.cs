using System;
using System.Threading.Tasks;
using FluentAssertions;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Common.Services.Examination;
using MedicalExaminer.Models;
using Moq;
using Xunit;

namespace MedicalExaminer.API.Tests.Services.Examination
{
    public class ExaminationRetrievalServiceTests : ServiceTestsBase<
        ExaminationRetrievalQuery,
        ExaminationConnectionSettings,
        MedicalExaminer.Models.Examination,
        MedicalExaminer.Models.Examination,
        ExaminationRetrievalService>
    {
        [Fact]
        public async Task ExaminationIdFoundReturnsExpectedExamination()
        {
            // Arrange
            const string id = "a";

            // Act
            var result = await Service.Handle(new ExaminationRetrievalQuery(id, new Mock<MeUser>().Object));

            // Assert
            result.Should().NotBeNull();
            Assert.Equal("a", result.ExaminationId);
        }

        [Fact]
        public async void ExaminationIdNotFoundReturnsNull()
        {
            // Arrange
            const string examinationId = "c";

            // Act
            var results = await Service.Handle(new ExaminationRetrievalQuery(examinationId, null));

            // Assert
            results.Should().BeNull();
        }

        protected override MedicalExaminer.Models.Examination[] GetExamples()
        {
            var examination1 = new MedicalExaminer.Models.Examination()
            {
                ExaminationId = "a"
            };
            var examination2 = new MedicalExaminer.Models.Examination()
            {
                ExaminationId = "b"
            };
            return new []{ examination1, examination2};
        }

        protected override ExaminationRetrievalQuery GetQuery()
        {
            return new ExaminationRetrievalQuery(null, null);
        }
    }
}
