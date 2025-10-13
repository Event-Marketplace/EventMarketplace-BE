using EventMarketplace.Application.Abstract;
using EventMarketplace.Application.Abstract.Dispatchers;
using EventMarketplace.Application.Patterns;
using EventMarketplace.Application.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace EventMarketplace.Application;

public static class Extensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.Scan(scan => scan.FromAssembliesOf(typeof(IQueryHandler<,>))
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<>)))
                .AsImplementedInterfaces()
                .WithTransientLifetime()
            .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)))
                .AsImplementedInterfaces()
                .WithTransientLifetime());

        services.AddScoped<IPasswordManager, PasswordManager>();
        services.AddScoped<ICommandDispatcher, CommandDispatcher>();
        services.AddScoped<IQueryDispatcher, QueryDispatcher>();
        
        return services;
    }
}