using System;
using System.Collections.Generic;
using MedicalExaminer.Common.Services;

namespace MedicalExaminer.Common.Loggers
{
    /// <summary>
    /// Construct log objects and submit to logging destination
    /// </summary>
    public class MELogger : IMELogger
    {
        private readonly IAsyncQueryHandler<LogMessageActionDefault, string> _writeLogService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="MELogger" /> class.
        /// </summary>
        /// <param name="writeLogService">Write Log Service.</param>
        public MELogger(IAsyncQueryHandler<LogMessageActionDefault, string> writeLogService)
        {
            _writeLogService = writeLogService;
        }

        /// <inheritdoc />
        public async void Log(
            string userName,
            string userAuthenticationType,
            bool userIsAuthenticated,
            string controllerName,
            string controllerMethod,
            IList<string> parameters,
            string remoteIP,
            DateTime timeStamp)
        {
            var logEntry = new LogMessageActionDefault(
                userName,
                userAuthenticationType,
                userIsAuthenticated,
                controllerName,
                controllerMethod,
                parameters,
                remoteIP,
                timeStamp);

            await _writeLogService.Handle(logEntry);
        }
    }
}