using MedicalExaminer.API.Models.V1.Examinations;

namespace MedicalExaminer.API.Models.V1.PatientDetails
{
    /// <summary>
    ///     The response returned when a request to PUT the patients details
    /// </summary>
    public class PutPatientDetailsResponse : ResponseBase
    {
        /// <summary>
        ///     The id of the examination
        /// </summary>
        public PatientCardItem Header { get; set; }
    }
}