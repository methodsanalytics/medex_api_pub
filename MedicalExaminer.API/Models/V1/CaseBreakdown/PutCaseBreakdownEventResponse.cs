using MedicalExaminer.API.Models.V1.Examinations;

namespace MedicalExaminer.API.Models.V1.CaseBreakdown
{
    /// <summary>
    /// Put Case Breakdown Event Response.
    /// </summary>
    public class PutCaseBreakdownEventResponse : ResponseBase
    {
        /// <summary>
        /// Patient Details Header
        /// </summary>
        public PatientCardItem Header { get; set; }

        /// <summary>
        /// The id of the event
        /// </summary>
        public string EventId { get; set; }
    }
}
