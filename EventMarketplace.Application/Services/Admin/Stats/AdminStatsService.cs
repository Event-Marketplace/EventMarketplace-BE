using AutoMapper;
using EventMarketplace.Application.Response.AdminResponse;
using EventMarketplace.Application.Services.Admin.Stats.Queries;
using EventMarketplace.Domain.Repositories;

namespace EventMarketplace.Application.Services.Admin.Stats;

public class AdminStatsService(IAdminRepository adminRepository, IMapper mapper) : IAdminStatsService
{
    public async Task<AlertsResponse> GetAlerts(GetAlertQuery query)
    {
        var alerts = await adminRepository.GetAdminAlertsAsync();
        return mapper.Map<AlertsResponse>(alerts);
    }

    public async Task<StatsResponse> GetStats(GetStatsQuery query)
    {
        var stats = await adminRepository.GetAdminStatsAsync();
        return mapper.Map<StatsResponse>(stats);
    }
}