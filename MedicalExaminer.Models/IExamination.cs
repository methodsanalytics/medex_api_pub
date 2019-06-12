using System;
using System.Collections.Generic;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.Models
{
    /// <summary>
    /// Examination Interface.
    /// </summary>
    public interface IExamination
    {
        /// <summary>
        /// Patients first hospital number.
        /// </summary>
        string HospitalNumber_1 { get; set; }

        /// <summary>
        /// Patients second hospital number.
        /// </summary>
        string HospitalNumber_2 { get; set; }

        /// <summary>
        /// Patients third hospital number.
        /// </summary>
        string HospitalNumber_3 { get; set; }

        /// <summary>
        /// ID of MEO user who will be working on the scrutiny
        /// </summary>
        string MedicalExaminerOfficeResponsible { get; set; }

        /// <summary>
        /// ID of MEO user who will be working on the scrutiny
        /// </summary>
        string GenderDetails { get; set; }

        /// <summary>
        /// the unique identifier for the examination
        /// </summary>
        string ExaminationId { get; set; }

        /// <summary>
        /// time of death
        /// </summary>
        TimeSpan? TimeOfDeath { get; set; }

        /// <summary>
        /// Given Names (first names).
        /// </summary>
        string GivenNames { get; set; }

        /// <summary>
        /// surname / last name.
        /// </summary>
        string Surname { get; set; }

        /// <summary>
        /// the patients NHS number.
        /// </summary>
        string NhsNumber { get; set; }

        /// <summary>
        /// The patients gender.
        /// </summary>
        ExaminationGender Gender { get; set; }

        /// <summary>
        /// patients house name or number.
        /// </summary>
        string HouseNameNumber { get; set; }

        /// <summary>
        /// patients street.
        /// </summary>
        string Street { get; set; }

        /// <summary>
        /// patients town.
        /// </summary>
        string Town { get; set; }

        /// <summary>
        /// patients county.
        /// </summary>
        string County { get; set; }

        /// <summary>
        /// patients postcode.
        /// </summary>
        string Postcode { get; set; }

        /// <summary>
        /// patients country.
        /// </summary>
        string Country { get; set; }

        /// <summary>
        /// patients last occupation.
        /// </summary>
        string LastOccupation { get; set; }

        /// <summary>
        /// the location ID of the NHS organisation to administer care before the patient died.
        /// </summary>
        string OrganisationCareBeforeDeathLocationId { get; set; }

        /// <summary>
        /// The mode of disposal - e.g. buried / cremation.
        /// </summary>
        ModeOfDisposal ModeOfDisposal { get; set; }

        /// <summary>
        /// The name of the funeral directors.
        /// </summary>
        string FuneralDirectors { get; set; }

        /// <summary>
        /// Have any personal effects been collected from the patient?.
        /// </summary>
        // Personal affects
        bool AnyPersonalEffects { get; set; }

        /// <summary>
        /// If there have been personal effects collected from the patient, provide some details.
        /// </summary>
        string PersonalEffectDetails { get; set; }

        /// <summary>
        /// Where did the patient die?.
        /// </summary>
        string PlaceDeathOccured { get; set; }

        /// <summary>
        /// The date of birth of the patient.
        /// </summary>
        DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Date of death.
        /// </summary>
        DateTime? DateOfDeath { get; set; }

        // Flags that effect priority

        /// <summary>
        /// Cultural priority flag
        /// </summary>
        bool CulturalPriority { get; set; }

        /// <summary>
        /// is a Faith priority flag
        /// </summary>
        bool FaithPriority { get; set; }

        /// <summary>
        /// is a Child priority flag
        /// </summary>
        bool ChildPriority { get; set; }

        /// <summary>
        /// is a Coroner priority flag
        /// </summary>
        bool CoronerPriority { get; set; }

        /// <summary>
        /// is a other priority flag
        /// </summary>
        bool OtherPriority { get; set; }

        /// <summary>
        /// details about the other priority selected.
        /// </summary>
        string PriorityDetails { get; set; }

        /// <summary>
        /// Case completed.
        /// </summary>
        bool CaseCompleted { get; set; }

        /// <summary>
        /// Coroner status, updated with interaction with coroner
        /// </summary>
        CoronerStatus CoronerStatus { get; set; }

        /// <summary>
        /// Does the patient have any implants that may effect mode of disposal?.
        /// </summary>
        bool? AnyImplants { get; set; }

        /// <summary>
        /// Implant details if has implants.
        /// </summary>
        string ImplantDetails { get; set; }

        /// <summary>
        /// List of Representatives.
        /// </summary>
        IEnumerable<Representative> Representatives { get; set; }
    }
}