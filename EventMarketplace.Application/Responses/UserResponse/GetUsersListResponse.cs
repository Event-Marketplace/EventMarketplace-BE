using EventMarketplace.Application.Response.UserResponse;

namespace EventMarketplace.Application.Responses.UserResponse;

public class GetUsersListResponse
{
    public List<GetUserInfoResponse> Users { get; set; }
}