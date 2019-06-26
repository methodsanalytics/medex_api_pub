using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Queries.User
{
    /// <summary>
    /// User Delete Query.
    /// </summary>
    public class UserDeleteQuery : AuthenticatedQuery<MeUser>
    {
        /// <summary>
        /// Initialise a new instance of <see cref="UserDeleteQuery"/>
        /// </summary>
        /// <param name="userId">The user identifier to delete.</param>
        /// <param name="authenticatedUser">The authenticated user performing the query.</param>
        public UserDeleteQuery(string userId, MeUser authenticatedUser)
            : base(authenticatedUser)
        {
            UserId = userId;
        }

        /// <summary>
        /// User Id
        /// </summary>
        /// <remarks>The ID of the user who is to be deleted.</remarks>
        public string UserId { get; }
    }
}