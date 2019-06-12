namespace MedicalExaminer.Models
{
    /// <summary>
    /// Location Path.
    /// </summary>
    public class LocationPath : ILocationPath
    {
        /// <inheritdoc/>
        public string NationalLocationId { get; set; }

        /// <inheritdoc/>
        public string RegionLocationId { get; set; }

        /// <inheritdoc/>
        public string TrustLocationId { get; set; }

        /// <inheritdoc/>
        public string SiteLocationId { get; set; }
    }
}
