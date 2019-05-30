using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MedicalExaminer.Common.Services.User
{
    public class UserRetrievalByOktaIdService : QueryHandler<UserRetrievalByOktaIdQuery, MeUser>
    {
        public UserRetrievalByOktaIdService(IDatabaseAccess databaseAccess, IConnectionSettings connectionSettings) : base(databaseAccess, connectionSettings)
        {
        }

        public override Task<MeUser> Handle(UserRetrievalByOktaIdQuery param)
        {
            return DatabaseAccess.GetItemAsync<MeUser>(ConnectionSettings, x=>x.)
        }
    }
}
