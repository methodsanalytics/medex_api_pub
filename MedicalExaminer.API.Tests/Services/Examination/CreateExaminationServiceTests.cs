﻿using System;
using System.Collections.Generic;
using System.Text;
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
    public class CreateExaminationServiceTests
    {
        [Fact]
        public void CreateExaminationQuerySuccessReturnsExaminationId()
        {
            // Arrange
            IExamination examination = new MedicalExaminer.Models.Examination();
            var connectionSettings = new Mock<IExaminationConnectionSettings>();
            var query = new CreateExaminationQuery(examination);
            var dbAccess = new Mock<IDatabaseAccess>();
            dbAccess.Setup(db => db.Create(connectionSettings.Object, examination)).Returns(Task.FromResult("a")).Verifiable();
            var sut = new CreateExaminationService(dbAccess.Object, connectionSettings.Object);
            
            // Act
            var result = sut.Handle(query);

            // Assert
            dbAccess.Verify(db => db.Create(connectionSettings.Object, examination), Times.Once);
            Assert.NotNull(result.Result);
        }

        [Fact]
        public void CreateExaminationQueryIsNullThrowsException()
        {
            // Arrange
            var connectionSettings = new Mock<IExaminationConnectionSettings>();
            CreateExaminationQuery query = null;
            var dbAccess = new Mock<IDatabaseAccess>();
            var sut = new CreateExaminationService(dbAccess.Object, connectionSettings.Object);

            Action act = () => sut.Handle(query);
            act.Should().Throw<ArgumentNullException>();
        }
    }
}