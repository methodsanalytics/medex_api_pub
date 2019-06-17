﻿using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.API.Models.V1.Permissions
{
    /// <summary>
    ///     Put Permission Request.
    /// </summary>
    public class PutPermissionRequest
    {
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