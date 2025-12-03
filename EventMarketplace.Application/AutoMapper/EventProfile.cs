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
            .ForMember(dest => dest.Address, opt => opt.MapFrom<AddressResolver>())
            .ForAllMembers(opt => 
                opt.Condition((src, dest, srcMember) => srcMember != null));
    }
}