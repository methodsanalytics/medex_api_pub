using MedicalExaminer.Migration.MigrationDefinitions;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;

namespace MedicalExaminer.Migration
{
    public abstract class MigrationProcessorBase
    {

        protected Dictionary<string, object> MigrateToVersion(Dictionary<string, object> migratedAsDictionary, IMigrationDefinition migrationRule)
        {
            Dictionary<string, object> obtainedDictionary = new Dictionary<string, object>();

            if (migrationRule.PropertyToGet != null)
            {
                if (migratedAsDictionary.ContainsKey(migrationRule.PropertyToGet))
                {
                    var content = migratedAsDictionary[migrationRule.PropertyToGet];
                    obtainedDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(content.ToString());
                }
            }

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

        protected Dictionary<string, object> GetDictionary(object objectToGetDictionary)
        {
            var result = new Dictionary<string, object>();
            foreach (PropertyInfo propertyInfo in objectToGetDictionary.GetType().GetProperties())
            {
                result.Add(propertyInfo.Name, propertyInfo.GetValue(objectToGetDictionary, null));
            }
            if (result.ContainsKey("Payload"))
            {
                return ((JObject)result["Payload"]).ToObject<Dictionary<string, object>>();
            }
            return result;
        }
    }
}
