using EventManagmentSystem.DAL;
using EventManagmentSystem.Enums;
using EventManagmentSystem.Models.DbModel;
using EventManagmentSystem.Models.ViewModel;
using EventManagmentSystem.ResultObject;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace EventManagmentSystem.Services;

public class EventService
{
    private readonly EventDbContext _context;
    
    public EventService(EventDbContext context)
    {
        _context = context;
    }
    

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

    //holt Event aus Db mittels ID/primary key
    public async Task<ResultObject<Event>> GetEventByIdAsync(int id)
    {
        var eventById = await _context.Events.FindAsync(id);
        if (eventById==null)
        {
            return new ResultObject<Event>().Failure("Event konnte nicht gefunden werden");
        }
        
        return new ResultObject<Event>().Success(eventById);
    }

    public async Task<List<Event>> GetAllSellerEventsAsync(int userId)
    {
        return await _context.Events.Where(e => e.UserId == userId).ToListAsync();
    }
    
    public async Task<ResultObject<Event>> ChangeEventAsync(int eventId, Event eventModel)
    {
        var changingEvent = await GetEventByIdAsync(eventId);
    
        //Check if event was found
        if (!changingEvent.IsSuccess)
        {
            return new ResultObject<Event>().Failure("Event konnte nicht geändert werden.");
        }

        //Update Eventdata
        changingEvent.Value.Title = eventModel.Title;
        changingEvent.Value.Description = eventModel.Description;
        changingEvent.Value.AmountOfTickets = eventModel.AmountOfTickets;
        changingEvent.Value.Date = eventModel.Date;
        changingEvent.Value.Price = eventModel.Price;
        changingEvent.Value.State = eventModel.State;

        _context.Events.Update(changingEvent.Value);

        //save changes for the DB
        await _context.SaveChangesAsync();
        return new ResultObject<Event>().Success(changingEvent.Value);
    }

    public async Task<bool> DeleteEventById(int eventId)
    {
        var eventById = await GetEventByIdAsync(eventId);

        if (!eventById.IsSuccess)
        {
            return false;
        }

        var bookingsEvent = await  _context.Bookings.Where(b => b.EventId == eventId).ToListAsync();

        if (!bookingsEvent.IsNullOrEmpty())
        {
            foreach (var booking in bookingsEvent)
            {
                _context.Bookings.Remove(booking);
            }

            await _context.SaveChangesAsync();
        }        
        
        _context.Events.Remove(eventById.Value);
        
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateEventState(int eventId, EventStateEnum eventState)
    {
        var eventById = await GetEventByIdAsync(eventId);

        if (!eventById.IsSuccess)
        {
            return false;
        }

        eventById.Value.State = eventState;

        _context.Events.Update(eventById.Value);

        await _context.SaveChangesAsync();
        return true;
    }
}