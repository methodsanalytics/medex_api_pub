﻿using System;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Extensions.MeUser;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Common.Queries.Location;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.Common.Services.Examination
{
    /// <summary>
    /// Create Examination Service.
    /// </summary>
    public class CreateExaminationService : IAsyncQueryHandler<CreateExaminationQuery, Models.Examination>
    {
        private readonly IDatabaseAccess _databaseAccess;
        private readonly IConnectionSettings _connectionSettings;
        private readonly IAsyncQueryHandler<LocationRetrievalByIdQuery, Models.Location> _locationHandler;

        /// <summary>
        /// Initialise a new instance of <see cref="CreateExaminationService"/>.
        /// </summary>
        /// <param name="databaseAccess">Database Access.</param>
        /// <param name="connectionSettings">Connection Settings.</param>
        /// <param name="locationHandler">Location Handler.</param>
        public CreateExaminationService(
            IDatabaseAccess databaseAccess,
            IExaminationConnectionSettings connectionSettings,
            IAsyncQueryHandler<LocationRetrievalByIdQuery, Models.Location> locationHandler)
        {
            _databaseAccess = databaseAccess;
            _connectionSettings = connectionSettings;
            _locationHandler = locationHandler;
        }

        /// <summary>
        /// Handle.
        /// </summary>
        /// <param name="param">Query.</param>
        /// <returns>Examination.</returns>
        public async Task<Models.Examination> Handle(CreateExaminationQuery param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            param.Examination.ExaminationId = Guid.NewGuid().ToString();
            param.Examination.MedicalExaminerOfficeResponsibleName = _locationHandler.Handle(new LocationRetrievalByIdQuery(param.Examination.MedicalExaminerOfficeResponsible)).Result.Name;
            param.Examination.Unassigned = true;
            param.Examination.CaseBreakdown = new CaseBreakDown();

            param.Examination.CaseBreakdown.DeathEvent = new DeathEvent()
            {
                Created = param.Examination.CreatedAt.DateTime,
                DateOfDeath = param.Examination.DateOfDeath,
                TimeOfDeath = param.Examination.TimeOfDeath,
                UserId = param.Examination.CreatedBy,
                UsersRole = param.User.UsersRoleIn(new[] { UserRoles.MedicalExaminer, UserRoles.MedicalExaminerOfficer }).ToString(),
                UserFullName = param.User.FullName(),
                EventId = Guid.NewGuid().ToString()
            };
            param.Examination.UpdateCaseUrgencyScore();
            param.Examination.LastModifiedBy = param.User.UserId;
            param.Examination.ModifiedAt = DateTime.Now;

            return await _databaseAccess.CreateItemAsync(_connectionSettings, param.Examination);
        }
    }
}