using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Queries.CaseOutcome
{
    /// <summary>
    /// Coroner Referral Query.
    /// </summary>
    public class CoronerReferralQuery : IQuery<string>
    {
        /// <summary>
        /// Initialise a new instance of <see cref="CoronerReferralQuery"/>.
        /// </summary>
        /// <param name="examinationId">Examination Id.</param>
        /// <param name="user">User Id.</param>
        public CoronerReferralQuery(string examinationId, MeUser user)
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
