using EventManagmentSystem.DAL;
using EventManagmentSystem.Models;

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
        
    }
}