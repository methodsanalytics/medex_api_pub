namespace MedicalExaminer.Common.Queries.Location
{
    /// <summary>
    /// Location Retrieval by Id Query.
    /// </summary>
    public class LocationRetrievalByIdQuery : IQuery<Models.Location>
    {
        /// <summary>
        /// Initialise a new instance of <see cref="LocationRetrievalByIdQuery"/>.
        /// </summary>
        /// <param name="locationId">Location id.</param>
        public LocationRetrievalByIdQuery(string locationId)
        {
            LocationId = locationId;
        }

        /// <summary>
        /// Location Id.
        /// </summary>
        public string LocationId { get; set; }
    }
}