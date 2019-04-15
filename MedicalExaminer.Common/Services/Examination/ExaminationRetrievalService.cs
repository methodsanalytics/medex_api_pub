using System;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.Examination;
using Microsoft.Extensions.Logging;

namespace MedicalExaminer.Common.Services.Examination
{
    /// <summary>
    /// Examination Retrieval Service.
    /// </summary>
    public class ExaminationRetrievalService : QueryHandler<ExaminationRetrievalQuery, Models.Examination>
    {
        /// <summary>
        /// Initialise a new instance of <see cref="ExaminationRetrievalService"/>.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="databaseAccess">Database Access.</param>
        /// <param name="connectionSettings">Connection Settings.</param>
        public ExaminationRetrievalService(
            ILogger<ExaminationRetrievalService> logger,
            IDatabaseAccess databaseAccess,
            IExaminationConnectionSettings connectionSettings)
        : base(logger, databaseAccess, connectionSettings)
        {
        }

        /// <inheritdoc/>
        public override Task<Models.Examination> Handle(ExaminationRetrievalQuery param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            try
            {

                var result = GetItemAsync(x => x.ExaminationId == param.ExaminationId);

                if (result.Result != null)
                {
                    return Task.FromResult(result.Result);
                }

                return result;
            }
            catch (Exception e)
            {
                Logger.LogCritical();
                throw;
            }
        }
    }
}