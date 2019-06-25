﻿using System.Net;
using System.Net.Http.Headers;
using System.Reflection;
using Microsoft.Azure.Documents;

namespace MedicalExaminer.API.Tests.Helpers
{
    public static class CosmosExceptionThrowerHelper
    {
        public static DocumentClientException CreateDocumentClientExceptionForTesting(
                                          Error error, HttpStatusCode httpStatusCode)
        {
            var type = typeof(DocumentClientException);

            // we are using the overload with 3 parameters (error, responseheaders, statuscode)
            // use any one appropriate for you.

            var documentClientExceptionInstance = type.Assembly.CreateInstance(type.FullName,
                false, BindingFlags.Instance | BindingFlags.NonPublic, null,
                new object[] { error, (HttpResponseHeaders)null, httpStatusCode }, null, null);

            return (DocumentClientException)documentClientExceptionInstance;
        }
    }
}
