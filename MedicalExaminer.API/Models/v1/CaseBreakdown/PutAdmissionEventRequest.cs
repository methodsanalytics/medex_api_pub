﻿using System;
using MedicalExaminer.API.Attributes;

namespace MedicalExaminer.API.Models.v1.CaseBreakdown
{
    /// <summary>
    ///     Put Other Event Request Object.
    /// </summary>
    public class PutAdmissionEventRequest
    {
        /// <summary>
        /// Event Id
        /// </summary>
        public string EventId { get; set; }

        /// <summary>
        /// Event Text (Length to be confirmed).
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// Draft is false, final true
        /// </summary>
        public bool IsFinal { get; set; }

        /// <summary>
        /// date of last admission, if known
        /// </summary>
        public DateTime? AdmittedDate { get; set; }

        /// <summary>
        /// date of last admission - Unknown or not
        /// </summary>
        public bool? AdmittedDateUnknown { get; set; }

        /// <summary>
        /// time of last admission, if known
        /// </summary>
        public TimeSpan? AdmittedTime { get; set; }

        /// <summary>
        /// time of last admission - Unknown or not
        /// </summary>
        public bool? AdmittedTimeUnknown { get; set; }

        /// <summary>
        /// Do you suspect this case may need Immediate Coroner Referral Yes = true, No = false
        /// </summary>
        [RequiredIfAttributesMatch(nameof(IsFinal), true)]
        public bool? ImmediateCoronerReferral { get; set; }
    }
}