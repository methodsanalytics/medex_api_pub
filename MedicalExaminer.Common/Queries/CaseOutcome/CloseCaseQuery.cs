using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Queries.CaseOutcome
{
    /// <summary>
    /// Close Case Query.
    /// </summary>
    public class CloseCaseQuery : IQuery<string>
    {
        /// <summary>
        /// Initialise a new instance of <see cref="CloseCaseQuery"/>.
        /// </summary>
        /// <param name="examinationId">Examination Id.</param>
        /// <param name="user">User Id.</param>
        public CloseCaseQuery(string examinationId, MeUser user)
        {
            ExaminationId = examinationId;
            User = user;
        }

        /// <summary>
        /// Examination Id.
        /// </summary>
        public string ExaminationId { get; }

        /// <summary>
        /// User.
        /// </summary>
        public MeUser User { get; }
    }
}
