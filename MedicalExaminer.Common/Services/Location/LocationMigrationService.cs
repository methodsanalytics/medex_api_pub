using System.Collections.Generic;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.MirationProcessors;
using MedicalExaminer.Common.MirationProcessors.Location;
using MedicalExaminer.Common.Queries;
using Microsoft.Azure.Documents.SystemFunctions;

namespace MedicalExaminer.Common.Services.Location
{
    public class LocationMigrationService : QueryHandler<IMigrationQuery, bool>
    {
        private Dictionary<int, ILocationMigrationProcessor> _processors = new Dictionary<int, ILocationMigrationProcessor>();

        public LocationMigrationService(IDatabaseAccess databaseAccess, ILocationConnectionSettings connectionSettings)
                : base(databaseAccess, connectionSettings)
        {
            _processors.Add(1, new LocationMigrationProcessorV1());
        }

        public async override Task<bool> Handle(IMigrationQuery param)
        {
            var locations = await DatabaseAccess.GetItemsAsync<Models.Location>(ConnectionSettings, x => x.Version < param.VersionNumber
            || !x.Version.IsDefined());

            foreach (var location in locations)
            {
                var processor = _processors[param.VersionNumber];
                var migratedLocation = processor.Handle(location, param);
                DatabaseAccess.UpdateItemAsync(ConnectionSettings, migratedLocation);
            }

            return true;
        }
    }
}
