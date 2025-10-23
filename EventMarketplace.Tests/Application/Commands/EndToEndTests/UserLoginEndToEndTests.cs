using System.Net;
using System.Net.Http.Json;
using EventMarketplace.Application.Dtos.UserDtos;
using EventMarketplace.Infrastructure.DAL;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace EventMarketplace.Tests.Application.Commands.EndToEndTests;

public class UserLoginEndToEndTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    
    public UserLoginEndToEndTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                //pobieramy dbContext
                var descriptor = services.SingleOrDefault(x =>
                    x.ServiceType == typeof(DbContextOptions<EventMarketplaceDbContext>));

                //usuwamy dbContext
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }
                
                //tworzymy nowy dbContext z inMemoryDb
                services.AddDbContext<EventMarketplaceDbContext>(option =>
                {
                    option.UseInMemoryDatabase("TestDb");
                });
            });
        }).CreateClient();
    }

    [Fact]
    public async Task LoginUser_ShouldReturn_Token()
    {
        //arrange
        var dto = new LoginUserDto()
        {
            Email = "b.longota2@wp.pl",
            Password = "Password.123"
        };

        //act
        var response = await _client.PostAsJsonAsync("/api/User/login", dto);
        
        //asserts
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var token = response.Content.ReadFromJsonAsync<object>();
        Assert.NotNull(token);
    }
}