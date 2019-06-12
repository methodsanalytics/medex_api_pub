using System;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.Models
{
    /// <summary>
    /// Examination Extension Methods.
    /// </summary>
    public static class ExaminationExtensionMethods
    {
        /// <summary>
        /// Add Event.
        /// </summary>
        /// <param name="examination">An examination.</param>
        /// <param name="theEvent">The event.</param>
        /// <returns>Examination.</returns>
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

        /// <summary>
        /// Update Case Urgency Score.
        /// </summary>
        /// <param name="examination">The examination.</param>
        /// <returns>Examination.</returns>
        public static Examination UpdateCaseUrgencyScore(this Examination examination)
        {
            var score = 0;
            const int defaultScoreWeighting = 100;
            const int overdueScoreWeighting = 1000;

            if (examination == null)
            {
                throw new ArgumentNullException(nameof(examination));
            }

            if (examination.CaseCompleted)
            {
                examination.UrgencyScore = score;
                return examination;
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

        /// <summary>
        /// Update Case Status.
        /// </summary>
        /// <param name="examination">The examination.</param>
        /// <returns>Examination.</returns>
        public static Examination UpdateCaseStatus(this Examination examination)
        {
            examination.Unassigned = !(examination.MedicalTeam.MedicalExaminerOfficerUserId != null && examination.MedicalTeam.MedicalExaminerUserId != null);
            examination.PendingAdmissionNotes = CalculateAdmissionNotesPending(examination);
            examination.AdmissionNotesHaveBeenAdded = !examination.PendingAdmissionNotes;
            examination.ReadyForMEScrutiny = CalculateReadyForScrutiny(examination);
            examination.HaveBeenScrutinisedByME = examination.ScrutinyConfirmed;
            examination.PendingDiscussionWithQAP = CalculatePendingQAPDiscussion(examination);
            examination.PendingDiscussionWithRepresentative = CalculatePendingDiscussionWithRepresentative(examination);
            examination.PendingScrutinyNotes = CalculateScrutinyNotesPending(examination);
            examination.HaveFinalCaseOutcomesOutstanding = !examination.OutstandingCaseItemsCompleted;
            examination.CaseOutcome.CaseOutcomeSummary = CalculateScrutinyOutcome(examination);

            return examination;
        }

        /// <summary>
        /// Calculate Outstanding Case Outcomes Completed.
        /// </summary>
        /// <param name="examination">The examination.</param>
        /// <returns>True if outstanding case outcomes completed.</returns>
        public static bool CalculateOutstandingCaseOutcomesCompleted(this Examination examination)
        {
            if (examination.CaseOutcome.MccdIssued != null && examination.CaseOutcome.MccdIssued.Value)
            {
                if (examination.CaseOutcome.CremationFormStatus == CremationFormStatus.No ||
                    examination.CaseOutcome.CremationFormStatus == CremationFormStatus.Unknown)
                {
                    return true;
                }
                else if (examination.CaseOutcome.GpNotifiedStatus == GPNotified.GPNotified ||
                        examination.CaseOutcome.GpNotifiedStatus == GPNotified.NA)
                {
                    return true;
                }
            }

            if (examination.CaseOutcome.CaseOutcomeSummary == CaseOutcomeSummary.ReferToCoroner)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Calculate Can Complete Scrutiny.
        /// </summary>
        /// <param name="examination">The examination.</param>
        /// <returns>True if can complete scrutiny.</returns>
        public static bool CalculateCanCompleteScrutiny(this Examination examination)
        {
            examination = examination.UpdateCaseStatus();

            if (!examination.ReadyForMEScrutiny)
            {
                return false;
            }

            if (examination.Unassigned)
            {
                return false;
            }

            if (examination.PendingScrutinyNotes)
            {
                return false;
            }

            if (examination.PendingAdmissionNotes)
            {
                return false;
            }

            if (examination.PendingDiscussionWithQAP)
            {
                if (examination.PendingDiscussionWithRepresentative)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Calculate Requires Coroner Referral.
        /// </summary>
        /// <param name="examination">The examination.</param>
        /// <returns>True if requires coroner referral.</returns>
        public static bool CalculateRequiresCoronerReferral(this Examination examination)
        {
            return examination.CaseOutcome.CaseOutcomeSummary == CaseOutcomeSummary.ReferToCoroner ||
                examination.CaseOutcome.CaseOutcomeSummary == CaseOutcomeSummary.IssueMCCDWith100a;
        }

        private static bool CalculatePendingDiscussionWithRepresentative(Examination examination)
        {
            if (examination.ReadyForMEScrutiny)
            {
                return false;
            }
            else
            {
                if (examination.CaseBreakdown.AdmissionNotes.Latest != null && examination.CaseBreakdown.AdmissionNotes.Latest.ImmediateCoronerReferral.Value)
                {
                    return false;
                }
                else
                {
                    var latest = examination.CaseBreakdown.BereavedDiscussion.Latest;

                    if (latest != null && !latest.DiscussionUnableHappen)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private static CaseOutcomeSummary? CalculateScrutinyOutcome(Examination examination)
        {
            if (examination.CaseBreakdown.PreScrutiny.Latest == null
                && examination.CaseBreakdown.QapDiscussion.Latest == null
                && examination.CaseBreakdown.BereavedDiscussion.Latest == null)
            {
                return null;
            }

            if (examination.CaseBreakdown.QapDiscussion?.Latest?.QapDiscussionOutcome == QapDiscussionOutcome.ReferToCoronerFor100a)
            {
                return CaseOutcomeSummary.ReferToCoroner;
            }

            if (examination.CaseBreakdown.PreScrutiny?.Latest?.OutcomeOfPreScrutiny == OverallOutcomeOfPreScrutiny.ReferToCoronerFor100a
                && examination.CaseBreakdown.QapDiscussion?.Latest?.QapDiscussionOutcome == QapDiscussionOutcome.ReferToCoronerFor100a)
            {
                return CaseOutcomeSummary.ReferToCoroner;
            }

            if (examination.CaseBreakdown.BereavedDiscussion?.Latest?.BereavedDiscussionOutcome == BereavedDiscussionOutcome.ConcernsCoronerInvestigation)
            {
                return CaseOutcomeSummary.ReferToCoroner;
            }
            else if (examination.CaseBreakdown.BereavedDiscussion?.Latest?.BereavedDiscussionOutcome == BereavedDiscussionOutcome.ConcernsRequires100a)
            {
                return CaseOutcomeSummary.IssueMCCDWith100a;
            }
            else
            {
                return CaseOutcomeSummary.IssueMCCD;
            }
        }

        private static bool CalculatePendingQAPDiscussion(Examination examination)
        {
            if (examination.ReadyForMEScrutiny)
            {
                return false;
            }
            else
            {
                if (examination.CaseBreakdown.AdmissionNotes.Latest != null && examination.CaseBreakdown.AdmissionNotes.Latest.ImmediateCoronerReferral.Value)
                {
                    return false;
                }
                else
                {
                    var latest = examination.CaseBreakdown.QapDiscussion.Latest;

                    if (latest != null && !latest.DiscussionUnableHappen)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private static bool CalculateAdmissionNotesPending(Examination examination)
        {
            if (examination.CaseBreakdown.AdmissionNotes.Latest != null)
            {
                if (examination.MedicalTeam.ConsultantResponsible != null)
                {
                    return false;
                }
            }

            return true;
        }

        private static bool CalculateReadyForScrutiny(this Examination examination)
        {
            if (examination.CaseBreakdown.AdmissionNotes.Latest != null)
            {
                if (examination.CaseBreakdown.AdmissionNotes.Latest.ImmediateCoronerReferral.Value)
                {
                    return true;
                }
            }

            if (examination.CaseBreakdown.MeoSummary.Latest != null)
            {
                return true;
            }

            return false;
        }

        private static bool CalculateScrutinyNotesPending(Examination examination)
        {
            if (examination.CaseBreakdown.PreScrutiny.Latest != null)
            {
                return false;
            }

            return true;
        }
    }
}
