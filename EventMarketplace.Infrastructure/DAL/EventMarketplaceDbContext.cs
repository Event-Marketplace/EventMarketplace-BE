using Microsoft.EntityFrameworkCore;

namespace EventMarketplace.Infrastructure.DAL;

public class EventMarketplaceDbContext(DbContextOptions<EventMarketplaceDbContext> options) : DbContext
{
   // public DbSet<> Type { get; set; }

   protected override void OnModelCreating(ModelBuilder modelBuilder)
   {
      base.OnModelCreating(modelBuilder);
   }
}