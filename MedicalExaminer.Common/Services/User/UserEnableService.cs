using System;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.User;

namespace MedicalExaminer.Common.Services.User
{
    /// <summary>
    /// User Enable Service.
    /// </summary>
    public class UserEnableService : QueryHandler<UserEnableQuery, Models.MeUser>
    {
        /// <summary>
        /// Initialise a new instance of <see cref="UserEnableService"/>.
        /// </summary>
        /// <param name="databaseAccess">Database Access.</param>
        /// <param name="connectionSettings">User Connection Settings.</param>
        public UserEnableService(IDatabaseAccess databaseAccess, IUserConnectionSettings connectionSettings)
            : base(databaseAccess, connectionSettings)
        {
        }

        /// <inheritdoc/>
        public override async Task<Models.MeUser> Handle(UserEnableQuery param)
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

            userToUpdate.Active = param.EnableUser;

            userToUpdate.LastModifiedBy = param.CurrentUser.UserId;
            userToUpdate.ModifiedAt = DateTime.Now;

            var result = await UpdateItemAsync(userToUpdate);
            return result;
        }
    }
}