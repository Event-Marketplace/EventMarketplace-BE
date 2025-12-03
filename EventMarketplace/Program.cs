using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using DotNetEnv;
using EventMarketplace.Application;
using EventMarketplace.Application.Commands.EventCommands.Handlers;
using EventMarketplace.Application.Mapper;
using EventMarketplace.Infrastructure;
using EventMarketplace.Infrastructure.DAL.QueryHandlers;
using EventMarketplace.Infrastructure.Middleware;
using EventMarketplace.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();
builder.Services.AddOpenApi();
builder.Services.AddControllers()
    .AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddAutoMapper(typeof(EventProfile).Assembly);

var currentDirectory = Directory.GetCurrentDirectory();
var parentDirectory = Directory.GetParent(currentDirectory)?.FullName;
if (parentDirectory != null)
{
    var envFilePath = Path.Combine(parentDirectory, ".env");
    Env.Load(envFilePath);
}

builder.Services.AddMediatR(conf =>
{
    conf.RegisterServicesFromAssembly(typeof(CreateEventCommandHandler).Assembly);
    conf.RegisterServicesFromAssembly(typeof(GetEventQueryHandler).Assembly);
});

builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerWithJwt();

builder.Services.AddCors(options =>
{
    options.AddPolicy("FE", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddAuthorization();

var app = builder.Build();
app.UseRateLimiter();
app.UseCors("FE");
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ErrorMiddleware>();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
//}


app.MapControllers();

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();   
}


app.Run();

Log.CloseAndFlush();

public partial class Program{}