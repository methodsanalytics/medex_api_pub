using System;

namespace MedicalExaminer.API.Helpers
{
    /// <summary>
    /// Extracts token element from HttpRequest Authorization string
    /// </summary>
    public class OktaTokenParser
    {
        /// <summary>
        /// Parse Http Request Authorisation.
        /// </summary>
        /// <param name="httpRequestAuthorization">Http Request Authorisation.</param>
        /// <returns>The token.</returns>
        public static string ParseHttpRequestAuthorisation(string httpRequestAuthorization)
        {
            const string httpRequestPrefix = "Bearer ";

            if (httpRequestAuthorization == null)
            {
                throw new ArgumentNullException();
            }

            if (httpRequestAuthorization.Length < httpRequestPrefix.Length)
            {
                throw new ArgumentException("httpRequestAuthorization insufficient length");
            }

            return httpRequestAuthorization.Substring(httpRequestPrefix.Length);
        }
    }
}
