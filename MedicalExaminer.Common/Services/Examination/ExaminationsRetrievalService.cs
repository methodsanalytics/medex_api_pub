using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Cosmonaut;
using Cosmonaut.Extensions;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.Common.Services.Examination
{
    /// <summary>
    /// Examinations Retrieval Service.
    /// </summary>
    public class ExaminationsRetrievalService : QueryHandler<ExaminationsRetrievalQuery, IEnumerable<Models.Examination>>
    {
        private readonly ExaminationsQueryExpressionBuilder _examinationQueryBuilder;
        private readonly ICosmosStore<Models.Examination> _store;

        /// <summary>
        /// Initialise a new instance of <see cref="ExaminationsRetrievalService"/>.
        /// </summary>
        /// <param name="databaseAccess">Database Access.</param>
        /// <param name="connectionSettings">Connection Settings.</param>
        /// <param name="examinationQueryBuilder">Examination Query Builder.</param>
        /// <param name="store">Cosmos Store for paging.</param>
        public ExaminationsRetrievalService(
            IDatabaseAccess databaseAccess,
            IExaminationConnectionSettings connectionSettings,
            ExaminationsQueryExpressionBuilder examinationQueryBuilder,
            ICosmosStore<Models.Examination> store)
            : base(databaseAccess, connectionSettings)
        {
            _examinationQueryBuilder = examinationQueryBuilder;
            _store = store;
        }

        /// <inheritdoc/>
        public async override Task<IEnumerable<Models.Examination>> Handle(ExaminationsRetrievalQuery param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            var predicate = _examinationQueryBuilder.GetPredicate(param);

            var statusPredicate = GetCaseStatusPredicate(param.FilterCaseStatus);

            predicate = predicate.And(statusPredicate);

            switch (param.FilterOrderBy)
            {
                case ExaminationsOrderBy.Urgency:
                {
                    var t = _store.Query()
                        .Where(predicate).OrderByDescending(x => x.UrgencyScore)
                        .WithPagination(param.FilterPageNumber, param.FilterPageSize)
                        .ToListAsync().Result;
                    return t;
                }

                case ExaminationsOrderBy.CaseCreated:
                    return _store.Query().Where(predicate).OrderBy(x => x.CreatedAt).WithPagination(param.FilterPageNumber, param.FilterPageSize).ToListAsync().Result;
                case null:
                    return _store.Query().WithPagination(param.FilterPageNumber, param.FilterPageSize).Where(predicate).ToListAsync().Result;
                default:
                    throw new ArgumentOutOfRangeException(nameof(param.FilterOrderBy));
            }
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
                    return examination => examination.HaveFinalCaseOutstandingOutcomes;
                case null:
                    return null;
                default:
                    throw new ArgumentOutOfRangeException(nameof(paramFilterCaseStatus), paramFilterCaseStatus, null);
            }
        }
    }
}