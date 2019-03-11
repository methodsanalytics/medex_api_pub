﻿using System.Linq;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.PatientDetails;

namespace MedicalExaminer.Common.Services.PatientDetails
{
    public class PatientDetailsUpdateService : IAsyncQueryHandler<PatientDetailsUpdateQuery, Models.Examination>
    {
        private readonly IDatabaseAccess _databaseAccess;
        private readonly IExaminationConnectionSettings _connectionSettings;

        public PatientDetailsUpdateService(IDatabaseAccess databaseAccess,
            IExaminationConnectionSettings connectionSettings)
        {
            _databaseAccess = databaseAccess;
            _connectionSettings = connectionSettings;
        }
        public async Task<Models.Examination> Handle(PatientDetailsUpdateQuery param)
        {
            var caseToReplace = await
                _databaseAccess
                    .GetItemAsync<Models.Examination>(_connectionSettings, examination => examination.Id == param.CaseId);

            caseToReplace.GivenNames = param.PatientDetails.GivenNames;
            caseToReplace.Surname = param.PatientDetails.Surname;
            caseToReplace.Country = param.PatientDetails.Country;
            //caseToReplace.
            var result = await _databaseAccess.UpdateItemAsync(_connectionSettings, caseToReplace);
            return result;
        }
    }
}
