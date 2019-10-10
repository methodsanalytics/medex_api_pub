using System;
using MedicalExaminer.API.Models.v1.CaseBreakdown;
using MedicalExaminer.API.Models.v1.MedicalTeams;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.API.Models.v1.Report
{
    public interface IDownloadResponse
    {
        bool AbleToIssueMCCD { get; set; }
        bool? AnyImplants { get; set; }
        string CauseOfDeath1a { get; set; }
        string CauseOfDeath1b { get; set; }
        string CauseOfDeath1c { get; set; }
        string CauseOfDeath2 { get; set; }
        ClinicalProfessionalItem Consultant { get; set; }
        string County { get; set; }
        DateTime? DateOfBirth { get; set; }
        DateTime? DateOfDeath { get; set; }
        string DetailsAboutMedicalHistory { get; set; }
        ExaminationGender Gender { get; set; }
        string GivenNames { get; set; }
        ClinicalProfessionalItem GP { get; set; }
        string HouseNameNumber { get; set; }
        string ImplantDetails { get; set; }
        AdmissionEventItem LatestAdmissionDetails { get; set; }
        BereavedDiscussionEventItem LatestBereavedDiscussion { get; set; }
        string NhsNumber { get; set; }
        string PlaceOfDeath { get; set; }
        string Postcode { get; set; }
        ClinicalProfessionalItem Qap { get; set; }
        string Street { get; set; }
        string Surname { get; set; }
        TimeSpan? TimeOfDeath { get; set; }
        string Town { get; set; }
    }
}