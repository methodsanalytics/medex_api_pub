﻿using AutoMapper;

namespace MedicalExaminer.API.Extensions.Data
{
    /// <summary>
    ///     Extension method to add our profiles to the mapper configuration.
    /// </summary>
    public static class MedicalExaminerProfiles
    {
        /// <summary>
        ///     Add Our Profiles to the mapper configuration.
        /// </summary>
        /// <param name="config">The mapper configuration.</param>
        public static void AddMedicalExaminerProfiles(this IMapperConfigurationExpression config)
        {
            config.AddProfile<ExaminationProfile>();
            config.AddProfile<PermissionsProfile>();
            config.AddProfile<UsersProfile>();
            config.AddProfile<MedicalTeamProfile>();
            config.AddProfile<PatientDetailsProfile>();
            config.AddProfile<OtherEventProfile>();
            config.AddProfile<AdmissionEventProfile>();
            config.AddProfile<BereavedDiscussionEventProfile>();
            config.AddProfile<MedicalHistoryEventProfile>();
            config.AddProfile<MeoSummaryEventProfile>();
            config.AddProfile<NewExaminationProfile>();
            config.AddProfile<PreScrutinyEventProfile>();
            config.AddProfile<QapDiscussionEventProfile>();
            config.AddProfile<CaseOutcomeProfile>();
        }
    }
}