using System;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.CaseOutcome;
using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Services.CaseOutcome
{
    /// <summary>
    /// Confirmation of Scrutiny Service.
    /// </summary>
    public class ConfirmationOfScrutinyService : QueryHandler<ConfirmationOfScrutinyQuery, Models.Examination>
    {
        /// <summary>
        /// Initialise a new instance of <see cref="ConfirmationOfScrutinyService"/>.
        /// </summary>
        /// <param name="databaseAccess">Database access.</param>
        /// <param name="connectionSettings">Connection settings.</param>
        public ConfirmationOfScrutinyService(
            IDatabaseAccess databaseAccess,
            IExaminationConnectionSettings connectionSettings)
            : base(databaseAccess, connectionSettings)
        {
        }

        /// <inheritdoc/>
        public override async Task<Models.Examination> Handle(ConfirmationOfScrutinyQuery param)
        {
            var examinationToUpdate = await
                DatabaseAccess
                    .GetItemAsync<Models.Examination>(
                        ConnectionSettings,
                        examination => examination.ExaminationId == param.ExaminationId);

            examinationToUpdate.ConfirmationOfScrutinyCompletedAt = DateTime.Now;
            examinationToUpdate.ConfirmationOfScrutinyCompletedBy = param.User.UserId;
            examinationToUpdate.ModifiedAt = DateTimeOffset.Now;
            examinationToUpdate.LastModifiedBy = param.User.UserId;
            examinationToUpdate.CaseOutcome.ScrutinyConfirmedOn = DateTime.Now;
            examinationToUpdate.ScrutinyConfirmed = true;

            examinationToUpdate.UpdateCaseStatus();
            examinationToUpdate.UpdateCaseUrgencyScore();

            var result = await DatabaseAccess.UpdateItemAsync(ConnectionSettings, examinationToUpdate);
            return result;
        }
    }
}
