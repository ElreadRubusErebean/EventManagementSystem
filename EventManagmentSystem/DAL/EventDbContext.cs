using EventManagmentSystem.Models;
using Microsoft.EntityFrameworkCore;
using EventManagmentSystem.Models.DbModel;

namespace EventManagmentSystem.DAL
{
    public class EventDbContext : DbContext
    {
        public EventDbContext(DbContextOptions options) : base(options)

        { }
        public DbSet<User> Users { get; set; }
<<<<<<< HEAD
        public DbSet<Event> Events { get; set; }
=======
        // public DbSet<Event> Events { get; set; }

        //Das brauchen wir später wegen der Beziehung zu der Event-Tabelle und Bookingtable
       /*
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(u => u.Events)  // Annahme, dass eine ICollection<Event> in User definiert ist
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);  // Setzt die Löschkaskade
        }
       */
>>>>>>> KontoVerwaltung
    }
}
