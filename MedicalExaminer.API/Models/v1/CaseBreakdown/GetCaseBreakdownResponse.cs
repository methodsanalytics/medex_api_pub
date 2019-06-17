﻿using MedicalExaminer.API.Models.V1.Examinations;

namespace MedicalExaminer.API.Models.V1.CaseBreakdown
{
    /// <summary>
    /// Get Case Break Down response.
    /// </summary>
    public class GetCaseBreakdownResponse : ResponseBase
    {
        /// <summary>
        /// Patient Details Header
        /// </summary>
        public PatientCardItem Header { get; set; }

        /// <summary>
        /// Case breakdown item which consists of different types of events
        /// </summary>
        public CaseBreakDownItem CaseBreakdown { get; set; }
    }
}
