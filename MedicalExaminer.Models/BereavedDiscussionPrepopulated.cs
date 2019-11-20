﻿using System;
using System.Collections.Generic;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.Models
{
    public class BereavedDiscussionPrepopulated
    {
        /// <summary>
        ///     Details of any representatives
        /// </summary>
        public IEnumerable<Representative> Representatives { get; set; }

        /// <summary>
        /// name of medical examiner
        /// </summary>
        public string MedicalExaminer { get; set; }

        /// <summary>
        /// name of medical examiner
        /// </summary>
        public string MedicalExaminerGmcNumber { get; set; }

        /// <summary>
        /// enum in [PrescrutinyHappened, PrescrutinyNotHappened]
        /// </summary>
        public PreScrutinyStatus PreScrutinyStatus { get; set; }

        /// <summary>
        /// date of latest pre-scrutiny event
        /// </summary>
        public DateTime? DateOfLatestPreScrutiny { get; set; }

        /// <summary>
        /// name of person who added last pre-scrutiny event
        /// </summary>
        public string UserForLatestPrescrutiny { get; set; }

        /// <summary>
        /// GMC Number Of User For Latest Pre-scrutiny
        /// </summary>
        public string GmcNumberOfUserForLatestPrescrutiny { get; set; }

        /// <summary>
        /// enum in [HappenedNoRevision, HappenedWithRevisions, CouldNotHappen, NoRecord]
        /// </summary>
        public QAPDiscussionStatus QAPDiscussionStatus { get; set; }

        /// <summary>
        /// date of latest qap discussion event
        /// </summary>
        public DateTime? DateOfLatestQAPDiscussion { get; set; }

        /// <summary>
        /// name of person who added last qap discussion event
        /// </summary>
        public string UserForLatestQAPDiscussion { get; set; }

        /// <summary>
        /// Gmc Number Of User For Latest QAP Discussion
        /// </summary>
        public string GmcNumberOfUserForLatestQAPDiscussion { get; set; }

        /// <summary>
        /// name of qap from the discussion event
        /// </summary>
        public string QAPNameForLatestQAPDiscussion { get; set; }

        /// <summary>
        /// cause of death 1a
        /// </summary>
        public string CauseOfDeath1a { get; set; }

        /// <summary>
        /// cause of death 1b
        /// </summary>
        public string CauseOfDeath1b { get; set; }

        /// <summary>
        /// cause of death 1c
        /// </summary>
        public string CauseOfDeath1c { get; set; }

        /// <summary>
        /// cause of death 2 
        /// </summary>
        public string CauseOfDeath2 { get; set; }
    }
}
