using MedicalExaminer.Migration.MigrationDefinitions;
using MedicalExaminer.Migration.MigrationDefinitions.Examinations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MedicalExaminer.Migration
{
    public class MigrationProcessor<T> : MigrationProcessorBase
    {
        private Dictionary<int, IMigrationDefinition> _migrations = new Dictionary<int, IMigrationDefinition>()
        {
            {1, new ExaminationMigrationDefinitionV1() },
            {2, new ExaminationMigrationDefinitionV2() }
        };

        public IEnumerable<T> Migrate(IEnumerable<object> recordsToMigrate, int migrateToVersion)
        {
            var results = new List<T>();
            try
            {
                foreach (var record in recordsToMigrate)
                {
                    bool updated = false;
                    long version = 1;
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
                        version += 1;
                        migrationDefinition = _migrations[(int)version];
                        migratedAsDictionary = MigrateToVersion(recordAsDictionary, migrationDefinition);
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
            catch(Exception ex)
            {
                var t = ex;
                throw new Exception();
            }
        }        
    }
}
