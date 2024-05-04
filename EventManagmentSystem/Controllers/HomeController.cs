using System.Diagnostics;
using EventManagmentSystem.Models;
using EventManagmentSystem.Models.ViewModel;
using EventManagmentSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EventManagmentSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly EventService _eventService;

        public HomeController(ILogger<HomeController> logger, EventService eventService)
        {
            _logger = logger;
            _eventService = eventService;
        }

        public async Task<IActionResult> Index()
        {
            // hole alle events aus der Datenbank
            var events = await _eventService.GetAllEventsAsync();

            // instanzen aus OverView Model
            var model = new EventOverview
            {
                ListOfEvents = events
            };

            // schicke die Daten an die View
            return View(model);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
