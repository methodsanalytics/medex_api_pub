﻿using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.PatientDetails;
using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Services.PatientDetails
{
    public class PatientDetailsRetrievalService : IAsyncQueryHandler<PatientDetailsByCaseIdQuery, Models.Examination>
    {
        private readonly IDatabaseAccess _databaseAccess;
        private readonly IConnectionSettings _connectionSettings;
        public PatientDetailsRetrievalService(IDatabaseAccess databaseAccess, IExaminationConnectionSettings connectionSettings)
        {
            _databaseAccess = databaseAccess;
            _connectionSettings = connectionSettings;
        }
        public async Task<Models.Examination> Handle(PatientDetailsByCaseIdQuery param)
        {
            var result = await _databaseAccess.GetItemAsync<Models.Examination>(_connectionSettings,
                examination => examination.id == param.ExaminationId);

            return result;
        }
    }
}
