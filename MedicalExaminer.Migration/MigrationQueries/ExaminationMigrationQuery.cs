using MedicalExaminer.Common.Queries;
using MedicalExaminer.Common.Settings;

namespace MedicalExaminer.Migration.MigrationQueries
{
    public class ExaminationMigrationQuery : IQuery<bool>, IMigrationQuery
    {
        private IMigrationSettings _examinationMigrationSettings;
        public ExaminationMigrationQuery(IMigrationSettings examinationMigrationSettings)
        {
            _examinationMigrationSettings = examinationMigrationSettings;
        }

        public int VersionNumber => _examinationMigrationSettings.Version;
    }
}
