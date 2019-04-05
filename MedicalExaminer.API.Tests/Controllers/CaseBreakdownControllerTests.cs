﻿using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using MedicalExaminer.API.Controllers;
using MedicalExaminer.API.Models.v1.CaseBreakdown;
using MedicalExaminer.Common.Loggers;
using MedicalExaminer.Common.Queries.CaseBreakdown;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace MedicalExaminer.API.Tests.Controllers
{
    public class CaseBreakdownControllerTests
    {
        [Fact]
        public async void GetOtherEvent_When_Called_With_Null_Returns_Bad_Request()
        {
            // Arrange
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();
            var usersRetrievalByEmailService = new Mock<IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser>>();

            var otherEventCreationService = new Mock<IAsyncQueryHandler<CreateEventQuery, string>>();
            var examinationRetrievalQueryService =
                new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();
            var sut = new CaseBreakdownController(logger.Object, mapper.Object, 
                otherEventCreationService.Object, examinationRetrievalQueryService.Object, usersRetrievalByEmailService.Object);
            // Act
            var response = await sut.GetCaseBreakdown(null);

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<GetCaseBreakdownResponse>>().Subject;
            var notFoundResult = taskResult.Result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
            notFoundResult.Value.Should().BeAssignableTo<GetCaseBreakdownResponse>();
        }

        [Fact]
        public async void GetOtherEvent_When_Called_With_Invalid_Examination_Id_Returns_Bad_Request()
        {
            // Arrange
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();
            var usersRetrievalByEmailService = new Mock<IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser>>();
            var otherEventCreationService = new Mock<IAsyncQueryHandler<CreateEventQuery, string>>();
            var examinationRetrievalQueryService =
                new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();

            var sut = new CaseBreakdownController(logger.Object, mapper.Object, 
                otherEventCreationService.Object, examinationRetrievalQueryService.Object, usersRetrievalByEmailService.Object);
            // Act
            var response = await sut.GetCaseBreakdown("aaaa");

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<GetCaseBreakdownResponse>>().Subject;
            var notFoundResult = taskResult.Result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
            notFoundResult.Value.Should().BeAssignableTo<GetCaseBreakdownResponse>();
        }

        [Fact]
        public async void GetOtherEvent_When_Called_With_Valid_Examination_Id_But_Examination_Not_Found_Returns_Not_Found_Response()
        {
            // Arrange
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();

            var examinationId = "7E5D50CE-05BF-4A1F-AA6E-25418A723A7F";
            var usersRetrievalByEmailService = new Mock<IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser>>();
            var otherEventCreationService = new Mock<IAsyncQueryHandler<CreateEventQuery, string>>();
            var examinationRetrievalQueryService =
                new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();

            examinationRetrievalQueryService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()))
                .Returns(Task.FromResult(default(Examination))).Verifiable();

            var sut = new CaseBreakdownController(logger.Object, mapper.Object,
               otherEventCreationService.Object, examinationRetrievalQueryService.Object, usersRetrievalByEmailService.Object);

            // Act
            var response = await sut.GetCaseBreakdown(examinationId);

            // Assert
            examinationRetrievalQueryService.Verify(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()));
            var taskResult = response.Should().BeOfType<ActionResult<GetCaseBreakdownResponse>>().Subject;
            var notFoundResult = taskResult.Result.Should().BeAssignableTo<NotFoundObjectResult>().Subject;
            notFoundResult.Value.Should().BeAssignableTo<GetCaseBreakdownResponse>();
        }

        [Fact]
        public async void GetOtherEvent_When_Called_With_Valid_Examination_Id_Examination_Found_Returns_All_Other_Events()
        {
            // Arrange
            var logger = new Mock<IMELogger>();
            var examinationObj = new Mock<Examination>().Object;
            var examinationId = "7E5D50CE-05BF-4A1F-AA6E-25418A723A7F";
            var getOtherResponse = new Mock<GetCaseBreakdownResponse>().Object;
            var usersRetrievalByEmailService = new Mock<IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser>>();
            var mapper = new Mock<IMapper>();
            mapper.Setup(m => m.Map<GetCaseBreakdownResponse>(examinationObj)).Returns(getOtherResponse);

            var otherEventCreationService = new Mock<IAsyncQueryHandler<CreateEventQuery, string>>();
            var examinationRetrievalQueryService =
                new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();


            examinationRetrievalQueryService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()))
                .Returns(Task.FromResult(examinationObj)).Verifiable();

            var sut = new CaseBreakdownController(logger.Object, mapper.Object,
               otherEventCreationService.Object, examinationRetrievalQueryService.Object, usersRetrievalByEmailService.Object);

            // Act
            var response = await sut.GetCaseBreakdown(examinationId);

            // Assert
            examinationRetrievalQueryService.Verify(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()));

            var taskResult = response.Should().BeOfType<ActionResult<GetCaseBreakdownResponse>>().Subject;
            var okResult = taskResult.Result.Should().BeAssignableTo<OkObjectResult>().Subject;
            okResult.Value.Should().BeAssignableTo<GetCaseBreakdownResponse>();
        }

        [Fact]
        public async void Put_Final_MedicalHistory_Event_Null_Request_Object_Returns_Invalid_Request()
        {
            // Arrange
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();
            var usersRetrievalByEmailService = new Mock<IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser>>();
            var medicalHistoryEventCreationService = new Mock<IAsyncQueryHandler<CreateEventQuery, string>>();
            var examinationRetrievalQueryService =
                new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();


            examinationRetrievalQueryService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()))
                .Returns(Task.FromResult(default(Examination))).Verifiable();

            var sut = new CaseBreakdownController(logger.Object, mapper.Object,
               medicalHistoryEventCreationService.Object, examinationRetrievalQueryService.Object, usersRetrievalByEmailService.Object);

            // Act
            var response = await sut.UpsertNewMedicalHistoryEvent("a", null);

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<PutCaseBreakdownEventResponse>>().Subject;
            var badRequestResult = taskResult.Result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
            badRequestResult.Value.Should().BeAssignableTo<PutCaseBreakdownEventResponse>();
        }

        [Fact]
        public async void Put_Final_MedicalHistory_Event_Invalid_Request_Object_Returns_Invalid_Request()
        {
            // Arrange
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();
            var usersRetrievalByEmailService = new Mock<IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser>>();
            var medicalHistoryEventCreationService = new Mock<IAsyncQueryHandler<CreateEventQuery, string>>();
            var examinationRetrievalQueryService =
                new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();

            var invalidRequest = new PutMedicalHistoryEventRequest
            {
                EventId = null,
                IsFinal = true,
                Text = null
            };

            examinationRetrievalQueryService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()))
                .Returns(Task.FromResult(default(Examination))).Verifiable();

            var sut = new CaseBreakdownController(logger.Object, mapper.Object,
               medicalHistoryEventCreationService.Object, examinationRetrievalQueryService.Object, usersRetrievalByEmailService.Object);

            sut.ModelState.AddModelError("i", "broke it");
            // Act
            var response = await sut.UpsertNewMedicalHistoryEvent("a", invalidRequest);

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<PutCaseBreakdownEventResponse>>().Subject;
            var badRequestResult = taskResult.Result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
            badRequestResult.Value.Should().BeAssignableTo<PutCaseBreakdownEventResponse>();
        }

        [Fact]
        public async void Put_Final_MedicalHistory_Event_Valid_Request_Object_Cannot_Find_Examination()
        {
            // Arrange
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();
            var usersRetrievalByEmailService = new Mock<IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser>>();
            var medicalHistoryEventCreationService = new Mock<IAsyncQueryHandler<CreateEventQuery, string>>();
            var examinationRetrievalQueryService =
                new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();

            var validRequest = new PutMedicalHistoryEventRequest
            {
                EventId = "1",
                IsFinal = true,
                Text = "Hello Planet"
            };

            examinationRetrievalQueryService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()))
                .Returns(Task.FromResult(default(Examination))).Verifiable();

            var sut = new CaseBreakdownController(logger.Object, mapper.Object,
               medicalHistoryEventCreationService.Object, examinationRetrievalQueryService.Object, usersRetrievalByEmailService.Object);

            // Act
            var response = await sut.UpsertNewMedicalHistoryEvent("a", validRequest);

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<PutCaseBreakdownEventResponse>>().Subject;
            var badRequestResult = taskResult.Result.Should().BeAssignableTo<NotFoundObjectResult>().Subject;
            badRequestResult.Value.Should().BeAssignableTo<PutCaseBreakdownEventResponse>();
        }

        [Fact]
        public async void Put_Final_MedicalHistory_Event_Valid_Request_Object_Finds_Examination_Then_Ok_Result()
        {
            // Arrange
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();
            var examination = new Mock<Examination>();
            var eventCreationService = new Mock<IAsyncQueryHandler<CreateEventQuery, string>>();
            var examinationRetrievalQueryService =
                new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();
            var usersRetrievalByEmailService = new Mock<IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser>>();
            var validRequest = new PutMedicalHistoryEventRequest
            {
                EventId = "1",
                IsFinal = true,
                Text = "Hello Planet"
            };
            eventCreationService.Setup(service => service.Handle(It.IsAny<CreateEventQuery>()))
                .Returns(Task.FromResult("hi mark")).Verifiable();

            examinationRetrievalQueryService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()))
                .Returns(Task.FromResult(examination.Object)).Verifiable();

            var sut = new CaseBreakdownController(logger.Object, mapper.Object,
               eventCreationService.Object, examinationRetrievalQueryService.Object, usersRetrievalByEmailService.Object);

            // Act
            var response = await sut.UpsertNewMedicalHistoryEvent("a", validRequest);

            eventCreationService.Verify(x => x.Handle(It.IsAny<CreateEventQuery>()), Times.Once);

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<PutCaseBreakdownEventResponse>>().Subject;
            var goodRequestResult = taskResult.Result.Should().BeAssignableTo<OkObjectResult>().Subject;
            goodRequestResult.Value.Should().BeAssignableTo<PutCaseBreakdownEventResponse>();
        }

        [Fact]
        public async void Put_Final_Other_Event_Null_Request_Object_Returns_Invalid_Request()
        {
            // Arrange
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();
            
            var otherEventCreationService = new Mock<IAsyncQueryHandler<CreateEventQuery, string>>();
            var examinationRetrievalQueryService =
                new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();
            var usersRetrievalByEmailService = new Mock<IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser>>();

            examinationRetrievalQueryService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()))
                .Returns(Task.FromResult(default(Examination))).Verifiable();

            var sut = new CaseBreakdownController(logger.Object, mapper.Object,
               otherEventCreationService.Object, examinationRetrievalQueryService.Object, usersRetrievalByEmailService.Object);

            // Act
            var response = await sut.UpsertNewOtherEvent("a", null);

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<PutCaseBreakdownEventResponse>>().Subject;
            var badRequestResult = taskResult.Result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
            badRequestResult.Value.Should().BeAssignableTo<PutCaseBreakdownEventResponse>();
        }

        [Fact]
        public async void Put_Final_Other_Event_Invalid_Request_Object_Returns_Invalid_Request()
        {
            // Arrange
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();

            var otherEventCreationService = new Mock<IAsyncQueryHandler<CreateEventQuery, string>>();
            var examinationRetrievalQueryService =
                new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();
            var usersRetrievalByEmailService = new Mock<IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser>>();
            var invalidRequest = new PutOtherEventRequest
            {
             EventId = null,
             IsFinal = true,
             Text = null
            };

            examinationRetrievalQueryService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()))
                .Returns(Task.FromResult(default(Examination))).Verifiable();

            var sut = new CaseBreakdownController(logger.Object, mapper.Object,
               otherEventCreationService.Object, examinationRetrievalQueryService.Object, usersRetrievalByEmailService.Object);

            sut.ModelState.AddModelError("i", "broke it");
            // Act
            var response = await sut.UpsertNewOtherEvent("a", invalidRequest);

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<PutCaseBreakdownEventResponse>>().Subject;
            var badRequestResult = taskResult.Result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
            badRequestResult.Value.Should().BeAssignableTo<PutCaseBreakdownEventResponse>();
        }

        [Fact]
        public async void Put_Final_Other_Event_Valid_Request_Object_Cannot_Find_Examination()
        {
            // Arrange
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();
            var usersRetrievalByEmailService = new Mock<IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser>>();
            var otherEventCreationService = new Mock<IAsyncQueryHandler<CreateEventQuery, string>>();
            var examinationRetrievalQueryService =
                new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();

            var validRequest = new PutOtherEventRequest
            {
                EventId = "1",
                IsFinal = true,
                Text = "Hello Planet"
            };

            examinationRetrievalQueryService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()))
                .Returns(Task.FromResult(default(Examination))).Verifiable();

            var sut = new CaseBreakdownController(logger.Object, mapper.Object,
               otherEventCreationService.Object, examinationRetrievalQueryService.Object, usersRetrievalByEmailService.Object);

            // Act
            var response = await sut.UpsertNewOtherEvent("a", validRequest);

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<PutCaseBreakdownEventResponse>>().Subject;
            var badRequestResult = taskResult.Result.Should().BeAssignableTo<NotFoundObjectResult>().Subject;
            badRequestResult.Value.Should().BeAssignableTo<PutCaseBreakdownEventResponse>();
        }

        [Fact]
        public async void Put_Final_Other_Event_Valid_Request_Object_Finds_Examination_Then_Ok_Result()
        {
            // Arrange
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();
            var examination = new Mock<Examination>();
            var eventCreationService = new Mock<IAsyncQueryHandler<CreateEventQuery, string>>();
            var examinationRetrievalQueryService =
                new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();
            var usersRetrievalByEmailService = new Mock<IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser>>();
            var validRequest = new PutOtherEventRequest
            {
                EventId = "1",
                IsFinal = true,
                Text = "Hello Planet"
            };
            eventCreationService.Setup(service => service.Handle(It.IsAny<CreateEventQuery>()))
                .Returns(Task.FromResult("hi mark")).Verifiable();

            examinationRetrievalQueryService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()))
                .Returns(Task.FromResult(examination.Object)).Verifiable();

            var sut = new CaseBreakdownController(logger.Object, mapper.Object,
               eventCreationService.Object, examinationRetrievalQueryService.Object, usersRetrievalByEmailService.Object);

            // Act
            var response = await sut.UpsertNewOtherEvent("a", validRequest);

            eventCreationService.Verify(x => x.Handle(It.IsAny<CreateEventQuery>()), Times.Once);

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<PutCaseBreakdownEventResponse>>().Subject;
            var goodRequestResult = taskResult.Result.Should().BeAssignableTo<OkObjectResult>().Subject;
            goodRequestResult.Value.Should().BeAssignableTo<PutCaseBreakdownEventResponse>();
        }

        [Fact]
        public async void Put_Final_Admission_Event_Null_Request_Object_Returns_Invalid_Request()
        {
            // Arrange
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();
            var usersRetrievalByEmailService = new Mock<IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser>>();
            var admissionEventCreationService = new Mock<IAsyncQueryHandler<CreateEventQuery, string>>();
            var examinationRetrievalQueryService =
                new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();


            examinationRetrievalQueryService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()))
                .Returns(Task.FromResult(default(Examination))).Verifiable();

            var sut = new CaseBreakdownController(logger.Object, mapper.Object,
               admissionEventCreationService.Object, examinationRetrievalQueryService.Object, usersRetrievalByEmailService.Object);

            // Act
            var response = await sut.UpsertNewAdmissionEvent("a", null);

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<PutCaseBreakdownEventResponse>>().Subject;
            var badRequestResult = taskResult.Result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
            badRequestResult.Value.Should().BeAssignableTo<PutCaseBreakdownEventResponse>();
        }

        [Fact]
        public async void Put_Final_Admission_Event_Invalid_Request_Object_Returns_Invalid_Request()
        {
            // Arrange
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();
            var usersRetrievalByEmailService = new Mock<IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser>>();
            var admissionEventCreationService = new Mock<IAsyncQueryHandler<CreateEventQuery, string>>();
            var examinationRetrievalQueryService =
                new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();

            var invalidRequest = new PutAdmissionEventRequest
            {
                EventId = null,
                IsFinal = true,
                Text = null
            };

            examinationRetrievalQueryService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()))
                .Returns(Task.FromResult(default(Examination))).Verifiable();

            var sut = new CaseBreakdownController(logger.Object, mapper.Object,
               admissionEventCreationService.Object, examinationRetrievalQueryService.Object, usersRetrievalByEmailService.Object);

            sut.ModelState.AddModelError("i", "broke it");
            // Act
            var response = await sut.UpsertNewAdmissionEvent("a", invalidRequest);

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<PutCaseBreakdownEventResponse>>().Subject;
            var badRequestResult = taskResult.Result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
            badRequestResult.Value.Should().BeAssignableTo<PutCaseBreakdownEventResponse>();
        }

        [Fact]
        public async void Put_Final_Admission_Event_Valid_Request_Object_Cannot_Find_Examination()
        {
            // Arrange
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();
            var usersRetrievalByEmailService = new Mock<IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser>>();
            var admissionEventCreationService = new Mock<IAsyncQueryHandler<CreateEventQuery, string>>();
            var examinationRetrievalQueryService =
                new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();

            var validRequest = new PutAdmissionEventRequest
            {
                EventId = "1",
                IsFinal = true,
                Text = "Hello Planet"
            };

            examinationRetrievalQueryService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()))
                .Returns(Task.FromResult(default(Examination))).Verifiable();

            var sut = new CaseBreakdownController(logger.Object, mapper.Object,
               admissionEventCreationService.Object, examinationRetrievalQueryService.Object, usersRetrievalByEmailService.Object);

            // Act
            var response = await sut.UpsertNewAdmissionEvent("a", validRequest);

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<PutCaseBreakdownEventResponse>>().Subject;
            var badRequestResult = taskResult.Result.Should().BeAssignableTo<NotFoundObjectResult>().Subject;
            badRequestResult.Value.Should().BeAssignableTo<PutCaseBreakdownEventResponse>();
        }

        [Fact]
        public async void Put_Final_Admission_Event_Valid_Request_Object_Finds_Examination_Then_Ok_Result()
        {
            // Arrange
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();
            var examination = new Mock<Examination>();
            var eventCreationService = new Mock<IAsyncQueryHandler<CreateEventQuery, string>>();
            var examinationRetrievalQueryService =
                new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();
            var usersRetrievalByEmailService = new Mock<IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser>>();
            var validRequest = new PutAdmissionEventRequest
            {
                EventId = "1",
                IsFinal = true,
                Text = "Hello Planet"
            };
            eventCreationService.Setup(service => service.Handle(It.IsAny<CreateEventQuery>()))
                .Returns(Task.FromResult("hi mark")).Verifiable();

            examinationRetrievalQueryService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()))
                .Returns(Task.FromResult(examination.Object)).Verifiable();

            var sut = new CaseBreakdownController(logger.Object, mapper.Object,
               eventCreationService.Object, examinationRetrievalQueryService.Object, usersRetrievalByEmailService.Object);

            // Act
            var response = await sut.UpsertNewAdmissionEvent("a", validRequest);

            eventCreationService.Verify(x => x.Handle(It.IsAny<CreateEventQuery>()), Times.Once);

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<PutCaseBreakdownEventResponse>>().Subject;
            var goodRequestResult = taskResult.Result.Should().BeAssignableTo<OkObjectResult>().Subject;
            goodRequestResult.Value.Should().BeAssignableTo<PutCaseBreakdownEventResponse>();
        }

        [Fact]
        public async void Put_Final_PreScrutiny_Event_Null_Request_Object_Returns_Invalid_Request()
        {
            // Arrange
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();
            var examinationRetrievalQueryService = new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();
            var eventCreationService = new Mock<IAsyncQueryHandler<CreateEventQuery, string>>();
            examinationRetrievalQueryService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()))
                .Returns(Task.FromResult(default(Examination))).Verifiable();
            var usersRetrievalByEmailService = new Mock<IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser>>();
            var sut = new CaseBreakdownController(logger.Object, mapper.Object, eventCreationService.Object, 
                examinationRetrievalQueryService.Object, usersRetrievalByEmailService.Object);

            // Act
            var response = await sut.UpsertNewPreScrutinyEvent("a", null);

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<PutCaseBreakdownEventResponse>>().Subject;
            var badRequestResult = taskResult.Result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
            badRequestResult.Value.Should().BeAssignableTo<PutCaseBreakdownEventResponse>();
        }

        [Fact]
        public async void Put_Final_PreScrutiny_Event_Invalid_Request_Object_Returns_Invalid_Request()
        {
            // Arrange
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();
            var eventCreationService = new Mock<IAsyncQueryHandler<CreateEventQuery, string>>();
            var examinationRetrievalQueryService = new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();
            var usersRetrievalByEmailService = new Mock<IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser>>();
            var invalidRequest = new PutPreScrutinyEventRequest
            {
                EventId = null,
                IsFinal = true,
                MedicalExaminerThoughts = null
            };

            examinationRetrievalQueryService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()))
                .Returns(Task.FromResult(default(Examination))).Verifiable();

            var sut = new CaseBreakdownController(logger.Object, mapper.Object, eventCreationService.Object, 
                examinationRetrievalQueryService.Object, usersRetrievalByEmailService.Object);

            sut.ModelState.AddModelError("i", "broke it");
            // Act
            var response = await sut.UpsertNewPreScrutinyEvent("a", invalidRequest);

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<PutCaseBreakdownEventResponse>>().Subject;
            var badRequestResult = taskResult.Result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
            badRequestResult.Value.Should().BeAssignableTo<PutCaseBreakdownEventResponse>();
        }

        [Fact]
        public async void Put_Final_PreScrutiny_Event_Valid_Request_Object_Cannot_Find_Examination()
        {
            // Arrange
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();
            var eventCreationService = new Mock<IAsyncQueryHandler<CreateEventQuery, string>>();
            var examinationRetrievalQueryService =
                new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();
            var usersRetrievalByEmailService = new Mock<IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser>>();
            var validRequest = new PutPreScrutinyEventRequest
            {
                EventId = "1",
                IsFinal = true,
                MedicalExaminerThoughts = "Hello Planet"
            };

            examinationRetrievalQueryService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()))
                .Returns(Task.FromResult(default(Examination))).Verifiable();

            var sut = new CaseBreakdownController(logger.Object, mapper.Object, eventCreationService.Object, 
                examinationRetrievalQueryService.Object, usersRetrievalByEmailService.Object);

            // Act
            var response = await sut.UpsertNewPreScrutinyEvent("a", validRequest);

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<PutCaseBreakdownEventResponse>>().Subject;
            var badRequestResult = taskResult.Result.Should().BeAssignableTo<NotFoundObjectResult>().Subject;
            badRequestResult.Value.Should().BeAssignableTo<PutCaseBreakdownEventResponse>();
        }

        [Fact]
        public async void Put_Final_PreScrutiny_Event_Valid_Request_Object_Finds_Examination_Then_Ok_Result()
        {
            // Arrange
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();
            var examination = new Mock<Examination>();
            var eventCreationService = new Mock<IAsyncQueryHandler<CreateEventQuery, string>>();
            var examinationRetrievalQueryService =
                new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();
            var usersRetrievalByEmailService = new Mock<IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser>>();
            var validRequest = new PutPreScrutinyEventRequest
            {
                EventId = "1",
                IsFinal = true,
                MedicalExaminerThoughts = "Hello Planet"
            };
            eventCreationService.Setup(service => service.Handle(It.IsAny<CreateEventQuery>()))
                .Returns(Task.FromResult("hi mark")).Verifiable();

            examinationRetrievalQueryService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()))
                .Returns(Task.FromResult(examination.Object)).Verifiable();

            var sut = new CaseBreakdownController(logger.Object, mapper.Object, eventCreationService.Object, 
                examinationRetrievalQueryService.Object, usersRetrievalByEmailService.Object);

            // Act
            var response = await sut.UpsertNewPreScrutinyEvent("a", validRequest);

            eventCreationService.Verify(x => x.Handle(It.IsAny<CreateEventQuery>()), Times.Once);

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<PutCaseBreakdownEventResponse>>().Subject;
            var goodRequestResult = taskResult.Result.Should().BeAssignableTo<OkObjectResult>().Subject;
            goodRequestResult.Value.Should().BeAssignableTo<PutCaseBreakdownEventResponse>();
        }

        [Fact]
        public async void Put_Final_BereavedDiscussion_Event_Null_Request_Object_Returns_Invalid_Request()
        {
            // Arrange
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();
            var usersRetrievalByEmailService = new Mock<IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser>>();
            var eventCreationService = new Mock<IAsyncQueryHandler<CreateEventQuery, string>>();
            var examinationRetrievalQueryService =
                new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();


            examinationRetrievalQueryService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()))
                .Returns(Task.FromResult(default(Examination))).Verifiable();

            var sut = new CaseBreakdownController(logger.Object, mapper.Object,
               eventCreationService.Object, examinationRetrievalQueryService.Object, usersRetrievalByEmailService.Object);

            // Act
            var response = await sut.UpsertNewBereavedDiscussionEvent("a", null);

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<PutCaseBreakdownEventResponse>>().Subject;
            var badRequestResult = taskResult.Result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
            badRequestResult.Value.Should().BeAssignableTo<PutCaseBreakdownEventResponse>();
        }

        [Fact]
        public async void Put_Final_BereavedDiscussion_Event_Invalid_Request_Object_Returns_Invalid_Request()
        {
            // Arrange
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();
            var usersRetrievalByEmailService = new Mock<IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser>>();
            var eventCreationService = new Mock<IAsyncQueryHandler<CreateEventQuery, string>>();
            var examinationRetrievalQueryService =
                new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();

            var invalidRequest = new PutBereavedDiscussionEventRequest
            {
                EventId = null,
                IsFinal = true,
                DiscussionDetails = null
            };

            examinationRetrievalQueryService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()))
                .Returns(Task.FromResult(default(Examination))).Verifiable();

            var sut = new CaseBreakdownController(logger.Object, mapper.Object,
               eventCreationService.Object, examinationRetrievalQueryService.Object, usersRetrievalByEmailService.Object);

            sut.ModelState.AddModelError("i", "broke it");
            // Act
            var response = await sut.UpsertNewBereavedDiscussionEvent("a", invalidRequest);

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<PutCaseBreakdownEventResponse>>().Subject;
            var badRequestResult = taskResult.Result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
            badRequestResult.Value.Should().BeAssignableTo<PutCaseBreakdownEventResponse>();
        }

        [Fact]
        public async void Put_Final_BereavedDiscussion_Event_Valid_Request_Object_Cannot_Find_Examination()
        {
            // Arrange
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();
            var usersRetrievalByEmailService = new Mock<IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser>>();
            var eventCreationService = new Mock<IAsyncQueryHandler<CreateEventQuery, string>>();
            var examinationRetrievalQueryService =
                new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();

            var validRequest = new PutBereavedDiscussionEventRequest
            {
                EventId = "1",
                IsFinal = true,
                DiscussionDetails = "Hello Planet"
            };

            examinationRetrievalQueryService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()))
                .Returns(Task.FromResult(default(Examination))).Verifiable();

            var sut = new CaseBreakdownController(logger.Object, mapper.Object,
               eventCreationService.Object, examinationRetrievalQueryService.Object, usersRetrievalByEmailService.Object);

            // Act
            var response = await sut.UpsertNewBereavedDiscussionEvent("a", validRequest);

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<PutCaseBreakdownEventResponse>>().Subject;
            var badRequestResult = taskResult.Result.Should().BeAssignableTo<NotFoundObjectResult>().Subject;
            badRequestResult.Value.Should().BeAssignableTo<PutCaseBreakdownEventResponse>();
        }

        [Fact]
        public async void Put_Final_BereavedDiscussion_Event_Valid_Request_Object_Finds_Examination_Then_Ok_Result()
        {
            // Arrange
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();
            var examination = new Mock<Examination>();
            var eventCreationService = new Mock<IAsyncQueryHandler<CreateEventQuery, string>>();
            var examinationRetrievalQueryService =
                new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();
            var usersRetrievalByEmailService = new Mock<IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser>>();
            var validRequest = new PutBereavedDiscussionEventRequest
            {
                EventId = "1",
                IsFinal = false,
                DiscussionDetails = "Hello Planet"
            };
            eventCreationService.Setup(service => service.Handle(It.IsAny<CreateEventQuery>()))
                .Returns(Task.FromResult("hi mark")).Verifiable();

            examinationRetrievalQueryService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()))
                .Returns(Task.FromResult(examination.Object)).Verifiable();

            var sut = new CaseBreakdownController(logger.Object, mapper.Object,
               eventCreationService.Object, examinationRetrievalQueryService.Object, usersRetrievalByEmailService.Object);

            // Act
            var response = await sut.UpsertNewBereavedDiscussionEvent("a", validRequest);

            eventCreationService.Verify(x => x.Handle(It.IsAny<CreateEventQuery>()), Times.Once);

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<PutCaseBreakdownEventResponse>>().Subject;
            var goodRequestResult = taskResult.Result.Should().BeAssignableTo<OkObjectResult>().Subject;
            goodRequestResult.Value.Should().BeAssignableTo<PutCaseBreakdownEventResponse>();
        }

        [Fact]
        public async void Put_Final_QapDiscussion_Event_Null_Request_Object_Returns_Invalid_Request()
        {
            // Arrange
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();
            var usersRetrievalByEmailService = new Mock<IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser>>();
            var qapDiscussionEventCreationService =
                new Mock<IAsyncQueryHandler<CreateEventQuery, string>>();
            var examinationRetrievalQueryService =
                new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();


            examinationRetrievalQueryService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()))
                .Returns(Task.FromResult(default(Examination))).Verifiable();

            var sut = new CaseBreakdownController(logger.Object, mapper.Object,
                qapDiscussionEventCreationService.Object, examinationRetrievalQueryService.Object, usersRetrievalByEmailService.Object);

            // Act
            var response = await sut.UpsertNewQapDiscussionEvent("a", null);

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<PutCaseBreakdownEventResponse>>().Subject;
            var badRequestResult = taskResult.Result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
            badRequestResult.Value.Should().BeAssignableTo<PutCaseBreakdownEventResponse>();
        }

        [Fact]
        public async void Put_Final_QapDiscussion_Event_Invalid_Request_Object_Returns_Invalid_Request()
        {
            // Arrange
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();
            var usersRetrievalByEmailService = new Mock<IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser>>();
            var qapDiscussionEventCreationService =
                new Mock<IAsyncQueryHandler<CreateEventQuery, string>>();
            var examinationRetrievalQueryService =
                new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();

            var invalidRequest = new PutQapDiscussionEventRequest
            {
                EventId = null,
                IsFinal = false,
                DiscussionDetails = null
            };

            examinationRetrievalQueryService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()))
                .Returns(Task.FromResult(default(Examination))).Verifiable();

            var sut = new CaseBreakdownController(logger.Object, mapper.Object,
                qapDiscussionEventCreationService.Object, examinationRetrievalQueryService.Object, usersRetrievalByEmailService.Object);

            sut.ModelState.AddModelError("i", "broke it");
            // Act
            var response = await sut.UpsertNewQapDiscussionEvent("a", invalidRequest);

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<PutCaseBreakdownEventResponse>>().Subject;
            var badRequestResult = taskResult.Result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
            badRequestResult.Value.Should().BeAssignableTo<PutCaseBreakdownEventResponse>();
        }

        [Fact]
        public async void Put_Final_QapDiscussion_Event_Valid_Request_Object_Cannot_Find_Examination()
        {
            // Arrange
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();
            var usersRetrievalByEmailService = new Mock<IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser>>();
            var qapDiscussionEventCreationService =
                new Mock<IAsyncQueryHandler<CreateEventQuery, string>>();
            var examinationRetrievalQueryService =
                new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();

            var validRequest = new PutQapDiscussionEventRequest
            {
                EventId = "1",
                IsFinal = false,
                DiscussionDetails = "Hello Planet"
            };

            examinationRetrievalQueryService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()))
                .Returns(Task.FromResult(default(Examination))).Verifiable();

            var sut = new CaseBreakdownController(logger.Object, mapper.Object,
                qapDiscussionEventCreationService.Object, examinationRetrievalQueryService.Object, usersRetrievalByEmailService.Object);

            // Act
            var response = await sut.UpsertNewQapDiscussionEvent("a", validRequest);

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<PutCaseBreakdownEventResponse>>().Subject;
            var badRequestResult = taskResult.Result.Should().BeAssignableTo<NotFoundObjectResult>().Subject;
            badRequestResult.Value.Should().BeAssignableTo<PutCaseBreakdownEventResponse>();
        }

        [Fact]
        public async void Put_Final_QapDiscussion_Event_Valid_Request_Object_Finds_Examination_Then_Ok_Result()
        {
            // Arrange
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();
            var examination = new Mock<Examination>();
            var qapDiscussionEventCreationService =
                new Mock<IAsyncQueryHandler<CreateEventQuery, string>>();
            var examinationRetrievalQueryService =
                new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();
            var usersRetrievalByEmailService = new Mock<IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser>>();
            var validRequest = new PutQapDiscussionEventRequest
            {
                EventId = "1",
                IsFinal = false,
                DiscussionDetails = "Hello Planet"
            };
            qapDiscussionEventCreationService
                .Setup(service => service.Handle(It.IsAny<CreateEventQuery>()))
                .Returns(Task.FromResult("hi mark")).Verifiable();

            examinationRetrievalQueryService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()))
                .Returns(Task.FromResult(examination.Object)).Verifiable();

            var sut = new CaseBreakdownController(logger.Object, mapper.Object,
                qapDiscussionEventCreationService.Object, examinationRetrievalQueryService.Object, usersRetrievalByEmailService.Object);

            // Act
            var response = await sut.UpsertNewQapDiscussionEvent("a", validRequest);

            qapDiscussionEventCreationService.Verify(x => x.Handle(It.IsAny<CreateEventQuery>()),
                Times.Once);

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<PutCaseBreakdownEventResponse>>().Subject;
            var goodRequestResult = taskResult.Result.Should().BeAssignableTo<OkObjectResult>().Subject;
            goodRequestResult.Value.Should().BeAssignableTo<PutCaseBreakdownEventResponse>();
        }

        [Fact]
        public async void Put_Final_MeoSummary_Event_Null_Request_Object_Returns_Invalid_Request()
        {
            // Arrange
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();
            var usersRetrievalByEmailService = new Mock<IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser>>();
            var meoSummaryEventCreationService = new Mock<IAsyncQueryHandler<CreateEventQuery, string>>();
            var examinationRetrievalQueryService = new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();


            examinationRetrievalQueryService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()))
               .Returns(Task.FromResult(default(Examination))).Verifiable();

            var sut = new CaseBreakdownController(logger.Object, mapper.Object,
               meoSummaryEventCreationService.Object, examinationRetrievalQueryService.Object, usersRetrievalByEmailService.Object);

            // Act
            var response = await sut.UpsertNewMeoSummaryEvent("a", null);

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<PutCaseBreakdownEventResponse>>().Subject;
            var badRequestResult = taskResult.Result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
            badRequestResult.Value.Should().BeAssignableTo<PutCaseBreakdownEventResponse>();
        }

        [Fact]
        public async void Put_Final_MeoSummary_Event_Invalid_Request_Object_Returns_Invalid_Request()
        {
            // Arrange
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();
            var usersRetrievalByEmailService = new Mock<IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser>>();
            var meoSummaryEventCreationService = new Mock<IAsyncQueryHandler<CreateEventQuery, string>>();
            var examinationRetrievalQueryService = new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();

            var invalidRequest = new PutMeoSummaryEventRequest
            {
                EventId = null,
                IsFinal = true,
                SummaryDetails = null
            };

            examinationRetrievalQueryService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()))
               .Returns(Task.FromResult(default(Examination))).Verifiable();

            var sut = new CaseBreakdownController(logger.Object, mapper.Object,
               meoSummaryEventCreationService.Object, examinationRetrievalQueryService.Object, usersRetrievalByEmailService.Object);

            sut.ModelState.AddModelError("i", "broke it");
            // Act
            var response = await sut.UpsertNewMeoSummaryEvent("a", invalidRequest);

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<PutCaseBreakdownEventResponse>>().Subject;
            var badRequestResult = taskResult.Result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
            badRequestResult.Value.Should().BeAssignableTo<PutCaseBreakdownEventResponse>();
        }

        [Fact]
        public async void Put_Final_MeoSummary_Event_Valid_Request_Object_Cannot_Find_Examination()
        {
            // Arrange
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();
            var usersRetrievalByEmailService = new Mock<IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser>>();
            var meoSummaryEventCreationService = new Mock<IAsyncQueryHandler<CreateEventQuery, string>>();
            var examinationRetrievalQueryService = new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();

            var validRequest = new PutMeoSummaryEventRequest
            {
                EventId = "1",
                IsFinal = true,
                SummaryDetails = "Hello Planet"
            };

            examinationRetrievalQueryService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()))
               .Returns(Task.FromResult(default(Examination))).Verifiable();

            var sut = new CaseBreakdownController(logger.Object, mapper.Object,
               meoSummaryEventCreationService.Object, examinationRetrievalQueryService.Object, usersRetrievalByEmailService.Object);

            // Act
            var response = await sut.UpsertNewMeoSummaryEvent("a", validRequest);

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<PutCaseBreakdownEventResponse>>().Subject;
            var badRequestResult = taskResult.Result.Should().BeAssignableTo<NotFoundObjectResult>().Subject;
            badRequestResult.Value.Should().BeAssignableTo<PutCaseBreakdownEventResponse>();
        }

        [Fact]
        public async void Put_Final_MeoSummary_Event_Valid_Request_Object_Finds_Examination_Then_Ok_Result()
        {
            // Arrange
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();
            var examination = new Mock<Examination>();
            var usersRetrievalByEmailService = new Mock<IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser>>();
            var meoSummaryEventCreationService = new Mock<IAsyncQueryHandler<CreateEventQuery, string>>();
            var examinationRetrievalQueryService = new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();

            var validRequest = new PutMeoSummaryEventRequest
            {
                EventId = "1",
                IsFinal = true,
                SummaryDetails = "Hello Planet"
            };
            meoSummaryEventCreationService
                .Setup(service => service.Handle(It.IsAny<CreateEventQuery>()))
                .Returns(Task.FromResult("hi mark")).Verifiable();

            examinationRetrievalQueryService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()))
               .Returns(Task.FromResult(examination.Object)).Verifiable();

            var sut = new CaseBreakdownController(logger.Object, mapper.Object,
               meoSummaryEventCreationService.Object, examinationRetrievalQueryService.Object, usersRetrievalByEmailService.Object);

            // Act
            var response = await sut.UpsertNewMeoSummaryEvent("a", validRequest);

            meoSummaryEventCreationService.Verify(x => x.Handle(It.IsAny<CreateEventQuery>()),
               Times.Once);

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<PutCaseBreakdownEventResponse>>().Subject;
            var goodRequestResult = taskResult.Result.Should().BeAssignableTo<OkObjectResult>().Subject;
            goodRequestResult.Value.Should().BeAssignableTo<PutCaseBreakdownEventResponse>();
        }
    }
}
