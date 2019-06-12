using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MedicalExaminer.Models.Enums
{
    /// <summary>
    /// Case Outcome Summary.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum CaseOutcomeSummary
    {
        /// <summary>
        /// Refer to Coroner.
        /// </summary>
        ReferToCoroner,

        /// <summary>
        /// Issue MCCD With 100a
        /// </summary>
        IssueMCCDWith100a,

        /// <summary>
        /// Issue MCCD.
        /// </summary>
        IssueMCCD
    }
}
