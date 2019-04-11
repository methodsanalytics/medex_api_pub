using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalExaminer.Common.Queries.CaseOutcomes
{
    public class CaseOutcomesRetrievalQuery : IQuery<Models.Examination>
    {
        /// <summary>
        /// Examination ID
        /// </summary>
        public readonly string ExaminationId;

        public CaseOutcomesRetrievalQuery(string examinationId)
        {
            ExaminationId = examinationId;
        }
    }
}
