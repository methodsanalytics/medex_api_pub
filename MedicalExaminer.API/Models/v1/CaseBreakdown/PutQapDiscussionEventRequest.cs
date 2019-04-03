﻿using System;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.API.Models.v1.CaseBreakdown
{
    public class PutQapDiscussionEventRequest
    {
        /// <summary>
        /// Event Identification.
        /// </summary>
        public string EventId { get; set; }

        /// <summary>
        /// User Identifier.
        /// </summary>
        public string UserId { get; set; }

        //***************** uncomment after the generic event classes approved *********************
        ///// <summary>
        ///// Dictionary for the status (Draft or Final).
        ///// </summary>
        //public EventStatus EventStatus { get; set; }

        ///// <summary>
        ///// list of ammendments for this item
        ///// </summary>
        //public IEnumerable<IEvent> Amendments { get; set; }

        ///// <summary>
        ///// the type of event this is
        ///// </summary>
        //public EventType EventType => EventType.BereavedDiscussion;

        /// <summary>
        /// Participant's roll.
        /// </summary>
        public string ParticipantRoll { get; set; }

        /// <summary>
        /// Participant's organisation.
        /// </summary>
        public string ParticipantOrganisation { get; set; }

        /// <summary>
        /// Participant's phone number.
        /// </summary>
        public string ParticipantPhoneNumber { get; set; }

        /// <summary>
        /// Date of Conversation.
        /// </summary>
        public DateTime DateOfConversation { get; set; }

        /// <summary>
        /// Unable to happen.
        /// </summary>
        public bool DiscussionUnableHappen { get; set; }

        /// <summary>
        /// Details of the Discussion.
        /// </summary>
        public string DiscussionDetails { get; set; }

        ///// <summary>
        ///// Outcome of the Bereaved Discussion.
        ///// </summary>
        public QapDiscussionOutcome QapDiscussionOutcome { get; set; }
    }
}
