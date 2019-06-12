﻿using AutoMapper;
using MedicalExaminer.API.Models.v1.CaseBreakdown;
using MedicalExaminer.Models;
using System.Linq;

namespace MedicalExaminer.API.Extensions.Data
{
    public class CaseBreakdownProfile : Profile
    {
        public CaseBreakdownProfile()
        {
            CreateMap<CaseBreakDown, CaseBreakDownItem>()
            .ForMember(caseBreakDownItem => caseBreakDownItem.PatientDeathEvent, cbd => cbd.MapFrom(caseBreakDown => caseBreakDown.DeathEvent))
            .ForMember(caseBreakDownItem => caseBreakDownItem.OtherEvents, ev => ev.MapFrom((source, destination, destinationMember, context) =>
            {
                return EventContainerMapping(source.OtherEvents, context);
            }))
            .ForMember(caseBreakDownItem => caseBreakDownItem.MedicalHistory, ev => ev.MapFrom((source, destination, destinationMember, context) =>
             {
                 return EventContainerMapping(source.MedicalHistory, context);
             }))
            .ForMember(caseBreakDownItem => caseBreakDownItem.AdmissionNotes, ev => ev.MapFrom((source, destination, destinationMember, context) =>
            {
                return EventContainerMapping(source.AdmissionNotes, context);
            }))
            .ForMember(caseBreakDownItem => caseBreakDownItem.BereavedDiscussion, ev => ev.MapFrom((source, destination, destinationMember, context) =>
            {
                return EventContainerMapping(source.BereavedDiscussion, context);
            }))
            .ForMember(caseBreakDownItem => caseBreakDownItem.MeoSummary, ev => ev.MapFrom((source, destination, destinationMember, context) =>
            {
                return EventContainerMapping(source.MeoSummary, context);
            }))
            .ForMember(caseBreakDownItem => caseBreakDownItem.PreScrutiny, ev => ev.MapFrom((source, destination, destinationMember, context) =>
            {
                return EventContainerMapping(source.PreScrutiny, context);
            }))
            .ForMember(caseBreakDownItem => caseBreakDownItem.QapDiscussion, ev => ev.MapFrom((source, destination, destinationMember, context) =>
            {
                return EventContainerMapping(source.QapDiscussion, context);
            }));

        }

        private EventContainerItem<T> EventContainerMapping<T>(
            BaseEventContainer<T> source,
            ResolutionContext context)
            where T : IEvent
        {
            var myUser = (MeUser)context.Items["user"];
            var usersDraft = source.Drafts.SingleOrDefault(draft => draft.UserId == myUser.UserId);
            var usersDraftItem = context.Mapper.Map<T>(usersDraft);
            return new EventContainerItem<T>
            {
                UsersDraft = usersDraftItem,
                History = source.History.Select(hist => context.Mapper.Map<T>(hist)),
                Latest = context.Mapper.Map<T>(source.Latest)
            };
        }
    }
}
