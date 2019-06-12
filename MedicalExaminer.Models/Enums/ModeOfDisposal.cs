using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MedicalExaminer.Models.Enums
{
    /// <summary>
    /// Mode of Disposal.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ModeOfDisposal
    {
        /// <summary>
        /// Unknown.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Cremation.
        /// </summary>
        Cremation = 1,

        /// <summary>
        /// Burial.
        /// </summary>
        Burial = 2,

        /// <summary>
        /// Buried at sea.
        /// </summary>
        BuriedAtSea = 3,

        /// <summary>
        /// Repatriation.
        /// </summary>
        Repatriation = 4,
    }
}