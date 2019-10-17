﻿using System.ComponentModel.DataAnnotations.Schema;
using MedicalExaminer.API.Attributes;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.API.Models.v1.CaseBreakdown
{
    public class PutPreScrutinyEventRequest
    {
        /// <summary>
        /// Event Identification.
        /// </summary>
        public string EventId { get; set; }

        /// <summary>
        /// Event Text.
        /// </summary>
        public string MedicalExaminerThoughts { get; set; }

        ///// <summary>
        ///// IsFinal true for final, false for draft
        ///// </summary>
        public bool IsFinal { get; set; }

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
        /// To validate Clinical Governance Review Text
        /// </summary>
        [NotMapped]
        public bool IsFinalAndClinicalGovernanceReview => IsFinal &&
                       ClinicalGovernanceReview.Equals(MedicalExaminer.Models.Enums.ClinicalGovernanceReview.Yes);

        /// <summary>
        /// Details of Clinical Governance Review if said yes for Clinical Governance Review radio button.
        /// </summary>
        [RequiredIfAttributesMatch(nameof(IsFinalAndClinicalGovernanceReview), true)]
        public string ClinicalGovernanceReviewText { get; set; }

        /// <summary>
        /// is the case a mortality review case
        /// </summary>
        public bool IsMortalityCaseRecordReview { get; set; }

        /// <summary>
        /// were there concerns raised by staff or relatives
        /// </summary>
        public bool DeathWhereRelativeStaffRaisedConcerns { get; set; }

        /// <summary>
        /// patient had learning difficulties or mental illness
        /// </summary>
        public bool DeathWithLearningDifficultiesOrMentalIllness { get; set; }

        /// <summary>
        /// death in a specialty, diagnosis or treatment group where alarms have been raised
        /// </summary>
        public bool DeathInSpecialtyWithAlarmRaised { get; set; }

        /// <summary>
        /// the patient was not expected to die.
        /// </summary>
        public bool DeathWherePatientNotExpectedToDie { get; set; }

        /// <summary>
        /// is there learning that can come from this death
        /// </summary>
        public bool DeathWhereThereIsLearning { get; set; }

        /// <summary>
        /// maternity or neonatal death
        /// </summary>
        public bool DeathInMaternityOrNeonatal { get; set; }

        /// <summary>
        /// was this a serious incident
        /// </summary>
        public bool SeriousIncident { get; set; }

        /// <summary>
        /// for discussion in m+m meeting.
        /// </summary>
        public bool MandMMeeting { get; set; }
    }
}
