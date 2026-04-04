using EventMarketplace.Application.Response.EventResponse.AdminResponses;
using MediatR;

namespace EventMarketplace.Application.Queries.AdminQueries;

public class GetAdminEventsQueryHandler(IAdminEventReadModel adminEventReadModel) : IRequestHandler<GetAdminEventsQuery, AdminEventListResponse>
{
    public async Task<AdminEventListResponse> Handle(GetAdminEventsQuery request, CancellationToken cancellationToken)
    {
        return await adminEventReadModel.GetAdminEventListAsync(request, cancellationToken);
    }
}