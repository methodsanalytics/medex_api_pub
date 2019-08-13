﻿using System.Collections.Generic;

namespace MedicalExaminer.API.Models.v1.Examinations
{
    /// <inheritdoc />
    /// <summary>
    ///     Response object for a list of examinations.
    /// </summary>
    public class GetExaminationsResponse : ResponseBase
    {
        /// <summary>
        /// Count of Total Cases.
        /// </summary>
        public int CountOfTotalCases { get; set; }

        /// <summary>
        /// Count of Urgent Cases.
        /// </summary>
        public int CountOfUrgentCases { get; set; }

        /// <summary>
        /// Count Of Have Unknown Basic Details
        /// </summary>
        public int CountOfHaveUnknownBasicDetails { get; set; }

        /// <summary>
        /// Count of Cases Ready for Medical Examiner Scrutiny.
        /// </summary>
        public int CountOfCasesReadyForMEScrutiny { get; set; }

        /// <summary>
        /// Count of Cases Unassigned.
        /// </summary>
        public int CountOfCasesUnassigned { get; set; }

        /// <summary>
        /// Count of Cases which have been Scrutinised by a Medical Examiner.
        /// </summary>
        public int CountOfCasesHaveBeenScrutinisedByME { get; set; }

        /// <summary>
        /// Count Of Pending Additional Details
        /// </summary>
        public int CountOfPendingAdditionalDetails { get; set; }

        /// <summary>
        /// Count of Cases pending Discussion with QAP.
        /// </summary>
        public int CountOfCasesPendingDiscussionWithQAP { get; set; }

        /// <summary>
        /// Count of Cases pending Discussion with Represenative.
        /// </summary>
        public int CountOfCasesPendingDiscussionWithRepresentative { get; set; }

        /// <summary>
        /// Count of Cases that have final case outstanding outcomes.
        /// </summary>
        public int CountOfCasesHaveFinalCaseOutstandingOutcomes { get; set; }

        /// <summary>
        ///     List of Examinations.
        /// </summary>
        public IEnumerable<PatientCardItem> Examinations { get; set; }
    }
}