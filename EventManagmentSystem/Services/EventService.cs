using EventManagmentSystem.DAL;
using EventManagmentSystem.Enums;
using EventManagmentSystem.Models.DbModel;
using EventManagmentSystem.Models.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace EventManagmentSystem.Services;

public class EventService
{
    private readonly EventDbContext _context;
    
    public EventService(EventDbContext context)
    {
        _context = context;
    }

    /*
    public void CreateEvent(Event eventModel)
    {
        using (_context)
        {
            _context.Add(eventModel);
            _context.SaveChanges();
        }
    }
    */

    public async Task<bool> CreateEventAsync(Event eventModel, int userId)
    {
        //ich checke ob der Benutzer ein Seller ist
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId && u.Role == UserRole.Seller);
        if (user == null)
        {
            return false;
        }
        //Ansonsten Event hinzufügen
        _context.Events.Add(eventModel);
        await _context.SaveChangesAsync();

        return true;
    }

    //Events list anzeigen
    public async Task<List<Event>> GetAllEventsAsync()
    {
        return await _context.Events.ToListAsync();
    }
}