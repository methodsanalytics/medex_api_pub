using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MedicalExaminer.Models.Enums
{
    /// <summary>
    /// Present at death.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PresentAtDeath
    {
        /// <summary>
        /// Yes
        /// </summary>
        Yes,

        /// <summary>
        /// No
        /// </summary>
        No,

        /// <summary>
        /// Unknown.
        /// </summary>
        Unknown
    }
}