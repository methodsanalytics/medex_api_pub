using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MedicalExaminer.Models.Enums
{
    /// <summary>
    /// Location Type.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum LocationType
    {
        /// <summary>
        /// Site.
        /// </summary>
        Site = 0,

        /// <summary>
        /// Trust.
        /// </summary>
        Trust = 1,

        /// <summary>
        /// Region.
        /// </summary>
        Region = 2,

        /// <summary>
        /// National.
        /// </summary>
        National = 3
    }
}