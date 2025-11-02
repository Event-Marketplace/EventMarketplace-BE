using EventMarketplace.Application.Abstract;
using EventMarketplace.Application.Response.UserResponse;

namespace EventMarketplace.Application.Queries;

public sealed class GetUserInfoQuery : IQuery<GetUserInfoResponse>
{
    public string Email { get; set; }
}