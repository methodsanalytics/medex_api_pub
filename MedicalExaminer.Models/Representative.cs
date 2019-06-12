﻿using System;
using System.ComponentModel.DataAnnotations;
using MedicalExaminer.Models.Enums;
using Newtonsoft.Json;

namespace MedicalExaminer.Models
{
    /// <summary>
    /// Representative.
    /// </summary>
    public class Representative
    {
        /// <summary>
        /// Full Name.
        /// </summary>
        [JsonProperty(PropertyName = "full_name")]
        [Required]
        [DataType(DataType.Text)]
        [StringLength(250)]
        public string FullName { get; set; }

        /// <summary>
        /// Representatives relationship to the patient
        /// </summary>
        [JsonProperty(PropertyName = "relationship")]
        [Required]
        [DataType(DataType.Text)]
        [StringLength(250)]
        public string Relationship { get; set; }

        /// <summary>
        /// Representatives phone number
        /// </summary>
        [JsonProperty(PropertyName = "phone_number")]
        [Required]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Was the representative present at the death?
        /// </summary>
        [JsonProperty(PropertyName = "present_at_death")]
        [Required]
        public PresentAtDeath PresentAtDeath { get; set; }

        /// <summary>
        /// Has the representative been informed?
        /// </summary>
        [JsonProperty(PropertyName = "informed")]
        [Required]
        public Informed Informed { get; set; }

        /// <summary>
        /// appointment date
        /// </summary>
        [JsonProperty(PropertyName = "appointment_date")]
        [Required]
        public DateTime? AppointmentDate { get; set; }

        /// <summary>
        /// appointment time
        /// </summary>
        [JsonProperty(PropertyName = "appointment_time")]
        [Required]
        public TimeSpan? AppointmentTime { get; set; }

        /// <summary>
        /// representative notes
        /// </summary>
        [JsonProperty(PropertyName = "notes")]
        public string Notes { get; set; }
    }
}