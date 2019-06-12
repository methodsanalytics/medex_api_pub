using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MedicalExaminer.Models.Enums
{
    /// <summary>
    /// Analysis Entry Type.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum AnalysisEntryType
    {
        /// <summary>
        /// MEO Summary.
        /// </summary>
        MEOSummary = 0,

        /// <summary>
        /// Bereaved Information.
        /// </summary>
        BereavedInformation = 1,

        /// <summary>
        /// QAP Information.
        /// </summary>
        QAPInformation = 2,

        /// <summary>
        /// ME Scrutiny.
        /// </summary>
        MEScrutiny = 3,

        /// <summary>
        /// Coroner Information.
        /// </summary>
        CoronerInformation = 4,

        /// <summary>
        /// Other information.
        /// </summary>
        OtherInformation = 5
    }
}