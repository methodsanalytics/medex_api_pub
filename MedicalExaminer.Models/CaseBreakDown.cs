namespace MedicalExaminer.Models
{
    /// <summary>
    /// Case Break Down.
    /// </summary>
    public class CaseBreakDown
    {
        /// <summary>
        /// Death Event.
        /// </summary>
        public DeathEvent DeathEvent { get; set; }

        /// <summary>
        /// Other Events.
        /// </summary>
        public BaseEventContainer<OtherEvent> OtherEvents { get; set; } = new OtherEventContainer();

        /// <summary>
        /// Pre Scrutiny ... events?
        /// </summary>
        public BaseEventContainer<PreScrutinyEvent> PreScrutiny { get; set; } = new PreScrutinyEventContainer();

        /// <summary>
        /// Bereaved Discussion... events...
        /// </summary>
        public BaseEventContainer<BereavedDiscussionEvent> BereavedDiscussion { get; set; } = new BereavedDiscussionEventContainer();

        /// <summary>
        /// Meo Summary...
        /// </summary>
        public BaseEventContainer<MeoSummaryEvent> MeoSummary { get; set; } = new MeoSummaryEventContainer();

        /// <summary>
        /// Qap Discussion.
        /// </summary>
        public BaseEventContainer<QapDiscussionEvent> QapDiscussion { get; set; } = new QapDiscussionEventContainer();

        /// <summary>
        /// Medical History.
        /// </summary>
        public BaseEventContainer<MedicalHistoryEvent> MedicalHistory { get; set; } = new MedicalHistoryEventContainer();

        /// <summary>
        /// Admission Notes.
        /// </summary>
        public BaseEventContainer<AdmissionEvent> AdmissionNotes { get; set; } = new AdmissionNotesEventContainer();
    }
}
