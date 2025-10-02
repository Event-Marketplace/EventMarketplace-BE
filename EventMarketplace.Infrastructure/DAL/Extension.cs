using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EventMarketplace.Infrastructure.DAL;

public static class Extension
{
    public static IServiceCollection AddPostgres(this IServiceCollection services)
    {
        var port = Environment.GetEnvironmentVariable("DB_PORT");
        var database = Environment.GetEnvironmentVariable("POSTGRES_DB");
        var username = Environment.GetEnvironmentVariable("POSTGRES_USER");
        var password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");

        var connectionString = $"Host=localhost;Port={port};Database={database};Username={username};Password={password}";
        
        services.AddDbContext<EventMarketplaceDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });
        
        return services;
    }
}