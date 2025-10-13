using DotNetEnv;
using EventMarketplace.Application;
using EventMarketplace.Infrastructure;
using EventMarketplace.Infrastructure.Middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();

var currentDirectory = Directory.GetCurrentDirectory();
var parentDirectory = Directory.GetParent(currentDirectory)?.FullName;
if (parentDirectory != null)
{
    var envFilePath = Path.Combine(parentDirectory, ".env");
    Env.Load(envFilePath);
}

builder.Services.AddApplication();
builder.Services.AddInfrastructure();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

var app = builder.Build();

app.UseMiddleware<ErrorMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.UseCors("FE");
app.UseHttpsRedirection();
app.UseAuthorization();


app.Run();

Log.CloseAndFlush();