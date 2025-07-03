using boardroombooking1.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace boardroombooking1.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> o) : base(o) { }

    public DbSet<Venue> Venues => Set<Venue>();
    public DbSet<Booking> Bookings => Set<Booking>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        base.OnModelCreating(mb);

        mb.Entity<Venue>()
          .Property(v => v.RowVersion)
          .IsRowVersion();

        mb.Entity<Booking>()
          .HasOne(b => b.Venue)
          .WithMany(v => v.Bookings)
          .HasForeignKey(b => b.VenueId)
          .OnDelete(DeleteBehavior.Cascade);
    }
}
