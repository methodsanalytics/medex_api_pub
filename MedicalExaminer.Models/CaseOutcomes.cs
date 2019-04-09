using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalExaminer.Models
{
    public class CaseOutcomes
    {
        /// <summary>
        /// Determine whether MCCD is Issued (Check box)
        /// </summary>
        public bool MccdIssued { get; set; }

        /// <summary>
        /// Determine whether the Cremation Form is Completed (Yes/No Radio Button Set)
        /// </summary>
        public bool CremationFormCompleted { get; set; }


    }
}
