using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Queries.User
{
    public class UserRetrievalByOktaIdQuery : IQuery<MeUser>
    {
        public string UsersOktaId { get; private set; }

        public UserRetrievalByOktaIdQuery(string usersOktaId)
        {
            UsersOktaId = usersOktaId;
        }
    }
}
