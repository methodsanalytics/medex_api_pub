using System;
using System.Collections.Generic;

namespace MedicalExaminer.Migration.MigrationDefinitions.Examinations
{
    public class ExaminationMigrationDefinitionV2 : IMigrationDefinition
    {
        public Dictionary<string, string> PropertiesToRename => new Dictionary<string, string>();
        public List<string> PropertiesToRemove => new List<string>
        {
            "case_outcome"
        };

        public List<string> PropertiesToGet => new List<string>
        {
            "case_outcome"
        };

        public Dictionary<string, Func<Dictionary<string, object>, object>> Transforms => new Dictionary<string, Func<Dictionary<string, object>, object>>
        {
            {"version", AddVersionNumber},
        };

        private static object AddVersionNumber(Dictionary<string, object> arg)
        {
            return 2;
        }

        //private object GetCaseOutcome(string arg)
        //{
            
        //}
    }
}
