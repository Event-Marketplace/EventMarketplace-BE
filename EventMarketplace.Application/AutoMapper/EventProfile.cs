using System.ComponentModel;
using AutoMapper;
using EventMarketplace.Application.Dtos.EventDtos;
using EventMarketplace.Domain.Entities;

namespace EventMarketplace.Application.Mapper;

public class EventProfile : Profile
{
    public EventProfile()
    {
        CreateMap<EditEventDto, Event>()
            .ForAllMembers(opt => 
                opt.Condition((src, dest, srcMember) => srcMember != null));
    }
}