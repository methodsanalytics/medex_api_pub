using System;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.User;

namespace MedicalExaminer.Common.Services.User
{
    /// <summary>
    /// User Delete Service.
    /// </summary>
    /// <remarks>Soft Delete. Marks the user as deleted and updates the modified by.</remarks>
    public class UserDeleteService : QueryHandler<UserDeleteQuery, Models.MeUser>
    {
        /// <summary>
        /// Initialise a new instance of <see cref="UserUpdateService"/>.
        /// </summary>
        /// <param name="databaseAccess">Database Access.</param>
        /// <param name="connectionSettings">User Connection Settings.</param>
        public UserDeleteService(IDatabaseAccess databaseAccess, IUserConnectionSettings connectionSettings)
            : base(databaseAccess, connectionSettings)
        {
        }

        /// <inheritdoc/>
        public override async Task<Models.MeUser> Handle(UserDeleteQuery param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            var userToUpdate = await GetItemByIdAsync(param.UserId);

            if (userToUpdate == null)
            {
                throw new InvalidOperationException($"User with id `{param.UserId}` not found.");
            }

            userToUpdate.Deleted = true;
            userToUpdate.LastModifiedBy = param.AuthenticatedUser.UserId;
            userToUpdate.ModifiedAt = DateTime.UtcNow;

            var result = await UpdateItemAsync(userToUpdate);
            return result;
        }
    }
}