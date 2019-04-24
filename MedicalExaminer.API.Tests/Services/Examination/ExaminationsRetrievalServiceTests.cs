using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cosmonaut;
using FluentAssertions;
using FluentAssertions.Collections;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Common.Services.Examination;
using MedicalExaminer.Models.Enums;
using Moq;
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
            // Arrange
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(CaseStatus.Unassigned,
                "", null, 1, 10, "", true);

            // Act
            var results = await Service.Handle(examinationsDashboardQuery);

            // Assert
            results.Should().NotBeNull();
            Assert.Single(results);
        }


        [Fact]
        public virtual async Task OrderByCaseUrgencyScoreReturnsCorrectItemsOnGivenPage()
        {
            // Arrange
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(null,
                "", ExaminationsOrderBy.Urgency, 2, 3, "", true);

            // Act
            var results = await Service.Handle(examinationsDashboardQuery);

            var expected = new List<MedicalExaminer.Models.Examination>()
            {
                examination6,
                examination7,
                examination4
            };
            // Assert
            results.Should().NotBeNull();
            Assert.Equal(3, results.Count());
            results.SequenceEqual(expected);
        }


        [Fact]
        public virtual async Task ReadyForMEScrutinyCasesReturnsCorrectCount()
        {
            // Arrange
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(CaseStatus.ReadyForMEScrutiny,
                "", null, 1, 10, "", true);

            // Act
            var results = await Service.Handle(examinationsDashboardQuery);

            // Assert
            results.Should().NotBeNull();
            Assert.Equal(2, results.Count());
        }

        [Fact]
        public virtual async Task ReadyForMEScrutinyAndLocationCasesReturnsCorrectCount()
        {
            // Arrange
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(CaseStatus.ReadyForMEScrutiny,
                "a", null, 1, 10, "", true);

            // Act
            var results = await Service.Handle(examinationsDashboardQuery);

            // Assert
            results.Should().NotBeNull();
            Assert.Single(results);
        }

        [Fact]
        public virtual async Task EmptyQueryReturnsAllOpenCases()
        {
            // Arrange
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(null,
                "", null, 1, 10, "", true);

            // Act
            var results = await Service.Handle(examinationsDashboardQuery);

            // Assert
            results.Should().NotBeNull();
            Assert.Equal(10, results.Count());
        }

        [Fact]
        public virtual async Task EmptyQueryWithOrderByReturnsAllOpenCasesInOrder()
        {
            // Arrange
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(null,
                "", null, 1, 10, "", true);

            // Act
            var results = await Service.Handle(examinationsDashboardQuery);

            // Assert
            results.Should().NotBeNull();
            Assert.Equal(10, results.Count());
        }

        [Fact]
        public virtual async Task ClosedCasesQueryReturnsCorrectCount()
        {
            // Arrange
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(null,
                "", null, 1, 10, "", false);

            // Act
            var results = await Service.Handle(examinationsDashboardQuery);

            // Assert
            results.Should().NotBeNull();
            Assert.Single(results);
        }

        [Fact]
        public virtual async Task AdmissionNotesHaveBeenAddedQueryReturnsCorrectCount()
        {
            // Arrange
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(CaseStatus.AdmissionNotesHaveBeenAdded,
                "",null, 1, 10, "", true);
            
            //Act
            var results = await Service.Handle(examinationsDashboardQuery);

            //Assert
            results.Should().NotBeNull();
            Assert.Single(results);
        }

        [Fact]
        public virtual async Task HaveBeenScrutinisedByMEQueryReturnsCorrectCount()
        {
            // Arrange
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(CaseStatus.HaveBeenScrutinisedByME,
                "", null, 1, 10, "", true);

            // Act
            var results = await Service.Handle(examinationsDashboardQuery);

            // Assert
            results.Should().NotBeNull();
            Assert.Single(results);
        }

        [Fact]
        public virtual async Task PendingAdmissionNotesQueryReturnsCorrectCount()
        {
            //Arrange
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(CaseStatus.PendingAdmissionNotes,
                "", null, 1, 10, "", true);

            //Act
            var results = await Service.Handle(examinationsDashboardQuery);

            //Assert
            results.Should().NotBeNull();
            Assert.Single(results);
        }

        [Fact]
        public virtual async Task PendingDiscussionWithQAPQueryReturnsCorrectCount()
        {
            //Arrange
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(CaseStatus.PendingDiscussionWithQAP,
                "", null, 1, 10, "", true);

            //Act
            var results = await Service.Handle(examinationsDashboardQuery);

            //Assert
            results.Should().NotBeNull();
            Assert.Single(results);
        }

        [Fact]
        public virtual async Task PendingDiscussionWithRepresentativeQueryReturnsCorrectCount()
        {
            //Arrange
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(CaseStatus.PendingDiscussionWithRepresentative,
                "", null, 1, 10, "", true);
            
            //Act
            var results = await Service.Handle(examinationsDashboardQuery);

            //Assert
            results.Should().NotBeNull();
            Assert.Single(results);
        }

        [Fact]
        public virtual async Task HaveFinalCaseOutstandingOutcomesQueryReturnsCorrectCount()
        {
            //Arrange
            var examinationsDashboardQuery = new ExaminationsRetrievalQuery(CaseStatus.HaveFinalCaseOutstandingOutcomes,
                "", null, 1, 10, "", true);
            
            //Act
            var results = await Service.Handle(examinationsDashboardQuery);

            //Assert
            results.Should().NotBeNull();
            Assert.Single(results);
        }

        /// <inheritdoc/>
        protected override MedicalExaminer.Models.Examination[] GetExamples()
        {
            return new[] {
                examination1, examination2, examination3, examination4, examination5,
                           examination6, examination7, examination8, examination9, examination10,
                           examination11
            };
        }

        MedicalExaminer.Models.Examination examination1 = new MedicalExaminer.Models.Examination()
        {
            Unassigned = true,
            Completed = false,
            UrgencyScore = 1
        };

        MedicalExaminer.Models.Examination examination2 = new MedicalExaminer.Models.Examination()
        {
            ReadyForMEScrutiny = true,
            Completed = false,
            UrgencyScore = 2
        };

        MedicalExaminer.Models.Examination examination3 = new MedicalExaminer.Models.Examination()
        {
            MedicalExaminerOfficeResponsible = "a",
            ReadyForMEScrutiny = true,
            Completed = false,
            UrgencyScore = 3
        };

        MedicalExaminer.Models.Examination examination4 = new MedicalExaminer.Models.Examination()
        {
            Completed = true,
            UrgencyScore = 4
        };

        MedicalExaminer.Models.Examination examination5 = new MedicalExaminer.Models.Examination()
        {
            Completed = false,
            UrgencyScore = 3
        };

        MedicalExaminer.Models.Examination examination6 = new MedicalExaminer.Models.Examination()
        {
            Completed = false,
            AdmissionNotesHaveBeenAdded = true,
            UrgencyScore = 13
        };

        MedicalExaminer.Models.Examination examination7 = new MedicalExaminer.Models.Examination()
        {
            Completed = false,
            PendingDiscussionWithQAP = true,
            UrgencyScore = 11
        };

        MedicalExaminer.Models.Examination examination8 = new MedicalExaminer.Models.Examination()
        {
            Completed = false,
            PendingDiscussionWithRepresentative = true,
            UrgencyScore = 14
        };

        MedicalExaminer.Models.Examination examination9 = new MedicalExaminer.Models.Examination()
        {
            Completed = false,
            HaveFinalCaseOutstandingOutcomes = true,
            UrgencyScore = 100
        };

        MedicalExaminer.Models.Examination examination10 = new MedicalExaminer.Models.Examination()
        {
            Completed = false,
            HaveBeenScrutinisedByME = true,
            UrgencyScore = 0
        };

        MedicalExaminer.Models.Examination examination11 = new MedicalExaminer.Models.Examination()
        {
            Completed = false,
            PendingAdmissionNotes = true,
            UrgencyScore = 10000
        };
    }
}