using System;
using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Queries.User
{
    /// <summary>
    /// User Retrieval By Okta Token Query.
    /// </summary>
    public class UserRetrievalByOktaTokenQuery : IQuery<MeUser>
    {
        /// <summary>
        /// Initialise a new instance of <see cref="UserRetrievalByOktaTokenQuery"/>.
        /// </summary>
        /// <param name="user">User.</param>
        public UserRetrievalByOktaTokenQuery(MeUser user)
        {
            OktaToken = user.OktaToken;
            OktaTokenExpiry = user.OktaTokenExpiry;
        }

        /// <summary>
        /// Okta Token.
        /// </summary>
        public string OktaToken { get; }

        /// <summary>
        /// Okta Token Expiry.
        /// </summary>
        public DateTimeOffset OktaTokenExpiry { get; }
    }
}
