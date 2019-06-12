using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;

namespace MedicalExaminer.Common.Database
{
    /// <summary>
    /// Database Access Interface.
    /// </summary>
    public interface IDatabaseAccess
    {
        /// <summary>
        /// Create Item Async.
        /// </summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="connectionSettings">Connection settings.</param>
        /// <param name="item">Item.</param>
        /// <param name="disableAutomaticIdGeneration">Disable automatic id generation.</param>
        /// <returns>Created Item.</returns>
        Task<T> CreateItemAsync<T>(
            IConnectionSettings connectionSettings,
            T item,
            bool disableAutomaticIdGeneration = false);

        /// <summary>
        /// Update Item Async.
        /// </summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="connectionSettings">Connection settings.</param>
        /// <param name="item">Item.</param>
        /// <returns>Updated Item.</returns>
        Task<T> UpdateItemAsync<T>(IConnectionSettings connectionSettings, T item);

        /// <summary>
        /// Get Item By Id.
        /// </summary>
        /// <remarks>Faster and cheaper to call than a SQL query.</remarks>
        /// <typeparam name="T">Type of document.</typeparam>
        /// <param name="connectionSettings">Connection Settings.</param>
        /// <param name="id">The ID of the document.</param>
        /// <returns>Item.</returns>
        Task<T> GetItemByIdAsync<T>(IConnectionSettings connectionSettings, string id);

        /// <summary>
        /// Get Item Async.
        /// </summary>
        /// <typeparam name="T">Type of document.</typeparam>
        /// <param name="connectionSettings">Connection Settings.</param>
        /// <param name="predicate">Predicate.</param>
        /// <returns>Item.</returns>
        Task<T> GetItemAsync<T>(IConnectionSettings connectionSettings, Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Ensure Collection Available.
        /// </summary>
        /// <param name="connectionSettings">Connection Settings.</param>
        void EnsureCollectionAvailable(IConnectionSettings connectionSettings);

        /// <summary>
        /// Get Items Async.
        /// </summary>
        /// <typeparam name="T">Type of document.</typeparam>
        /// <param name="connectionSettings">Connection Settings.</param>
        /// <param name="predicate">Predicate.</param>
        /// <returns>List of Items.</returns>
        Task<IEnumerable<T>> GetItemsAsync<T>(
            IConnectionSettings connectionSettings,
            Expression<Func<T, bool>> predicate)
            where T : class;

        /// <summary>
        /// Get Items Async.
        /// </summary>
        /// <typeparam name="T">Type of document.</typeparam>
        /// <typeparam name="TKey">Type of ordering.</typeparam>
        /// <param name="connectionSettings">Connection Settings.</param>
        /// <param name="predicate">Predicate.</param>
        /// <param name="orderBy">Order by,</param>
        /// <returns>List of items.</returns>
        Task<IEnumerable<T>> GetItemsAsync<T, TKey>(IConnectionSettings connectionSettings,
            Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> orderBy)
            where T : class;

        /// <summary>
        /// Get Count Async.
        /// </summary>
        /// <typeparam name="T">Type of document.</typeparam>
        /// <param name="connectionSettings">Connection Settings.</param>
        /// <param name="predicate">Predicate.</param>
        /// <returns>Number of items.</returns>
        Task<int> GetCountAsync<T>(IConnectionSettings connectionSettings, Expression<Func<T, bool>> predicate);
    }
}