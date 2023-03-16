using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;

using WebApplication.Models.LocalDb;

namespace WebApplication.Data
{
  public partial class LocalDbContext : Microsoft.EntityFrameworkCore.DbContext
  {
    public LocalDbContext(DbContextOptions<LocalDbContext> options):base(options)
    {
    }

    public LocalDbContext()
    {
    }

    partial void OnModelBuilding(ModelBuilder builder);

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<WebApplication.Models.LocalDb.Appointment>()
              .HasOne(i => i.User)
              .WithMany(i => i.Appointments)
              .HasForeignKey(i => i.UserModifierId)
              .HasPrincipalKey(i => i.Id);
        builder.Entity<WebApplication.Models.LocalDb.Appointment>()
              .HasOne(i => i.Person)
              .WithMany(i => i.Appointments)
              .HasForeignKey(i => i.PersonId)
              .HasPrincipalKey(i => i.Id);
        builder.Entity<WebApplication.Models.LocalDb.Order>()
              .HasOne(i => i.Person)
              .WithMany(i => i.Orders)
              .HasForeignKey(i => i.PersonId)
              .HasPrincipalKey(i => i.Id);
        builder.Entity<WebApplication.Models.LocalDb.Order>()
              .HasOne(i => i.User)
              .WithMany(i => i.Orders)
              .HasForeignKey(i => i.UserModifierId)
              .HasPrincipalKey(i => i.Id);

        builder.Entity<WebApplication.Models.LocalDb.Person>()
              .Property(p => p.IsDeleted)
              .HasDefaultValueSql("false");

        builder.Entity<WebApplication.Models.LocalDb.User>()
              .Property(p => p.IsDeleted)
              .HasDefaultValueSql("false");

        this.OnModelBuilding(builder);
    }


    public DbSet<WebApplication.Models.LocalDb.Appointment> Appointments
    {
      get;
      set;
    }

    public DbSet<WebApplication.Models.LocalDb.CosmeticService> CosmeticServices
    {
      get;
      set;
    }

    public DbSet<WebApplication.Models.LocalDb.Order> Orders
    {
      get;
      set;
    }

    public DbSet<WebApplication.Models.LocalDb.Person> People
    {
      get;
      set;
    }

    public DbSet<WebApplication.Models.LocalDb.Subscription> Subscriptions
    {
      get;
      set;
    }

    public DbSet<WebApplication.Models.LocalDb.User> Users
    {
      get;
      set;
    }
  }
}
