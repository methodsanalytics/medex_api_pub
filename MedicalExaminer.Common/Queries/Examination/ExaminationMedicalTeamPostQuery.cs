using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Queries.Examination
{
    /// <summary>
    /// Examination Medical Team Post Query.
    /// </summary>
    public class ExaminationMedicalTeamPostQuery : IQuery<IMedicalTeam>
    {
        /// <summary>
        /// Initialise a new instance of <see cref="ExaminationMedicalTeamPostQuery"/>.
        /// </summary>
        /// <param name="medicalTeam">Medical team.</param>
        public ExaminationMedicalTeamPostQuery(IMedicalTeam medicalTeam)
        {
            MedicalTeam = medicalTeam;
        }

        /// <summary>
        /// Medical Team.
        /// </summary>
        private IMedicalTeam MedicalTeam { get; }
    }
}