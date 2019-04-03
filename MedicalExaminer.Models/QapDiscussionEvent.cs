﻿using System;
using System.Collections.Generic;
using System.Text;
using MedicalExaminer.Models.Enums;
using Newtonsoft.Json;

namespace MedicalExaminer.Models
{
    public class QapDiscussionEvent
    {
        /// <summary>
        /// Event Identification.
        /// </summary>
        [JsonProperty(PropertyName = "event_id")]
        public string EventId { get; set; }

        /// <summary>
        /// User Identifier.
        /// </summary>
        [JsonProperty(PropertyName = "user_id")]
        public string UserId { get; set; }

        //***************** uncomment after the generic event classes approved *********************
        ///// <summary>
        ///// Dictionary for the status (Draft or Final).
        ///// </summary>
        //[JsonProperty(PropertyName = "event_status")]
        //public EventStatus EventStatus { get; set; }

        ///// <summary>
        ///// list of ammendments for this item
        ///// </summary>
        //[JsonProperty(PropertyName = "amendments")]
        //public IEnumerable<IEvent> Amendments { get; set; }

        ///// <summary>
        ///// the type of event this is
        ///// </summary>
        //[JsonProperty(PropertyName = "event_type")]
        //public EventType EventType => EventType.BereavedDiscussion;

        /// <summary>
        /// Participant's roll.
        /// </summary>
        [JsonProperty(PropertyName = "participant_roll")]
        public string ParticipantRoll { get; set; }

        /// <summary>
        /// Participant's organisation.
        /// </summary>
        [JsonProperty(PropertyName = "participant_organisation")]
        public string ParticipantOrganisation { get; set; }

        /// <summary>
        /// Participant's phone number.
        /// </summary>
        [JsonProperty(PropertyName = "participant_phone_number")]
        public string ParticipantPhoneNumber { get; set; }
        
        /// <summary>
        /// Date of Conversation.
        /// </summary>
        [JsonProperty(PropertyName = "date_of_conversation")]
        public DateTime DateOfConversation { get; set; }

        /// <summary>
        /// Unable to happen.
        /// </summary>
        [JsonProperty(PropertyName = "discussion_unable_happen")]
        public bool DiscussionUnableHappen { get; set; }

        /// <summary>
        /// Details of the Discussion.
        /// </summary>
        [JsonProperty(PropertyName = "discussion_details")]
        public string DiscussionDetails { get; set; }

        ///// <summary>
        ///// Outcome of the Bereaved Discussion.
        ///// </summary>
        [JsonProperty(PropertyName = "bereaved_discussion_outcome")]
        public QapDiscussionOutcome QapDiscussionOutcome { get; set; }
    }
}
