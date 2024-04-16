using EventManagmentSystem.Enums;
using EventManagmentSystem.Models.DbModel;
using EventManagmentSystem.Models.ViewModel;
using EventManagmentSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace EventManagmentSystem.Controllers
{
    public class EventController : ValidationController
    {
        private readonly EventService _eventService;
        public EventController(EventService eventService)
        {
            _eventService = eventService;
        }

        public async Task<IActionResult> EventOverview()
        {
            //Die Methode aus dem Service aufrufen
            var events = await _eventService.GetAllEventsAsync();
            var viewModel = new EventOverview
            {
                //Die Liste von Events in das ViewModel übergeben
                ListOfEvents = events
            };
            return View(viewModel);
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

        /*
        public void CreateEvent(Event eventModel)
        {
            _eventService.CreateEvent(eventModel);
            //return SellerView wo man herkommt
        }
        */

        [HttpPost]
        public async Task<IActionResult> Create(EventViewModel eventModel)
        {
            if(!ModelState.IsValid)
            {
                return View(eventModel);
            }
            //ich hole hier die USerId mit Seller Rolle aus dem session
            var userId = GetUserId();

            //Neues Event erstellen

            var newEvent = new Event
            {
                Title = eventModel.Title,
                Description = eventModel.Description,
                Date = eventModel.Date,
                Price = eventModel.Price,
                AmountOfTickets = eventModel.AmountOfTickets,
                UserId = userId,
                State = EventStateEnum.ForSale

            };
            //EventService aufrufen
            bool result = await _eventService.CreateEventAsync(newEvent, userId);
            if (result)
            {
                SetSuccessMessage("Event erfolgreich erstellt");
                return RedirectToAction("Index", "Home");
            }
            //ToDo: Fehlermeldung
            //Check ob der User angemeldet ist
            //wenn nicht Benachrichtigung anzeigen und auf die Login Seite weiterleiten
            else
            {
                SetErrorMessage("Event konnte nicht erstellt werden");
                return View("EventCreate", eventModel);
            }

        }
        //Methode um UserId aus dem Session zu holen
        //ToDo: diese Methode muss ausgelagert werden in einem Controller damit wir diese Methode in allen Controllern verwenden können
        private int GetUserId()
        {
            var userId = Convert.ToInt32(HttpContext.Session.GetString("UserID"));
            return userId;
        }
    }
}
