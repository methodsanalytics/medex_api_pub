using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MedicalExaminer.Models.Enums
{
    /// <summary>
    /// Case Status.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum CaseStatus
    {
        /// <summary>
        /// Admission notes have been Added.
        /// </summary>
        AdmissionNotesHaveBeenAdded,

        /// <summary>
        /// Ready for ME scrutiny.
        /// </summary>
        ReadyForMEScrutiny,

        /// <summary>
        /// Unassigned.
        /// </summary>
        Unassigned,

        /// <summary>
        /// Have been scrutinised by ME.
        /// </summary>
        HaveBeenScrutinisedByME,

        /// <summary>
        /// Pending Admission Notes.
        /// </summary>
        PendingAdmissionNotes,

        /// <summary>
        /// Pending Discussion with QAP.
        /// </summary>
        PendingDiscussionWithQAP,

        /// <summary>
        /// Pending Discussion with Representative.
        /// </summary>
        PendingDiscussionWithRepresentative,

        /// <summary>
        /// Have Final Case Outstanding Outcomes.
        /// </summary>
        HaveFinalCaseOutstandingOutcomes
    }
}