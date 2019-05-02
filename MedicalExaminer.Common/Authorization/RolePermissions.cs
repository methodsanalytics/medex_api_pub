﻿using System;
using System.Collections.Generic;
using System.Linq;
using MedicalExaminer.Common.Authorization.Roles;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.Common.Authorization
{
    /// <summary>
    /// Role Permissions.
    /// </summary>
    /// <inheritdoc cref="IRolePermissions"/>
    public class RolePermissions : IRolePermissions
    {
        /// <summary>
        /// Roles.
        /// </summary>
        private readonly IDictionary<UserRoles, Role> _roles;

        /// <summary>
        /// Initialise a new instance of <see cref="RolePermissions"/>.
        /// </summary>
        /// <param name="roles">List of roles.</param>
        public RolePermissions(IEnumerable<Role> roles)
        {
            // Ensures only a single role handler can be registered againt the enum type.
            _roles = roles.ToDictionary(r => r.UserRole, r => r);
        }

        /// <inheritdoc/>
        public bool Can(UserRoles role, Permission permission)
        {
            try
            {
                return _roles[role].Can(permission);
            }
            catch
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public IDictionary<Permission, IEnumerable<UserRoles>> PermissionsForRoles(IEnumerable<UserRoles> roles)
        {
            var result = new Dictionary<Permission, HashSet<UserRoles>>();

            var values = Enum.GetValues(typeof(Permission));

            foreach (var value in values)
            {
                result[(Permission)value] = new HashSet<UserRoles>();
            }

            foreach (var role in roles)
            {
                var permissions = _roles[role].Granted;

                foreach (var permission in permissions)
                {
                    result[permission].Add(role);
                }
            }

            return result
                .ToDictionary(
                    k => k.Key,
                    v => v.Value.AsEnumerable());
        }
    }
}
