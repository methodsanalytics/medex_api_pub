using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Queries.CaseOutcome
{
    /// <summary>
    /// Save Outstanding Case Items Query.
    /// </summary>
    public class SaveOutstandingCaseItemsQuery : IQuery<string>
    {
        /// <summary>
        /// Initialise a new instance of <see cref="SaveOutstandingCaseItemsQuery"/>.
        /// </summary>
        /// <param name="examinationId">Examination Id.</param>
        /// <param name="caseOutcome">Case Outcome.</param>
        /// <param name="user">User.</param>
        public SaveOutstandingCaseItemsQuery(string examinationId, Models.CaseOutcome caseOutcome, MeUser user)
        {
            ExaminationId = examinationId;
            CaseOutcome = caseOutcome;
            User = user;
        }

        /// <summary>
        /// Examination Id.
        /// </summary>
        public string ExaminationId { get; set; }

        /// <summary>
        /// Case Outcome.
        /// </summary>
        public Models.CaseOutcome CaseOutcome { get; }

        /// <summary>
        /// User.
        /// </summary>
        public MeUser User { get; }
    }
}
