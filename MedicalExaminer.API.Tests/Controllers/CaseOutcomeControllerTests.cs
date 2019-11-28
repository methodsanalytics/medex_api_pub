﻿using System;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using MedicalExaminer.API.Controllers;
using MedicalExaminer.API.Models.v1.CaseOutcome;
using MedicalExaminer.Common.Queries.CaseOutcome;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Common.Queries.MELogger;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace MedicalExaminer.API.Tests.Controllers
{
    public class CaseOutcomeControllerTests : AuthorizedControllerTestsBase<CaseOutcomeController>
    {
        [Fact]
        public async void PutConfirmationOfScrutiny_When_Called_With_Id_Not_Found_Returns_NotFound()
        {
            // Arrange
            var logger = new Mock<IAsyncQueryHandler<CreateMELoggerQuery, LogMessageActionDefault>>();
            var mapper = new Mock<IMapper>();
            var usersRetrievalByOktaIdService = new Mock<IAsyncQueryHandler<UserRetrievalByOktaIdQuery, MeUser>>();
            var confirmationOfScrutinyService = new Mock<IAsyncQueryHandler<ConfirmationOfScrutinyQuery, Examination>>();
            var closeCaseService = new Mock<IAsyncQueryHandler<CloseCaseQuery, string>>();
            var coronerReferralService = new Mock<IAsyncQueryHandler<CoronerReferralQuery, string>>();
            var examinationRetrievalService = new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();
            var saveOutstandingCaseItems = new Mock<IAsyncQueryHandler<SaveOutstandingCaseItemsQuery, string>>();
            var voidCaseService = new Mock<IAsyncQueryHandler<VoidCaseQuery, Examination>>();

            var sut = new CaseOutcomeController(
                logger.Object,
                mapper.Object,
                coronerReferralService.Object,
                closeCaseService.Object,
                examinationRetrievalService.Object,
                saveOutstandingCaseItems.Object,
                confirmationOfScrutinyService.Object,
                usersRetrievalByOktaIdService.Object,
                voidCaseService.Object,
                AuthorizationServiceMock.Object,
                PermissionServiceMock.Object);

            sut.ControllerContext = GetControllerContext();

            // Act
            var response = await sut.PutConfirmationOfScrutiny(string.Empty);

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<PutConfirmationOfScrutinyResponse>>().Subject;
            var badRequestResult = taskResult.Result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
            badRequestResult.Value.Should().BeAssignableTo<PutConfirmationOfScrutinyResponse>();
        }

        [Fact]
        public async void PutConfirmationOfScrutiny_When_Called_With_Null_Id_Returns_BadRequest()
        {
            // Arrange
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();
            var usersRetrievalByOktaIdService = new Mock<IAsyncQueryHandler<UserRetrievalByOktaIdQuery, MeUser>>();
            var confirmationOfScrutinyService = new Mock<IAsyncQueryHandler<ConfirmationOfScrutinyQuery, Examination>>();
            var closeCaseService = new Mock<IAsyncQueryHandler<CloseCaseQuery, string>>();
            var coronerReferralService = new Mock<IAsyncQueryHandler<CoronerReferralQuery, string>>();
            var examinationRetrievalService = new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();
            var saveOutstandingCaseItems = new Mock<IAsyncQueryHandler<SaveOutstandingCaseItemsQuery, string>>();
            var voidCaseService = new Mock<IAsyncQueryHandler<VoidCaseQuery, Examination>>();

            var sut = new CaseOutcomeController(
                logger.Object,
                mapper.Object,
                coronerReferralService.Object,
                closeCaseService.Object,
                examinationRetrievalService.Object,
                saveOutstandingCaseItems.Object,
                confirmationOfScrutinyService.Object,
                usersRetrievalByOktaIdService.Object,
                voidCaseService.Object,
                AuthorizationServiceMock.Object,
                PermissionServiceMock.Object);

            sut.ControllerContext = GetControllerContext();

            // Act
            var response = await sut.PutConfirmationOfScrutiny(null);

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<PutConfirmationOfScrutinyResponse>>().Subject;
            var badRequestResult = taskResult.Result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
            badRequestResult.Value.Should().BeAssignableTo<PutConfirmationOfScrutinyResponse>();
        }

        [Fact]
        public async void PutConfirmationOfScrutiny_When_Called_With_User_Without_Permission_Returns_Forbidden()
        {
            // Arrange
            SetupAuthorize(AuthorizationResult.Failed());
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();
            var usersRetrievalByOktaIdService = new Mock<IAsyncQueryHandler<UserRetrievalByOktaIdQuery, MeUser>>();
            var confirmationOfScrutinyService = new Mock<IAsyncQueryHandler<ConfirmationOfScrutinyQuery, Examination>>();
            var closeCaseService = new Mock<IAsyncQueryHandler<CloseCaseQuery, string>>();
            var coronerReferralService = new Mock<IAsyncQueryHandler<CoronerReferralQuery, string>>();
            var examinationRetrievalService = new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();
            var saveOutstandingCaseItems = new Mock<IAsyncQueryHandler<SaveOutstandingCaseItemsQuery, string>>();
            var voidCaseService = new Mock<IAsyncQueryHandler<VoidCaseQuery, Examination>>();

            var sut = new CaseOutcomeController(
                logger.Object,
                mapper.Object,
                coronerReferralService.Object,
                closeCaseService.Object,
                examinationRetrievalService.Object,
                saveOutstandingCaseItems.Object,
                confirmationOfScrutinyService.Object,
                usersRetrievalByOktaIdService.Object,
                voidCaseService.Object,
                AuthorizationServiceMock.Object,
                PermissionServiceMock.Object);

            sut.ControllerContext = GetControllerContext();

            // Act
            var response = await sut.PutConfirmationOfScrutiny("ExaminationId");

            // Assert
            response.Result.Should().BeAssignableTo<ForbidResult>();
        }

        [Fact]
        public async void PutConfirmationOfScrutiny_When_Called_With_User_Who_Is_Not_The_ME_Assigned_Returns_BadRequest()
        {
            // Arrange
            var examination = new Examination
            {
                ExaminationId = Guid.NewGuid().ToString(),
                MedicalTeam = new MedicalTeam
                {
                    MedicalExaminerUserId = "MedicalExaminerUserId"
                }
            };
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();
            var mockMeUser = new Mock<MeUser>();
            mockMeUser.Object.UserId = "UserId";
            var usersRetrievalByOktaIdService = new Mock<IAsyncQueryHandler<UserRetrievalByOktaIdQuery, MeUser>>();
            usersRetrievalByOktaIdService.Setup(service => service.Handle(It.IsAny<UserRetrievalByOktaIdQuery>())).Returns(Task.FromResult(mockMeUser.Object));
            var confirmationOfScrutinyService = new Mock<IAsyncQueryHandler<ConfirmationOfScrutinyQuery, Examination>>();
            var closeCaseService = new Mock<IAsyncQueryHandler<CloseCaseQuery, string>>();
            var coronerReferralService = new Mock<IAsyncQueryHandler<CoronerReferralQuery, string>>();
            var examinationRetrievalService = new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();
            examinationRetrievalService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>())).Returns(Task.FromResult(examination));
            var saveOutstandingCaseItems = new Mock<IAsyncQueryHandler<SaveOutstandingCaseItemsQuery, string>>();
            var voidCaseService = new Mock<IAsyncQueryHandler<VoidCaseQuery, Examination>>();

            var sut = new CaseOutcomeController(
                logger.Object,
                mapper.Object,
                coronerReferralService.Object,
                closeCaseService.Object,
                examinationRetrievalService.Object,
                saveOutstandingCaseItems.Object,
                confirmationOfScrutinyService.Object,
                usersRetrievalByOktaIdService.Object,
                voidCaseService.Object,
                AuthorizationServiceMock.Object,
                PermissionServiceMock.Object);

            sut.ControllerContext = GetControllerContext();

            // Act
            var response = await sut.PutConfirmationOfScrutiny(examination.ExaminationId);

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<PutConfirmationOfScrutinyResponse>>().Subject;
            var badRequestResult = taskResult.Result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
            badRequestResult.Value.Should().BeAssignableTo<PutConfirmationOfScrutinyResponse>();
        }

        [Fact]
        public async void PutConfirmationOfScrutiny_Called_When_The_User_Not_Allowed_To_Confirm_Scrutiny_Returns_BadRequest()
        {
            // Arrange
            var examination = new Examination
            {
                ExaminationId = Guid.NewGuid().ToString(),
                MedicalTeam = new MedicalTeam
                {
                    MedicalExaminerUserId = "MedicalExaminerUserId"
                }
            };
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();
            var mockMeUser = new Mock<MeUser>();
            mockMeUser.Object.UserId = "MedicalExaminerUserId";
            var usersRetrievalByOktaIdService = new Mock<IAsyncQueryHandler<UserRetrievalByOktaIdQuery, MeUser>>();
            usersRetrievalByOktaIdService.Setup(service => service.Handle(It.IsAny<UserRetrievalByOktaIdQuery>())).Returns(Task.FromResult(mockMeUser.Object));
            var confirmationOfScrutinyService = new Mock<IAsyncQueryHandler<ConfirmationOfScrutinyQuery, Examination>>();
            var closeCaseService = new Mock<IAsyncQueryHandler<CloseCaseQuery, string>>();
            var coronerReferralService = new Mock<IAsyncQueryHandler<CoronerReferralQuery, string>>();
            var examinationRetrievalService = new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();
            examinationRetrievalService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>())).Returns(Task.FromResult(examination));
            var saveOutstandingCaseItems = new Mock<IAsyncQueryHandler<SaveOutstandingCaseItemsQuery, string>>();
            var voidCaseService = new Mock<IAsyncQueryHandler<VoidCaseQuery, Examination>>();

            var sut = new CaseOutcomeController(
                logger.Object,
                mapper.Object,
                coronerReferralService.Object,
                closeCaseService.Object,
                examinationRetrievalService.Object,
                saveOutstandingCaseItems.Object,
                confirmationOfScrutinyService.Object,
                usersRetrievalByOktaIdService.Object,
                voidCaseService.Object,
                AuthorizationServiceMock.Object,
                PermissionServiceMock.Object);

            sut.ControllerContext = GetControllerContext();

            // Act
            var response = await sut.PutConfirmationOfScrutiny(examination.ExaminationId);

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<PutConfirmationOfScrutinyResponse>>().Subject;
            var badRequestResult = taskResult.Result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
            badRequestResult.Value.Should().BeAssignableTo<string>();
        }

        [Fact]
        public async void PutConfirmationOfScrutiny_Called_With_Valid_Status_Of_The_Examination_Returns_OK()
        {
            // Arrange
            var examination = new Examination
            {
                ExaminationId = Guid.NewGuid().ToString(),
                ReadyForMEScrutiny = true,
                PendingScrutinyNotes = false,
                PendingAdmissionNotes = false,
                PendingDiscussionWithQAP = false,
                MedicalTeam = new MedicalTeam
                {
                    MedicalExaminerUserId = "MedicalExaminerUserId"
                }
            };
            var logger = new Mock<IMELogger>();
            var mapper = new Mock<IMapper>();
            var mockMeUser = new Mock<MeUser>();
            mockMeUser.Object.UserId = "MedicalExaminerUserId";
            var usersRetrievalByOktaIdService = new Mock<IAsyncQueryHandler<UserRetrievalByOktaIdQuery, MeUser>>();
            usersRetrievalByOktaIdService.Setup(service => service.Handle(It.IsAny<UserRetrievalByOktaIdQuery>())).Returns(Task.FromResult(mockMeUser.Object));
            var confirmationOfScrutinyService = new Mock<IAsyncQueryHandler<ConfirmationOfScrutinyQuery, Examination>>();
            var closeCaseService = new Mock<IAsyncQueryHandler<CloseCaseQuery, string>>();
            var coronerReferralService = new Mock<IAsyncQueryHandler<CoronerReferralQuery, string>>();
            var examinationRetrievalService = new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();
            examinationRetrievalService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>())).Returns(Task.FromResult(examination));
            var saveOutstandingCaseItems = new Mock<IAsyncQueryHandler<SaveOutstandingCaseItemsQuery, string>>();
            var voidCaseService = new Mock<IAsyncQueryHandler<VoidCaseQuery, Examination>>();

            var sut = new CaseOutcomeController(
                logger.Object,
                mapper.Object,
                coronerReferralService.Object,
                closeCaseService.Object,
                examinationRetrievalService.Object,
                saveOutstandingCaseItems.Object,
                confirmationOfScrutinyService.Object,
                usersRetrievalByOktaIdService.Object,
                voidCaseService.Object,
                AuthorizationServiceMock.Object,
                PermissionServiceMock.Object);

            sut.ControllerContext = GetControllerContext();

            // Act
            var response = await sut.PutConfirmationOfScrutiny(examination.ExaminationId);

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<PutConfirmationOfScrutinyResponse>>().Subject;
            var okResult = taskResult.Result.Should().BeAssignableTo<OkObjectResult>().Subject;
            okResult.Value.Should().BeAssignableTo<PutConfirmationOfScrutinyResponse>();
        }

        [Fact]
        public void GetCaseOutcome_When_Called_With_Invalid_Case_Id_Returns_Not_Found()
        {
            // Arrange
            var logger = new Mock<IAsyncQueryHandler<CreateMELoggerQuery, LogMessageActionDefault>>();
            var mapper = new Mock<IMapper>();
            var usersRetrievalByOktaIdService = new Mock<IAsyncQueryHandler<UserRetrievalByOktaIdQuery, MeUser>>();
            var confirmationOfScrutinyService = new Mock<IAsyncQueryHandler<ConfirmationOfScrutinyQuery, Examination>>();
            var closeCaseService = new Mock<IAsyncQueryHandler<CloseCaseQuery, string>>();
            var coronerReferralService = new Mock<IAsyncQueryHandler<CoronerReferralQuery, string>>();
            var examinationRetrievalService = new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();
            var saveOutstandingCaseItems = new Mock<IAsyncQueryHandler<SaveOutstandingCaseItemsQuery, string>>();
            var voidCaseService = new Mock<IAsyncQueryHandler<VoidCaseQuery, Examination>>();

            var sut = new CaseOutcomeController(
                logger.Object,
                mapper.Object,
                coronerReferralService.Object,
                closeCaseService.Object,
                examinationRetrievalService.Object,
                saveOutstandingCaseItems.Object,
                confirmationOfScrutinyService.Object,
                usersRetrievalByOktaIdService.Object,
                voidCaseService.Object,
                AuthorizationServiceMock.Object,
                PermissionServiceMock.Object);

            sut.ControllerContext = GetControllerContext();

            // Act
            var response = sut.GetCaseOutcome("InvalidExaminationId").Result;

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<GetCaseOutcomeResponse>>().Subject;
            var notFoundResult = taskResult.Result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
            notFoundResult.Value.Should().BeAssignableTo<GetCaseOutcomeResponse>();
        }

        [Fact]
        public void GetCaseOutcome_When_Called_With_Valid_Id_Returns_Expected_Type()
        {
            // Arrange
            var logger = new Mock<IAsyncQueryHandler<CreateMELoggerQuery, LogMessageActionDefault>>();
            var mapper = new Mock<IMapper>();
            var examination = new Examination
            {
                ExaminationId = Guid.NewGuid().ToString()
            };

            var usersRetrievalByOktaIdService = new Mock<IAsyncQueryHandler<UserRetrievalByOktaIdQuery, MeUser>>();
            var mockMeUser = new Mock<MeUser>();
            usersRetrievalByOktaIdService.Setup(service => service.Handle(It.IsAny<UserRetrievalByOktaIdQuery>())).Returns(Task.FromResult(mockMeUser.Object));
            var closeCaseService = new Mock<IAsyncQueryHandler<CloseCaseQuery, string>>();
            var coronerReferralService = new Mock<IAsyncQueryHandler<CoronerReferralQuery, string>>();
            var examinationRetrievalService = new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();
            examinationRetrievalService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>())).Returns(Task.FromResult(examination));
            var saveOutstandingCaseItems = new Mock<IAsyncQueryHandler<SaveOutstandingCaseItemsQuery, string>>();
            var confirmationOfScrutinyService = new Mock<IAsyncQueryHandler<ConfirmationOfScrutinyQuery, Examination>>();
            var voidCaseService = new Mock<IAsyncQueryHandler<VoidCaseQuery, Examination>>();

            var sut = new CaseOutcomeController(
                logger.Object,
                mapper.Object,
                coronerReferralService.Object,
                closeCaseService.Object,
                examinationRetrievalService.Object,
                saveOutstandingCaseItems.Object,
                confirmationOfScrutinyService.Object,
                usersRetrievalByOktaIdService.Object,
                voidCaseService.Object,
                AuthorizationServiceMock.Object,
                PermissionServiceMock.Object);

            sut.ControllerContext = GetControllerContext();

            // Act
            var response = sut.GetCaseOutcome(examination.ExaminationId).Result;

            // Assert
            var taskResult = response.Should().BeOfType<ActionResult<GetCaseOutcomeResponse>>().Subject;
            var okResult = taskResult.Result.Should().BeAssignableTo<OkObjectResult>().Subject;
        }

        [Fact]
        public async void PutOutstandingCaseItems_When_Called_With_No_Case_Id_Returns_Bad_Request()
        {
            // Arrange
            var logger = new Mock<IAsyncQueryHandler<CreateMELoggerQuery, LogMessageActionDefault>>();
            var mapper = new Mock<IMapper>();
            var usersRetrievalByOktaIdService = new Mock<IAsyncQueryHandler<UserRetrievalByOktaIdQuery, MeUser>>();
            var closeCaseService = new Mock<IAsyncQueryHandler<CloseCaseQuery, string>>();
            var coronerReferralService = new Mock<IAsyncQueryHandler<CoronerReferralQuery, string>>();
            var examinationRetrievalService = new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();
            var saveOutstandingCaseItems = new Mock<IAsyncQueryHandler<SaveOutstandingCaseItemsQuery, string>>();
            var confirmationOfScrutinyService = new Mock<IAsyncQueryHandler<ConfirmationOfScrutinyQuery, Examination>>();
            var voidCaseService = new Mock<IAsyncQueryHandler<VoidCaseQuery, Examination>>();

            var sut = new CaseOutcomeController(
                logger.Object,
                mapper.Object,
                coronerReferralService.Object,
                closeCaseService.Object,
                examinationRetrievalService.Object,
                saveOutstandingCaseItems.Object,
                confirmationOfScrutinyService.Object,
                usersRetrievalByOktaIdService.Object,
                voidCaseService.Object,
                AuthorizationServiceMock.Object,
                PermissionServiceMock.Object);

            sut.ControllerContext = GetControllerContext();

            var putOutstandingCaseItemsRequest = new PutOutstandingCaseItemsRequest
            {
                MccdIssued = true,
                CremationFormStatus = CremationFormStatus.Yes,
                GpNotifiedStatus = GPNotified.GPNotified
            };

            // Act
            var response = await sut.PutOutstandingCaseItems(string.Empty, putOutstandingCaseItemsRequest);

            // Assert
            response.Should().BeAssignableTo<BadRequestObjectResult>();
        }

        [Fact]
        public async void PutOutstandingCaseItems_When_Called_With_Invalid_Case_Id_Returns_Bad_Request()
        {
            // Arrange
            var logger = new Mock<IAsyncQueryHandler<CreateMELoggerQuery, LogMessageActionDefault>>();
            var mapper = new Mock<IMapper>();
            var usersRetrievalByOktaIdService = new Mock<IAsyncQueryHandler<UserRetrievalByOktaIdQuery, MeUser>>();
            var closeCaseService = new Mock<IAsyncQueryHandler<CloseCaseQuery, string>>();
            var coronerReferralService = new Mock<IAsyncQueryHandler<CoronerReferralQuery, string>>();
            var examinationRetrievalService = new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();
            var saveOutstandingCaseItems = new Mock<IAsyncQueryHandler<SaveOutstandingCaseItemsQuery, string>>();
            var confirmationOfScrutinyService = new Mock<IAsyncQueryHandler<ConfirmationOfScrutinyQuery, Examination>>();
            var voidCaseService = new Mock<IAsyncQueryHandler<VoidCaseQuery, Examination>>();

            var sut = new CaseOutcomeController(
                logger.Object,
                mapper.Object,
                coronerReferralService.Object,
                closeCaseService.Object,
                examinationRetrievalService.Object,
                saveOutstandingCaseItems.Object,
                confirmationOfScrutinyService.Object,
                usersRetrievalByOktaIdService.Object,
                voidCaseService.Object,
                AuthorizationServiceMock.Object,
                PermissionServiceMock.Object);

            sut.ControllerContext = GetControllerContext();

            var putOutstandingCaseItemsRequest = new PutOutstandingCaseItemsRequest
            {
                MccdIssued = true,
                CremationFormStatus = CremationFormStatus.Yes,
                GpNotifiedStatus = GPNotified.GPNotified
            };

            // Act
            var response = await sut.PutOutstandingCaseItems("invalidCaseId", putOutstandingCaseItemsRequest);

            // Assert
            response.Should().BeAssignableTo<BadRequestObjectResult>();
        }

        [Fact]
        public async void PutOutstandingCaseItems_When_Called_With_Valid_Examination_Id_Returns_Ok()
        {
            // Arrange
            var logger = new Mock<IAsyncQueryHandler<CreateMELoggerQuery, LogMessageActionDefault>>();
            var mapper = new Mock<IMapper>();
            var examination = new Examination
            {
                ExaminationId = Guid.NewGuid().ToString(),
                ScrutinyConfirmed = true
            };

            var mockMeUser = new Mock<MeUser>();
            var usersRetrievalByOktaIdService = new Mock<IAsyncQueryHandler<UserRetrievalByOktaIdQuery, MeUser>>();
            usersRetrievalByOktaIdService.Setup(service => service.Handle(It.IsAny<UserRetrievalByOktaIdQuery>()))
                .Returns(Task.FromResult(mockMeUser.Object));

            var closeCaseService = new Mock<IAsyncQueryHandler<CloseCaseQuery, string>>();
            closeCaseService.Setup(service => service.Handle(It.IsAny<CloseCaseQuery>()))
                .Returns(Task.FromResult("test")).Verifiable();

            var coronerReferralService = new Mock<IAsyncQueryHandler<CoronerReferralQuery, string>>();
            var saveOutstandingCaseItems = new Mock<IAsyncQueryHandler<SaveOutstandingCaseItemsQuery, string>>();

            var examinationRetrievalService = new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();
            examinationRetrievalService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>()))
                .Returns(Task.FromResult(examination)).Verifiable();

            var confirmationOfScrutinyService = new Mock<IAsyncQueryHandler<ConfirmationOfScrutinyQuery, Examination>>();
            var voidCaseService = new Mock<IAsyncQueryHandler<VoidCaseQuery, Examination>>();

            var sut = new CaseOutcomeController(
                logger.Object,
                mapper.Object,
                coronerReferralService.Object,
                closeCaseService.Object,
                examinationRetrievalService.Object,
                saveOutstandingCaseItems.Object,
                confirmationOfScrutinyService.Object,
                usersRetrievalByOktaIdService.Object,
                voidCaseService.Object,
                AuthorizationServiceMock.Object,
                PermissionServiceMock.Object);

            sut.ControllerContext = GetControllerContext();

            var putOutstandingCaseItemsRequest = new PutOutstandingCaseItemsRequest
            {
                MccdIssued = true,
                CremationFormStatus = CremationFormStatus.Yes,
                GpNotifiedStatus = GPNotified.GPNotified
            };

            // Act
            var response = await sut.PutOutstandingCaseItems(examination.ExaminationId, putOutstandingCaseItemsRequest);

            // Assert
            var okResult = response.Should().BeAssignableTo<OkResult>().Subject;
        }

        [Fact]
        public async void PutCoronerReferral_When_Called_With_No_Case_Id_Returns_Bad_Request()
        {
            // Arrange
            var logger = new Mock<IAsyncQueryHandler<CreateMELoggerQuery, LogMessageActionDefault>>();
            var mapper = new Mock<IMapper>();
            var usersRetrievalByOktaIdService = new Mock<IAsyncQueryHandler<UserRetrievalByOktaIdQuery, MeUser>>();
            var closeCaseService = new Mock<IAsyncQueryHandler<CloseCaseQuery, string>>();
            var coronerReferralService = new Mock<IAsyncQueryHandler<CoronerReferralQuery, string>>();
            var examinationRetrievalService = new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();
            var saveOutstandingCaseItems = new Mock<IAsyncQueryHandler<SaveOutstandingCaseItemsQuery, string>>();
            var confirmationOfScrutinyService = new Mock<IAsyncQueryHandler<ConfirmationOfScrutinyQuery, Examination>>();
            var voidCaseService = new Mock<IAsyncQueryHandler<VoidCaseQuery, Examination>>();

            var sut = new CaseOutcomeController(
                logger.Object,
                mapper.Object,
                coronerReferralService.Object,
                closeCaseService.Object,
                examinationRetrievalService.Object,
                saveOutstandingCaseItems.Object,
                confirmationOfScrutinyService.Object,
                usersRetrievalByOktaIdService.Object,
                voidCaseService.Object,
                AuthorizationServiceMock.Object,
                PermissionServiceMock.Object);

            sut.ControllerContext = GetControllerContext();

            // Act
            var response = await sut.PutCoronerReferral(string.Empty);

            // Assert
            response.Should().BeAssignableTo<BadRequestObjectResult>();
        }

        [Fact]
        public async void PutCoronerReferral_When_Called_With_Invalid_Case_Id_Returns_Bad_Request()
        {
            // Arrange
            var logger = new Mock<IAsyncQueryHandler<CreateMELoggerQuery, LogMessageActionDefault>>();
            var mapper = new Mock<IMapper>();
            var usersRetrievalByOktaIdService = new Mock<IAsyncQueryHandler<UserRetrievalByOktaIdQuery, MeUser>>();
            var closeCaseService = new Mock<IAsyncQueryHandler<CloseCaseQuery, string>>();
            var coronerReferralService = new Mock<IAsyncQueryHandler<CoronerReferralQuery, string>>();
            var examinationRetrievalService = new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();
            var saveOutstandingCaseItems = new Mock<IAsyncQueryHandler<SaveOutstandingCaseItemsQuery, string>>();
            var confirmationOfScrutinyService = new Mock<IAsyncQueryHandler<ConfirmationOfScrutinyQuery, Examination>>();
            var voidCaseService = new Mock<IAsyncQueryHandler<VoidCaseQuery, Examination>>();

            var sut = new CaseOutcomeController(
                logger.Object,
                mapper.Object,
                coronerReferralService.Object,
                closeCaseService.Object,
                examinationRetrievalService.Object,
                saveOutstandingCaseItems.Object,
                confirmationOfScrutinyService.Object,
                usersRetrievalByOktaIdService.Object,
                voidCaseService.Object,
                AuthorizationServiceMock.Object,
                PermissionServiceMock.Object);

            sut.ControllerContext = GetControllerContext();

            // Act
            var response = await sut.PutCoronerReferral("invalidCaseId");

            // Assert
            response.Should().BeAssignableTo<BadRequestObjectResult>();
        }

        [Fact]
        public async void PutCoronerReferral_When_Called_With_Valid_Examination_Id_Returns_Ok()
        {
            // Arrange
            var logger = new Mock<IAsyncQueryHandler<CreateMELoggerQuery, LogMessageActionDefault>>();
            var mapper = new Mock<IMapper>();
            var caseOutcome = new CaseOutcome
            {
                CaseOutcomeSummary = CaseOutcomeSummary.ReferToCoroner
            };
            var examination = new Examination
            {
                ExaminationId = Guid.NewGuid().ToString(),
                CaseOutcome = caseOutcome
            };

            var mockMeUser = new Mock<MeUser>();
            var usersRetrievalByOktaIdService = new Mock<IAsyncQueryHandler<UserRetrievalByOktaIdQuery, MeUser>>();
            usersRetrievalByOktaIdService.Setup(service => service.Handle(It.IsAny<UserRetrievalByOktaIdQuery>())).Returns(Task.FromResult(mockMeUser.Object));
            var closeCaseService = new Mock<IAsyncQueryHandler<CloseCaseQuery, string>>();
            closeCaseService.Setup(service => service.Handle(It.IsAny<CloseCaseQuery>())).Returns(Task.FromResult("test")).Verifiable();
            var coronerReferralService = new Mock<IAsyncQueryHandler<CoronerReferralQuery, string>>();
            var examinationRetrievalService = new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();
            examinationRetrievalService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>())).Returns(Task.FromResult(examination)).Verifiable();
            var saveOutstandingCaseItems = new Mock<IAsyncQueryHandler<SaveOutstandingCaseItemsQuery, string>>();
            var confirmationOfScrutinyService = new Mock<IAsyncQueryHandler<ConfirmationOfScrutinyQuery, Examination>>();
            var voidCaseService = new Mock<IAsyncQueryHandler<VoidCaseQuery, Examination>>();

            var sut = new CaseOutcomeController(
                logger.Object,
                mapper.Object,
                coronerReferralService.Object,
                closeCaseService.Object,
                examinationRetrievalService.Object,
                saveOutstandingCaseItems.Object,
                confirmationOfScrutinyService.Object,
                usersRetrievalByOktaIdService.Object,
                voidCaseService.Object,
                AuthorizationServiceMock.Object,
                PermissionServiceMock.Object);

            sut.ControllerContext = GetControllerContext();

            // Act
            var response = await sut.PutCoronerReferral(examination.ExaminationId);

            // Assert
            var okResult = response.Should().BeAssignableTo<OkResult>().Subject;
        }

        [Fact]
        public async void PutCloseCase_When_Called_With_No_Case_Id_Returns_Bad_Request()
        {
            // Arrange
            var logger = new Mock<IAsyncQueryHandler<CreateMELoggerQuery, LogMessageActionDefault>>();
            var mapper = new Mock<IMapper>();
            var usersRetrievalByOktaIdService = new Mock<IAsyncQueryHandler<UserRetrievalByOktaIdQuery, MeUser>>();
            var closeCaseService = new Mock<IAsyncQueryHandler<CloseCaseQuery, string>>();
            var coronerReferralService = new Mock<IAsyncQueryHandler<CoronerReferralQuery, string>>();
            var examinationRetrievalService = new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();
            var saveOutstandingCaseItems = new Mock<IAsyncQueryHandler<SaveOutstandingCaseItemsQuery, string>>();
            var confirmationOfScrutinyService = new Mock<IAsyncQueryHandler<ConfirmationOfScrutinyQuery, Examination>>();
            var voidCaseService = new Mock<IAsyncQueryHandler<VoidCaseQuery, Examination>>();

            var sut = new CaseOutcomeController(
                logger.Object,
                mapper.Object,
                coronerReferralService.Object,
                closeCaseService.Object,
                examinationRetrievalService.Object,
                saveOutstandingCaseItems.Object,
                confirmationOfScrutinyService.Object,
                usersRetrievalByOktaIdService.Object,
                voidCaseService.Object,
                AuthorizationServiceMock.Object,
                PermissionServiceMock.Object);

            sut.ControllerContext = GetControllerContext();

            // Act
            var response = await sut.PutCloseCase(string.Empty);

            // Assert
            response.Should().BeAssignableTo<BadRequestObjectResult>();
        }

        [Fact]
        public async void PutCloseCase_When_Called_With_Invalid_Case_Id_Returns_Bad_Request()
        {
            // Arrange
            var logger = new Mock<IAsyncQueryHandler<CreateMELoggerQuery, LogMessageActionDefault>>();
            var mapper = new Mock<IMapper>();
            var usersRetrievalByOktaIdService = new Mock<IAsyncQueryHandler<UserRetrievalByOktaIdQuery, MeUser>>();
            var closeCaseService = new Mock<IAsyncQueryHandler<CloseCaseQuery, string>>();
            var coronerReferralService = new Mock<IAsyncQueryHandler<CoronerReferralQuery, string>>();
            var examinationRetrievalService = new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();
            var saveOutstandingCaseItems = new Mock<IAsyncQueryHandler<SaveOutstandingCaseItemsQuery, string>>();
            var confirmationOfScrutinyService = new Mock<IAsyncQueryHandler<ConfirmationOfScrutinyQuery, Examination>>();
            var voidCaseService = new Mock<IAsyncQueryHandler<VoidCaseQuery, Examination>>();

            var sut = new CaseOutcomeController(
                logger.Object,
                mapper.Object,
                coronerReferralService.Object,
                closeCaseService.Object,
                examinationRetrievalService.Object,
                saveOutstandingCaseItems.Object,
                confirmationOfScrutinyService.Object,
                usersRetrievalByOktaIdService.Object,
                voidCaseService.Object,
                AuthorizationServiceMock.Object,
                PermissionServiceMock.Object);

            sut.ControllerContext = GetControllerContext();
            // Act
            var response = await sut.PutCloseCase("invalidCaseId");

            // Assert
            response.Should().BeAssignableTo<BadRequestObjectResult>();
        }

        [Fact]
        public async void PutCloseCase_When_Called_With_Valid_Examination_Id_Returns_Ok()
        {
            // Arrange
            var logger = new Mock<IAsyncQueryHandler<CreateMELoggerQuery, LogMessageActionDefault>>();
            var mapper = new Mock<IMapper>();
            var caseOutcome = new CaseOutcome
            {
                CaseOutcomeSummary = CaseOutcomeSummary.IssueMCCD,
                MccdIssued = true,
                CremationFormStatus = CremationFormStatus.Yes,
                GpNotifiedStatus = GPNotified.GPNotified
            };

            var examination = new Examination
            {
                ExaminationId = Guid.NewGuid().ToString(),
                OutstandingCaseItemsCompleted = true,
                CaseOutcome = caseOutcome
            };

            var mockMeUser = new Mock<MeUser>();
            var usersRetrievalByOktaIdService = new Mock<IAsyncQueryHandler<UserRetrievalByOktaIdQuery, MeUser>>();
            usersRetrievalByOktaIdService.Setup(service => service.Handle(It.IsAny<UserRetrievalByOktaIdQuery>())).Returns(Task.FromResult(mockMeUser.Object));
            var closeCaseService = new Mock<IAsyncQueryHandler<CloseCaseQuery, string>>();
            closeCaseService.Setup(service => service.Handle(It.IsAny<CloseCaseQuery>())).Returns(Task.FromResult("test")).Verifiable();
            var coronerReferralService = new Mock<IAsyncQueryHandler<CoronerReferralQuery, string>>();
            var examinationRetrievalService = new Mock<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>>();
            examinationRetrievalService.Setup(service => service.Handle(It.IsAny<ExaminationRetrievalQuery>())).Returns(Task.FromResult(examination)).Verifiable();
            var saveOutstandingCaseItems = new Mock<IAsyncQueryHandler<SaveOutstandingCaseItemsQuery, string>>();
            var confirmationOfScrutinyService = new Mock<IAsyncQueryHandler<ConfirmationOfScrutinyQuery, Examination>>();
            var voidCaseService = new Mock<IAsyncQueryHandler<VoidCaseQuery, Examination>>();

            var sut = new CaseOutcomeController(
                logger.Object,
                mapper.Object,
                coronerReferralService.Object,
                closeCaseService.Object,
                examinationRetrievalService.Object,
                saveOutstandingCaseItems.Object,
                confirmationOfScrutinyService.Object,
                usersRetrievalByOktaIdService.Object,
                voidCaseService.Object,
                AuthorizationServiceMock.Object,
                PermissionServiceMock.Object);

            sut.ControllerContext = GetControllerContext();

            // Act
            var response = await sut.PutCloseCase(examination.ExaminationId);

            // Assert
            var okResult = response.Should().BeAssignableTo<OkResult>().Subject;
        }
    }
}
