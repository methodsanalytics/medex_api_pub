using MedicalExaminer.Models.Enums;
using Newtonsoft.Json;

namespace MedicalExaminer.Models
{
    /// <summary>
    /// Location.
    /// </summary>
    public class Location : ILocationPath
    {
        /// <summary>
        /// Location Id.
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string LocationId { get; set; }

        /// <summary>
        /// Name.
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Code.
        /// </summary>
        [JsonProperty(PropertyName = "code")]
        public string Code { get; set; }

        /// <summary>
        /// Parent Id.
        /// </summary>
        [JsonProperty(PropertyName = "parentId")]
        public string ParentId { get; set; }

        /// <summary>
        /// Is Active.
        /// </summary>
        [JsonProperty(PropertyName = "isActive")]
        public bool IsActive { get; set; }

        /// <summary>
        /// Location Type.
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        public LocationType Type { get; set; }

        /// <inheritdoc/>
        [JsonProperty(PropertyName = "national_location_id")]
        public string NationalLocationId { get; set; }

        /// <inheritdoc/>
        [JsonProperty(PropertyName = "region_location_id")]
        public string RegionLocationId { get; set; }

        /// <inheritdoc/>
        [JsonProperty(PropertyName = "trust_location_id")]
        public string TrustLocationId { get; set; }

        /// <inheritdoc/>
        [JsonProperty(PropertyName = "site_location_id")]
        public string SiteLocationId { get; set; }
    }
}