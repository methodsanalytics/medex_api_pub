using MedicalExaminer.Common.Database;
using MedicalExaminer.Migration.MigrationDefinitions;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Reflection;

namespace MedicalExaminer.Migration
{
    public class MigrationService
    {
        IDatabaseAccess _dataAccess;
        public MigrationService(IDatabaseAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        private Dictionary<int, IMigrationDefinition> _migrations = new Dictionary<int, IMigrationDefinition>()
        {
        };

        public IEnumerable<T> Migrate<T>()
        {
            var versionToMigrateTo = 1; // get from appsettings
            var results = new List<T>();
            var recordsToMigrate = new object[] { };  // _dataAccess.GetItems(...)

            foreach (var record in recordsToMigrate)
            {
                int version = 0;
                var recordAsDictionary = GetDictionary(record);
                var migratedAsDictionary = new Dictionary<string, object>();
                if (!recordAsDictionary.ContainsKey("Version"))
                {
                    migratedAsDictionary = MigrateToVersion(0, recordAsDictionary);
                }

                version = (int)migratedAsDictionary["Version"];

                while (version < versionToMigrateTo)
                {
                    version = version + 1;
                    migratedAsDictionary = MigrateToVersion(version, migratedAsDictionary);
                }

                var migratedAsJson = JsonConvert.SerializeObject(migratedAsDictionary);
                var migratedObject = JsonConvert.DeserializeObject<T>(migratedAsJson);
                results.Add(migratedObject);
            }

            return results;
        }

        private Dictionary<string, object> MigrateToVersion(int version, Dictionary<string, object> migratedAsDictionary)
        {
            var migrationRule = _migrations[version];

            foreach (var transform in migrationRule.Transforms)
            {
                if (migratedAsDictionary.ContainsKey(transform.Key))
                {
                    migratedAsDictionary[transform.Key] = transform.Value(migratedAsDictionary);
                }
                else
                {
                    migratedAsDictionary.Add(transform.Key, transform.Value(migratedAsDictionary));
                }
            }

            foreach (var rename in migrationRule.PropertiesToRename)
            {
                if (migratedAsDictionary.ContainsKey(rename.Key))
                {
                    var temp = migratedAsDictionary[rename.Key];
                    migratedAsDictionary.Remove(rename.Key);
                    migratedAsDictionary.Add(rename.Value, temp);
                }
                else
                {
                    throw new System.Exception();
                }
            }


            foreach (var toBeRemoved in migrationRule.PropertiesToRemove)
            {
                migratedAsDictionary.Remove(toBeRemoved);
            }

            return migratedAsDictionary;
        }

        private Dictionary<string, object> GetDictionary(object objectToGetDictionary)
        {
            var result = new Dictionary<string, object>();
            foreach (PropertyInfo propertyInfo in objectToGetDictionary.GetType().GetProperties())
            {
                result.Add(propertyInfo.Name, propertyInfo.GetValue(objectToGetDictionary, null));
            }
            return result;
        }
    }
}
