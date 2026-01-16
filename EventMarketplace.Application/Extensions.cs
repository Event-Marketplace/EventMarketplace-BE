using System.Text;
using System.Threading.RateLimiting;
using Azure.Storage.Blobs;
using EventMarketplace.Application.Behaviors;
using EventMarketplace.Application.Commands.EventCommands.CreateEvent;
using EventMarketplace.Application.Commands.EventCommands.Handlers;
using EventMarketplace.Application.Services.Users.Get;
using EventMarketplace.Application.Utils;
using EventMarketplace.Application.Utils.Azure;
using EventMarketplace.Application.Utils.Jwt;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace EventMarketplace.Application;

public static class Extensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IPasswordManager, PasswordManager>();
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
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                    ClockSkew = TimeSpan.Zero,
                    IgnoreTrailingSlashWhenValidatingAudience = true,
                    RequireExpirationTime = true,
                    RequireAudience = true,
                    RequireSignedTokens = true,
                };
                
                // DODAJ TO:
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        // pobierz token z query string, jeśli to połączenie do SignalR
                        var accessToken = context.Request.Query["access_token"];

                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/eventHub"))
                        {
                            context.Token = accessToken;
                        }

                        return Task.CompletedTask;
                    }
                };
            });

        #endregion

        services.AddScoped<IBlobStorageService, BlobStorageService>();
        services.AddScoped<IUserService, UserService>();
        
        var protocol = Environment.GetEnvironmentVariable("DEFAULT_PROTOCOL");
        var accountName = Environment.GetEnvironmentVariable("ACCOUNT_NAME");
        var accountKey = Environment.GetEnvironmentVariable("ACCOUNT_KEY");
        var endpointSuffix = Environment.GetEnvironmentVariable("ENDPOINT_SUFFIX");
            
        var azureBlobConnString = $"DefaultEndpointsProtocol={protocol};AccountName={accountName};AccountKey={accountKey};EndpointSuffix={endpointSuffix}";
        services.AddSingleton(new BlobServiceClient(azureBlobConnString));

        services.AddValidatorsFromAssemblyContaining<CreateEventCommandValidator>();
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));
        services.AddScoped<IGetUserUseCase, GetUserUseCase>();
        
        //ustawienie limitera
        services.AddRateLimiter(opt =>
        {
            opt.AddPolicy("RegenerateTokensPolicy", context => RateLimitPartition.GetFixedWindowLimiter(
                partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "annonymous",
                factory: _ => new FixedWindowRateLimiterOptions()
                {
                    PermitLimit = 5,
                    Window = TimeSpan.FromMinutes(1),
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                    QueueLimit = 0
                }));
        });
       
        return services;
    }
}