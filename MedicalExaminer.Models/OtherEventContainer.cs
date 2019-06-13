﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace MedicalExaminer.Models
{
    /// <summary>
    /// Other Event Container.
    /// </summary>
    public class OtherEventContainer : BaseEventContainer<OtherEvent>
    {
        /// <summary>
        /// Initialise a new instance of <see cref="OtherEventContainer"/>.
        /// </summary>
        public OtherEventContainer()
        {
            Drafts = new List<OtherEvent>();
            History = new List<OtherEvent>();
        }

        /// <inheritdoc/>
        public override void Add(OtherEvent theEvent)
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
                var draft = Drafts.SingleOrDefault(d => d.EventId == theEvent.EventId);
                if (draft != null)
                {
                    Drafts.Remove(draft);
                }

                return;
            }
            else
            {
                theEvent.Created = theEvent.Created == null ? DateTime.Now : theEvent.Created;
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

    /// <summary>
    /// Pre Scrutiny Event Container.
    /// </summary>
    public class PreScrutinyEventContainer : BaseEventContainer<PreScrutinyEvent>
    {
    }

    /// <summary>
    /// Bereaved Discussion Event Container.
    /// </summary>
    public class BereavedDiscussionEventContainer : BaseEventContainer<BereavedDiscussionEvent>
    {
    }

    /// <summary>
    /// Meo Summary Event Container.
    /// </summary>
    public class MeoSummaryEventContainer : BaseEventContainer<MeoSummaryEvent>
    {
    }

    /// <summary>
    /// Admission Notes Event Container.
    /// </summary>
    public class AdmissionNotesEventContainer : BaseEventContainer<AdmissionEvent>
    {
    }

    /// <summary>
    /// Medical History Event Container.
    /// </summary>
    public class MedicalHistoryEventContainer : BaseEventContainer<MedicalHistoryEvent>
    {
    }

    /// <summary>
    /// Qap Discussion Event Container.
    /// </summary>
    public class QapDiscussionEventContainer : BaseEventContainer<QapDiscussionEvent>
    {
    }
}