using System;
using System.Collections.Generic;
using System.Text;
using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Queries
{
    /// <summary>
    /// Authenticated Query.
    /// </summary>
    /// <typeparam name="T">Type of query.</typeparam>
    public class AuthenticatedQuery<T> : IQuery<T>
    {
        /// <summary>
        /// Initialise a new instance of <see cref="AuthenticatedQuery{T}"/>.
        /// </summary>
        /// <param name="authenticatedUser">The authenticated user.</param>
        public AuthenticatedQuery(MeUser authenticatedUser)
        {
            AuthenticatedUser = authenticatedUser;
        }

        /// <summary>
        /// Authenticated User.
        /// </summary>
        public MeUser AuthenticatedUser { get; }
    }
}
