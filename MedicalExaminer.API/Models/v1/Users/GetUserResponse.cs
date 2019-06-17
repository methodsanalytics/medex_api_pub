using System.Collections.Generic;

namespace MedicalExaminer.API.Models.V1.Users
{
    /// <inheritdoc />
    /// <summary>
    ///     Response object for a single user.
    /// </summary>
    public class GetUserResponse : ResponseBase
    {
        /// <summary>
        ///     The User identifier.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        ///     The User's first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        ///     The User's last name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        ///     The User's email address.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Permissions
        /// </summary>
        public IEnumerable<UserPermission> Permissions { get; set; }
    }
}