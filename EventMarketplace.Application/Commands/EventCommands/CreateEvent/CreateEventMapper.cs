using EventMarketplace.Application.Dtos.EventDtos;
using EventMarketplace.Domain.Enums;
using EventMarketplace.Domain.ValueObjects;

namespace EventMarketplace.Application.Commands.EventCommands.CreateEvent;

public static class CreateEventMapper
{
    public static (Address? Address, string? DescriptionPlace) MapLocation(CreateEventDto dto)
    {
        return dto.LocationType switch
        {
            LocationType.Description => (Address.Create(dto.Street, dto.City, dto.Number, dto.PostalCode),
                string.Empty),
            LocationType.Address => (null, dto.EventPlaceDescribtion),

            _ => throw new ArgumentOutOfRangeException(),
        };
    }
}