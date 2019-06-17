namespace MedicalExaminer.API.Models.V1.CaseBreakdown
{
    /// <summary>
    /// Put MEO Summary Event Request.
    /// </summary>
    public class PutMeoSummaryEventRequest
    {
        /// <summary>
        /// Event Identification.
        /// </summary>
        public string EventId { get; set; }

        /// <summary>
        /// bool for the status (Draft or Final).
        /// </summary>
        public bool IsFinal { get; set; }

        /// <summary>
        /// Details of the MEO Summary.
        /// </summary>
        public string SummaryDetails { get; set; }
    }
}
