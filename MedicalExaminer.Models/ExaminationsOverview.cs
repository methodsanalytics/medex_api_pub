namespace MedicalExaminer.Models
{
    /// <summary>
    /// Examinations Overview.
    /// </summary>
    public class ExaminationsOverview
    {
        /// <summary>
        /// Number of total cases.
        /// </summary>
        public int TotalCases { get; set; }

        /// <summary>
        /// Number of urgent cases.
        /// </summary>
        public int CountOfUrgentCases { get; set; }

        /// <summary>
        /// Number of cases where admission notes have been added.
        /// </summary>
        public int CountOfAdmissionNotesHaveBeenAdded { get; set; }

        /// <summary>
        /// Number of cases that are ready for ME scrutiny.
        /// </summary>
        public int CountOfReadyForMEScrutiny { get; set; }

        /// <summary>
        /// Number of cases unassigned.
        /// </summary>
        public int CountOfUnassigned { get; set; }

        /// <summary>
        /// Number of cases that have been scrutinised by the ME
        /// </summary>
        public int CountOfHaveBeenScrutinisedByME { get; set; }

        /// <summary>
        /// Number of cases pending admission notes.
        /// </summary>
        public int CountOfPendingAdmissionNotes { get; set; }

        /// <summary>
        /// Number of cases pending discussion with qap.
        /// </summary>
        public int CountOfPendingDiscussionWithQAP { get; set; }

        /// <summary>
        /// Number of cases pending discussion with representation.
        /// </summary>
        public int CountOfPendingDiscussionWithRepresentative { get; set; }

        /// <summary>
        /// Number of cases that have final case outstanding outcomes.
        /// </summary>
        public int CountOfHaveFinalCaseOutstandingOutcomes { get; set; }
    }
}
