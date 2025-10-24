using EventMarketplace.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EventMarketplace.Infrastructure.DAL;

public class EventMarketplaceInitializer(IServiceProvider serviceProvider) : IHostedService
{

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<EventMarketplaceDbContext>();
            
        await context.Database.MigrateAsync(cancellationToken: cancellationToken);
            
        if (!context.Events.Any())
        {
            List<Event> newEvents =
            [
                new Event
                {
                    Title = "Festiwal Muzyki Alternatywnej SoundWave",
                    Description = "Trzydniowy festiwal z udziałem najlepszych artystów muzyki alternatywnej z Polski i Europy.",
                    StartDate = new DateTime(2025, 7, 12, 16, 0, 0, DateTimeKind.Utc),
                    EndDate = new DateTime(2025, 7, 14, 23, 59, 0, DateTimeKind.Utc),
                    Price = 349.99,
                    AvailableTickets = 1200,
                    ImageUrl = "https://example.com/images/soundwave2025.jpg",
                    IsActive = true,
                    CreateAt = DateTime.UtcNow,
                },
                new Event
                {
                    Title = "Maraton Krakowski 2025",
                    Description = "Coroczny maraton uliczny w samym sercu Krakowa. Dystans 42 km pełen sportowych emocji.",
                    StartDate = new DateTime(2025, 5, 18, 8, 0, 0, DateTimeKind.Utc),
                    EndDate = new DateTime(2025, 5, 18, 14, 0, 0, DateTimeKind.Utc),
                    Price = 150.00,
                    AvailableTickets = 5000,
                    ImageUrl = "https://example.com/images/krakow-marathon.jpg",
                    IsActive = true,
                    CreateAt = DateTime.UtcNow,
                },
                new Event
                {
                    Title = "Targi Gier Planszowych Planszówkon",
                    Description = "Największe w Polsce targi dla miłośników gier planszowych i RPG.",
                    StartDate = new DateTime(2025, 9, 5, 10, 0, 0, DateTimeKind.Utc),
                    EndDate = new DateTime(2025, 9, 7, 18, 0, 0, DateTimeKind.Utc),
                    Price = 59.99,
                    AvailableTickets = 3000,
                    ImageUrl = "https://example.com/images/planszowkon2025.jpg",
                    IsActive = true,
                    CreateAt = DateTime.UtcNow,
                },
                new Event
                {
                    Title = "Konferencja IT FutureTech",
                    Description = "Dwudniowa konferencja dla pasjonatów nowych technologii, AI i cyberbezpieczeństwa.",
                    StartDate = new DateTime(2025, 4, 10, 9, 0, 0, DateTimeKind.Utc),
                    EndDate = new DateTime(2025, 4, 11, 17, 0, 0, DateTimeKind.Utc),
                    Price = 499.00,
                    AvailableTickets = 800,
                    ImageUrl = "https://example.com/images/futuretech2025.jpg",
                    IsActive = true,
                    CreateAt = DateTime.UtcNow,
                },
                new Event
                {
                    Title = "Zlot Miłośników Motoryzacji Classic Drive",
                    Description = "Spotkanie fanów klasycznych samochodów i motocykli. Pokazy, wystawy, koncerty.",
                    StartDate = new DateTime(2025, 8, 2, 10, 0, 0, DateTimeKind.Utc),
                    EndDate = new DateTime(2025, 8, 2, 22, 0, 0, DateTimeKind.Utc),
                    Price = 40.00,
                    AvailableTickets = 1500,
                    ImageUrl = "https://example.com/images/classicdrive2025.jpg",
                    IsActive = true,
                    CreateAt = DateTime.UtcNow,
                },
                new Event
                {
                    Title = "Wieczór Stand-up Comedy – Polska Scena Śmiechu",
                    Description = "Najlepsi polscy komicy na jednej scenie! Gwarancja śmiechu do łez.",
                    StartDate = new DateTime(2025, 3, 22, 19, 0, 0, DateTimeKind.Utc),
                    EndDate = new DateTime(2025, 3, 22, 22, 0, 0, DateTimeKind.Utc),
                    Price = 89.00,
                    AvailableTickets = 400,
                    ImageUrl = "https://example.com/images/standup2025.jpg",
                    IsActive = true,
                    CreateAt = DateTime.UtcNow,
                },
                new Event
                {
                    Title = "Festiwal Sztuki Nowoczesnej FormArt",
                    Description = "Przestrzeń dla artystów, galerii i miłośników nowoczesnej sztuki. Warsztaty i panele dyskusyjne.",
                    StartDate = new DateTime(2025, 6, 6, 10, 0, 0, DateTimeKind.Utc),
                    EndDate = new DateTime(2025, 6, 8, 18, 0, 0, DateTimeKind.Utc),
                    Price = 120.00,
                    AvailableTickets = 1000,
                    ImageUrl = "https://example.com/images/formart2025.jpg",
                    IsActive = true,
                    CreateAt = DateTime.UtcNow,
                },
                new Event
                {
                    Title = "Turniej e-sportowy League Masters",
                    Description = "Zmagania najlepszych graczy League of Legends w turnieju z pulą nagród 50 000 zł.",
                    StartDate = new DateTime(2025, 11, 15, 12, 0, 0, DateTimeKind.Utc),
                    EndDate = new DateTime(2025, 11, 17, 20, 0, 0, DateTimeKind.Utc),
                    Price = 99.99,
                    AvailableTickets = 2500,
                    ImageUrl = "https://example.com/images/league-masters.jpg",
                    IsActive = true,
                    CreateAt = DateTime.UtcNow,
                },
                new Event
                {
                    Title = "Koncert Symfoniczny Chopin & Friends",
                    Description = "Wyjątkowy wieczór z muzyką klasyczną – orkiestra symfoniczna i soliści z całego świata.",
                    StartDate = new DateTime(2025, 12, 5, 19, 30, 0, DateTimeKind.Utc),
                    EndDate = new DateTime(2025, 12, 5, 22, 30, 0, DateTimeKind.Utc),
                    Price = 180.00,
                    AvailableTickets = 700,
                    ImageUrl = "https://example.com/images/chopin2025.jpg",
                    IsActive = true,
                    CreateAt = DateTime.UtcNow,
                },
                new Event
                {
                    Title = "Warsztaty Kulinarne z MasterChefem",
                    Description = "Spotkanie z finalistą programu MasterChef. Naucz się gotować dania z kuchni świata.",
                    StartDate = new DateTime(2025, 5, 25, 11, 0, 0, DateTimeKind.Utc),
                    EndDate = new DateTime(2025, 5, 25, 16, 0, 0, DateTimeKind.Utc),
                    Price = 220.00,
                    AvailableTickets = 50,
                    ImageUrl = "https://example.com/images/masterchef2025.jpg",
                    IsActive = true,
                    CreateAt = DateTime.UtcNow,
                }
                    
            ];
            
            await context.Events.AddRangeAsync(newEvents, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}