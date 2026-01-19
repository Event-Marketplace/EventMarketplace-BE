using System.ComponentModel;
using AutoMapper;
using EventMarketplace.Application.Dtos.EventDtos;
using EventMarketplace.Application.Response;
using EventMarketplace.Application.Response.EventResponse;
using EventMarketplace.Domain.Entities;
using EventMarketplace.Domain.Enums;
using EventMarketplace.Domain.ValueObjects;

namespace EventMarketplace.Application.Mapper;

public class EventProfile : Profile
{
    public EventProfile()
    {
        CreateMap<EditEventDto, Event>()
            .ForMember(dest => dest.Address,
                opt =>
                {
                    opt.MapFrom<AddressResolver>();
                    opt.Condition((src, dest, srcMember) => true); // zawsze mapuj
                })
            .ForAllMembers(opt =>
                opt.Condition((src, dest, srcMember) => srcMember != null));


        CreateMap<Address, AddressResponse>();
        CreateMap<Event, EventResponse>()
            .ForMember(dest => dest.AddressResponse, opt => 
                opt.MapFrom(src => src.LocationType == LocationType.Address ? src.Address : null))
            .ForMember(dest => dest.DescriptionEventPlace, opt => 
                opt.MapFrom(src => src.LocationType == LocationType.DescriptionPlace ? src.EventPlaceDescription : string.Empty))
            .ForMember(dest => dest.LocationType, opt => opt.MapFrom(src => src.LocationType.ToString()))
            .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.DurationOfTheEvent.StartEvent))
            .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.DurationOfTheEvent.EndEvent))
            .ForMember(dest => dest.StatusDisplayName, opt => opt.MapFrom(src => src.EventStatus.GetDisplayName()))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.EventStatus.ToString()))
            .ForMember(dest => dest.LocationType, opt => opt.MapFrom(src => src.LocationType.ToString()))
            .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.EventComments));

        CreateMap<EventComment, EventCommentResponse>()
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User.FullName))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")))
            .ForMember(dest => dest.WasRead, opt  => opt.MapFrom(src => src.WasReadByAdmin));

    }
}