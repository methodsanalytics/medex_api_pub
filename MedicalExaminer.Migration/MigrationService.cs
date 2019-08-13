using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Migration.MigrationQueries;
using MedicalExaminer.Models;
using Microsoft.Azure.Documents.SystemFunctions;

namespace MedicalExaminer.Migration
{
    public class MigrationService : QueryHandler<ExaminationMigrationQuery, bool>
    {
        private MigrationProcessor<Models.Examination> _migrationProcessor;
        public MigrationService(MigrationProcessor<Examination> migrationProcessor, IDatabaseAccess databaseAccess, IExaminationConnectionSettings connectionSettings) : base(databaseAccess, connectionSettings)
        {
            _migrationProcessor = migrationProcessor;
        }

        public async override Task<bool> Handle(ExaminationMigrationQuery param)
        {
            var examinations = await DatabaseAccess.GetItemsForMigration(ConnectionSettings, x => x.Version < param.VersionNumber
            || !x.Version.IsDefined());

            var migratedObjects = _migrationProcessor.Migrate(examinations, param.VersionNumber);

            return true;
        }
    }
}
