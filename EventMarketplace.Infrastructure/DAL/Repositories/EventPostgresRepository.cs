using EventMarketplace.Domain.Entities;
using EventMarketplace.Domain.Enums;
using EventMarketplace.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace EventMarketplace.Infrastructure.DAL.Repositories;

public class EventPostgresRepository(EventMarketplaceDbContext context) : IEventRepository
{

    public async Task AddEventAsync(Event eventEntity, CancellationToken cancellationToken)
    {
        await context.Events.AddAsync(eventEntity, cancellationToken);
    }

    public Task DeleteEventAsync(Event @event)
    {
        context.Events.Remove(@event);
        return Task.CompletedTask;
    }

    public async Task<Event> GetEventByIdAsync(Guid id)
        => await context.Events
            .Include(x => x.EventComments)
            .SingleOrDefaultAsync(x => x.Id == id);

    public async Task DeleteEventByIdAsync(Guid id)
    {
        await context.Events.Where(x => x.Id == id).ExecuteDeleteAsync();
    }

    public Task UpdateEventAsync(Event @event)
    {
        context.Events.Update(@event);
        return Task.CompletedTask;
    }

    public async Task<bool> CheckIsEventExist(Guid eventId)
    {
        return await context.Events.AnyAsync(x => x.Id == eventId);
    }

    public async Task<bool> ExistsByIdAndOwner(Guid eventId, Guid ownerId)
        => await context.Events.AnyAsync(x => x.Id == eventId && x.OrganizerId == ownerId);

    public async Task<EventRegistrationResult> RegisterParticipantAsync(
        Guid eventId,
        Guid participantId,
        CancellationToken cancellationToken)
    {
        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

        var eventToRegister = await context.Events
            .SingleOrDefaultAsync(x => x.Id == eventId, cancellationToken);

        if (eventToRegister == null)
            return EventRegistrationResult.EventNotFound;

        if (eventToRegister.IsDeleted || eventToRegister.EventStatus != EventStatus.Approved)
            return EventRegistrationResult.EventUnavailable;

        if (await context.EventRegistrations.AnyAsync(
                x => x.EventId == eventId && x.ParticipantId == participantId,
                cancellationToken))
            return EventRegistrationResult.AlreadyRegistered;

        var updatedEvents = await context.Events
            .Where(x => x.Id == eventId && x.EventStatus == EventStatus.Approved && !x.IsDeleted && x.AvailableTickets > 0)
            .ExecuteUpdateAsync(
                setters => setters.SetProperty(x => x.AvailableTickets, x => x.AvailableTickets - 1),
                cancellationToken);

        if (updatedEvents == 0)
        {
            await context.Entry(eventToRegister).ReloadAsync(cancellationToken);
            return eventToRegister.EventStatus == EventStatus.Approved && !eventToRegister.IsDeleted && eventToRegister.AvailableTickets == 0
                ? EventRegistrationResult.NoSeatsAvailable
                : EventRegistrationResult.EventUnavailable;
        }

        context.EventRegistrations.Add(new EventRegistration
        {
            EventId = eventId,
            ParticipantId = participantId,
            RegisteredAt = DateTime.UtcNow
        });

        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException exception) when (IsUniqueConstraintViolation(exception))
        {
            return EventRegistrationResult.AlreadyRegistered;
        }

        await transaction.CommitAsync(cancellationToken);
        return EventRegistrationResult.Registered;
    }

    private static bool IsUniqueConstraintViolation(DbUpdateException exception)
        => exception.InnerException is PostgresException { SqlState: PostgresErrorCodes.UniqueViolation };
    
}
