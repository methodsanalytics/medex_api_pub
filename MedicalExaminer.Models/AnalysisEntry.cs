using System.ComponentModel.DataAnnotations;
using MedicalExaminer.Models.Enums;
using Newtonsoft.Json;

namespace MedicalExaminer.Models
{
    /// <summary>
    /// Analysis Entry.
    /// </summary>
    public class AnalysisEntry : Record
    {
        /// <summary>
        /// Analysis Entry Id.
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "analysis_entry_id")]
        public string AnalysisEntryId { get; set; }

        /// <summary>
        /// User Id.
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "user_id")]
        public string UserId { get; set; }

        /// <summary>
        /// Examination Id.
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "examination_id")]
        public string ExaminationId { get; set; }

        /// <summary>
        /// Notes.
        /// </summary>
        [Required]
        [JsonProperty(PropertyName= "notes")]
        public string Notes { get; set; }

        /// <summary>
        /// Analysis Entry Type.
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "analysis_entry_type")]
        public AnalysisEntryType AnalysisEntryType { get; set; }

        /// <summary>
        /// Present At Death.
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "present_at_death")]
        public bool PresentAtDeath { get; set; }

        /// <summary>
        /// Informed Of Death.
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "informed_of_death")]
        public bool InformedOfDeath { get; set; }
    }
}