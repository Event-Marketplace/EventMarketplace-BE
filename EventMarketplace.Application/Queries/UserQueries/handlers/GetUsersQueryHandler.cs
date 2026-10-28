using AutoMapper;
using EventMarketplace.Application.Response.UserResponse;
using EventMarketplace.Application.Responses.UserResponse;
using MediatR;

namespace EventMarketplace.Application.Queries.handlers;

public class GetUsersQueryHandler(IUserReadModel userReadModel) : IRequestHandler<GetUsersQuery, GetUsersListResponse>
{
    public async Task<GetUsersListResponse> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        return await userReadModel.GetUsersAsync(request, cancellationToken);
    }
}