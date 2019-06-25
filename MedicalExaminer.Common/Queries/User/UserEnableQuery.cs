using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Queries.User
{
    /// <summary>
    /// User Enable  Query.
    /// </summary>
    public class UserEnableQuery : IQuery<MeUser>
    {
        /// <summary>
        /// Initialise a new instance of <see cref="UserEnableQuery"/>.
        /// </summary>
        /// <param name="userId">The ID of the user who to enable or disable</param>
        /// <param name="enableUser">True to enable. False to disable.</param>
        /// <param name="currentUser">The user performing the change.</param>
        public UserEnableQuery(string userId, bool enableUser, MeUser currentUser)
        {
            CurrentUser = currentUser;
            UserId = userId;
            EnableUser = enableUser;
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
        /// Enable User.
        /// </summary>
        /// <remarks>True to enable, False to disable</remarks>
        public bool EnableUser { get; }
    }
}