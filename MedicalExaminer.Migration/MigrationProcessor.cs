using MedicalExaminer.Common.Settings;
using MedicalExaminer.Migration.MigrationDefinitions;
using MedicalExaminer.Migration.MigrationDefinitions.Examinations;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MedicalExaminer.Migration
{
    public class MigrationProcessor<T> : MigrationProcessorBase
    {
        private Dictionary<int, IMigrationDefinition> _migrations = new Dictionary<int, IMigrationDefinition>()
        {
            {0, new ExaminationMigrationDefinitionV0() }
        };

        public IEnumerable<T> Migrate(IEnumerable<object> recordsToMigrate, int migrateToVersion)
        {
            var results = new List<T>();
            
            foreach (var record in recordsToMigrate)
            {
                int version = 0;
                IMigrationDefinition migrationDefinition = null;
                var recordAsDictionary = GetDictionary(record);
                var migratedAsDictionary = new Dictionary<string, object>();
                if (!recordAsDictionary.ContainsKey("version"))
                {
                    migrationDefinition = _migrations[version];
                    migratedAsDictionary = MigrateToVersion(recordAsDictionary, migrationDefinition);
                }

                version = (int)migratedAsDictionary["version"];

                while (version < migrateToVersion)
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
