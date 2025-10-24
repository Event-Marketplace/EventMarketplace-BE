using EventMarketplace.Application.Abstract;
using EventMarketplace.Domain.Repositories;
using EventMarketplace.Infrastructure.DAL.Repositories;
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
        
        services.AddScoped<IUserRepository, UserPostgresRepository>();
        services.AddHostedService<EventMarketplaceInitializer>();
        
        var infrastructureAssembly = typeof(EventMarketplaceDbContext).Assembly;
        
        services.Scan(scan => scan.FromAssemblies(infrastructureAssembly)
            .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)))
            .AsImplementedInterfaces()
            .WithTransientLifetime()
        );
        
        return services;
    }
}