using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MedicalExaminer.Models.Enums
{
    /// <summary>
    /// QAP Discussion Outcome.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum QapDiscussionOutcome
    {
        /// <summary>
        /// MCCD Cause of death provided by QAP.
        /// </summary>
        MccdCauseOfDeathProvidedByQAP,

        /// <summary>
        /// MCCD Cause of death provided by ME.
        /// </summary>
        MccdCauseOfDeathProvidedByME,

        /// <summary>
        /// MCCD Cause of death agreed by QAP and ME
        /// </summary>
        MccdCauseOfDeathAgreedByQAPandME,

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
