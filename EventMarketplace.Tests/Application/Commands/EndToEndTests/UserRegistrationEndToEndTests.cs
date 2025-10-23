using System.Net;
using System.Net.Http.Json;
using System.Text;
using EventMarketplace.Application.Dtos.UserDtos;
using EventMarketplace.Infrastructure.DAL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using EventMarketplace;
using EventMarketplace.Application.Patterns;
using EventMarketplace.Domain.Repositories;
using EventMarketplace.Infrastructure.DAL.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Xunit;
using Xunit.Abstractions;

namespace EventMarketplace.Tests.Application.Commands.EndToEndTests;

public class UserRegistrationEndToEndTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly HttpClient _client;
    
    public UserRegistrationEndToEndTests(WebApplicationFactory<Program> factory, ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Usuń wszystkie rejestracje powiązane z DbContext i repozytoriami z PostgreSQL
                var descriptors = services.Where(d =>
                    d.ServiceType == typeof(DbContextOptions<EventMarketplaceDbContext>) ||
                    d.ServiceType == typeof(IUserRepository) ||
                    d.ServiceType == typeof(IUnitOfWork))
                    .ToList();

                foreach (var d in descriptors)
                    services.Remove(d);

                // Dodaj InMemory DbContext
                services.AddDbContext<EventMarketplaceDbContext>(options =>
                    options.UseInMemoryDatabase($"E2ETestDb_{Guid.NewGuid()}"));

                // Dodaj repozytoria kompatybilne z InMemory
                services.AddScoped<IUserRepository, UserPostgresRepository>();
                services.AddScoped<IUnitOfWork, UnitOfWork>();
            });
        }).CreateClient();
    }
    
    [Fact]
    public async Task RegisterUser_Endpoit_ShouldReturn_Created()
    {
        var dto = new RegisterUserDto()
        {
            Email = "user@test.com",
            Password = "Password.123",
            ConfirmPassword = "Password.123",
            IsOrganizerAccount = false
        };

        var body = new { dto };

        var response = await _client.PostAsJsonAsync("/api/User/register", body);
        var responseContent = await response.Content.ReadAsStringAsync();
        _testOutputHelper.WriteLine($" content - {responseContent}");
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<object>();
        Assert.NotNull(result);
    }
}