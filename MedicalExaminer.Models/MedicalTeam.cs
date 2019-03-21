﻿using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;


namespace MedicalExaminer.Models
{
    /// <summary>
    /// Details of medical team associated with medical examination
    /// </summary>
    public interface IMedicalTeam
    {
        /// <summary>
        /// Consultant primarily responsible for care
        /// </summary>
        ClinicalProfessional ConsultantResponsible { get; set; }


        /// <summary>
        /// Other consultants involved in care of the patient
        /// </summary>
        ClinicalProfessional[] ConsultantsOther { get; set; }

        /// <summary>
        /// General practitioner responsible for patient
        /// </summary>
        ClinicalProfessional GeneralPractitioner { get; set; }

        /// <summary>
        /// Clinician responsible for certification
        /// </summary>
        ClinicalProfessional Qap { get; set; }

        /// <summary>
        /// Nursing team information
        /// </summary>
        string NursingTeamInformation { get; set; }

        /// <summary>
        /// Medical Examiner
        /// </summary>
        MeUser MedicalExaminer { get; set; }

        /// <summary>
        /// Medical Examiner Officer
        /// </summary>
        MeUser MedicalExaminerOfficer { get; set; }
    }

    /// <inheritdoc />
    public class MedicalTeam : IMedicalTeam
    {
        /// <inheritdoc />
        [Required]
        [JsonProperty(PropertyName = "consultant_responsible")]
        public ClinicalProfessional ConsultantResponsible { get; set; }

        /// <inheritdoc />
        [Required]
        [JsonProperty(PropertyName = "consultants_other")]
        public ClinicalProfessional[] ConsultantsOther { get; set; }

        /// <inheritdoc />
        [Required]
        [JsonProperty(PropertyName = "general_practitioner")]
        public ClinicalProfessional GeneralPractitioner { get; set; }

        /// <inheritdoc />
        [Required]
        [JsonProperty(PropertyName = "qap")]
        public ClinicalProfessional Qap { get; set; }

        /// <inheritdoc />
        [JsonProperty(PropertyName = "nursing_team_information")]
        public string NursingTeamInformation { get; set; }

        /// <inheritdoc />
        [Required]
        [JsonProperty(PropertyName = "medical_examiner")]
        public MeUser MedicalExaminer { get; set; }

        /// <inheritdoc />
        [Required]
        [JsonProperty(PropertyName = "medical_examiner_officer")]
        public MeUser MedicalExaminerOfficer { get; set; }
    }
}