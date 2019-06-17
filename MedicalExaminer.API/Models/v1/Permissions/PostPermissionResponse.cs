﻿using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.API.Models.V1.Permissions
{
    /// <inheritdoc />
    /// <summary>
    ///     Response for Post Permission.
    /// </summary>
    public class PostPermissionResponse : ResponseBase
    {
        /// <summary>
        ///     Gets or sets the permission ID.
        /// </summary>
        public string PermissionId { get; set; }

        /// <summary>
        ///     Gets or sets the User identifier.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        ///     Gets or sets the location ID.
        /// </summary>
        public string LocationId { get; set; }

        /// <summary>
        ///     Gets or sets the User Role for the Permission.
        /// </summary>
        public UserRoles UserRole { get; set; }
    }
}