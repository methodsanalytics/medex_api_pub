using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MedicalExaminer.Models.Enums
{
    /// <summary>
    /// Examinations Order By.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ExaminationsOrderBy
    {
        /// <summary>
        /// Urgency.
        /// </summary>
        Urgency,

        /// <summary>
        /// Case Created.
        /// </summary>
        CaseCreated
    }
}
