using System;

namespace MedicalExaminer.Common.ConnectionSettings
{
    /// <summary>
    ///     User Connection Settings
    /// </summary>
    public class UserConnectionSettings : IUserConnectionSettings
    {
        /// <summary>
        /// Initialise a new instance of <see cref="UserConnectionSettings"/>.
        /// </summary>
        /// <param name="endPointUri">End Point URI</param>
        /// <param name="primaryKey">Primary Key.</param>
        /// <param name="databaseId">Database Id.</param>
        public UserConnectionSettings(Uri endPointUri, string primaryKey, string databaseId)
        {
            EndPointUri = endPointUri;
            PrimaryKey = primaryKey;
            DatabaseId = databaseId;
            Collection = "Users";
        }

        /// <summary>
        /// End Point Uri.
        /// </summary>
        public Uri EndPointUri { get; }

        /// <summary>
        /// Primary Key.
        /// </summary>
        public string PrimaryKey { get; }

        /// <summary>
        /// Database Id.
        /// </summary>
        public string DatabaseId { get; }

        /// <summary>
        /// Collection.
        /// </summary>
        public string Collection { get; }
    }
}