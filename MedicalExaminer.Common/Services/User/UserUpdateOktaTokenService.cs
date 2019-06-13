using System;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.User;

namespace MedicalExaminer.Common.Services.User
{
    /// <summary>
    /// User Update Okta Token Service.
    /// </summary>
    public class UserUpdateOktaTokenService : IAsyncQueryHandler<UsersUpdateOktaTokenQuery, Models.MeUser>
    {
        private readonly IConnectionSettings _connectionSettings;
        private readonly IDatabaseAccess _databaseAccess;

        /// <summary>
        /// Initialise a new instance of <see cref="UserUpdateOktaTokenService"/>.
        /// </summary>
        /// <param name="databaseAccess">Database access</param>
        /// <param name="connectionSettings">Connection settings</param>
        public UserUpdateOktaTokenService(
            IDatabaseAccess databaseAccess,
            IUserConnectionSettings connectionSettings)
        {
            _databaseAccess = databaseAccess;
            _connectionSettings = connectionSettings;
        }

        /// <summary>
        /// Handle query.
        /// </summary>
        /// <param name="userUpdate">The query.</param>
        /// <returns>User.</returns>
        public async Task<Models.MeUser> Handle(UsersUpdateOktaTokenQuery userUpdate)
        {
            if (userUpdate == null)
            {
                throw new ArgumentNullException(nameof(userUpdate));
            }

            var userToUpdate =
                _databaseAccess
                    .GetItemAsync<Models.MeUser>(
                        _connectionSettings,
                        meUser => meUser.UserId == userUpdate.UserId).Result;

            if (userToUpdate == null)
            {
                throw new ArgumentNullException(nameof(userToUpdate));
            }

            userToUpdate.OktaToken = userUpdate.OktaToken;
            userToUpdate.OktaTokenExpiry = userUpdate.OktaTokenExpiry;

            var result = await _databaseAccess.UpdateItemAsync(_connectionSettings, userToUpdate);
            return result;
        }
    }
}
