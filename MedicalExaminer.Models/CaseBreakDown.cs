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
        public BaseEventContainter<OtherEvent> OtherEvents { get; set; } = new OtherEventContainer();

        /// <summary>
        /// Pre Scrutiny ... events?
        /// </summary>
        public BaseEventContainter<PreScrutinyEvent> PreScrutiny { get; set; } = new PreScrutinyEventContainer();

        /// <summary>
        /// Bereaved Discussion... events...
        /// </summary>
        public BaseEventContainter<BereavedDiscussionEvent> BereavedDiscussion { get; set; } = new BereavedDiscussionEventContainer();

        /// <summary>
        /// Meo Summary...
        /// </summary>
        public BaseEventContainter<MeoSummaryEvent> MeoSummary { get; set; } = new MeoSummaryEventContainer();

        /// <summary>
        /// Qap Discussion.
        /// </summary>
        public BaseEventContainter<QapDiscussionEvent> QapDiscussion { get; set; } = new QapDiscussionEventContainer();

        /// <summary>
        /// Medical History.
        /// </summary>
        public BaseEventContainter<MedicalHistoryEvent> MedicalHistory { get; set; } = new MedicalHistoryEventContainer();

        /// <summary>
        /// Admission Notes.
        /// </summary>
        public BaseEventContainter<AdmissionEvent> AdmissionNotes { get; set; } = new AdmissionNotesEventContainer();
    }
}
