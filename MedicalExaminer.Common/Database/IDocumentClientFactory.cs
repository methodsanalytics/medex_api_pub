using Cosmonaut;
using MedicalExaminer.Common.ConnectionSettings;
using Microsoft.Azure.Documents;

namespace MedicalExaminer.Common.Database
{
    /// <summary>
    /// Document Client Factory Interface.
    /// </summary>
    public interface IDocumentClientFactory
    {
        /// <summary>
        /// Create Client.
        /// </summary>
        /// <param name="connectionSettings">Connection settings.</param>
        /// <returns>Document client.</returns>
        IDocumentClient CreateClient(IClientSettings connectionSettings);

        /// <summary>
        /// Create Cosmos Store.
        /// </summary>
        /// <typeparam name="TEntity">Type of store.</typeparam>
        /// <param name="connectionSettings">Connection settings.</param>
        /// <returns>Document client.</returns>
        ICosmosStore<TEntity> CreateCosmosStore<TEntity>(IConnectionSettings connectionSettings)
            where TEntity : class;
    }
}
