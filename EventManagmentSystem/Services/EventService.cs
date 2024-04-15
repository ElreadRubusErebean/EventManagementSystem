using EventManagmentSystem.DAL;
using EventManagmentSystem.Models.DbModel;

namespace EventManagmentSystem.Services;

public class EventService
{
    private readonly EventDbContext _context;
    
    public EventService(EventDbContext context)
    {
        _context = context;
    }

    public void CreateEvent(Event eventModel)
    {
        using (_context)
        {
            _context.Add(eventModel);
            _context.SaveChanges();
        }
    }
}