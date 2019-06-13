using System;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.CaseBreakdown;
using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Services.Examination
{
    /// <summary>
    /// Create event service.
    /// </summary>
    public class CreateEventService : IAsyncQueryHandler<CreateEventQuery, EventCreationResult>
    {
        private readonly IConnectionSettings _connectionSettings;
        private readonly IDatabaseAccess _databaseAccess;

        /// <summary>
        /// Initialise a new instance of <see cref="CreateEventService"/>.
        /// </summary>
        /// <param name="databaseAccess">Database access.</param>
        /// <param name="connectionSettings">Connection settings.</param>
        public CreateEventService(
            IDatabaseAccess databaseAccess,
            IExaminationConnectionSettings connectionSettings)
        {
            _databaseAccess = databaseAccess;
            _connectionSettings = connectionSettings;
        }

        /// <summary>
        /// Handle the query.
        /// </summary>
        /// <param name="param">The query.</param>
        /// <returns>Event creation result.</returns>
        public async Task<EventCreationResult> Handle(CreateEventQuery param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            var examinationToUpdate = await
                            _databaseAccess
                                .GetItemAsync<Models.Examination>(
                                    _connectionSettings,
                                    examination => examination.ExaminationId == param.CaseId);

            examinationToUpdate = examinationToUpdate
                                            .AddEvent(param.Event)
                                            .UpdateCaseUrgencyScore()
                                            .UpdateCaseStatus();

            examinationToUpdate.LastModifiedBy = param.Event.UserId;
            examinationToUpdate.ModifiedAt = DateTime.Now;
            var result = await _databaseAccess.UpdateItemAsync(_connectionSettings, examinationToUpdate);
            return new EventCreationResult(param.Event.EventId, result);
        }
    }
}