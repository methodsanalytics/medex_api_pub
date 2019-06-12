using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MedicalExaminer.Models.Enums
{
    /// <summary>
    /// Cremation Form Status.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum CremationFormStatus
    {
        /// <summary>
        /// No
        /// </summary>
        No,

        /// <summary>
        /// Yes
        /// </summary>
        Yes,

        /// <summary>
        /// Unknown
        /// </summary>
        Unknown
    }
}
