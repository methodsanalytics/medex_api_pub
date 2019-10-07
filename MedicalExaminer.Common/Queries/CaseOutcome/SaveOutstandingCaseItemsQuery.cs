using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Queries.CaseOutcome
{
    public class SaveOutstandingCaseItemsQuery : IQuery<string>
    {
        public string ExaminationId { get; set; }

        public Models.Examination Examination { get; }

        public MeUser User { get; }

        public SaveOutstandingCaseItemsQuery(string examinationId, Models.Examination examination, MeUser user)
        {
            ExaminationId = examinationId;
            Examination = examination;
            User = user;
        }
    }
}
