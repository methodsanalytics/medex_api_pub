﻿using System;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.API.Models.V1.CaseBreakdown
{
    /// <summary>
    /// Pre Scrutiny Event Item.
    /// </summary>
    public class PreScrutinyEventItem : IEvent
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
        /// Event Identification.
        /// </summary>
        public string EventId { get; set; }

        /// <summary>
        /// User Identifier.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Event Text.
        /// </summary>
        public string MedicalExaminerThoughts { get; set; }

        /// <summary>
        /// IsFinal, final = true, draft = false
        /// </summary>
        public bool IsFinal { get; set; }

        /// <summary>
        /// the type of event this is
        /// </summary>
        public EventType EventType => EventType.PreScrutiny;

        /// <summary>
        /// Dictionary for circumstances of death radio button.
        /// </summary>
        public OverallCircumstancesOfDeath? CircumstancesOfDeath { get; set; }

        /// <summary>
        /// Possible cause of death established during scrutiny by Medical Examiner 1a
        /// </summary>
        public string CauseOfDeath1a { get; set; }

        /// <summary>
        /// Possible cause of death established during scrutiny by Medical Examiner 1b
        /// </summary>
        public string CauseOfDeath1b { get; set; }

        /// <summary>
        /// Possible cause of death established during scrutiny by Medical Examiner 1c
        /// </summary>
        public string CauseOfDeath1c { get; set; }

        /// <summary>
        /// Possible cause of death established during scrutiny by Medical Examiner 2
        /// </summary>
        public string CauseOfDeath2 { get; set; }

        /// <summary>
        /// Dictionary for Outcome Of Pre-Scrutiny radio button.
        /// </summary>
        public OverallOutcomeOfPreScrutiny? OutcomeOfPreScrutiny { get; set; }

        /// <summary>
        /// Dictionary for Clinical Governance Review radio button.
        /// </summary>
        public ClinicalGovernanceReview? ClinicalGovernanceReview { get; set; }

        /// <summary>
        /// Details of Clinical Governance Review if said yes for Clinical Governance Review radio button.
        /// </summary>
        public string ClinicalGovernanceReviewText { get; set; }
    }
}
