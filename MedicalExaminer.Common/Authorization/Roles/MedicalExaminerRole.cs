﻿using System;
using System.Collections.Generic;
using System.Text;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.Common.Authorization.Roles
{
    /// <summary>
    /// Medical Examiner Role.
    /// </summary>
    public class MedicalExaminerRole : Role
    {
        /// <summary>
        /// Initialise a new instance of <see cref="MedicalExaminerRole"/>.
        /// </summary>
        public MedicalExaminerRole()
            : base(UserRoles.MedicalExaminer)
        {
            Grant(
                Permission.GetLocations,
                Permission.GetLocation,

                Permission.GetExaminations,
                Permission.GetExamination,
                Permission.CreateExamination,
                Permission.AssignExaminationToMedicalExaminer,
                Permission.UpdateExamination,
                Permission.UpdateExaminationState,

                Permission.AddEventToExamination,
                Permission.GetExaminationEvents,
                Permission.GetExaminationEvent,

                Permission.GetProfile,
                Permission.UpdateProfile,

                // TODO: Discuss which ones MEs has
                Permission.BereavedDiscussionEvent,
                Permission.MeoSummaryEvent,
                Permission.QapDiscussionEvent,
                Permission.OtherEvent,
                Permission.AdmissionEvent,
                Permission.MedicalHistoryEvent,
                Permission.PreScrutinyEvent);
        }
    }
}
