using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.API.Models.v1.CaseOutcome
{
    /// <summary>
    /// Put Outstanding Case Items Request.
    /// </summary>
    public class PutOutstandingCaseItemsRequest
    {
        /// <summary>
        /// MCCD Issued.
        /// </summary>
        public bool? MccdIssued { get; set; }

        /// <summary>
        /// Cremation From Status.
        /// </summary>
        public CremationFormStatus? CremationFormStatus { get; set; }

        /// <summary>
        /// GP Notified Status.
        /// </summary>
        public GPNotified? GpNotifiedStatus { get; set; }
    }
}
