namespace MedicalExaminer.API.Models.V1.Examinations
{
    /// <summary>
    ///     The response object when a new case is added
    /// </summary>
    public class PutExaminationResponse : ResponseBase
    {
        /// <summary>
        ///     The id of the new case
        /// </summary>
        public string ExaminationId { get; set; }
    }
}