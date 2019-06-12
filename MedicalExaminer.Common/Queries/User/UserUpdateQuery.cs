using System.Collections.Generic;
using System.Linq;
using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Queries.User
{
    /// <summary>
    /// User Update Query.
    /// </summary>
    public class UserUpdateQuery : IQuery<MeUser>
    {
        /// <summary>
        /// Initialise a new instance of <see cref="UserUpdateQuery"/>.
        /// </summary>
        /// <param name="userToUpdate">User to update.</param>
        /// <param name="currentUser">Current user.</param>
        public UserUpdateQuery(MeUser userToUpdate, MeUser currentUser)
        {
            CurrentUser = currentUser;
            UserId = userToUpdate.UserId;
            Email = userToUpdate.Email;
            Permissions = userToUpdate.Permissions != null
                ? userToUpdate.Permissions.Select(up => new MEUserPermission()
                {
                    PermissionId = up.PermissionId,
                    LocationId = up.LocationId,
                    UserRole = up.UserRole,
                })
                : Enumerable.Empty<MEUserPermission>();
        }

        /// <summary>
        /// Current User.
        /// </summary>
        public MeUser CurrentUser { get; }

        /// <summary>
        /// User Id.
        /// </summary>
        public string UserId { get; }

        /// <summary>
        /// Email.
        /// </summary>
        public string Email { get; }

        /// <summary>
        /// Permissions.
        /// </summary>
        public IEnumerable<MEUserPermission> Permissions { get; }
    }
}