using EventMarketplace.Application.Response.EventResponse.AdminResponses;
using EventMarketplace.Domain.Enums;
using MediatR;

namespace EventMarketplace.Application.Queries;

public record GetAdminEventsQuery(
    int PageNumber,
    string? OrganizerFilter,
    DateTime? CreatedFromFilter,
    DateTime? CreatedToFilter,
    string? CityFilter,
    string? TitleFilter,
    List<EventStatus>? EventStatuses,
    AdminEventListTab? Tab = AdminEventListTab.Pending) : IRequest<AdminEventListResponse>;