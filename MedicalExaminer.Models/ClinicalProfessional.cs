using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace MedicalExaminer.Models
{
    /// <summary>
    /// Clinical Professional.
    /// </summary>
    public class ClinicalProfessional
    {
        /// <summary>
        /// Name.
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Role.
        /// </summary>
        [JsonProperty(PropertyName = "role")]
        public string Role { get; set; }

        /// <summary>
        /// Organisation.
        /// </summary>
        [JsonProperty(PropertyName = "organisation")]
        public string Organisation { get; set; }

        /// <summary>
        /// Phone.
        /// </summary>
        [JsonProperty(PropertyName = "phone")]
        public string Phone { get; set; }

        /// <summary>
        /// Notes.
        /// </summary>
        [JsonProperty(PropertyName = "notes")]
        public string Notes { get; set; }

        /// <summary>
        /// GMC Number.
        /// </summary>
        [JsonProperty(PropertyName = "gmc_number")]
        public string GMCNumber { get; set; }
    }
}