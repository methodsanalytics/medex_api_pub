﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Medical_Examiner_API.Loggers
{
    public class LogMessageActionDefault
    {
        public string UserName { get; private set; }
        public string UserAuthenticationType { get; private set; }
        public bool UserIsAuthenticated { get; private set; }
        public string ControllerName { get; private set; }
        public string ControllerMethod { get; private set; }
        public IList<string> Parameters { get; private set; }
        public string RemoteIP { get; private set; }
        public DateTime TimeStamp { get; private set; }
        public LogMessageActionDefault(string userName, string userAuthenticationType, bool userIsAuthenticated, string controllerName, string controllerMethod, IList<string> parameters, string remoteIP, DateTime timestamp)
        {
            UserName = userName;
            UserAuthenticationType = userAuthenticationType;
            UserIsAuthenticated = userIsAuthenticated;
            ControllerName = controllerName;
            ControllerMethod = controllerMethod;
            Parameters = parameters;
            RemoteIP = remoteIP;
            TimeStamp = timestamp;

        }
    }
}
