using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MedicalExaminer.Models.Enums
{
    /// <summary>
    /// Bereaved Discussion Outcome.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum BereavedDiscussionOutcome
    {
        /// <summary>
        /// Cause of Death Accepted.
        /// </summary>
        CauseOfDeathAccepted = 0,

        /// <summary>
        /// Concerns Coroner Investigation.
        /// </summary>
        ConcernsCoronerInvestigation = 1,

        /// <summary>
        /// Concerns requires 100a.
        /// </summary>
        ConcernsRequires100a = 2,

        /// <summary>
        /// Concerns Addressed without Coroner
        /// </summary>
        ConcernsAddressedWithoutCoroner = 3
    }
}