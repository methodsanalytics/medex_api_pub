﻿using System.Collections.Generic;

namespace MedicalExaminer.Common.Queries.Location
{
    /// <summary>
    /// Location Parents Query.
    /// </summary>
    public class LocationParentsQuery : IQuery<IEnumerable<Models.Location>>
    {
        /// <summary>
        /// Initialise a new instance of <see cref="LocationParentsQuery"/>.
        /// </summary>
        /// <param name="locationId">Location Id.</param>
        public LocationParentsQuery(string locationId)
        {
            LocationId = locationId;
        }

        /// <summary>
        /// Location Id.
        /// </summary>
        public string LocationId { get; }
    }
}
