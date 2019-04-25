using MedicalExaminer.Common.Authorization;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MedicalExaminer.Common.Extensions.MeUser
{
    public static class MeUserExtensions
    {
        /// <summary>
        /// Get the full name, combining last name and first name
        /// </summary>
        /// <param name="meUser">User object.</param>
        /// <returns>Full name string.</returns>
        public static string FullName(this Models.MeUser meUser)
        {
            // TODO: Do we have a standard way of doing this? Can we assume this format? Does it need to be last name first?
            return $"{meUser.FirstName} {meUser.LastName}";
        }

        /// <summary>
        /// Role For Examination.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="examination">The examination.</param>
        /// <returns>The highest role they have for this examination.</returns>
        public static UserRoles RoleForExamination(this Models.MeUser user, Examination examination)
        {
            var locations = examination.LocationIds();

            var permissions = user.Permissions.Where(p => locations.Contains(p.LocationId));

            var topPermission = permissions.OrderByDescending(p => p.UserRole).First();

            return (UserRoles)topPermission.UserRole;
        }
    }
}
