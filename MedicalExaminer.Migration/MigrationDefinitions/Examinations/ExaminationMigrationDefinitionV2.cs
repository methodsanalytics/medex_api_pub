using System;
using System.Collections.Generic;

namespace MedicalExaminer.Migration.MigrationDefinitions.Examinations
{
    public class ExaminationMigrationDefinitionV2 : IMigrationDefinition
    {
        public Dictionary<string, string> PropertiesToRename => new Dictionary<string, string>();
        public List<string> PropertiesToRemove => new List<string>();

        public Dictionary<string, Func<Dictionary<string, object>, object>> Transforms => new Dictionary<string, Func<Dictionary<string, object>, object>>
        {
            {"version", UpdateVersionNumber}
        };

        private object UpdateVersionNumber(Dictionary<string, object> arg)
        {
            return 2;
        }
    }
}
