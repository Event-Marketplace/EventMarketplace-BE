using EventMarketplace.Application.Dtos.EventDtos;
using EventMarketplace.Application.Response;
using EventMarketplace.Domain.Enums;
using EventMarketplace.Domain.ValueObjects;

namespace EventMarketplace.Application.Commands.EventCommands.CreateEvent;

public static class EventAddressMapper
{
    public static (Address? Address, string? DescriptionPlace) MapLocationToEntity(CreateEventDto dto)
    {
        return dto.LocationType switch
        {
            LocationType.DescriptionPlace => (null, dto.EventPlaceDescription),
            LocationType.Address => (Address.Create(dto.Street, dto.City, dto.Number, dto.PostalCode),
                string.Empty),

            _ => throw new ArgumentOutOfRangeException(),
        };
    }
    
    public static (AddressResponse? AddressResponse, string? DescriptionPlace) MapLocationToResponse(LocationType locationType, Address? address, string? descriptionPlace)
    {
        return locationType switch
        {
            LocationType.DescriptionPlace => (null, descriptionPlace),
            LocationType.Address => (new AddressResponse(){PostalCode = address.PostalCode, City = address.City, Number = address.Number, Street = address.Street}, string.Empty),

            _ => throw new ArgumentOutOfRangeException(),
        };
    }

}