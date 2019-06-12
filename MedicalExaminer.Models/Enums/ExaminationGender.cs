using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MedicalExaminer.Models.Enums
{
    /// <summary>
    /// Examination Gender.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ExaminationGender
    {
        /// <summary>
        /// Male.
        /// </summary>
        Male = 1,

        /// <summary>
        /// Female
        /// </summary>
        Female = 2,

        /// <summary>
        /// Other.
        /// </summary>
        Other = 3
    }
}