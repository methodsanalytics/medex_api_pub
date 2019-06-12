using System.Collections.Generic;
using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Queries.PatientDetails
{
    /// <summary>
    /// Patient Details Update Query.
    /// </summary>
    public class PatientDetailsUpdateQuery : IQuery<Models.Examination>
    {
        /// <summary>
        /// Initialise a new instance of <see cref="PatientDetailsUpdateQuery"/>.
        /// </summary>
        /// <param name="caseId">Case id.</param>
        /// <param name="patientDetails">Patient details.</param>
        /// <param name="user">User.</param>
        /// <param name="locations">Locations.</param>
        public PatientDetailsUpdateQuery(string caseId, Models.PatientDetails patientDetails, MeUser user, IEnumerable<Models.Location> locations)
        {
            CaseId = caseId;
            PatientDetails = patientDetails;
            User = user;
            Locations = locations;
        }

        /// <summary>
        /// Case Id.
        /// </summary>
        public string CaseId { get; }

        /// <summary>
        /// User.
        /// </summary>
        public MeUser User { get; }

        /// <summary>
        /// Patient Details.
        /// </summary>
        public Models.PatientDetails PatientDetails { get; }

        /// <summary>
        /// Locations.
        /// </summary>
        public IEnumerable<Models.Location> Locations { get; }
    }
}