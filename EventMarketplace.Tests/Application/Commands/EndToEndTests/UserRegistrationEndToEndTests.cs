using System.Net;
using System.Net.Http.Json;
using EventMarketplace.Application.Dtos.UserDtos;
using EventMarketplace.Infrastructure.DAL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;

namespace EventMarketplace.Tests.Application.Commands.EndToEndTests;

public class UserRegistrationEndToEndTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    
    public UserRegistrationEndToEndTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Usuń prawdziwy DbContext (np. SQL)
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<EventMarketplaceDbContext>));
                if (descriptor != null)
                    services.Remove(descriptor);

                // Dodaj InMemory bazę dla testów
                services.AddDbContext<EventMarketplaceDbContext>(options =>
                    options.UseInMemoryDatabase("E2ETestDb"));
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

        var response = await _client.PostAsJsonAsync("api/User/register", dto);
        
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<object>();
        Assert.NotNull(result);
    }
}