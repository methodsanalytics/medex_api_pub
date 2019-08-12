using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalExaminer.Migration.MigrationDefinitions
{
    public interface IMigrationDefinition
    {
        Dictionary<string, string> PropertiesToRename { get; }
        List<string> PropertiesToRemove { get; }

        Dictionary<string, Func<Dictionary<string, object>, object>> Transforms { get; }
    }
}
