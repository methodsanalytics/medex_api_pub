using System.Collections.Generic;

namespace MedicalExaminer.Common.Queries.Location
{
    /// <summary>
    /// Location Retrieval By Query.
    /// </summary>
    public class LocationsRetrievalByQuery : IQuery<IEnumerable<Models.Location>>
    {
        /// <summary>
        /// Initialise a new instance of the <see cref="LocationsRetrievalByQuery"/>.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="parentId">Parent Id.</param>
        /// <param name="forLookup">Is the query targeted at lookups.</param>
        /// <param name="onlyMeOffices">Filter by locations that are only ME offices.</param>
        /// <param name="permissedLocations">Permissed locations.</param>
        public LocationsRetrievalByQuery(string name, string parentId, bool forLookup, bool onlyMeOffices, IEnumerable<string> permissedLocations = null)
        {
            Name = name;
            ParentId = parentId;
            ForIdsOnly = false;
            ForLookup = forLookup;
            PermissedLocations = permissedLocations;
            OnlyMeOffices = onlyMeOffices;
        }

        /// <summary>
        /// Initialise a new instance of the <see cref="LocationsRetrievalByQuery"/>.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="parentId">Parent Id.</param>
        /// <param name="forIdsOnly">Is the query just for returning IDs</param>
        /// <param name="forLookup">Is the query targeted at lookups.</param>
        /// <param name="onlyMeOffices">Filter by locations that are only ME offices.</param>
        /// <param name="permissedLocations">Permissed locations.</param>
        public LocationsRetrievalByQuery(string name, string parentId, bool forIdsOnly, bool forLookup, bool onlyMeOffices, IEnumerable<string> permissedLocations = null)
        {
            Name = name;
            ParentId = parentId;
            ForIdsOnly = forIdsOnly;
            ForLookup = forLookup;
            PermissedLocations = permissedLocations;
            OnlyMeOffices = onlyMeOffices;
        }

        /// <summary>
        /// return only locations defined as ME Offices
        /// </summary>
        public bool OnlyMeOffices { get; }

        /// <summary>
        /// For IDs only.
        /// </summary>
        /// <remarks>Set to true to restrict results to only return IDs</remarks>
        public bool ForIdsOnly { get; }

        /// <summary>
        /// For Lookup
        /// </summary>
        /// <remarks>Set to true to restrict the fields that get returned.</remarks>
        public bool ForLookup { get; }

        /// <summary>
        /// Name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Parent Id.
        /// </summary>
        public string ParentId { get; }

        /// <summary>
        /// Permissed Locations.
        /// </summary>
        public IEnumerable<string> PermissedLocations { get; }
    }
}