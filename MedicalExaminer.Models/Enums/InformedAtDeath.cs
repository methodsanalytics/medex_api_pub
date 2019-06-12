using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MedicalExaminer.Models.Enums
{
    /// <summary>
    /// Informed At Death.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum InformedAtDeath
    {
        /// <summary>
        /// Yes.
        /// </summary>
        Yes,

        /// <summary>
        /// No.
        /// </summary>
        No,

        /// <summary>
        /// Unknown.
        /// </summary>
        Unknown
    }
}