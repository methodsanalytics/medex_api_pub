using System.Threading.Tasks;

namespace MedicalExaminer.Common.Services
{
    /// <summary>
    /// Async Update Document Handler
    /// TODO: Marry up with IAsyncQueryHandler?
    /// </summary>
    public interface IAsyncUpdateDocumentHandler
    {
        /// <summary>
        /// Handle query.
        /// </summary>
        /// <param name="document">The document</param>
        /// <param name="userId">The user.</param>
        /// <returns>Examination.</returns>
        Task<Models.Examination> Handle(Models.Examination document, string userId);
    }
}