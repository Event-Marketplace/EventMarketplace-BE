using EventMarketplace.Application.Responses.UserResponse;
using EventMarketplace.Domain.Enums;
using MediatR;

namespace EventMarketplace.Application.Queries;

public class GetUsersQuery : IRequest<GetUsersListResponse>
{
    public int PageNumber { get; set; } = 1;
    public string? FullName { get; set; }
    public List<Guid>? Roles { get; set; }
    public UserActivity? UserActivity { get; set; }
    public string? Address { get; set; }
    public string? Email { get; set; }
    public DateOnly? RegistrationDate { get; set; }
}
