using System.Collections.Generic;

namespace MedicalExaminer.Models
{
    /// <summary>
    /// Event Container Interface.
    /// </summary>
    /// <typeparam name="TEvent">Type of event.</typeparam>
    public interface IEventContainer<TEvent>
        where TEvent : IEvent
    {
        /// <summary>
        /// Latest Event.
        /// </summary>
        TEvent Latest { get; }

        /// <summary>
        /// Draft Events.
        /// </summary>
        IList<TEvent> Drafts { get; set; }

        /// <summary>
        /// History Events.
        /// </summary>
        IList<TEvent> History { get; set; }

        /// <summary>
        /// Add Event.
        /// </summary>
        /// <param name="theEvent">The event to add.</param>
        void Add(TEvent theEvent);
    }
}
