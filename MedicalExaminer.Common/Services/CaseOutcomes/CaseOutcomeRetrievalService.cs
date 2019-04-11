using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.CaseOutcomes;
using MedicalExaminer.Common.Queries.PatientDetails;

namespace MedicalExaminer.Common.Services.CaseOutcomes
{
    public class CaseOutcomeRetrievalService : IAsyncQueryHandler<CaseOutcomesRetrievalQuery, Models.Examination>
    {
        private readonly IConnectionSettings _connectionSettings;
        private readonly IDatabaseAccess _databaseAccess;

        public CaseOutcomeRetrievalService(
            IDatabaseAccess databaseAccess,
            IExaminationConnectionSettings connectionSettings)
        {
            _databaseAccess = databaseAccess;
            _connectionSettings = connectionSettings;
        }

        public async Task<Models.Examination> Handle(CaseOutcomesRetrievalQuery param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            var result = await _databaseAccess.GetItemAsync<Models.Examination>(_connectionSettings,
                examination => examination.ExaminationId == param.ExaminationId);

            return result;
        }
    }
}
