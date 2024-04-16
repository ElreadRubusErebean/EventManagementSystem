using Microsoft.EntityFrameworkCore;
using EventManagmentSystem.Models.DbModel;

namespace EventManagmentSystem.DAL
{
    public class EventDbContext : DbContext
    {
        public EventDbContext(DbContextOptions options) : base(options)

        { }
        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }
        //Das brauchen wir später wegen der Beziehung zu der Event-Tabelle und Bookingtable
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
