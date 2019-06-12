using System;
using MedicalExaminer.API.Models.v1.Examinations;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.API.Models.v1.CaseOutcome
{
    /// <summary>
    /// Get Case Outcome Response.
    /// </summary>
    public class GetCaseOutcomeResponse : ResponseBase
    {
        /// <summary>
        /// Header.
        /// </summary>
        public PatientCardItem Header { get; set; }

        /// <summary>
        /// Case Outcome Summary.
        /// </summary>
        public CaseOutcomeSummary? CaseOutcomeSummary { get; set; }

        /// <summary>
        /// Outcome of Representative Discussion.
        /// </summary>
        public BereavedDiscussionOutcome? OutcomeOfRepresentativeDiscussion { get; set; }

        /// <summary>
        /// Outcome of Pre Scrutiny.
        /// </summary>
        public OverallOutcomeOfPreScrutiny? OutcomeOfPrescrutiny { get; set; }

        /// <summary>
        /// Outcome QAP Discussion.
        /// </summary>
        public QapDiscussionOutcome? OutcomeQapDiscussion { get; set; }

        /// <summary>
        /// Case Completed.
        /// </summary>
        public bool CaseCompleted { get; set; }

        /// <summary>
        /// Scrutiny Confirmed On.
        /// </summary>
        public DateTime? ScrutinyConfirmedOn { get; set; }

        /// <summary>
        /// Coroner Referral Sent.
        /// </summary>
        public bool CoronerReferralSent { get; set; }

        /// <summary>
        /// Case Medical Examiner Full Name.
        /// </summary>
        public string CaseMedicalExaminerFullName { get; set; }

        /// <summary>
        /// Case Medical Examiner Id.
        /// </summary>
        public string CaseMedicalExaminerId { get; set; }

        /// <summary>
        /// MCDD Issued.
        /// </summary>
        public bool? MccdIssued { get; set; }

        /// <summary>
        /// Cremation form Status.
        /// </summary>
        public CremationFormStatus? CremationFormStatus { get; set; }

        /// <summary>
        /// GP Notified Status.
        /// </summary>
        public GPNotified? GpNotifiedStatus { get; set; }
    }
}
