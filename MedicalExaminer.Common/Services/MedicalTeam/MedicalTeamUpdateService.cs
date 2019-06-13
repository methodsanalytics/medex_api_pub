using System;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Common.Extensions.MeUser;
using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Services.MedicalTeam
{
    /// <summary>
    /// Medical Team Update Service.
    /// </summary>
    public class MedicalTeamUpdateService : IAsyncUpdateDocumentHandler
    {
        private readonly IConnectionSettings _connectionSettings;
        private readonly IDatabaseAccess _databaseAccess;
        private readonly IAsyncQueryHandler<UserRetrievalByIdQuery, MeUser> _userService;

        /// <summary>
        /// Initialise a new instance of <see cref="MedicalTeamUpdateService"/>.
        /// </summary>
        /// <param name="databaseAccess">Database access.</param>
        /// <param name="connectionSettings">Connection settings.</param>
        /// <param name="userService">User service.</param>
        public MedicalTeamUpdateService(
            IDatabaseAccess databaseAccess,
            IExaminationConnectionSettings connectionSettings,
            IAsyncQueryHandler<UserRetrievalByIdQuery, MeUser> userService)
        {
            _databaseAccess = databaseAccess;
            _connectionSettings = connectionSettings;
            _userService = userService;
        }

        /// <summary>
        /// Handle query.
        /// </summary>
        /// <param name="examination">Examination.</param>
        /// <param name="userId">User id.</param>
        /// <returns>Examination with medical team updated.</returns>
        public async Task<Models.Examination> Handle(Models.Examination examination, string userId)
        {
            if (examination == null)
            {
                throw new ArgumentNullException(nameof(examination));
            }

            var me = await _userService.Handle(new UserRetrievalByIdQuery(examination.MedicalTeam.MedicalExaminerUserId));
            var meo = await _userService.Handle(new UserRetrievalByIdQuery(examination.MedicalTeam.MedicalExaminerOfficerUserId));

            examination.MedicalTeam.MedicalExaminerFullName = me.FullName();
            examination.MedicalTeam.MedicalExaminerOfficerFullName = meo.FullName();

            examination = examination.UpdateCaseUrgencyScore();
            examination = examination.UpdateCaseStatus();
            examination.LastModifiedBy = userId;
            examination.ModifiedAt = DateTime.Now;

            var returnedDocument = await _databaseAccess.UpdateItemAsync(_connectionSettings, examination);

            return returnedDocument;
        }
    }
}