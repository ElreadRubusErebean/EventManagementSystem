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
        private readonly UserService _userService;
        public EventController(EventService eventService, UserService userService)
        {
            _eventService = eventService;
            _userService = userService;
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

                if (resultEvent.Value.State!=viewModel.State)
                {
                    await UpdateEventState(viewModel.EventId,viewModel.State);
                }
                
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
        public async Task<IActionResult> ChangeEvent(EventViewModel eventViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Event",eventViewModel);
            }
            
            //das bereits existierende Event abfragen und mit Änderungen vergleichen, ob sie sich unterscheiden
            var unchangedEvent = await _eventService.GetEventByIdAsync(eventViewModel.EventId);

            var unchangedViewModel = new EventViewModel
            {
                EventId = unchangedEvent.Value.EventId,
                Title = unchangedEvent.Value.Title,
                Description = unchangedEvent.Value.Description,
                Date = unchangedEvent.Value.Date,
                Price = unchangedEvent.Value.Price,
                AmountOfTickets = unchangedEvent.Value.AmountOfTickets,
                State = unchangedEvent.Value.State
            };

            if (eventViewModel.Equals(unchangedViewModel))
            {
                SetErrorMessage("Es gibt keine Änderungen im Event. Bitte geben Sie neue Daten ein.");
                return View("Event", unchangedViewModel);
            }

            var eventModel = new Event
            {
                EventId = eventViewModel.EventId,
                Title = eventViewModel.Title,
                Description = eventViewModel.Description,
                Date = eventViewModel.Date,
                Price = eventViewModel.Price,
                AmountOfTickets = eventViewModel.AmountOfTickets,
                State = eventViewModel.State
            };
            
            ResultObject<Event> result = await _eventService.ChangeEventAsync(eventModel.EventId, eventModel);
            
            if (result.IsSuccess)
            {
                var changedEventViewModel = new EventViewModel
                {
                    Title = result.Value.Title,
                    Description = result.Value.Description,
                    Date = result.Value.Date,
                    Price = result.Value.Price,
                    AmountOfTickets = result.Value.AmountOfTickets,
                    State = result.Value.State
                };
                
                SetSuccessMessage("Event wurde geändert.");
                return View("Event",changedEventViewModel);
            }
            
            SetErrorMessage("Event konnte nicht geändert werden.");
            return View("Event", eventViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteEvent(int eventId)
        {
            var user = await _userService.GetUserAsync(GetUserId());
            if (user.Role!=UserRole.Seller)
            {
                SetErrorMessage("Sie sind kein Verkäufer und haben keine Rechte dieses Event zu löschen.");
                return RedirectToAction("Event", Event(eventId));
            }

            var result = await _eventService.DeleteEventById(eventId);

            if (!result)
            {
                SetErrorMessage("Das Event konnte nicht gelöscht werden.");
                return RedirectToAction("Event", Event(eventId));
            }
            
            SetSuccessMessage("Das Event wurde erfolgreich entfernt.");
            return RedirectToAction("MyEvents","Seller");
        }

        private async Task UpdateEventState(int eventId, EventStateEnum eventState)
        {
            var result = await _eventService.UpdateEventState(eventId,eventState);
            if (!result)
            {
                Console.Write("Der State konnte in der Datenbank nicht geändert werden.");
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
