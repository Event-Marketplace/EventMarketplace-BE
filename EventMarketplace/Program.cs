using DotNetEnv;
using EventMarketplace.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

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

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructure();
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