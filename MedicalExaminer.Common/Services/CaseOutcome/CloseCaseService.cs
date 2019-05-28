﻿using System;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Extensions.MeUser;
using MedicalExaminer.Common.Queries.CaseOutcome;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Services.CaseOutcome
{
    public class CloseCaseService : IAsyncQueryHandler<CloseCaseQuery, string>
    {
        private readonly IConnectionSettings _connectionSettings;
        private readonly IDatabaseAccess _databaseAccess;

        public CloseCaseService(
            IDatabaseAccess databaseAccess,
            IExaminationConnectionSettings connectionSettings)
        {
            _connectionSettings = connectionSettings;
            _databaseAccess = databaseAccess;
        }

        public async Task<string> Handle(CloseCaseQuery param)
        {
            if (string.IsNullOrEmpty(param.ExaminationId))
            {
                throw new ArgumentNullException(nameof(param.ExaminationId));
            }

            if (param.User == null)
            {
                throw new ArgumentNullException(nameof(param.User));
            }


            var examinationToUpdate = await
                _databaseAccess
                    .GetItemAsync<Models.Examination>(
                        _connectionSettings,
                        examination => examination.ExaminationId == param.ExaminationId);

            if (!examinationToUpdate.OutstandingCaseItemsCompleted)
            {
                return null;
            }

            examinationToUpdate.LastModifiedBy = param.User.UserId;
            examinationToUpdate.ModifiedAt = DateTime.Now;
            examinationToUpdate.CaseCompleted = true;
            examinationToUpdate.CaseOutcome.CaseCompleted = false;

            examinationToUpdate.CaseBreakdown.CaseClosedEvent = new CaseClosedEvent()
            {
                Created = DateTime.Now,
                DateCaseClosed = DateTime.Now,
                EventId = Guid.NewGuid().ToString(),
                UserFullName = param.User.FullName(),
                UserId = param.User.UserId,
                UsersRole = param.User.Role()?.ToString()
            };

            examinationToUpdate = examinationToUpdate.UpdateCaseUrgencyScore();
            examinationToUpdate = examinationToUpdate.UpdateCaseStatus();

            var result = await _databaseAccess.UpdateItemAsync(_connectionSettings, examinationToUpdate);
            return result.ExaminationId;
        }
    }
}
