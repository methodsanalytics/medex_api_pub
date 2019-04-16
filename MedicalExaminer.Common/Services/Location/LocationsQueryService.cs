﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.Location;
using Microsoft.Extensions.Logging;

namespace MedicalExaminer.Common.Services.Location
{
    /// <summary>
    /// Location Query Service.
    /// </summary>
    /// <inheritdoc/>
    public class LocationsQueryService : QueryHandler<LocationsRetrievalByQuery, IEnumerable<Models.Location>>
    {
        /// <summary>
        /// Initialise a new instance of the Location Id Service.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="databaseAccess">Database Access.</param>
        /// <param name="connectionSettings">Connection Settings.</param>
        public LocationsQueryService(
            ILogger<LocationsQueryService> logger,
            IDatabaseAccess databaseAccess,
            ILocationConnectionSettings connectionSettings)
            : base(logger, databaseAccess, connectionSettings)
        {
        }

        /// <inheritdoc/>
        public override Task<IEnumerable<Models.Location>> Handle(LocationsRetrievalByQuery param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            try
            {
                var predicate = GetPredicate(param);

                return GetItemsAsync(predicate);
            }
            catch (Exception e)
            {
                Logger.LogCritical(0, e, "Failed to get Locations");
                throw;
            }
        }

        /// <summary>
        /// Get Predicate.
        /// </summary>
        /// <param name="param">Params.</param>
        /// <returns>The Predicate.</returns>
        private Expression<Func<Models.Location, bool>> GetPredicate(LocationsRetrievalByQuery param)
        {
            Expression<Func<Models.Location, bool>> predicate;

            if (param.Name == null && param.ParentId == null)
            {
                predicate = location => true;
            }
            else if (param.Name == null)
            {
                predicate = location => location.ParentId == param.ParentId;
            }
            else if (param.ParentId == null)
            {
                predicate = location => location.Name == param.Name;
            }
            else
            {
                predicate = location => location.Name == param.Name
                                        && location.ParentId == param.ParentId;
            }

            return predicate;
        }
    }
}