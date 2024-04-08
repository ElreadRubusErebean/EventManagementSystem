using Microsoft.EntityFrameworkCore;
using EventManagmentSystem.Models.DbModel;

namespace EventManagmentSystem.DAL
{
    public class EventDbContext : DbContext
    {
        public EventDbContext(DbContextOptions options) : base(options)

        { }
        public DbSet<User> Users { get; set; }
        // public DbSet<Event> Events { get; set; }
    }
}
