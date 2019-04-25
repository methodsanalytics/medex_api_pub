﻿using System;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Enums;
using MedicalExaminer.Common.Extensions.MeUser;
using MedicalExaminer.Common.Queries.CaseBreakdown;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.Common.Services.Examination
{
    public class CreateEventService : IAsyncQueryHandler<CreateEventQuery, string>
    {
        private readonly IConnectionSettings _connectionSettings;
        private readonly IDatabaseAccess _databaseAccess;

        public CreateEventService(
            IDatabaseAccess databaseAccess,
            IExaminationConnectionSettings connectionSettings)
        {
            _databaseAccess = databaseAccess;
            _connectionSettings = connectionSettings;
        }

        public async Task<string> Handle(CreateEventQuery param)
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
            SetEventVariables(param.Event, param.User, examinationToUpdate);
            examinationToUpdate = examinationToUpdate
                                            .AddEvent(param.Event)
                                            .UpdateCaseUrgencyScore();


            examinationToUpdate.LastModifiedBy = param.Event.UserId;
            examinationToUpdate.ModifiedAt = DateTime.Now;
            var result = await _databaseAccess.UpdateItemAsync(_connectionSettings, examinationToUpdate);
            return param.Event.EventId;
        }

        private void SetEventVariables(IEvent theEvent, MeUser user, Models.Examination examination)
        {
            theEvent.UserId = user.UserId;
            theEvent.UserFullName = user.FullName();
            theEvent.UsersRole = user.RoleForExamination(examination).GetDescription();
        }
    }
}