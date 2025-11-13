using System.Text;
using AutoMapper;
using Azure.Storage.Blobs;
using EventMarketplace.Application.Abstract;
using EventMarketplace.Application.Abstract.Dispatchers;
using EventMarketplace.Application.Mapper;
using EventMarketplace.Application.Patterns;
using EventMarketplace.Application.Utils;
using EventMarketplace.Application.Utils.Azure;
using EventMarketplace.Application.Utils.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace EventMarketplace.Application;

public static class Extensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.Scan(scan => scan.FromAssembliesOf(typeof(ICommandHandler<>))
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<>)))
                .AsImplementedInterfaces()
                .WithTransientLifetime()
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<,>)))
                .AsImplementedInterfaces()
                .WithTransientLifetime()
           );

        services.AddScoped<IPasswordManager, PasswordManager>();
        services.AddScoped<ICommandDispatcher, CommandDispatcher>();
        services.AddScoped<IQueryDispatcher, QueryDispatcher>();
        services.AddScoped<IJwtProvider, JwtProvider>();
        services.AddHttpContextAccessor();
        
        #region JwtConfiguration

        var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY");
        var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
        var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE");

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtIssuer,
                    ValidAudience = jwtAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                };
                
                // 🔹 Pozwala odczytać JWT z cookie zamiast z nagłówka
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        if (context.Request.Cookies.ContainsKey("access_token"))
                        {
                            context.Token = context.Request.Cookies["access_token"];
                        }
                        return Task.CompletedTask;
                    }
                };
            });

        #endregion

        services.AddScoped<IBlobStorageService, BlobStorageService>();
        
        var protocol = Environment.GetEnvironmentVariable("DEFAULT_PROTOCOL");
        var accountName = Environment.GetEnvironmentVariable("ACCOUNT_NAME");
        var accountKey = Environment.GetEnvironmentVariable("ACCOUNT_KEY");
        var endpointSuffix = Environment.GetEnvironmentVariable("ENDPOINT_SUFFIX");
            
        var azureBlobConnString = $"DefaultEndpointsProtocol={protocol};AccountName={accountName};AccountKey={accountKey};EndpointSuffix={endpointSuffix}";
        services.AddSingleton(new BlobServiceClient(azureBlobConnString));
       
        return services;
    }
}