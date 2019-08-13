using MedicalExaminer.Models.Enums;
using System;
using System.Collections.Generic;

namespace MedicalExaminer.Migration.MigrationDefinitions.Examinations
{
    public class ExaminationMigrationDefinitionV1 : IMigrationDefinition
    {
        public Dictionary<string, string> PropertiesToRename => new Dictionary<string, string>();

        public List<string> PropertiesToRemove => new List<string>();

        public Dictionary<string, Func<Dictionary<string, object>, object>> Transforms => new Dictionary<string, Func<Dictionary<string, object>, object>>()
        {
            {"any_implants", MigrateAnyImplants },
            {"personal_effects_collected", MigrateAnyPersonalEffects },
            {"version", AddVersionNumber }
        };

        private object AddVersionNumber(Dictionary<string, object> arg)
        {
            return 1;            
        }

        private object MigrateAnyPersonalEffects(Dictionary<string, object> arg)
        {
            if (arg.ContainsKey("personal_effects_collected"))
            {
                return ConvertAnyPersonalEffects((bool?)arg["personal_effects_collected"]);
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
            if (arg.ContainsKey("any_implants"))
            {
                return ConvertAnyImplants((bool?)arg["any_implants"]);
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
