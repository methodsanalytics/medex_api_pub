using System;
using System.Linq.Expressions;
using FluentAssertions;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries;
using MedicalExaminer.Common.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Internal;
using Moq;
using Xunit;
// ReSharper disable VirtualMemberCallInConstructor

namespace MedicalExaminer.API.Tests.Services
{
    /// <summary>
    /// Service Test Base.
    /// </summary>
    /// <typeparam name="TQuery">The Query being Tested.</typeparam>
    /// <typeparam name="TConnectionSettings">The Connection String class.</typeparam>
    /// <typeparam name="TItem">The Item being returned.</typeparam>
    /// <typeparam name="TType">The Type of item being returned. May be same as TItem if not a collection.</typeparam>
    /// <typeparam name="TService">Finalyl the Service.</typeparam>
    public abstract class ServiceTestsBase<TQuery, TConnectionSettings, TItem, TType, TService>
        where TQuery : class, IQuery<TItem>
        where TConnectionSettings : class, IConnectionSettings
        where TService : QueryHandler<TQuery, TItem>
    {
        private Mock<ILogger<TService>> _loggerMock;

        private Mock<TConnectionSettings> _connectionSettings;

        /// <summary>
        /// Initialise a new instance of <see cref="ServiceTestsBase{TQuery,TConnectionSettings,TItem,TType,TService}"/>.
        /// </summary>
        protected ServiceTestsBase()
        {
            // Make sure you don't access the sub class inside this method since its being called in constructor.
            var client = CosmosMocker.CreateDocumentClient(GetExamples());
            var clientFactory = CosmosMocker.CreateClientFactory(client);
            var dataAccess = new DatabaseAccess(clientFactory.Object);
            _connectionSettings = CosmosMocker.CreateConnectionSettings<TConnectionSettings>();

            _loggerMock = new Mock<ILogger<TService>>(MockBehavior.Strict);

            Service = GetService(_loggerMock.Object, dataAccess, _connectionSettings.Object);
        }

        /// <summary>
        /// The Service under test.
        /// </summary>
        protected TService Service { get; }

        /// <summary>
        /// Query is null throws an exception.
        /// </summary>
        [Fact]
        public void QueryIsNullThrowsException()
        {
            // Arrange
            var connectionSettings = CosmosMocker.CreateConnectionSettings<TConnectionSettings>();
            var dbAccess = new Mock<IDatabaseAccess>();
            var logger = new Mock<ILogger<TService>>();

            var sut = GetService(logger.Object, dbAccess.Object, connectionSettings.Object);

            // Act
            Action act = () => sut.Handle(null).GetAwaiter().GetResult();

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async void LogWritten_WhenDataAccessThrowsException()
        {
            // Arrange
            _loggerMock.Setup(l => l.Log<object>(
                LogLevel.Critical,
                It.IsAny<EventId>(),
                It.IsAny<FormattedLogValues>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<object, Exception, string>>())).Verifiable();

            var dbAccess = new Mock<IDatabaseAccess>();
            dbAccess.Setup(da => da.GetItemAsync(It.IsAny<IConnectionSettings>(), It.IsAny<Expression<Func<TType, bool>>>()))
                .Throws<Exception>();
            dbAccess.Setup(da => da.GetItemsAsync(It.IsAny<IConnectionSettings>(), It.IsAny<Expression<Func<TType, bool>>>()))
                .Throws<Exception>();
            dbAccess.Setup(da => da.GetCountAsync(It.IsAny<IConnectionSettings>(), It.IsAny<Expression<Func<TType, bool>>>()))
                .Throws<Exception>();

            var service = GetService(_loggerMock.Object, dbAccess.Object, _connectionSettings.Object);

            var query = GetQuery();

            // Act
            Action act = () => service.Handle(query).GetAwaiter().GetResult();

            // Assert
            act.Should().Throw<Exception>();

            _loggerMock.Verify(l => l.Log<object>(
                LogLevel.Critical,
                It.IsAny<EventId>(),
                It.Is<FormattedLogValues>(v => v.ToString().Contains("Failed to")),
                It.IsAny<Exception>(),
                It.IsAny<Func<object, Exception, string>>()));
        }

        /// <summary>
        /// Base method to construct simple services. Override if you need to pass in other defaults.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="databaseAccess">Database Access.</param>
        /// <param name="connectionSettings">Connection Settings.</param>
        /// <returns>Service Instance.</returns>
        protected virtual TService GetService(ILogger<TService> logger, IDatabaseAccess databaseAccess, TConnectionSettings connectionSettings)
        {
            var service = (TService)Activator.CreateInstance(typeof(TService), logger, databaseAccess, connectionSettings);

            return service;
        }

        protected virtual TQuery GetQuery()
        {
            var query = (TQuery)Activator.CreateInstance(typeof(TQuery));

            return query;
        }

        /// <summary>
        /// Get Examples
        /// </summary>
        /// <remarks>Called from constructor so return only; do not interact with the sub class since it wont have been set up yet.</remarks>
        /// <returns></returns>
        protected abstract TType[] GetExamples();
    }
}
