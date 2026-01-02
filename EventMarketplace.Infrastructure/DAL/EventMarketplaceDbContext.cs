using EventMarketplace.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventMarketplace.Infrastructure.DAL;

public class EventMarketplaceDbContext(DbContextOptions<EventMarketplaceDbContext> options) : DbContext(options)
{
   public DbSet<Event> Events { get; set; }
   public DbSet<User> Users { get; set; }
   public DbSet<RefreshToken> RefreshTokens { get; set; }
   public DbSet<Role> Roles { get; set; }
   public DbSet<UserRole> UserRoles { get; set; }
   public DbSet<EventComment> EventComments { get; set; }

   protected override void OnModelCreating(ModelBuilder modelBuilder)
   {
      base.OnModelCreating(modelBuilder);

      modelBuilder.Entity<Event>(entity =>
      {
         entity.HasKey(e => e.Id);
      });

      modelBuilder.Entity<User>(entity =>
      {
         entity.HasKey(x => x.Id);
      });

      modelBuilder.Entity<RefreshToken>(entity =>
      {
         entity.HasKey(x => x.Id);
      });

      modelBuilder.Entity<UserRole>(entity =>
      {
         entity.HasKey(ur => new { ur.UserId, ur.RoleId });
      });

      modelBuilder.Entity<EventComment>(entity =>
      {
         entity.HasKey(ec => ec.Id);
      });

      #region RelationsConfig

      modelBuilder.Entity<Event>()
         .HasOne(e => e.Organizer)
         .WithMany(u => u.Events)
         .HasForeignKey(e => e.OrganizerId);

      modelBuilder.Entity<RefreshToken>()
         .HasOne(r => r.User)
         .WithMany(u => u.RefreshTokens)
         .HasForeignKey(r => r.UserId);

      modelBuilder.Entity<UserRole>()
         .HasOne(r => r.User)
         .WithMany(u => u.UserRoles)
         .HasForeignKey(r => r.UserId);
      
      modelBuilder.Entity<UserRole>()
         .HasOne(r => r.Role)
         .WithMany(u => u.UserRoles)
         .HasForeignKey(r => r.RoleId);

      modelBuilder.Entity<EventComment>()
         .HasOne(ec => ec.User)
         .WithMany(u => u.EventComments)
         .HasForeignKey(ec => ec.UserId);

      modelBuilder.Entity<EventComment>()
         .HasOne(ec => ec.Event)
         .WithMany(e => e.EventComments)
         .HasForeignKey(ec => ec.EventId)
         .OnDelete(DeleteBehavior.SetNull);

      #endregion

      #region ValueObjectsMapping

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

      modelBuilder.Entity<Event>().OwnsOne(e => e.DurationOfTheEvent, f =>
      {
         f.Property(p => p.StartEvent).HasColumnName("StartDate");
         f.Property(p => p.EndEvent).HasColumnName("EndDate");
      });

      modelBuilder.Entity<Event>().OwnsOne(e => e.Address, f =>
      {
         f.Property(p => p.PostalCode).HasColumnName("PostalCode");
         f.Property(p => p.City).HasColumnName("City");
         f.Property(p => p.Street).HasColumnName("Street");
         f.Property(p => p.Number).HasColumnName("Number");
      });

      #endregion


   }
}