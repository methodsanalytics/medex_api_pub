using System;
using System.Collections.Generic;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.Models
{
    /// <summary>
    /// Patient Details.
    /// </summary>
    public class PatientDetails
    {
        /// <summary>
        /// Cultural Priority.
        /// </summary>
        public bool CulturalPriority { get; set; }

        /// <summary>
        /// Faith Priority.
        /// </summary>
        public bool FaithPriority { get; set; }

        /// <summary>
        /// Child Priority.
        /// </summary>
        public bool ChildPriority { get; set; }

        /// <summary>
        /// Coroner Priority.
        /// </summary>
        public bool CoronerPriority { get; set; }

        /// <summary>
        /// Other Priority.
        /// </summary>
        public bool OtherPriority { get; set; }

        /// <summary>
        /// Priority Details.
        /// </summary>
        public string PriorityDetails { get; set; }

        /// <summary>
        /// Case Completed.
        /// </summary>
        public bool CaseCompleted { get; set; }

        /// <summary>
        /// Coroner Status.
        /// </summary>
        public CoronerStatus CoronerStatus { get; set; }

        /// <summary>
        /// Place Death Occured.
        /// </summary>
        public string PlaceDeathOccured { get; set; }

        /// <summary>
        /// Gender details.
        /// </summary>
        public string GenderDetails { get; set; }

        /// <summary>
        /// Id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Medical Examiner Office Responsible.
        /// </summary>
        public string MedicalExaminerOfficeResponsible { get; set; }

        /// <summary>
        /// Gender.
        /// </summary>
        public ExaminationGender Gender { get; set; }

        /// <summary>
        ///     Details of the patients date of birth
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        ///     Details of the patients date of death
        /// </summary>
        public DateTime? DateOfDeath { get; set; }

        /// <summary>
        ///     Patients NHS Number
        /// </summary>
        public string NhsNumber { get; set; }

        /// <summary>
        ///     Patients first hospital number
        /// </summary>
        public string HospitalNumber_1 { get; set; }

        /// <summary>
        ///     Patients second hospital number
        /// </summary>
        public string HospitalNumber_2 { get; set; }

        /// <summary>
        ///     Patients third hospital number
        /// </summary>
        public string HospitalNumber_3 { get; set; }

        /// <summary>
        ///     Patients time of death
        /// </summary>
        public TimeSpan? TimeOfDeath { get; set; }

        /// <summary>
        ///     Patients given names
        /// </summary>
        public string GivenNames { get; set; }

        /// <summary>
        ///     patients surname
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        ///     Patients Postcode
        /// </summary>
        public string PostCode { get; set; }

        /// <summary>
        ///     First line of patients addess
        /// </summary>
        public string HouseNameNumber { get; set; }

        /// <summary>
        ///     Second line of patients address
        /// </summary>
        public string Street { get; set; }

        /// <summary>
        ///     Patients town or city
        /// </summary>
        public string Town { get; set; }

        /// <summary>
        ///     Patients county
        /// </summary>
        public string County { get; set; }

        /// <summary>
        ///     Patients country
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        ///     Free text for any relevant patient occupation details
        /// </summary>
        public string LastOccupation { get; set; }

        /// <summary>
        ///     Organisation responsible for patient at time of death
        /// </summary>
        public string OrganisationCareBeforeDeathLocationId { get; set; }

        /// <summary>
        ///     Patients funeral arrangements
        /// </summary>
        public ModeOfDisposal ModeOfDisposal { get; set; }

        /// <summary>
        ///     Does the patient have any implants that may impact on cremation
        /// </summary>
        public bool? AnyImplants { get; set; }

        /// <summary>
        ///     Free text for the implant details
        /// </summary>
        public string ImplantDetails { get; set; }

        /// <summary>
        ///     Details of the patients funeral directors
        /// </summary>
        public string FuneralDirectors { get; set; }

        /// <summary>
        ///     Does the patient have any personal effects
        /// </summary>
        public bool AnyPersonalEffects { get; set; }

        /// <summary>
        ///     Free text details of any personal effects
        /// </summary>
        public string PersonalEffectDetails { get; set; }

        /// <summary>
        ///     Details of any representatives
        /// </summary>
        public IEnumerable<Representative> Representatives { get; set; }
    }
}