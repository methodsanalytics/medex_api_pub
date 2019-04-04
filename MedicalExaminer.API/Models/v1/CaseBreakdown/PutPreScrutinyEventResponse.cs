﻿namespace MedicalExaminer.API.Models.v1.CaseBreakdown
{
    public class PutPreScrutinyEventResponse : ResponseBase
    {
        /// <summary>
        ///     The id of the new Pre-Scrutiny Event.
        /// </summary>
        public string EventId { get; set; }
    }
}
