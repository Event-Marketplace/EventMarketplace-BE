using EventMarketplace.Application.Responses.UserResponse;
using MediatR;

namespace EventMarketplace.Application.Queries;

public record GetUsersQuery : IRequest<GetUsersListResponse>
{
    
}