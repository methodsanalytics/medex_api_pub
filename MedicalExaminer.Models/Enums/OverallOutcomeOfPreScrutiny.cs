using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MedicalExaminer.Models.Enums
{
    /// <summary>
    /// Overall outcome of pre scrutiny.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum OverallOutcomeOfPreScrutiny
    {
        /// <summary>
        /// Issue an MCCD.
        /// </summary>
        IssueAnMccd,

        /// <summary>
        /// Refer to coroner for 100a.
        /// </summary>
        ReferToCoronerFor100a,

        /// <summary>
        /// Refer to coroner investigation.
        /// </summary>
        ReferToCoronerInvestigation
    }
}
