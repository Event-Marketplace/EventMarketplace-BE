using AutoMapper;
using EventMarketplace.Application.Queries.AdminQueries;
using EventMarketplace.Application.Response.AdminResponse;
using EventMarketplace.Domain.Repositories;
using EventMarketplace.Domain.SharedModels;
using MediatR;

namespace EventMarketplace.Infrastructure.DAL.QueryHandlers.AdminQueryHandlers;

public class GetAlertsQueryHandler(IAdminRepository adminRepository, IMapper mapper) : IRequestHandler<GetAlertsQuery, AlertsResponse>
{
    public async Task<AlertsResponse> Handle(GetAlertsQuery request, CancellationToken cancellationToken)
    {
        var alerts = await adminRepository.GetAdminAlertsAsync();
        return mapper.Map<AlertsResponse>(alerts);
    }
}