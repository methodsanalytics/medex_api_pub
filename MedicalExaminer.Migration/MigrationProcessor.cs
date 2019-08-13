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
            {0, new ExaminationMigrationDefinitionV1() }
        };

        public IEnumerable<T> Migrate(IEnumerable<object> recordsToMigrate, int migrateToVersion)
        {
            var results = new List<T>();
            
            foreach (var record in recordsToMigrate)
            {
                bool updated = false;
                long version = 0;
                IMigrationDefinition migrationDefinition = null;
                var recordAsDictionary = GetDictionary(record);
                var migratedAsDictionary = new Dictionary<string, object>();
                if (!recordAsDictionary.ContainsKey("version"))
                {
                    migrationDefinition = _migrations[(int)version];
                    migratedAsDictionary = MigrateToVersion(recordAsDictionary, migrationDefinition);
                    updated = true;
                }

                version = (long)recordAsDictionary["version"];

                while (version < migrateToVersion)
                {
                    version = version + 1;
                    migrationDefinition = _migrations[(int)version];
                    migratedAsDictionary = MigrateToVersion(migratedAsDictionary, migrationDefinition);
                    updated = true;
                }
                if (updated)
                {
                    var migratedAsJson = JsonConvert.SerializeObject(migratedAsDictionary);
                    var migratedObject = JsonConvert.DeserializeObject<T>(migratedAsJson);
                    results.Add(migratedObject);
                }
            }

            return results;
        }        
    }
}
