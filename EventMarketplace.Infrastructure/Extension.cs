using EventMarketplace.Infrastructure.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventMarketplace.Infrastructure;

public static class Extension
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection service, IConfiguration configuration)
    {
        var port = Environment.GetEnvironmentVariable("DB_PORT");
        var database = Environment.GetEnvironmentVariable("POSTGRES_DB");
        var username = Environment.GetEnvironmentVariable("POSTGRES_USER");
        var password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");

        var connectionString = $"Host=db;Port={port};Database={database};Username={username};Password={password}";
        
        service.AddDbContext<EventMarketplaceDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString(connectionString));
        });
        
        return service;
    }
}