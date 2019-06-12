using System.Collections.Generic;
using MedicalExaminer.Models;

namespace MedicalExaminer.API.Models.v1.CaseBreakdown
{
    /// <summary>
    /// Event Container Item.
    /// </summary>
    /// <typeparam name="TEventType">Event type.</typeparam>
    public class EventContainerItem<TEventType>
    {
        /// <summary>
        /// History.
        /// </summary>
        public IEnumerable<TEventType> History { get; set; }

        /// <summary>
        /// Latest.
        /// </summary>
        public TEventType Latest { get; set; }

        /// <summary>
        /// User Draft.
        /// </summary>
        public TEventType UsersDraft { get; set; }
    }
}
