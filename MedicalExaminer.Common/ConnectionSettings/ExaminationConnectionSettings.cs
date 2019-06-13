using System;

namespace MedicalExaminer.Common.ConnectionSettings
{
    /// <summary>
    /// Examination Connection Settings.
    /// </summary>
    public class ExaminationConnectionSettings : IExaminationConnectionSettings
    {
        /// <summary>
        /// Initialise a new instance of <see cref="ExaminationConnectionSettings"/>.
        /// </summary>
        /// <param name="endPointUri">End point uri.</param>
        /// <param name="primaryKey">Primary key.</param>
        /// <param name="databaseId">Database id.</param>
        public ExaminationConnectionSettings(Uri endPointUri, string primaryKey, string databaseId)
        {
            if (endPointUri == null)
            {
                throw new ArgumentNullException(nameof(endPointUri));
            }

            if (string.IsNullOrEmpty(primaryKey))
            {
                throw new ArgumentNullException(nameof(primaryKey));
            }

            if (string.IsNullOrEmpty(databaseId))
            {
                throw new ArgumentNullException(nameof(databaseId));
            }

            EndPointUri = endPointUri;
            PrimaryKey = primaryKey;
            DatabaseId = databaseId;
            Collection = "Examinations";
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