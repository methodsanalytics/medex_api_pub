using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Queries.Examination
{
    /// <summary>
    /// Examination Retrieval Query.
    /// </summary>
    public class ExaminationRetrievalQuery : IQuery<Models.Examination>
    {
        /// <summary>
        /// Initialise a new instance of <see cref="ExaminationRetrievalQuery"/>.
        /// </summary>
        /// <param name="examinationId">Examination id.</param>
        /// <param name="user">User.</param>
        public ExaminationRetrievalQuery(string examinationId, MeUser user)
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