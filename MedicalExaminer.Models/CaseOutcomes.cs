using MedicalExaminer.Models.Enums;
using Newtonsoft.Json;

namespace MedicalExaminer.Models
{
    public class CaseOutcomes
    {
        /// <summary>
        /// Outcome Of Pre-Scrutiny.
        /// </summary>
        [JsonProperty(PropertyName = "outcome_of_pre_scrutiny")]
        public OverallOutcomeOfPreScrutiny OutcomeOfPreScrutiny { get; set; }

        /// <summary>
        /// Outcome Of QAP Discussion.
        /// </summary>
        [JsonProperty(PropertyName = "qap_discussion_outcome")]
        public QapDiscussionOutcome QapDiscussionOutcome { get; set; }

        /// <summary>
        /// Outcome Of Bereaved Discussion.
        /// </summary>
        [JsonProperty(PropertyName = "bereaved_discussion_outcome")]
        public BereavedDiscussionOutcome BereavedDiscussionOutcome { get; set; }

        /// <summary>
        /// Whether MCCD is Issued (Check box)
        /// </summary>
        [JsonProperty(PropertyName = "mccd_issued")]
        public bool MccdIssued { get; set; }

        /// <summary>
        /// Whether the Cremation Form is Completed (Yes/No Radio Button Set)
        /// </summary>
        [JsonProperty(PropertyName = "cremation_form_completed")]
        public bool CremationFormCompleted { get; set; }

        /// <summary>
        /// Whether the GP Notified or not.(Radio Button Set)
        /// </summary>
        [JsonProperty(PropertyName = "gp_notified")]
        public bool GpNotified { get; set; }
    }
}
