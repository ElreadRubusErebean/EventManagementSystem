using EventManagmentSystem.Models;
using EventManagmentSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace EventManagmentSystem.Controllers
{
    public class EventController : Controller
    {
        private readonly EventService _eventService;
        public EventController(EventService eventService)
        {
            _eventService = eventService;
        }
        
        public IActionResult EventOverview()
        {
            return View();
        }

        public IActionResult EventCreate()
        {
            return View();
        }

        public IActionResult Event(int id)
        {
            // Logik für die Anzeige eines spezifischen Events
            // Zum Beispiel das Laden des Events aus einer Datenbank
            return View();
        }
        
        public void CreateEvent(Event eventModel)
        {
            _eventService.CreateEvent(eventModel);
        }
    }
}