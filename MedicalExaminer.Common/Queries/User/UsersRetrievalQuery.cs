using System.Collections.Generic;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.Common.Queries.User
{
    /// <summary>
    /// Users Retrieval Query.
    /// </summary>
    public class UsersRetrievalQuery : IQuery<IEnumerable<MeUser>>
    {
        /// <summary>
        /// Initialise a new instance of <see cref="UsersRetrievalQuery"/>.
        /// </summary>
        /// <param name="userRole">User Role.</param>
        public UsersRetrievalQuery(UserRoles? userRole)
        {
            Role = userRole;
        }

        /// <summary>
        /// Role.
        /// </summary>
        public UserRoles? Role { get; }
    }
}