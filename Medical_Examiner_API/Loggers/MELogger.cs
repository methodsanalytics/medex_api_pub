﻿using System;
using System.Collections.Generic;
using Medical_Examiner_API.Persistence;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace Medical_Examiner_API.Loggers
{
    /// <summary>
    /// Construct log objects and submit to logging destination
    /// </summary>
    public class MELogger : IMELogger
    {
        private readonly IMeLoggerPersistence _mEloggerPersistence;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mEloggerPersistence">persistence object that writes to logging destination</param>
        public MELogger(IMeLoggerPersistence mEloggerPersistence)
        {
            _mEloggerPersistence = mEloggerPersistence;
        }

        /// <summary>
        /// Create log object from parameters and submit to logging destination
        /// </summary>
        /// <param name="userName">user name</param>
        /// <param name="userAuthenticationType">user authentication type</param>
        /// <param name="userIsAuthenticated">user authentication status</param>
        /// <param name="controllerName">name of controller called</param>
        /// <param name="controllerMethod">method of caller called</param>
        /// <param name="parameters">list of parameters passed to method</param>
        /// <param name="remoteIP">IP address of client</param>
        /// <param name="timeStamp">timestamp when method called</param>
        public async void Log(string userName, string userAuthenticationType, bool userIsAuthenticated,
            string controllerName, string controllerMethod, IList<string> parameters, string remoteIP,
            DateTime timeStamp)
        {
            var logEntry = new LogMessageActionDefault(userName, userAuthenticationType, userIsAuthenticated,
                controllerName, controllerMethod, parameters, remoteIP, timeStamp);
            await _mEloggerPersistence.SaveLogEntryAsync(logEntry);
        }
    }
}