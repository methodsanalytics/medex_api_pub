using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.API.Models.v1.CaseOutcomes
{
    public class GetCaseOutcomesResponse : ResponseBase
    {
        /// <summary>
        /// Outcome Of Pre-Scrutiny.
        /// </summary>
        public OverallOutcomeOfPreScrutiny OutcomeOfPreScrutiny { get; set; }

        /// <summary>
        /// Outcome Of QAP Discussion.
        /// </summary>
        public QapDiscussionOutcome QapDiscussionOutcome { get; set; }

        /// <summary>
        /// Outcome Of Bereaved Discussion.
        /// </summary>
        public BereavedDiscussionOutcome BereavedDiscussionOutcome { get; set; }

        /// <summary>
        /// Whether MCCD is Issued (Check box)
        /// </summary>
        public bool MccdIssued { get; set; }

        /// <summary>
        /// Whether the Cremation Form is Completed (Yes/No Radio Button Set)
        /// </summary>
        public bool CremationFormCompleted { get; set; }

        /// <summary>
        /// Whether the GP Notified or not.(Radio Button Set)
        /// </summary>
        public bool GpNotified { get; set; }
    }
}
