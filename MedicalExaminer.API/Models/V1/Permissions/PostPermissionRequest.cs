using System.ComponentModel.DataAnnotations;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.API.Models.V1.Permissions
{
    /// <summary>
    ///     Post Permission Request.
    /// </summary>
    public class PostPermissionRequest
    {
        /// <summary>
        ///     Gets or sets the location ID.
        /// </summary>
        [Required]
        public string LocationId { get; set; }

        /// <summary>
        ///     Gets or sets the User Role for the Permission.
        /// </summary>
        [Required]
        public UserRoles UserRole { get; set; }
    }
}