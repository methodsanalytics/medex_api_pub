using System;
using System.ComponentModel.DataAnnotations;
using MedicalExaminer.Models.Enums;
using Newtonsoft.Json;

namespace MedicalExaminer.Models
{
    public class PreScrutinyEvent : IEvent
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
        [JsonProperty(PropertyName = "created")]
        public DateTime? Created { get; set; }

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

        /// <summary>
        /// Event Text.
        /// </summary>
        [JsonProperty(PropertyName = "medical_examiner_thoughts")]
        public string MedicalExaminerThoughts { get; set; }


        /// <summary>
        /// IsFinal, final = true, draft = false
        /// </summary>
        [JsonProperty(PropertyName = "is_final")]
        public bool IsFinal { get; set; }


        ///// <summary>
        ///// the type of event this is
        ///// </summary>
        [JsonProperty(PropertyName = "event_type")]
        public EventType EventType => EventType.PreScrutiny;

        /// <summary>
        /// Dictionary for circumstances of death radio button.
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "circumstances_of_death")]
        public OverallCircumstancesOfDeath? CircumstancesOfDeath { get; set; }

        /// <summary>
        /// Possible cause of death established during scrutiny by Medical Examiner 1a
        /// </summary>
        [JsonProperty(PropertyName = "cause_of_death_1a")]
        public string CauseOfDeath1a { get; set; }

        /// <summary>
        /// Possible cause of death established during scrutiny by Medical Examiner 1b
        /// </summary>
        [JsonProperty(PropertyName = "cause_of_death_1b")]
        public string CauseOfDeath1b { get; set; }

        /// <summary>
        /// Possible cause of death established during scrutiny by Medical Examiner 1c
        /// </summary>
        [JsonProperty(PropertyName = "cause_of_death_1c")]
        public string CauseOfDeath1c { get; set; }

        /// <summary>
        /// Possible cause of death established during scrutiny by Medical Examiner 2
        /// </summary>
        [JsonProperty(PropertyName = "cause_of_death_2")]
        public string CauseOfDeath2 { get; set; }

        /// <summary>
        /// Dictionary for Outcome Of Pre-Scrutiny radio button.
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "outcome_of_pre_scrutiny")]
        public OverallOutcomeOfPreScrutiny? OutcomeOfPreScrutiny { get; set; }

        /// <summary>
        /// Dictionary for Clinical Governance Review radio button.
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "clinical_governance_review")]
        public ClinicalGovernanceReview? ClinicalGovernanceReview { get; set; }

        /// <summary>
        /// Details of Clinical Governance Review if said yes for Clinical Governance Review radio button.
        /// </summary>
        [JsonProperty(PropertyName = "clinical_governance_review_text")]
        public string ClinicalGovernanceReviewText { get; set; }

        /// <summary>
        /// is the case a mortality review case
        /// </summary>
        [JsonProperty(PropertyName = "is_mortality_case_record_review")]
        public bool IsMortalityCaseRecordReview { get; set; }

        /// <summary>
        /// were there concerns raised by staff or relatives
        /// </summary>
        [JsonProperty(PropertyName = "death_where_relative_staff_raised_concerns")]
        public bool DeathWhereRelativeStaffRaisedConcerns { get; set; }

        /// <summary>
        /// patient had learning difficulties or mental illness
        /// </summary>
        [JsonProperty(PropertyName = "death_with_learning_difficulties_or_mental_illness")]
        public bool DeathWithLearningDifficultiesOrMentalIllness { get; set; }

        /// <summary>
        /// death in a specialty, diagnosis or treatment group where alarms have been raised
        /// </summary>
        [JsonProperty(PropertyName = "death_in_specialty_with_alarm_raised")]
        public bool DeathInSpecialtyWithAlarmRaised { get; set; }

        /// <summary>
        /// the patient was not expected to die.
        /// </summary>
        [JsonProperty(PropertyName = "death_where_patient_not_expected_to_die")]
        public bool DeathWherePatientNotExpectedToDie { get; set; }

        /// <summary>
        /// is there learning that can come from this death
        /// </summary>
        [JsonProperty(PropertyName = "death_where_there_is_learning")]
        public bool DeathWhereThereIsLearning { get; set; }

        /// <summary>
        /// maternity or neonatal death
        /// </summary>
        [JsonProperty(PropertyName = "death_in_maternity_or_neonatal")]
        public bool DeathInMaternityOrNeonatal { get; set; }

        /// <summary>
        /// was this a serious incident
        /// </summary>
        [JsonProperty(PropertyName = "serious_incident")]
        public bool SeriousIncident { get; set; }

        /// <summary>
        /// for discussion in m+m meeting.
        /// </summary>
        [JsonProperty(PropertyName = "m_and_m_meeting")]
        public bool MandMMeeting { get; set; }
    }
}
