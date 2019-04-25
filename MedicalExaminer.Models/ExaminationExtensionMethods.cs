﻿using System;
using System.Linq;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.Models
{
    public static class ExaminationExtensionMethods
    {
        public static Examination AddEvent(this Examination examination, IEvent theEvent)
        {
            switch (theEvent.EventType)
            {
                case EventType.Other:
                    var otherEventContainer = examination.CaseBreakdown.OtherEvents;

                    if (otherEventContainer == null)
                    {
                        otherEventContainer = new OtherEventContainer();
                    }

                    otherEventContainer.Add((OtherEvent)theEvent);
                    break;
                case EventType.PreScrutiny:
                    var preScrutinyEventContainer = examination.CaseBreakdown.PreScrutiny;

                    if (preScrutinyEventContainer == null)
                    {
                        preScrutinyEventContainer = new PreScrutinyEventContainer();
                    }

                    preScrutinyEventContainer.Add((PreScrutinyEvent)theEvent);
                    break;
                case EventType.BereavedDiscussion:
                    var bereavedDiscussionEventContainer = examination.CaseBreakdown.BereavedDiscussion;

                    if (bereavedDiscussionEventContainer == null)
                    {
                        bereavedDiscussionEventContainer = new BereavedDiscussionEventContainer();
                    }

                    bereavedDiscussionEventContainer.Add((BereavedDiscussionEvent)theEvent);
                    break;
                case EventType.MeoSummary:
                    var meoSummaryEventContainer = examination.CaseBreakdown.MeoSummary;

                    if (meoSummaryEventContainer == null)
                    {
                        meoSummaryEventContainer = new MeoSummaryEventContainer();
                    }

                    meoSummaryEventContainer.Add((MeoSummaryEvent)theEvent);
                    break;
                case EventType.QapDiscussion:
                    var qapDiscussionEventContainer = examination.CaseBreakdown.QapDiscussion;

                    if (qapDiscussionEventContainer == null)
                    {
                        qapDiscussionEventContainer = new QapDiscussionEventContainer();
                    }

                    qapDiscussionEventContainer.Add((QapDiscussionEvent)theEvent);
                    break;
                case EventType.MedicalHistory:
                    var medicalHistoryEventContainer = examination.CaseBreakdown.MedicalHistory;

                    if (medicalHistoryEventContainer == null)
                    {
                        medicalHistoryEventContainer = new MedicalHistoryEventContainer();
                    }

                    medicalHistoryEventContainer.Add((MedicalHistoryEvent)theEvent);
                    break;
                case EventType.Admission:
                    var admissionEventContainer = examination.CaseBreakdown.AdmissionNotes;

                    if (admissionEventContainer == null)
                    {
                        admissionEventContainer = new AdmissionNotesEventContainer();
                    }

                    admissionEventContainer.Add((AdmissionEvent)theEvent);
                    break;
                default:
                    throw new NotImplementedException();
            }

            examination = UpdateCaseStatus(examination);
            return examination;
        }

        public static Examination UpdateCaseUrgencyScore(this Examination examination)
        {
            var score = 0;
            const int defaultScoreWeighting = 100;
            const int overdueScoreWeighting = 1000;

            if (examination == null)
            {
                throw new ArgumentNullException(nameof(examination));
            }

            if (examination.ChildPriority)
            {
                score = score + defaultScoreWeighting;
            }

            if (examination.CoronerPriority)
            {
                score = score + defaultScoreWeighting;
            }

            if (examination.CulturalPriority)
            {
                score = score + defaultScoreWeighting;
            }

            if (examination.FaithPriority)
            {
                score = score + defaultScoreWeighting;
            }

            if (examination.OtherPriority)
            {
                score = score + defaultScoreWeighting;
            }

            if (DateTime.Now.AddDays(-4) > examination.CreatedAt)
            {
                score = score + overdueScoreWeighting;
            }

            examination.UrgencyScore = score;
            return examination;
        }

        public static Examination UpdateCaseStatus(this Examination examination)
        {
            examination.PendingAdmissionNotes = CalculateAdmissionNotesPending(examination);
            examination.AdmissionNotesHaveBeenAdded = !examination.PendingAdmissionNotes;
            examination.PendingDiscussionWithQAP = CalculatePendingQAPDiscussion(examination);
            examination.Unassigned = !(examination.MedicalTeam.MedicalExaminerOfficerUserId != null && examination.MedicalTeam.MedicalExaminerUserId != null);

            return examination;
        }

        private static bool CalculatePendingQAPDiscussion(Examination examination)
        {
            if (examination.ReadyForMEScrutiny)
            {
                return false;
            }
            else
            {
                if (examination.CaseBreakdown.AdmissionNotes.Latest != null && examination.CaseBreakdown.AdmissionNotes.Latest.ImmediateCoronerReferral)
                {
                    return false;
                }
                else
                {
                    if (examination.CaseBreakdown.QapDiscussion.Latest != null && examination.CaseBreakdown.QapDiscussion.Latest.DiscussionUnableHappen)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private static bool CalculateAdmissionNotesPending(Examination examination)
        {
            if(examination.CaseBreakdown.AdmissionNotes.Latest != null)
            {
                if(examination.MedicalTeam.ConsultantResponsible != null)
                {
                    return false;
                }
            }
            return true;
        }

        private static bool CalculateReadyForScrutiny(this Examination examination)
        {
            if(examination.CaseBreakdown.AdmissionNotes.Latest != null)
            {
                return examination.CaseBreakdown.AdmissionNotes.Latest.ImmediateCoronerReferral;
            }
            else
            {
                if(examination.CaseBreakdown.MeoSummary != null)
                {
                    return true;
                }
            }
            return false;
        }

    }
}
