using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Queries.User
{
    /// <summary>
    /// Create User Query.
    /// </summary>
    public class CreateUserQuery : AuthenticatedQuery<MeUser>
    {
        /// <summary>
        /// Initialise a new instance of <see cref="CreateUserQuery"/>.
        /// </summary>
        /// <param name="userToCreate">User to create.</param>
        /// <param name="authenticatedUser">Authenticated user making the query</param>
        public CreateUserQuery(MeUser userToCreate, MeUser authenticatedUser)
            : base(authenticatedUser)
        {
            UserToCreate = userToCreate;
        }

        /// <summary>
        /// User to create.
        /// </summary>
        public MeUser UserToCreate { get; }
    }
}