using EventManagmentSystem.Enums;
using EventManagmentSystem.Models.DbModel;
using EventManagmentSystem.Models.ViewModel;
using EventManagmentSystem.Services;
using Microsoft.AspNetCore.Mvc;
using EventManagmentSystem.ResultObject;

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

        public async Task<IActionResult> Event(int id)
        {
            var resultEvent = await _eventService.GetEventByIdAsync(id);
            if (resultEvent.IsSuccess)
            {
                var viewModel = new EventViewModel()
                {
                    EventId = resultEvent.Value.EventId,
                    Title = resultEvent.Value.Title,
                    Description = resultEvent.Value.Description,
                    Date = resultEvent.Value.Date,
                    Price = resultEvent.Value.Price,
                    AmountOfTickets = resultEvent.Value.AmountOfTickets,
                    State = resultEvent.Value.State
                };
            
                return View(viewModel);
            }
            SetErrorMessage(resultEvent.Message);
            return RedirectToAction("EventOverview","Event");
        }
        

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
        
        [HttpPost]
        public async Task<IActionResult> ChangeEvent(int eventId, EventViewModel eventViewModel)
        {
            ResultObject<Event> result = await _eventService.ChangeEventAsync(eventId, eventViewModel);
            
            if (result.IsSuccess)
            {
                SetSuccessMessage("Event wurde geändert.");
                var changedEventViewModel = new EventViewModel
                {
                    Title = result.Value.Title,
                    Description = result.Value.Description,
                    Date = result.Value.Date,
                    Price = result.Value.Price,
                    AmountOfTickets = result.Value.AmountOfTickets,
                    State = result.Value.State
                };
                return View("Event",changedEventViewModel);
            }
            
            SetErrorMessage("Event konnte nicht geändert werden.");
            return View("Event", eventViewModel);
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
