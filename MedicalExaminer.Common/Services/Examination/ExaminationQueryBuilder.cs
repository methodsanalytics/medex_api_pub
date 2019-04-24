using System;
using System.Linq.Expressions;
using MedicalExaminer.Common.Queries;
using MedicalExaminer.Common.Queries.Examination;

namespace MedicalExaminer.Common.Services.Examination
{
    public class ExaminationsQueryExpressionBuilder
    {
        /// <summary>
        /// Gets the predicate
        /// </summary>
        /// <param name="queryObject"></param>
        /// <returns></returns>
        public Expression<Func<Models.Examination, bool>> GetPredicate(ExaminationsRetrievalQuery queryObject)
        {
            var medicalExaminerOfficeFilter = GetCaseMEOfficePredicate(queryObject.FilterLocationId);
            var userIdFilter = GetUserIdPredicate(queryObject.FilterUserId);
            var openCases = GetOpenCasesPredicate(queryObject.FilterOpenCases);
            var predicate = medicalExaminerOfficeFilter.And(userIdFilter).And(openCases);
            return predicate;
        }

        private Expression<Func<Models.Examination, bool>> GetOpenCasesPredicate(bool paramFilterOpenCases)
        {
            return examination => examination.Completed == !paramFilterOpenCases;
        }

        private Expression<Func<Models.Examination, bool>> GetCaseMEOfficePredicate(string meOffice)
        {
            if (string.IsNullOrEmpty(meOffice))
            {
                return null;
            }

            return examination => examination.MedicalExaminerOfficeResponsible == meOffice;
        }

        private Expression<Func<Models.Examination, bool>> GetUserIdPredicate(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return null;
            }

            Expression<Func<Models.Examination, bool>> mePredicate = examination => examination.MedicalTeam.MedicalExaminerUserId == userId;
            Expression<Func<Models.Examination, bool>> meoPredicate = examination => examination.MedicalTeam.MedicalExaminerOfficerUserId == userId;

            var predicate = mePredicate.Or(meoPredicate);
            return predicate;
        }
    }
}
