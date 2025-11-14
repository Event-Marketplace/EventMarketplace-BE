
using EventMarketplace.Application.Response.UserResponse;
using MediatR;

namespace EventMarketplace.Application.Queries;

public sealed class GetUserInfoQuery : IRequest<GetUserInfoResponse>
{
    public string Email { get; set; }
}