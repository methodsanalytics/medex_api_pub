﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MedicalExaminer.API.Authorization;
using MedicalExaminer.API.Controllers;
using MedicalExaminer.API.Filters;
using MedicalExaminer.API.Services;
using MedicalExaminer.Common.Loggers;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;
using Xunit;
using AuthenticationManager = Microsoft.AspNetCore.Http.Authentication.AuthenticationManager;

namespace MedicalExaminer.API.Tests.Controllers
{
    internal class MockConnectionInfo : ConnectionInfo
    {
        public override string Id
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public override IPAddress RemoteIpAddress
        {
            get => null;
            set { }
        }

        public override int RemotePort
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public override IPAddress LocalIpAddress
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public override int LocalPort
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public override X509Certificate2 ClientCertificate
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public override Task<X509Certificate2> GetClientCertificateAsync(
            CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }
    }

    internal class MockHttpContext : HttpContext
    {
        private ClaimsPrincipal _claimsPrincipal;

        public MockHttpContext()
        {
            _claimsPrincipal = new ClaimsPrincipal();
            Connection = new MockConnectionInfo();
        }

        public override IFeatureCollection Features => throw new NotImplementedException();

        public override HttpRequest Request => throw new NotImplementedException();

        public override HttpResponse Response => throw new NotImplementedException();

        public override ConnectionInfo Connection { get; }

        public override WebSocketManager WebSockets => throw new NotImplementedException();

        public override AuthenticationManager Authentication { get; }

        public override ClaimsPrincipal User
        {
            get => _claimsPrincipal;
            set => _claimsPrincipal = value;
        }

        public override IDictionary<object, object> Items
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public override IServiceProvider RequestServices
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public override CancellationToken RequestAborted
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public override string TraceIdentifier
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public override ISession Session
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public override void Abort()
        {
            throw new NotImplementedException();
        }
    }

    public class ControllerActionFilterTests
    {
        public ControllerActionFilterTests()
        {
            
            _mockLogger = new MELoggerMocker();
            _mapper = new Mock<IMapper>();
            var createUserService = new Mock<IAsyncQueryHandler<CreateUserQuery, MeUser>>();
            var userRetrievalService = new Mock<IAsyncQueryHandler<UserRetrievalByIdQuery, MeUser>>();
            var usersRetrievalService =
                new Mock<IAsyncQueryHandler<UsersRetrievalQuery, IEnumerable<MeUser>>>();
            var userUpdateService = new Mock<IAsyncQueryHandler<UserUpdateQuery, MeUser>>();

            var usersRetrievalByEmailServiceMock = new Mock<IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser>>();

            var authorizationServiceMock = new Mock<IAuthorizationService>();

            var permissionServiceMock = new Mock<IPermissionService>();

            _controller = new UsersController(
                _mockLogger,
                _mapper.Object,
                usersRetrievalByEmailServiceMock.Object,
                authorizationServiceMock.Object,
                permissionServiceMock.Object,
                createUserService.Object,
                userRetrievalService.Object,
                usersRetrievalService.Object,
                userUpdateService.Object);
        }

        private readonly MELoggerMocker _mockLogger;
        private readonly UsersController _controller;
        private readonly Mock<IMapper> _mapper;

        [Fact]
        public void CheckCallToLogger()
        {
            var controllerActionFilter = new ControllerActionFilter();
            var actionContext = new ActionContext { HttpContext = new MockHttpContext() };
            var identity = new ClaimsIdentity();
            identity.AddClaim(new Claim(MEClaimTypes.UserId, "UserId"));
            actionContext.HttpContext.User.AddIdentity(identity);
            actionContext.RouteData = new RouteData();
            actionContext.RouteData.Values.Add("Action", "MyAction");
            actionContext.RouteData.Values.Add("Method", "MyMethod");
            actionContext.ActionDescriptor = new ActionDescriptor();
            var filters = new List<IFilterMetadata>();
            var actionArguments = new Dictionary<string, object>();
            var actionExecutingContext =
                new ActionExecutingContext(actionContext, filters, actionArguments, _controller);
            controllerActionFilter.OnActionExecuting(actionExecutingContext);
            var logEntry = _mockLogger.LogEntry;

            var logEntryContents = logEntry.UserId + " " + logEntry.UserAuthenticationType + " " +
                                   logEntry.UserIsAuthenticated + " " + logEntry.ControllerName + " " +
                                   logEntry.ControllerMethod + " " + logEntry.RemoteIP;

            const string expectedMessage = "UserId Unknown False MyMethod MyAction Unknown";
            Assert.Equal(expectedMessage, logEntryContents);
        }
    }
}