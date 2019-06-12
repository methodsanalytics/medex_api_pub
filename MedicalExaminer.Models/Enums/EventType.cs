using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MedicalExaminer.Models.Enums
{
    /// <summary>
    /// Event Type.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum EventType
    {
        /// <summary>
        /// Other.
        /// </summary>
        Other,

        /// <summary>
        /// Pre Scrutiny.
        /// </summary>
        PreScrutiny,

        /// <summary>
        /// Bereaved Discussion.
        /// </summary>
        BereavedDiscussion,

        /// <summary>
        /// Medical History.
        /// </summary>
        MedicalHistory,

        /// <summary>
        /// Meo Summary.
        /// </summary>
        MeoSummary,

        /// <summary>
        /// Qap Discussion.
        /// </summary>
        QapDiscussion,

        /// <summary>
        /// Admission.
        /// </summary>
        Admission,

        /// <summary>
        /// Patient Died.
        /// </summary>
        PatientDied
    }
}
