using System.Data;
using AutoMapper;
using Dapper;
using EventMarketplace.Application.Queries.AdminQueries;
using EventMarketplace.Application.Response.AdminResponse;
using EventMarketplace.Domain.Enums;
using EventMarketplace.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EventMarketplace.Infrastructure.DAL.QueryHandlers.AdminQueryHandlers;

public class GetStatsQueryHandler(
    IAdminRepository adminRepository, 
    IMapper mapper) : IRequestHandler<GetStatsQuery, StatsResponse>
{
    public async Task<StatsResponse> Handle(GetStatsQuery request, CancellationToken cancellationToken)
    {
        var stats = await adminRepository.GetAdminStatsAsync();
        return mapper.Map<StatsResponse>(stats);
    }
}