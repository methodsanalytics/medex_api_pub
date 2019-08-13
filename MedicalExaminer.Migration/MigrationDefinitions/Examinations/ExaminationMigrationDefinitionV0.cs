using MedicalExaminer.Models.Enums;
using System;
using System.Collections.Generic;

namespace MedicalExaminer.Migration.MigrationDefinitions.Examinations
{
    public class ExaminationMigrationDefinitionV0 : IMigrationDefinition
    {
        public Dictionary<string, string> PropertiesToRename => new Dictionary<string, string>();

        public List<string> PropertiesToRemove => new List<string>();

        public Dictionary<string, Func<Dictionary<string, object>, object>> Transforms => new Dictionary<string, Func<Dictionary<string, object>, object>>()
        {
            {"AnyImplants", MigrateAnyImplants },
            {"AnyPersonalEffects", MigrateAnyPersonalEffects },
            {"Version", AddVersionNumber }
        };

        private object AddVersionNumber(Dictionary<string, object> arg)
        {
            return 0;            
        }

        private object MigrateAnyPersonalEffects(Dictionary<string, object> arg)
        {
            if (arg.ContainsKey("AnyPersonalEffects"))
            {
                return ConvertAnyPersonalEffects((bool?)arg["AnyPersonalEffects"]);
            }
            throw new Exception();
        }

        private object ConvertAnyPersonalEffects(bool? v)
        {
            if(v == null)
            {
                return null;
            }
            if(v == false)
            {
                return PersonalEffects.No;
            }
            return PersonalEffects.Yes;
        }

        private object MigrateAnyImplants(Dictionary<string, object> arg)
        {
            if (arg.ContainsKey("AnyImplants"))
            {
                return ConvertAnyImplants((bool?)arg["AnyImplants"]);
            }
            throw new Exception();
        }

        private object ConvertAnyImplants(bool? v)
        {
            if (v == null)
            {
                return null;
            }
            if (v == false)
            {
                return AnyImplants.No;
            }
            return AnyImplants.Yes;
        }
    }
}
