using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MedicalExaminer.Models.Enums
{
    /// <summary>
    /// GP Notified.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum GPNotified
    {
        /// <summary>
        /// GP Unable to be notified.
        /// </summary>
        GPUnabledToBeNotified,

        /// <summary>
        /// GP Notified
        /// </summary>
        GPNotified,

        /// <summary>
        /// Not Applicable.
        /// </summary>
        NA
    }
}
