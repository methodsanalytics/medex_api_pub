using System.Threading.Tasks;
using MedicalExaminer.Common.Queries;

namespace MedicalExaminer.Common.Services
{
    /// <summary>
    /// Async Query Handler Interface.
    /// </summary>
    /// <typeparam name="TQuery">Type of query.</typeparam>
    /// <typeparam name="TResult">Type of result.</typeparam>
    public interface IAsyncQueryHandler<in TQuery, TResult>
        where TQuery : IQuery<TResult>
    {
        /// <summary>
        /// Handle the query.
        /// </summary>
        /// <param name="param">The query.</param>
        /// <returns>The result from the query.</returns>
        Task<TResult> Handle(TQuery param);
    }
}