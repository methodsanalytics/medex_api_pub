using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Queries.User
{
    /// <summary>
    /// Create User Query.
    /// </summary>
    public class CreateUserQuery : IQuery<MeUser>
    {
        /// <summary>
        /// Initialise a new instance of <see cref="CreateUserQuery"/>.
        /// </summary>
        /// <param name="userToCreate">User to create.</param>
        /// <param name="currentUser">Current user.</param>
        public CreateUserQuery(MeUser userToCreate, MeUser currentUser)
        {
            UserToCreate = userToCreate;
            CurrentUser = currentUser;
        }

        /// <summary>
        /// User to create.
        /// </summary>
        public MeUser UserToCreate { get; }

        /// <summary>
        /// Current User.
        /// </summary>
        public MeUser CurrentUser { get; }
    }
}