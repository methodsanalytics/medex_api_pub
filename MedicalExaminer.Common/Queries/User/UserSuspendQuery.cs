using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Queries.User
{
    /// <summary>
    /// User Suspend Query.
    /// </summary>
    public class UserSuspendQuery : AuthenticatedQuery<MeUser>
    {
        /// <summary>
        /// Initialise a new instance of <see cref="UserSuspendQuery"/>.
        /// </summary>
        /// <param name="userId">The ID of the user who to suspend.</param>
        /// <param name="suspend">True to suspend a user.</param>
        /// <param name="authenticatedUser">The authenticated user performing the query.</param>
        public UserSuspendQuery(string userId, bool suspend, MeUser authenticatedUser)
            : base(authenticatedUser)
        {
            UserId = userId;
            Suspend = suspend;
        }

        /// <summary>
        /// User Id
        /// </summary>
        /// <remarks>The id of the user to change.</remarks>
        public string UserId { get; }

        /// <summary>
        /// Suspend User.
        /// </summary>
        /// <remarks>True to suspend</remarks>
        public bool Suspend { get; }
    }
}