using System;

namespace MedicalExaminer.Common.ConnectionSettings
{
    /// <summary>
    /// Location Connection Settings.
    /// </summary>
    public class LocationConnectionSettings : ILocationConnectionSettings
    {
        /// <summary>
        /// Initialise a new instance of <see cref="LocationConnectionSettings"/>.
        /// </summary>
        /// <param name="endPointUri">End Point Uri.</param>
        /// <param name="primaryKey">Primary key.</param>
        /// <param name="databaseId">Database Id.</param>
        public LocationConnectionSettings(Uri endPointUri, string primaryKey, string databaseId)
        {
            EndPointUri = endPointUri;
            PrimaryKey = primaryKey;
            DatabaseId = databaseId;
            Collection = "Locations";
        }

        /// <inheritdoc/>
        public Uri EndPointUri { get; }

        /// <inheritdoc/>
        public string PrimaryKey { get; }

        /// <inheritdoc/>
        public string DatabaseId { get; }

        /// <inheritdoc/>
        public string Collection { get; }
    }
}