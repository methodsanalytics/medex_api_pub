﻿using System;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.API.Models.v1.CaseBreakdown
{
    /// <summary>
    /// Other Event Item.
    /// </summary>
    public class OtherEventItem : IEvent
    {
        /// <summary>
        /// Users full name
        /// </summary>
        public string UserFullName { get; set; }

        /// <summary>
        /// Users Role
        /// </summary>
        public string UsersRole { get; set; }

        /// <summary>
        /// Date event was created
        /// </summary>
        public DateTime? Created { get; set; }

        /// <summary>
        /// Event Identication.
        /// </summary>
        public string EventId { get; set; }

        /// <summary>
        /// User Identifier.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Event Text (Length to be confirmed).
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// IsFinal, final = true, draft = false
        /// </summary>
        public bool IsFinal { get; set; }

        /// <summary>
        /// the type of event this is
        /// </summary>
        public EventType EventType => EventType.Other;
    }
}
