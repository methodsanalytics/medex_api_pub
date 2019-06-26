using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.Common.Services.Examination
{
    /// <summary>
    /// Examinations Dashboard Service.
    /// </summary>
    public class ExaminationsDashboardService : QueryHandler<ExaminationsRetrievalQuery, ExaminationsOverview>
    {
        private readonly ExaminationsQueryExpressionBuilder _baseQueryBuilder;

        /// <summary>
        /// Initialise a new instance of <see cref="ExaminationsDashboardService"/>.
        /// </summary>
        /// <param name="databaseAccess">Database Access.</param>
        /// <param name="connectionSettings">Connection Settings.</param>
        /// <param name="baseQueryBuilder">Base Query Builder.</param>
        public ExaminationsDashboardService(
            IDatabaseAccess databaseAccess,
            IExaminationConnectionSettings connectionSettings,
            ExaminationsQueryExpressionBuilder baseQueryBuilder)
            : base(databaseAccess, connectionSettings)
        {
            _baseQueryBuilder = baseQueryBuilder;
        }

        /// <inheritdoc/>
        public override async Task<ExaminationsOverview> Handle(ExaminationsRetrievalQuery param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            var baseQuery = GetBaseQuery(param);

            // Just get all the examinations once from cosmos
           // var examinations = (await GetItemsAsync(baseQuery)).ToList().AsQueryable();

            // Now do all the counting our side.
            var overView = new ExaminationsOverview
            {
                CountOfAdmissionNotesHaveBeenAdded = await GetCount(baseQuery, CaseStatus.AdmissionNotesHaveBeenAdded),
                CountOfUnassigned = await GetCount(baseQuery, CaseStatus.Unassigned),
                CountOfHaveBeenScrutinisedByME = await GetCount(baseQuery, CaseStatus.HaveBeenScrutinisedByME),
                CountOfHaveFinalCaseOutstandingOutcomes = await GetCount(baseQuery, CaseStatus.HaveFinalCaseOutstandingOutcomes),
                CountOfPendingAdmissionNotes = await GetCount(baseQuery, CaseStatus.PendingAdmissionNotes),
                CountOfPendingDiscussionWithQAP = await GetCount(baseQuery, CaseStatus.PendingDiscussionWithQAP),
                CountOfPendingDiscussionWithRepresentative = await GetCount(baseQuery, CaseStatus.PendingDiscussionWithRepresentative),
                CountOfReadyForMEScrutiny = await GetCount(baseQuery, CaseStatus.ReadyForMEScrutiny),
                TotalCases = GetCount(baseQuery).Result,
                CountOfUrgentCases = await GetCount(baseQuery, x => x.UrgencyScore > 0 && x.CaseCompleted == false)
            };

            return overView;
        }

        private async Task<int> GetCount(Expression<Func<Models.Examination, bool>> baseQuery, CaseStatus caseStatus)
        {
            var caseStatusPredicate = GetCaseStatusPredicate(caseStatus);
            var query = baseQuery.And(caseStatusPredicate);

            var result = await DatabaseAccess.GetCountAsync(ConnectionSettings, query);
            return result;
        }

        private async Task<int> GetCount(Expression<Func<Models.Examination, bool>> query)
        {
            return await DatabaseAccess.GetCountAsync(ConnectionSettings, query);
        }

        private async Task<int> GetCount(Expression<Func<Models.Examination, bool>> baseQuery, Expression<Func<Models.Examination, bool>> query)
        {
            var combinedQuery = query.And(baseQuery);
            return await DatabaseAccess.GetCountAsync(ConnectionSettings, combinedQuery);
        }

        private Expression<Func<Models.Examination, bool>> GetCaseStatusPredicate(CaseStatus? paramFilterCaseStatus)
        {
            switch (paramFilterCaseStatus)
            {
                case CaseStatus.AdmissionNotesHaveBeenAdded:
                    return examination => examination.AdmissionNotesHaveBeenAdded;
                case CaseStatus.ReadyForMEScrutiny:
                    return examination => examination.ReadyForMEScrutiny;
                case CaseStatus.Unassigned:
                    return examination => examination.Unassigned;
                case CaseStatus.HaveBeenScrutinisedByME:
                    return examination => examination.HaveBeenScrutinisedByME;
                case CaseStatus.PendingAdmissionNotes:
                    return examination => examination.PendingAdmissionNotes;
                case CaseStatus.PendingDiscussionWithQAP:
                    return examination => examination.PendingDiscussionWithQAP;
                case CaseStatus.PendingDiscussionWithRepresentative:
                    return examination => examination.PendingDiscussionWithRepresentative;
                case CaseStatus.HaveFinalCaseOutstandingOutcomes:
                    return examination => examination.HaveFinalCaseOutcomesOutstanding && examination.ScrutinyConfirmed;
                case null:
                    return null;
                default:
                    throw new ArgumentOutOfRangeException(nameof(paramFilterCaseStatus), paramFilterCaseStatus, null);
            }
        }

        private Expression<Func<Models.Examination, bool>> GetBaseQuery(ExaminationsRetrievalQuery param)
        {
            return _baseQueryBuilder.GetPredicate(param);
        }
    }
}
