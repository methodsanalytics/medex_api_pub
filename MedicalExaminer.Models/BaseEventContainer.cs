using System;
using System.Collections.Generic;
using System.Linq;

namespace MedicalExaminer.Models
{
    /// <summary>
    /// Base Event Container.
    /// </summary>
    /// <typeparam name="TEvent">Type of Event.</typeparam>
    public abstract class BaseEventContainer<TEvent> : IEventContainer<TEvent>
        where TEvent : IEvent
    {
        /// <inheritdoc/>
        public TEvent Latest { get; set; }

        /// <inheritdoc/>
        public IList<TEvent> Drafts { get; set; } = new List<TEvent>();

        /// <inheritdoc/>
        public IList<TEvent> History { get; set; } = new List<TEvent>();

        /// <inheritdoc/>
        public virtual void Add(TEvent theEvent)
        {
            if (string.IsNullOrEmpty(theEvent.EventId))
            {
                theEvent.EventId = Guid.NewGuid().ToString();
            }

            if (theEvent.IsFinal)
            {
                theEvent.Created = DateTime.Now;
                Latest = theEvent;
                History.Add(theEvent);
                Drafts.Clear();
            }
            else
            {
                theEvent.Created = theEvent.Created ?? DateTime.Now;
                var userHasDraft = Drafts.Any(draft => draft.UserId == theEvent.UserId);
                if (userHasDraft)
                {
                    var usersDraft = Drafts.SingleOrDefault(draft => draft.EventId == theEvent.EventId);
                    if (usersDraft == null)
                    {
                        throw new ArgumentException(nameof(theEvent.EventId));
                    }

                    Drafts.Remove(usersDraft);
                    Drafts.Add(theEvent);
                    return;
                }

                Drafts.Add(theEvent);
            }
        }

    }
}