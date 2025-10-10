using EventMarketplace.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventMarketplace.Infrastructure.DAL;

public class EventMarketplaceDbContext(DbContextOptions<EventMarketplaceDbContext> options) : DbContext(options)
{
   public DbSet<Event> Events { get; set; }
   public DbSet<User> Users { get; set; }

   protected override void OnModelCreating(ModelBuilder modelBuilder)
   {
      base.OnModelCreating(modelBuilder);

      modelBuilder.Entity<Event>(entity =>
      {
         entity.HasKey(e => e.Id);
      });

      modelBuilder.Entity<User>().OwnsOne(u => u.Address, a =>
      {
         a.Property(p => p.City).HasColumnName("City");
         a.Property(p => p.Street).HasColumnName("Street");
         a.Property(p => p.Number).HasColumnName("Number");
         a.Property(p => p.PostalCode).HasColumnName("PostalCode");
      });

      modelBuilder.Entity<User>().OwnsOne(u => u.EmailAddress, e =>
      {
         e.Property(p => p.Value).HasColumnName("Email").IsRequired();
      });

      modelBuilder.Entity<User>().OwnsOne(u => u.PhoneNumber, n =>
      {
         n.Property(p => p.PhoneValue).HasColumnName("PhoneNumber");
      });

      modelBuilder.Entity<User>().OwnsOne(u => u.FullName, f =>
      {
         f.Property(p => p.FirstName).HasColumnName("FirstName");
         f.Property(p => p.LastName).HasColumnName("LastName");
      });
   }
}