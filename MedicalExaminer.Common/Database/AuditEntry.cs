using MedicalExaminer.Models;
using System;

namespace MedicalExaminer.Common.Database
{
    /// <summary>
    /// Audit Entry.
    /// </summary>
    /// <typeparam name="T">Type of Audit Entry.</typeparam>
    public class AuditEntry<T>
    {
        /// <summary>
        /// Initialise a new instance of <see cref="AuditEntry{T}"/>.
        /// </summary>
        /// <param name="item">Item of type.</param>
        public AuditEntry(T item)
        {
            Entry = item;
            Id = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// Entry
        /// </summary>
        public T Entry { get; private set; }
    }
}
