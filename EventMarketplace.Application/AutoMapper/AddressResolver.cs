using AutoMapper;
using EventMarketplace.Application.Dtos.EventDtos;
using EventMarketplace.Application.Services.Events.CreateEvent;
using EventMarketplace.Domain.Entities;
using EventMarketplace.Domain.ValueObjects;

namespace EventMarketplace.Application.Mapper;

public class AddressResolver : IValueResolver<EditEventRequest, Event, Address?>
{
    public Address? Resolve(EditEventRequest source, Event destination, Address? destMember, ResolutionContext context)
    {
        if (string.IsNullOrEmpty(source.Street) && string.IsNullOrEmpty(source.City) &&
            string.IsNullOrEmpty(source.Number) && string.IsNullOrEmpty(source.PostalCode))
        {
            return destMember;
        }
        
        return Address.Create(
            source.Street ?? destMember?.Street,
            source.City ?? destMember?.City,
            source.Number ?? destMember?.Number,
            source.PostalCode ?? destMember?.PostalCode);
    }
}