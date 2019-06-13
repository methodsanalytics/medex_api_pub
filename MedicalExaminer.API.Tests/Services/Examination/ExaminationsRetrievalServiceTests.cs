﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cosmonaut;
using FluentAssertions;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Common.Services.Examination;
using MedicalExaminer.Models.Enums;
using Xunit;

namespace MedicalExaminer.API.Tests.Services.Examination
{
    public class ExaminationsRetrievalServiceTests : ServiceTestsBase<
        ExaminationsRetrievalQuery,
        ExaminationConnectionSettings,
        IEnumerable<MedicalExaminer.Models.Examination>,
        MedicalExaminer.Models.Examination,
        ExaminationsRetrievalService>
    {
        /// <inheritdoc/>
        /// <remarks>Overrides to pass extra constructor parameter.</remarks>
        protected override ExaminationsRetrievalService GetService(
            IDatabaseAccess databaseAccess,
            ExaminationConnectionSettings connectionSettings,
            ICosmosStore<MedicalExaminer.Models.Examination> cosmosStore = null)
        {
            var store = CosmosMocker.CreateCosmosStore(GetExamples());

            var examinationQueryBuilder = new ExaminationsQueryExpressionBuilder();

            return new ExaminationsRetrievalService(
                databaseAccess,
                connectionSettings,
                examinationQueryBuilder,
                store.Object);
        }

        [Fact]
        public virtual async Task UnassignedCasesReturnsCorrectCount()
        {
            //Arrange
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(PermissedLocations(), CaseStatus.Unassigned,
                "", null, 1, 10, "", true);

            //Act
            var results = await Service.Handle(examinationsDashboardQuery);

            //Assert
            results.Should().NotBeNull();
            Assert.Equal(9, results.Count());
        }

        [Fact]
        public virtual async Task ReadyForMEScrutinyCasesReturnsCorrectCount()
        {
            //Arrange
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(PermissedLocations(), CaseStatus.ReadyForMEScrutiny,
                "", null, 1, 10, "", true);

            //Act
            var results = await Service.Handle(examinationsDashboardQuery);

            //Assert
            results.Should().NotBeNull();
            Assert.Equal(1, results.Count());
        }

        [Fact]
        public virtual async Task ReadyForMEScrutinyAndLocationCasesReturnsCorrectCount()
        {
            // Arrange
            var permissedLocations = new[] { "expectedLocation" };
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(
                permissedLocations,
                CaseStatus.ReadyForMEScrutiny,
                "expectedLocation",
                null,
                1,
                10,
                string.Empty,
                true);

            // Act
            var results = (await Service.Handle(examinationsDashboardQuery)).ToList();

            // Assert
            results.Should().NotBeNull();
            Assert.Single(results);
        }

        [Fact]
        public virtual async Task EmptyQueryReturnsAllOpenCases()
        {
            //Arrange
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(PermissedLocations(), null,
                "", null, 1, 10, "", true);

            //Act
            var results = await Service.Handle(examinationsDashboardQuery);

            //Assert
            results.Should().NotBeNull();
            Assert.Equal(9, results.Count());
        }

        [Fact]
        public virtual async Task EmptyQueryWithOrderByReturnsAllOpenCasesInOrder()
        {
            //Arrange
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(PermissedLocations(), null,
                "", null, 1, 10, "", true);

            //Act
            var results = await Service.Handle(examinationsDashboardQuery);

            //Assert
            results.Should().NotBeNull();
            Assert.Equal(9, results.Count());
        }

        [Fact]
        public virtual async Task ClosedCasesQueryReturnsCorrectCount()
        {
            //Arrange
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(PermissedLocations(), null,
                "", null, 1, 10, "", false);

            //Act
            var results = await Service.Handle(examinationsDashboardQuery);

            //Assert
            results.Should().NotBeNull();
            Assert.Single(results);
        }

        [Fact]
        public virtual async Task AdmissionNotesHaveBeenAddedQueryReturnsCorrectCount()
        {
            //Arrange
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(PermissedLocations(), CaseStatus.AdmissionNotesHaveBeenAdded,
                "", null, 1, 10, "", true);

            //Act
            var results = await Service.Handle(examinationsDashboardQuery);

            //Assert
            results.Should().NotBeNull();
            Assert.Single(results);
        }

        [Fact]
        public virtual async Task HaveBeenScrutinisedByMEQueryReturnsCorrectCount()
        {
            //Arrange
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(PermissedLocations(), CaseStatus.HaveBeenScrutinisedByME,
                "", null, 1, 10, "", true);

            //Act
            var results = await Service.Handle(examinationsDashboardQuery);

            //Assert
            results.Should().NotBeNull();
            Assert.Single(results);
        }

        [Fact]
        public virtual async Task PendingAdmissionNotesQueryReturnsCorrectCount()
        {
            //Arrange
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(PermissedLocations(), CaseStatus.PendingAdmissionNotes,
                "", null, 1, 10, "", true);

            //Act
            var results = await Service.Handle(examinationsDashboardQuery);

            //Assert
            results.Should().NotBeNull();
            Assert.Equal(9, results.Count());
        }

        [Fact]
        public virtual async Task PendingDiscussionWithQAPQueryReturnsCorrectCount()
        {
            //Arrange
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(PermissedLocations(), CaseStatus.PendingDiscussionWithQAP,
                "", null, 1, 10, "", true);

            //Act
            var results = await Service.Handle(examinationsDashboardQuery);

            //Assert
            results.Should().NotBeNull();
            Assert.Equal(9, results.Count());
        }

        [Fact]
        public virtual async Task PendingDiscussionWithRepresentativeQueryReturnsCorrectCount()
        {
            //Arrange
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(PermissedLocations(), CaseStatus.PendingDiscussionWithRepresentative,
                "", null, 1, 10, "", true);

            //Act
            var results = await Service.Handle(examinationsDashboardQuery);

            //Assert
            results.Should().NotBeNull();
            Assert.Equal(9, results.Count());
        }

        [Fact]
        public virtual async Task HaveFinalCaseOutstandingOutcomesQueryReturnsCorrectCount()
        {
            //Arrange
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(PermissedLocations(), CaseStatus.HaveFinalCaseOutstandingOutcomes,
                "", null, 1, 10, "", true);

            //Act
            var results = await Service.Handle(examinationsDashboardQuery);

            //Assert
            results.Should().NotBeNull();
            Assert.Equal(9, results.Count());
        }

        [Fact]
        public virtual async Task FilterByLocationsReturnsCorrectCases()
        {
            // Arrange
            const string expectedLocationId = "expectedLocation";
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(
                PermissedLocations(),
                null,
                expectedLocationId,
                null,
                1,
                10,
                string.Empty,
                true);

            // Act
            var results = (await Service.Handle(examinationsDashboardQuery)).ToList();

            // Assert
            results.Should().NotBeNull();
            Assert.Equal(4, results.Count());
        }

        [Fact]
        public virtual async Task FilterByLocationsReturnsCorrectCasesWhenNoFilter()
        {
            // Arrange
            const string expectedLocationId = "";
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(
                PermissedLocations(),
                null,
                expectedLocationId,
                null,
                1,
                10,
                string.Empty,
                true);

            // Act
            var results = (await Service.Handle(examinationsDashboardQuery)).ToList();

            // Assert
            results.Should().NotBeNull();
            Assert.Equal(9, results.Count());
        }

        /// <inheritdoc/>
        protected override MedicalExaminer.Models.Examination[] GetExamples()
        {
            var examination1 = new MedicalExaminer.Models.Examination()
            {
                ExaminationId = "examination1",
                Unassigned = true,
                CaseCompleted = false,
                NationalLocationId = "expectedLocation",
            };

            var examination2 = new MedicalExaminer.Models.Examination()
            {
                ExaminationId = "examination2",
                ReadyForMEScrutiny = true,
                CaseCompleted = false,
                RegionLocationId = "expectedLocation",
            };

            var examination4 = new MedicalExaminer.Models.Examination()
            {
                ExaminationId = "examination4",
                CaseCompleted = true,
            };

            var examination5 = new MedicalExaminer.Models.Examination()
            {
                ExaminationId = "examination5",
                CaseCompleted = false,
                UrgencyScore = 3,
                TrustLocationId = "expectedLocation",
            };

            var examination6 = new MedicalExaminer.Models.Examination()
            {
                ExaminationId = "examination6",
                CaseCompleted = false,
                AdmissionNotesHaveBeenAdded = true,
                SiteLocationId = "expectedLocation",
            };

            var examination7 = new MedicalExaminer.Models.Examination()
            {
                ExaminationId = "examination7",
                CaseCompleted = false,
                PendingDiscussionWithQAP = true
            };

            var examination8 = new MedicalExaminer.Models.Examination()
            {
                ExaminationId = "examination8",
                CaseCompleted = false,
                PendingDiscussionWithRepresentative = true
            };

            var examination9 = new MedicalExaminer.Models.Examination()
            {
                ExaminationId = "examination9",
                CaseCompleted = false,
                HaveFinalCaseOutcomesOutstanding = true
            };

            var examination10 = new MedicalExaminer.Models.Examination()
            {
                ExaminationId = "examination10",
                CaseCompleted = false,
                HaveBeenScrutinisedByME = true
            };

            var examination11 = new MedicalExaminer.Models.Examination()
            {
                ExaminationId = "examination11",
                CaseCompleted = false,
                PendingAdmissionNotes = true
            };

            return SetLocationCache(new[]
            {
                examination1, examination2, examination4, examination5,
                           examination6, examination7, examination8, examination9, examination10,
                           examination11
            });
        }

        private MedicalExaminer.Models.Examination[] SetLocationCache(MedicalExaminer.Models.Examination[] examinations)
        {
            foreach (var examination in examinations)
            {
                if (examination.SiteLocationId == null)
                {
                    examination.SiteLocationId = "site1";
                }
            }

            return examinations;
        }

        private IEnumerable<string> PermissedLocations()
        {
            return new[] { "site1", "expectedLocation" };
        }
    }
}