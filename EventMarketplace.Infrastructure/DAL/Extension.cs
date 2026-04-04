
using System.Data;
using EventMarketplace.Application.Queries;
using EventMarketplace.Application.Queries.AdminQueries;
using EventMarketplace.Domain.Repositories;
using EventMarketplace.Infrastructure.DAL.QueryHandlers;
using EventMarketplace.Infrastructure.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Quartz;
using Quartz.Spi;

namespace EventMarketplace.Infrastructure.DAL;

public static class Extension
{
    public static IServiceCollection AddPostgres(this IServiceCollection services)
    {
        var port = Environment.GetEnvironmentVariable("DB_PORT");
        var database = Environment.GetEnvironmentVariable("POSTGRES_DB");
        var username = Environment.GetEnvironmentVariable("POSTGRES_USER");
        var password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");
        var dbHost = Environment.GetEnvironmentVariable("DB_HOST");

        var connectionString = $"Host={dbHost};Port={port};Database={database};Username={username};Password={password}";
        
        services.AddDbContext<EventMarketplaceDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });
        
        services.AddScoped<IUserRepository, UserPostgresRepository>();
        services.AddScoped<IEventRepository, EventPostgresRepository>();
        services.AddScoped<IAuthRepository, AuthRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();

        //rejestracja idbconnection dla dappera
        services.AddScoped<IDbConnection>(_ => new NpgsqlConnection(connectionString));
        services.AddScoped<IAdminRepository, AdminRepository>();
        services.AddScoped<IEventReadModel, EventReadModel>();
        services.AddScoped<IAdminEventReadModel, AdminEventReadModel>();
        
        // services.AddHostedService<EventMarketplaceInitializer>();
        
        // services.AddQuartz(q =>
        // {
        //     var jobKey = new JobKey("DailySetUnActive");
        //     q.AddJob<SetUnActiveEventsCronJob>(opt => opt.WithIdentity(jobKey));
        //     q.AddTrigger(opt =>
        //         opt.ForJob(jobKey).WithIdentity("DailySetUnActive-trigger").WithCronSchedule("0 0 2 * * ?"));
        // });
        
        //services.AddQuartzHostedService(opt => { opt.WaitForJobsToComplete = true; });
        
        return services;
    }
}