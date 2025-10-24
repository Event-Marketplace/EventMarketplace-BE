using EventMarketplace.Application.Abstract;
using EventMarketplace.Domain.Repositories;
using EventMarketplace.Infrastructure.Crons;
using EventMarketplace.Infrastructure.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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

        var connectionString = $"Host=localhost;Port={port};Database={database};Username={username};Password={password}";
        
        services.AddDbContext<EventMarketplaceDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });
        
        var infrastructureAssembly = typeof(EventMarketplaceDbContext).Assembly;
        
        services.Scan(scan => scan.FromAssemblies(infrastructureAssembly)
            .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)))
            .AsImplementedInterfaces()
            .WithTransientLifetime()
        );
        
        services.AddScoped<IUserRepository, UserPostgresRepository>();
        services.AddScoped<IEventRepository, EventPostgresRepository>();
        services.AddHostedService<EventMarketplaceInitializer>();
        

        services.AddQuartz(q =>
        {
            var jobKey = new JobKey("DailySetUnActive");
            q.AddJob<SetUnActiveEventsCronJob>(opt => opt.WithIdentity(jobKey));
            q.AddTrigger(opt =>
                opt.ForJob(jobKey).WithIdentity("DailySetUnActive-trigger").WithCronSchedule("0 0 2 * * ?"));
        });
        
        services.AddQuartzHostedService(opt => { opt.WaitForJobsToComplete = true; });
        
        return services;
    }
}