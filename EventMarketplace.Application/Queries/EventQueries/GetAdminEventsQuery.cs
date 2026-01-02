using EventMarketplace.Application.Response.EventResponse.AdminResponses;
using MediatR;

namespace EventMarketplace.Application.Queries;

public record GetAdminEventsQuery(
    int PageNumber, 
    string OrganizerFilter,
    string CreatedAtFilter,
    string CityFilter,
    string TitleFilter
    //tab - enum (trzeba dodać)
    //kategoria (trzeba dodać)
    ) : IRequest<AdminEventListResponse>;