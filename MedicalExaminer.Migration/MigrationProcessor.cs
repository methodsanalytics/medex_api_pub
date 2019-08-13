using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Settings;
using MedicalExaminer.Migration.MigrationDefinitions;
using MedicalExaminer.Migration.MigrationDefinitions.Examinations;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MedicalExaminer.Migration
{
    public class MigrationProcessor<T> : MigrationProcessorBase
    {
        IDatabaseAccess _dataAccess;
        IConnectionSettings _connectionSettings;
        IMigrationSettings _migrationSettings;

        public MigrationProcessor(IDatabaseAccess dataAccess, IConnectionSettings connectionSettings, IMigrationSettings migrationSettings)
        {
            _dataAccess = dataAccess;
            _connectionSettings = connectionSettings; // not sure this is needed, and tired.
            _migrationSettings = migrationSettings;
        }

        private Dictionary<int, IMigrationDefinition> _migrations = new Dictionary<int, IMigrationDefinition>()
        {
            {0, new ExaminationMigrationDefinitionV0() }
        };

        public IEnumerable<T> Migrate(object[] toMigrate)
        {
            var results = new List<T>();
            
            var recordsToMigrate = new object[] { };  // _dataAccess.GetItems(...)

            foreach (var record in recordsToMigrate)
            {
                int version = 0;
                IMigrationDefinition migrationDefinition = null;
                var recordAsDictionary = GetDictionary(record);
                var migratedAsDictionary = new Dictionary<string, object>();
                if (!recordAsDictionary.ContainsKey("Version"))
                {
                    migrationDefinition = _migrations[version];
                    migratedAsDictionary = MigrateToVersion(recordAsDictionary, migrationDefinition);
                }

                version = (int)migratedAsDictionary["Version"];

                while (version < _migrationSettings.Version)
                {
                    version = version + 1;
                    migrationDefinition = _migrations[version];
                    migratedAsDictionary = MigrateToVersion(migratedAsDictionary, migrationDefinition);
                }

                var migratedAsJson = JsonConvert.SerializeObject(migratedAsDictionary);
                var migratedObject = JsonConvert.DeserializeObject<T>(migratedAsJson);
                results.Add(migratedObject);
            }

            return results;
        }        
    }
}
