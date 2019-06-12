using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Queries.User
{
    /// <summary>
    /// User Retrieval by Id Query.
    /// </summary>
    public class UserRetrievalByIdQuery : IQuery<MeUser>
    {
        /// <summary>
        /// Initialise a new instance of <see cref="UserRetrievalByIdQuery"/>.
        /// </summary>
        /// <param name="userId">User Id.</param>
        public UserRetrievalByIdQuery(string userId)
        {
            UserId = userId;
        }

        /// <summary>
        /// User Id.
        /// </summary>
        public string UserId { get; }
    }
}
