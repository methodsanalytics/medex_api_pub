namespace MedicalExaminer.Common.Queries.CaseBreakdown
{
    /// <summary>
    /// Event Creation Result.
    /// </summary>
    public class EventCreationResult
    {
        /// <summary>
        /// Initialise a new instance of <see cref="EventCreationResult"/>.
        /// </summary>
        /// <param name="eventId">Event id.</param>
        /// <param name="examination">Examination.</param>
        public EventCreationResult(string eventId, Models.Examination examination)
        {
            EventId = eventId;
            Examination = examination;
        }

        /// <summary>
        /// Event Id.
        /// </summary>
        public string EventId { get; set; }

        /// <summary>
        /// Examination.
        /// </summary>
        public Models.Examination Examination { get; set; }
    }
}
