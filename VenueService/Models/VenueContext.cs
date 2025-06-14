using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace VenueService.Models
{
    public class VenueContext : DbContext
    {
        public VenueContext(DbContextOptions<VenueContext> options) : base(options) { }
        public DbSet<Venue> Venues { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Venue>()
                .HasKey(v => v.Id);

            modelBuilder.Entity<Venue>()
                .Property(v => v.Capacity)
                .IsRequired();
        }

    }
}
