﻿using System;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.API.Models.v1.CaseBreakdown
{
    public class BereavedDiscussionEventItem : IEvent
    {
        /// <summary>
        /// the users name that created the event
        /// </summary>
        public string UsersFullName { get; set; }

        /// <summary>
        /// the users role that created the event
        /// </summary>
        public string UsersRole { get; set; }

        /// <summary>
        /// Date event was created
        /// </summary>
        public DateTime? Created { get; set; }

        /// <summary>
        /// Event Identification.
        /// </summary>
        public string EventId { get; set; }

        /// <summary>
        /// User Identifier.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Draft is false, final true
        /// </summary>
        public bool IsFinal { get; set; }

        /// <summary>
        /// the type of event this is
        /// </summary>
        public EventType EventType => EventType.BereavedDiscussion;

        /// <summary>
        /// Participant's full name.
        /// </summary>
        public string ParticipantFullName { get; set; }

        /// <summary>
        /// Participant's relationship.
        /// </summary>
        public string ParticipantRelationship { get; set; }

        /// <summary>
        /// Participant's phone number.
        /// </summary>
        public string ParticipantPhoneNumber { get; set; }

        /// <summary>
        /// Participant Present At Death.
        /// </summary>
        public PresentAtDeath PresentAtDeath { get; set; }

        /// <summary>
        /// Participant Informed At Death.
        /// </summary>
        public InformedAtDeath InformedAtDeath { get; set; }

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
        public BereavedDiscussionOutcome BereavedDiscussionOutcome { get; set; }
    }
}
