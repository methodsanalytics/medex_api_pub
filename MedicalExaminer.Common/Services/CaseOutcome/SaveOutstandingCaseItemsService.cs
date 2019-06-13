﻿using System;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.CaseOutcome;
using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Services.CaseOutcome
{
    /// <summary>
    /// Save Outstanding Case Items Service.
    /// </summary>
    public class SaveOutstandingCaseItemsService : IAsyncQueryHandler<SaveOutstandingCaseItemsQuery, string>
    {
        private readonly IConnectionSettings _connectionSettings;
        private readonly IDatabaseAccess _databaseAccess;

        /// <summary>
        /// Initialise a new instance of <see cref="SaveOutstandingCaseItemsService"/>.
        /// </summary>
        /// <param name="databaseAccess">Database access</param>
        /// <param name="connectionSettings">Connection settings</param>
        public SaveOutstandingCaseItemsService(
            IDatabaseAccess databaseAccess,
            IExaminationConnectionSettings connectionSettings)
        {
            _connectionSettings = connectionSettings;
            _databaseAccess = databaseAccess;
        }

        /// <summary>
        /// Handle query.
        /// </summary>
        /// <param name="param">The query.</param>
        /// <returns>Examination id.</returns>
        public async Task<string> Handle(SaveOutstandingCaseItemsQuery param)
        {
            if (string.IsNullOrEmpty(param.CaseOutcome.ToString()))
            {
                throw new ArgumentNullException(nameof(param.CaseOutcome.ToString));
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

            if (!examinationToUpdate.ScrutinyConfirmed)
            {
                return null;
            }

            examinationToUpdate.LastModifiedBy = param.User.UserId;
            examinationToUpdate.ModifiedAt = DateTime.Now;

            examinationToUpdate.CaseOutcome.MccdIssued = param.CaseOutcome.MccdIssued;
            examinationToUpdate.CaseOutcome.CremationFormStatus = param.CaseOutcome.CremationFormStatus;
            examinationToUpdate.CaseOutcome.GpNotifiedStatus = param.CaseOutcome.GpNotifiedStatus;

            examinationToUpdate.OutstandingCaseItemsCompleted = examinationToUpdate.CalculateOutstandingCaseOutcomesCompleted();
            examinationToUpdate = examinationToUpdate.UpdateCaseUrgencyScore();
            examinationToUpdate = examinationToUpdate.UpdateCaseStatus();

            var result = await _databaseAccess.UpdateItemAsync(_connectionSettings, examinationToUpdate);
            return result.ExaminationId;
        }
    }
}
