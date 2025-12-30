using EventMarketplace.Domain.SharedModels;

namespace EventMarketplace.Domain.Repositories;

public interface IAdminRepository
{
    Task<AdminStats> GetAdminStatsAsync();
}