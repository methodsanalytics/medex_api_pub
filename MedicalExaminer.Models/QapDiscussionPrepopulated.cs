﻿using System;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.Models
{
    public class QapDiscussionPrepopulated
    {
        /// <summary>
        /// Clinical Professional: QAP details from Medical Team
        /// </summary>
        public ClinicalProfessional Qap { get; set; }

        /// <summary>
        /// Possible cause of death established during scrutiny by Medical Examiner 1a
        /// </summary>
        public string CauseOfDeath1a { get; set; }

        /// <summary>
        /// Possible cause of death established during scrutiny by Medical Examiner 1b
        /// </summary>
        public string CauseOfDeath1b { get; set; }

        /// <summary>
        /// Possible cause of death established during scrutiny by Medical Examiner 1c
        /// </summary>
        public string CauseOfDeath1c { get; set; }

        /// <summary>
        /// Possible cause of death established during scrutiny by Medical Examiner 2
        /// </summary>
        public string CauseOfDeath2 { get; set; }

        /// <summary>
        /// Medical examiners full name
        /// </summary>
        public string MedicalExaminer { get; set; }

        /// <summary>
        /// Medical Examiner Gmc Number
        /// </summary>
        public string MedicalExaminerGmcNumber { get; set; }

        /// <summary>
        /// has the pre-scrutiny occured
        /// </summary>
        public PreScrutinyStatus PreScrutinyStatus { get; set; }

        /// <summary>
        /// date of the latest pre-scrutiny
        /// </summary>
        public DateTime? DateOfLatestPreScrutiny { get; set; }

        /// <summary>
        /// the users full name that added the latest pre scrutiny
        /// </summary>
        public string UserForLatestPrescrutiny { get; set; }

        /// <summary>
        /// GMC Number Of User For Latest Pre-scrutiny
        /// </summary>
        public string GmcNumberOfUserForLatestPrescrutiny { get; set; }
    }
}
