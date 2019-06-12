using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MedicalExaminer.Models.Enums
{
    /// <summary>
    /// Coroner Status.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum CoronerStatus
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        /// Pending Send.
        /// </summary>
        PendingSend = 1,

        /// <summary>
        /// Sent Awaiting Confirm.
        /// </summary>
        SentAwaitingConfirm = 2,

        /// <summary>
        /// Sent confirmed.
        /// </summary>
        SentConfirmed = 3
    }
}