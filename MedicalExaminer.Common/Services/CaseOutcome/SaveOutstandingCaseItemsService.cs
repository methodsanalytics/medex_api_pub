using System;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.CaseOutcome;
using MedicalExaminer.Common.Settings;
using MedicalExaminer.Models;
using Microsoft.Extensions.Options;

namespace MedicalExaminer.Common.Services.CaseOutcome
{
    /// <summary>
    /// Save Outstanding Case Items Service
    /// </summary>
    public class SaveOutstandingCaseItemsService : IAsyncQueryHandler<SaveOutstandingCaseItemsQuery, string>
    {
        private readonly IConnectionSettings _connectionSettings;
        private readonly IDatabaseAccess _databaseAccess;
        private readonly UrgencySettings _urgencySettings;

        /// <summary>
        /// Constructor for Save Outstanding Case Items Service
        /// </summary>
        /// <param name="databaseAccess">Instance of IConnectionSettings</param>
        /// <param name="connectionSettings">Instance of IDatabaseAccess</param>
        /// <param name="urgencySettings">Instance of UrgencySettings</param>
        public SaveOutstandingCaseItemsService(
            IDatabaseAccess databaseAccess,
            IExaminationConnectionSettings connectionSettings,
            IOptions<UrgencySettings> urgencySettings)
        {
            _connectionSettings = connectionSettings;
            _databaseAccess = databaseAccess;
            _urgencySettings = urgencySettings.Value;
        }

        /// <summary>
        /// Handle - Save Outstanding Case Items
        /// </summary>
        /// <param name="param">Save Outstanding Case Items Query</param>
        /// <returns>Examination Id</returns>
        /// <exception cref="ArgumentNullException">Argument Null Exception</exception>
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
                    .GetItemByIdAsync<Models.Examination>(
                        _connectionSettings,
                        param.ExaminationId);

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
            examinationToUpdate = examinationToUpdate.UpdateCaseUrgencySort(_urgencySettings.DaysToPreCalculateUrgencySort);
            examinationToUpdate = examinationToUpdate.UpdateCaseStatus();

            var result = await _databaseAccess.UpdateItemAsync(_connectionSettings, examinationToUpdate);
            return result.ExaminationId;
        }
    }
}
