using Microsoft.EntityFrameworkCore;
using EventManagmentSystem.Models.DbModel;
using EventManagmentSystem.Models;

namespace EventManagmentSystem.DAL
{
    public class EventDbContext : DbContext
    {
        public EventDbContext(DbContextOptions options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }

        public DbSet<Booking> Bookings { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User zu Booking
            modelBuilder.Entity<User>()
                .HasMany(u => u.Bookings)
                .WithOne(b => b.User)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Event zu Booking
            modelBuilder.Entity<Event>()
                .HasMany(e => e.Bookings)
                .WithOne(b => b.Event)
                .HasForeignKey(b => b.EventId)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
