using System;

namespace MedicalExaminer.Common.ConnectionSettings
{
    /// <summary>
    /// Audit Connection Setting.
    /// </summary>
    public class AuditConnectionSetting : IConnectionSettings
    {
        /// <summary>
        /// Initialise a new instance of <see cref="AuditConnectionSetting"/>.
        /// </summary>
        /// <param name="endPointUri">End Point Uri</param>
        /// <param name="primaryKey">Primary Key.</param>
        /// <param name="databaseId">Database Id.</param>
        /// <param name="collection">Collection name.</param>
        public AuditConnectionSetting(
            Uri endPointUri,
            string primaryKey,
            string databaseId,
            string collection)
        {
            EndPointUri = endPointUri;
            PrimaryKey = primaryKey;
            DatabaseId = databaseId;
            Collection = collection.AuditCollection();
        }

        /// <summary>
        /// End Point Uri.
        /// </summary>
        public Uri EndPointUri { get; private set; }

        /// <summary>
        /// Primary Key.
        /// </summary>
        public string PrimaryKey { get; private set; }

        /// <summary>
        /// Database Id.
        /// </summary>
        public string DatabaseId { get; private set; }

        /// <summary>
        /// Collection.
        /// </summary>
        public string Collection { get; private set; }
    }
}
