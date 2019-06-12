namespace MedicalExaminer.API.Models.v1.CaseBreakdown
{
    /// <summary>
    /// Case Break down Item.
    /// </summary>
    public class CaseBreakDownItem
    {
        /// <summary>
        /// Patient Death Event.
        /// </summary>
        public PatientDeathEventItem PatientDeathEvent { get; set; }

        /// <summary>
        /// Case breakdown other events
        /// </summary>
        public EventContainerItem<OtherEventItem> OtherEvents { get; set; }

        /// <summary>
        /// Case breakdown pre scrutiny notes
        /// </summary>
        public EventContainerItem<PreScrutinyEventItem> PreScrutiny { get; set; }

        /// <summary>
        /// Case breakdown bereaved discussion notes
        /// </summary>
        public EventContainerItem<BereavedDiscussionEventItem> BereavedDiscussion { get; set; }

        /// <summary>
        /// Case breakdown meo summary notes
        /// </summary>
        public EventContainerItem<MeoSummaryEventItem> MeoSummary { get; set; }

        /// <summary>
        /// Case breakdown qap discussion
        /// </summary>
        public EventContainerItem<QapDiscussionEventItem> QapDiscussion { get; set; }

        /// <summary>
        /// case breakdown medical history
        /// </summary>
        public EventContainerItem<MedicalHistoryEventItem> MedicalHistory { get; set; }

        /// <summary>
        /// case breakdown admission notes
        /// </summary>
        public EventContainerItem<AdmissionEventItem> AdmissionNotes { get; set; }
    }
}
