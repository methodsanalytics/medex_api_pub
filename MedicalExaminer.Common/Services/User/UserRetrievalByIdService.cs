using System;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.User;
using Microsoft.Extensions.Logging;

namespace MedicalExaminer.Common.Services.User
{
    /// <summary>
    /// User Retrieval by Id Service.
    /// </summary>
    public class UserRetrievalByIdService : QueryHandler<UserRetrievalByIdQuery, Models.MeUser>
    {
        /// <summary>
        /// Initialise a new instance of <see cref="UserRetrievalByIdService"/>.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="databaseAccess">Database Access.</param>
        /// <param name="connectionSettings">Connection Settings.</param>
        public UserRetrievalByIdService(
            ILogger<UserRetrievalByIdService> logger,
            IDatabaseAccess databaseAccess,
            IUserConnectionSettings connectionSettings)
            : base(logger, databaseAccess, connectionSettings)
        {
        }

        /// <inheritdoc/>
        public override Task<Models.MeUser> Handle(UserRetrievalByIdQuery param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            try
            {
                var result = GetItemAsync(x => x.UserId == param.UserId);
                return result;
            }
            catch (Exception e)
            {
                Logger.LogCritical("Failed to retrieve examination data", e);
                throw;
            }
        }
    }
}
