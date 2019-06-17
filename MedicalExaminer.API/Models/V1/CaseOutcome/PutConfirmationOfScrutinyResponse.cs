using System;

namespace MedicalExaminer.API.Models.V1.CaseOutcome
{
    /// <summary>
    /// Put Confirmation of Scrutiny Response.
    /// </summary>
    public class PutConfirmationOfScrutinyResponse : ResponseBase
    {
        /// <summary>
        /// Scrutiny Confirmed On.
        /// </summary>
        public DateTime? ScrutinyConfirmedOn { get; set; }
    }
}
