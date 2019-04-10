using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalExaminer.Models
{
    public class CaseOutcomes
    {
        /// <summary>
        /// Whether MCCD is Issued (Check box)
        /// </summary>
        public bool MccdIssued { get; set; }

        /// <summary>
        /// Whether the Cremation Form is Completed (Yes/No Radio Button Set)
        /// </summary>
        public bool CremationFormCompleted { get; set; }

        /// <summary>
        /// Whether the GP Notified or not.(Radio Button Set)
        /// </summary>
        public bool GpNotified { get; set; }
    }
}
