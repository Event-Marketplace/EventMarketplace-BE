using EventMarketplace.Application.Response.AdminResponse;
using EventMarketplace.Application.Services.Admin.Stats.Queries;

namespace EventMarketplace.Application.Services.Admin.Stats;

public interface IAdminStatsService
{
    Task<AlertsResponse> GetAlerts(GetAlertQuery query);
    Task<StatsResponse> GetStats(GetStatsQuery query);
}