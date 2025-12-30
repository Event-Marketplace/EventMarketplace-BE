using System.Data;
using Dapper;
using EventMarketplace.Application.Response.AdminResponse;
using EventMarketplace.Domain.Enums;
using EventMarketplace.Domain.Repositories;
using EventMarketplace.Domain.SharedModels;
using EventMarketplace.Infrastructure.DAL.QueryHandlers;

namespace EventMarketplace.Infrastructure.DAL.Repositories;

public class AdminRepository(IDbConnection connection) : IAdminRepository
{
    public async Task<AdminStats> GetAdminStatsAsync()
        => await connection.QuerySingleAsync<AdminStats>(SqlQueries.AdminStats, 
            new
            {
                approvedStatus = EventStatus.Aproved,
                rejectedStatus = EventStatus.Rejected,
                pendingStatus = EventStatus.Submitted,
                organizerRoleType = RoleType.Organizer,
                participantRoleType = RoleType.Participant
            });
    
}