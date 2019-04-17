﻿using System;
using System.Threading.Tasks;
using Cosmonaut;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Services.Examination
{
    public class CreateExaminationService : IAsyncQueryHandler<CreateExaminationQuery, Models.Examination>
    {

        private readonly IDatabaseAccess _databaseAccess;
        private readonly IConnectionSettings _connectionSettings;

        public CreateExaminationService(
            IDatabaseAccess databaseAccess,
            IExaminationConnectionSettings connectionSettings)
        {
            _databaseAccess = databaseAccess;
            _connectionSettings = connectionSettings;
        }
        
        public async Task<Models.Examination> Handle(CreateExaminationQuery param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            try
            {
                param.Examination.ExaminationId = Guid.NewGuid().ToString();
                param.Examination.Unassigned = true;
                param.Examination.CaseBreakdown = new CaseBreakDown();
                param.Examination.CaseBreakdown.DeathEvent = new DeathEvent()
                {
                    Created = param.Examination.CreatedAt.Date,
                    DateOfDeath = param.Examination.DateOfDeath,
                    TimeOfDeath = param.Examination.TimeOfDeath,
                    UserId = param.Examination.CreatedBy,
                    EventId = Guid.NewGuid().ToString()
                };
                param.Examination.UpdateCaseUrgencyScore();

                return await _databaseAccess.CreateItemAsync(_connectionSettings, param.Examination);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}