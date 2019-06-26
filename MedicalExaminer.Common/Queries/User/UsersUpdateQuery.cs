using System.Collections.Generic;
using System.Linq;
using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Queries.User
{
    /// <summary>
    /// User Update Query.
    /// </summary>
    public class UserUpdateQuery : AuthenticatedQuery<MeUser>
    {
        /// <summary>
        /// User Update Query.
        /// </summary>
        /// <param name="userToUpdate">User to update.</param>
        /// <param name="authenticatedUser">Authenticated User.</param>
        public UserUpdateQuery(MeUser userToUpdate, MeUser authenticatedUser)
            : base(authenticatedUser)
        {
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

        public string UserId { get; }

        public string Email { get; }

        public IEnumerable<MEUserPermission> Permissions { get; }
    }
}