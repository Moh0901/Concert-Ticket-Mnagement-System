using Microsoft.EntityFrameworkCore;

namespace EventService.Models
{
    public class EventContext: DbContext
    {
        public EventContext(DbContextOptions<EventContext> options) : base(options) { }
        public DbSet<Event> Events { get; set; }
        public DbSet<TicketType> TicketTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TicketType>()
                .HasOne<Event>()
                .WithMany(e => e.TicketTypes)
                .HasForeignKey(t => t.EventId);

            modelBuilder.Entity<TicketType>()
                .Property(t => t.Price)
                .HasColumnType("decimal(10,2)");

        }

    }
}
