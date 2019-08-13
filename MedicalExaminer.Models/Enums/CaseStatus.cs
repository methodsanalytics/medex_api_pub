using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MedicalExaminer.Models.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum CaseStatus
    {
        HaveUnknownBasicDetails,
        // AdmissionNotesHaveBeenAdded,
        ReadyForMEScrutiny,
        Unassigned,
        HaveBeenScrutinisedByME,
        PendingAdditionalDetails,
        // PendingAdmissionNotes,
        PendingDiscussionWithQAP,
        PendingDiscussionWithRepresentative,
        HaveFinalCaseOutstandingOutcomes
    }
}