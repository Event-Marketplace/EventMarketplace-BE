using EventMarketplace.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventMarketplace.Infrastructure.DAL;

public class EventMarketplaceDbContext(DbContextOptions<EventMarketplaceDbContext> options) : DbContext(options)
{
   public DbSet<Event> Events { get; set; }

   protected override void OnModelCreating(ModelBuilder modelBuilder)
   {
      base.OnModelCreating(modelBuilder);

      modelBuilder.Entity<Event>(entity =>
      {
         entity.HasKey(e => e.Id);
      });
   }
}