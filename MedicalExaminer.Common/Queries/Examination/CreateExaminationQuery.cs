using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Queries.Examination
{
    /// <summary>
    /// Create Examination Query.
    /// </summary>
    public class CreateExaminationQuery : IQuery<Models.Examination>
    {
        /// <summary>
        /// Initialise a new instance of <see cref="CreateExaminationQuery"/>.
        /// </summary>
        /// <param name="examination">Examination.</param>
        /// <param name="user">User.</param>
        public CreateExaminationQuery(Models.Examination examination, MeUser user)
        {
            Examination = examination;
            User = user;
        }

        /// <summary>
        /// Examination.
        /// </summary>
        public Models.Examination Examination { get; }

        /// <summary>
        /// User.
        /// </summary>
        public MeUser User { get; }
    }
}