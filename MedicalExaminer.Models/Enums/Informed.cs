using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MedicalExaminer.Models.Enums
{
    /// <summary>
    /// Informed.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Informed
    {
        /// <summary>
        /// Yes.
        /// </summary>
        Yes,

        /// <summary>
        /// No.
        /// </summary>
        No
    }
}