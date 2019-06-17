using System.Linq;
using Microsoft.Azure.Documents.Linq;

namespace MedicalExaminer.API.Tests.Services
{
    public interface IFakeDocumentQuery<T> : IDocumentQuery<T>, IOrderedQueryable<T>
    {
    }
}
