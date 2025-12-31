using System.Data;
using Dapper;
using EventMarketplace.Domain.Enums;
using EventMarketplace.Domain.Repositories;
using EventMarketplace.Domain.SharedModels;
using EventMarketplace.Infrastructure.DAL.QueryHandlers;
using EventMarketplace.Infrastructure.DAL.QueryHandlers.AdminQueryHandlers;

namespace EventMarketplace.Infrastructure.DAL.Repositories;

public class AdminRepository(IDbConnection connection) : IAdminRepository
{
    public async Task<AdminStats> GetAdminStatsAsync()
        => await connection.QuerySingleAsync<AdminStats>(AdminSqlQueries.AdminStats, 
            new
            {
                approvedStatus = EventStatus.Aproved,
                rejectedStatus = EventStatus.Rejected,
                pendingStatus = EventStatus.Submitted,
                organizerRoleType = RoleType.Organizer,
                participantRoleType = RoleType.Participant
            });

    public async Task<AdminAlerts> GetAdminAlertsAsync()
    {
        await using var multiQuery = await connection.QueryMultipleAsync(AdminSqlQueries.AdminAlerts, new
        {
            pendingStatus = EventStatus.Submitted
        });

        var alerts = new AdminAlerts()
        {
            PendingEvents = multiQuery.Read<AdminPendingEvents>().ToList(),
            PendingEventsCount = multiQuery.Read<int>().First()
        };

        return alerts;
    }
}