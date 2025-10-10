using EventMarketplace.Domain.Repositories;
using EventMarketplace.Infrastructure.DAL;
using EventMarketplace.Infrastructure.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventMarketplace.Infrastructure;

public static class Extension
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddPostgres();
        services.AddScoped<IUserRepository, UserPostgresRepository>();
        return services;
    }
}