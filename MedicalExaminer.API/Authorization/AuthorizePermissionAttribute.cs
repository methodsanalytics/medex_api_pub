using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedicalExaminer.Common.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace MedicalExaminer.API.Authorization
{
    /// <summary>
    /// Authorize Permission Attribute.
    /// </summary>
    public class AuthorizePermissionAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// Initialise a new instance of <see cref="AuthorizePermissionAttribute"/>.
        /// </summary>
        /// <param name="permission">Permission.</param>
        public AuthorizePermissionAttribute(Permission permission)
            : base($"HasPermission={permission.ToString()}")
        {
        }
    }
}
