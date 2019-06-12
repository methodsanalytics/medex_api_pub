using System;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.User;

namespace MedicalExaminer.Common.Services.User
{
    /// <summary>
    /// Users Retrieval By Okta Token Service.
    /// </summary>
    public class UsersRetrievalByOktaTokenService : IAsyncQueryHandler<UserRetrievalByOktaTokenQuery, Models.MeUser>
    {
        private readonly IDatabaseAccess _databaseAccess;
        private readonly IUserConnectionSettings _connectionSettings;

        /// <summary>
        /// Initialise a new instance of <see cref="UsersRetrievalByOktaTokenService"/>.
        /// </summary>
        /// <param name="databaseAccess">Database Access.</param>
        /// <param name="connectionSettings">Connection Settings.</param>
        public UsersRetrievalByOktaTokenService(IDatabaseAccess databaseAccess, IUserConnectionSettings connectionSettings)
        {
            _databaseAccess = databaseAccess;
            _connectionSettings = connectionSettings;
        }

        /// <summary>
        /// Handle.
        /// </summary>
        /// <param name="param">Query.</param>
        /// <returns>User</returns>
        public Task<Models.MeUser> Handle(UserRetrievalByOktaTokenQuery param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            var result = _databaseAccess.GetItemAsync<Models.MeUser>(
                _connectionSettings,
                x => x.OktaToken == param.OktaToken);

            if (result.Result != null)
            {
                return result;
            }
            else
            {
                return null;
            }
        }
    }
}
