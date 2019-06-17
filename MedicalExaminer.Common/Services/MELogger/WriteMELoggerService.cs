using System;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Extensions.MeUser;
using MedicalExaminer.Common.Loggers;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Common.Queries.Location;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.Common.Services.Examination
{
    /// <summary>
    /// Create Examination Service.
    /// </summary>
    public class WriteMELoggerService : QueryHandler<LogMessageActionDefault, string>
    {
        /// <summary>
        /// Initialise a new instance of <see cref="WriteMELoggerService"/>.
        /// </summary>
        /// <param name="databaseAccess">Database Access.</param>
        /// <param name="connectionSettings">Connection Settings.</param>
        public WriteMELoggerService(
            IDatabaseAccess databaseAccess,
            IMELoggerConnectionSettings connectionSettings)
        : base(databaseAccess, connectionSettings)
        {
        }

        /// <inheritdoc/>
        public override async Task<string> Handle(LogMessageActionDefault param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            await DatabaseAccess.CreateItemAsync<LogMessageActionDefault>(ConnectionSettings, param);

            return string.Empty;
        }
    }
}