﻿using MedicalExaminer.API.Models.v1.Examinations;

namespace MedicalExaminer.API.Models.v1.MedicalTeams
{
    /// <summary>
    ///     GetMedicalTeamResponse class.
    /// </summary>
    public class GetMedicalTeamResponse : ResponseBase
    {
        /// <summary>
        /// Patient Details Header
        /// </summary>
        public PatientCardItem Header { get; set; }

        /// <summary>
        ///     Consultant primarily responsible for care of patient.
        /// </summary>
        public ClinicalProfessionalItem ConsultantResponsible { get; set; }

        /// <summary>
        ///     Other consultants involved in care of the patient.
        /// </summary>
        public ClinicalProfessionalItem[] ConsultantsOther { get; set; }

        /// <summary>
        ///     Consultant primarily responsible for care.
        /// </summary>
        public ClinicalProfessionalItem GeneralPractitioner { get; set; }

        /// <summary>
        ///     Clinician responsible for certification.
        /// </summary>
        public ClinicalProfessionalItem Qap { get; set; }

        /// <summary>
        ///     Nursing information.
        /// </summary>
        public string NursingTeamInformation { get; set; }

        /// <summary>
        ///     Medical Examiner.
        /// </summary>
        public string MedicalExaminerUserId { get; set; }

        /// <summary>
        ///     Medical Examiner Officer.
        /// </summary>
        public string MedicalExaminerOfficerUserId { get; set; }

        /// <summary>
        ///     Medical Examiner full name.
        /// </summary>
        public string MedicalExaminerFullName { get; set; }

        /// <summary>
        ///     Medical Examiner Officer full name.
        /// </summary>
        public string MedicalExaminerOfficerFullName { get; set; }
    }
}