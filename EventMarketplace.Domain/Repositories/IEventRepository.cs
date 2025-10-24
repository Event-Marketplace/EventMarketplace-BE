namespace EventMarketplace.Domain.Repositories;

public interface IEventRepository
{
    Task SetUnActiveByDate();
}