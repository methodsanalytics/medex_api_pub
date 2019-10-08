using System;
using System.Collections.Generic;

namespace MedicalExaminer.Migration.MigrationDefinitions
{
    public interface IMigrationDefinition
    {
        Dictionary<string, string> PropertiesToRename { get; }
        List<string> PropertiesToRemove { get; }
        string PropertyToGet { get; }

        Dictionary<string, Func<Dictionary<string, object>, object>> Transforms { get; }
    }
}
