﻿using System;
using System.Collections.Generic;
using MedicalExaminer.Models.Enums;
using Newtonsoft.Json;

namespace MedicalExaminer.Models
{
    public class PatientDetails
    {
        /// <summary>
        /// Details of the patients date of birth
        /// </summary>
        DateTime DateOfBirth { get; set; }
        /// <summary>
        /// Patients given names
        /// </summary>
        public string GivenNames { get; set; }
        /// <summary>
        /// patients surname
        /// </summary>
        public string Surname { get; set; }
        /// <summary>
        /// Patients Postcode
        /// </summary>
        public string PostCode { get; set; }

        /// <summary>
        /// First line of patients addess
        /// </summary>
        public string HouseNameNumber { get; set; }

        /// <summary>
        /// Second line of patients address
        /// </summary>
        public string Street { get; set; }

        /// <summary>
        /// Patients town or city
        /// </summary>
        
        public string Town { get; set; }

        /// <summary>
        /// Patients county
        /// </summary>
        
        public string County { get; set; }

        /// <summary>
        /// Patients country
        /// </summary>
       
        public string Country { get; set; }

        /// <summary>
        /// Free text for any relevant patient occupation details
        /// </summary>
        public string LastOccupation { get; set; }

        /// <summary>
        /// Organisation responsible for patient at time of death
        /// </summary>
        public string OrganisationCareBeforeDeathLocationId { get; set; }

        /// <summary>
        /// Patients funeral arrangements
        /// </summary>
        
        public ModeOfDisposal FuneralArrangements { get; set; }

        /// <summary>
        /// Does the patient have any implants that may impact on cremation
        /// </summary>
        public bool AnyImplants { get; set; }

        /// <summary>
        /// Free text for the implant details
        /// </summary>
        public string ImplantDetails { get; set; }

        /// <summary>
        /// Details of the patients funeral directors
        /// </summary>
        public string FuneralDirectors { get; set; }

        /// <summary>
        /// Does the patient have any personal effects
        /// </summary>
        public bool AnyPersonalEffects { get; set; }

        /// <summary>
        /// Free text details of any personal effects
        /// </summary>
        public string PersonalEffectDetails { get; set; }
        /// <summary>
        /// Details of any representatives
        /// </summary>
        public IEnumerable<Representative> Representatives { get; set; }
    }
}
