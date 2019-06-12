using System;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.Models
{
    /// <summary>
    /// Event Interface.
    /// </summary>
    public interface IEvent
    {
        /// <summary>
        /// Event Type.
        /// </summary>
        EventType EventType { get; }

        /// <summary>
        /// Event Id.
        /// </summary>
        string EventId { get; set; }

        /// <summary>
        /// Is Final.
        /// </summary>
        bool IsFinal { get; }

        /// <summary>
        /// User Id.
        /// </summary>
        string UserId { get; set; }

        /// <summary>
        /// User Full Name.
        /// </summary>
        string UserFullName { get; set; }

        /// <summary>
        /// User Role.
        /// </summary>
        string UsersRole { get; set; }

        /// <summary>
        /// Created.
        /// </summary>
        DateTime? Created { get; set; }
    }
}
