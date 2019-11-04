using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.Examination;

namespace MedicalExaminer.Common.Services.Examination
{
    /// <summary>
    /// summary text
    /// </summary>
    public class FinanceService : QueryHandler<FinanceQuery, IEnumerable<Models.Examination>>
    {
        private readonly ExaminationsQueryExpressionBuilder _examinationQueryBuilder;

        /// <summary>
        /// Constructor for Finance Service
        /// </summary>
        /// <param name="databaseAccess"></param>
        /// <param name="connectionSettings"></param>
        /// <param name="examinationQueryBuilder"></param>
        public FinanceService(
            IDatabaseAccess databaseAccess,
            IExaminationConnectionSettings connectionSettings,
            ExaminationsQueryExpressionBuilder examinationQueryBuilder)
            : base(databaseAccess, connectionSettings)
        {
            _examinationQueryBuilder = examinationQueryBuilder;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public override Task<IEnumerable<Models.Examination>> Handle(FinanceQuery param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            var queryExpression = _examinationQueryBuilder.GetFinancePredicate(param);

            Expression<Func<Models.Examination, dynamic>> select = examination => new
            {
                id = examination.ExaminationId,
                site_location_id = examination.SiteLocationId,
                trust_location_id = examination.TrustLocationId,
                region_location_id = examination.RegionLocationId,
                national_location_id = examination.NationalLocationId,
                medical_team = examination.MedicalTeam,
                CreatedAt = examination.CreatedAt,
                case_completed = examination.CaseCompleted,
                nhs_number = examination.NhsNumber,
                mode_of_disposal = examination.ModeOfDisposal,
                waive_fee = examination.WaiveFee,
                case_outcome = examination.CaseOutcome
            };

            return GetItemsAsync(queryExpression, select);
        }
    }
}
