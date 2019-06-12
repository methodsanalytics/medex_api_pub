using System;
using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Queries.CaseOutcome
{
    /// <summary>
    /// Confirmation of Scrutiny Query.
    /// </summary>
    public class ConfirmationOfScrutinyQuery : IQuery<Models.Examination>
    {
        /// <summary>
        /// Initialise a new instance of <see cref="ConfirmationOfScrutinyQuery"/>.
        /// </summary>
        /// <param name="examinationId">Examination Id.</param>
        /// <param name="user">User.</param>
        public ConfirmationOfScrutinyQuery(string examinationId, MeUser user)
        {
            if (string.IsNullOrEmpty(examinationId))
            {
                throw new ArgumentNullException(nameof(examinationId));
            }

            ExaminationId = examinationId;
            User = user ?? throw new ArgumentException(nameof(user));
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
