using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Queries.CaseBreakdown
{
    /// <summary>
    /// Create Event Query.
    /// </summary>
    public class CreateEventQuery : IQuery<EventCreationResult>
    {
        /// <summary>
        /// Initialise a new instance of <see cref="CreateEventQuery"/>.
        /// </summary>
        /// <param name="caseId">The case Id.</param>
        /// <param name="theEvent">The event.</param>
        public CreateEventQuery(string caseId, IEvent theEvent)
        {
            CaseId = caseId;
            Event = theEvent;
        }

        /// <summary>
        /// The Event.
        /// </summary>
        public IEvent Event { get; }

        /// <summary>
        /// Case Id.
        /// </summary>
        public string CaseId { get; }
    }
}
