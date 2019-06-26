using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Queries.User
{
    /// <summary>
    /// User Suspend Query.
    /// </summary>
    public class UserSuspendQuery : IQuery<MeUser>
    {
        /// <summary>
        /// Initialise a new instance of <see cref="UserSuspendQuery"/>.
        /// </summary>
        /// <param name="userId">The ID of the user who to suspend.</param>
        /// <param name="suspend">True to suspend a user.</param>
        /// <param name="currentUser">The user performing the change.</param>
        public UserSuspendQuery(string userId, bool suspend, MeUser currentUser)
        {
            CurrentUser = currentUser;
            UserId = userId;
            Suspend = suspend;
        }

        /// <summary>
        /// Current User
        /// </summary>
        /// <remarks>The user making the change.</remarks>
        public MeUser CurrentUser { get; }

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