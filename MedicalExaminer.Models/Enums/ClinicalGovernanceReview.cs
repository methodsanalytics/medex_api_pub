using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MedicalExaminer.Models.Enums
{
    /// <summary>
    /// Clinical Governance Review.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ClinicalGovernanceReview
    {
        /// <summary>
        /// Yes.
        /// </summary>
        Yes = 1,

        /// <summary>
        /// No.
        /// </summary>
        No = 2,

        /// <summary>
        /// Unknown.
        /// </summary>
        Unknown = 3
    }
}
