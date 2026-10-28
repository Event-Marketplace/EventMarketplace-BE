using System.Diagnostics;
using AutoMapper;
using EventMarketplace.Application.Queries;
using EventMarketplace.Application.Response.UserResponse;
using EventMarketplace.Application.Responses.UserResponse;
using EventMarketplace.Domain.Entities;
using EventMarketplace.Infrastructure.DAL.DbOperations;
using Microsoft.EntityFrameworkCore;

namespace EventMarketplace.Infrastructure.DAL.QueryHandlers;

public class UserReadModel(EventMarketplaceDbContext context, IMapper mapper) : IUserReadModel
{
    public async Task<GetUsersListResponse> GetUsersAsync(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var stopwatch = new Stopwatch();
        
        stopwatch.Start();
        var usersFiltered = await context
            .Users
            .FilterUsers(request)
            .ToListAsync(cancellationToken);
        
        stopwatch.Stop();
        var elapsedTime = stopwatch.ElapsedMilliseconds;
        //mapowanie po filtrowaniu / sortowaniu i paginacji ? albo przed paginacja? 
        var usersMapped = usersFiltered.Select(mapper.Map<GetUserInfoResponse>).ToList();

        return new GetUsersListResponse()
        {
            Users = usersMapped,
            ElapsedTime = elapsedTime
        };
    }
}