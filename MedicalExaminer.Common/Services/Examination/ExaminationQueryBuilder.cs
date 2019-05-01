﻿using System;
using System.Linq.Expressions;
using MedicalExaminer.Common.Queries;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.Common.Services.Examination
{
    public class ExaminationsQueryExpressionBuilder
    {
        public Expression<Func<Models.Examination, bool>> GetPredicate(ExaminationsRetrievalQuery queryObject)
        {
            var caseStatusFilter = GetCaseStatusPredicate(queryObject.FilterCaseStatus);
            var medicalExaminerOfficeFilter = GetCaseMEOfficePredicate(queryObject.FilterLocationId);
            var userIdFilter = GetUserIdPredicate(queryObject.FilterUserId);
            var openCases = GetOpenCasesPredicate(queryObject.FilterOpenCases);

            var predicate = caseStatusFilter.And(medicalExaminerOfficeFilter)
                .And(userIdFilter).And(openCases);

            return predicate;
        }

        private Expression<Func<Models.Examination, bool>> GetOpenCasesPredicate(bool paramFilterOpenCases)
        {
            return examination => examination.CaseCompleted == !paramFilterOpenCases;
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
                    return examination => examination.HaveFinalCaseOutcomesOutstanding;
                case null:
                    return null;
                default:
                    throw new ArgumentOutOfRangeException(nameof(paramFilterCaseStatus), paramFilterCaseStatus, null);
            }
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
