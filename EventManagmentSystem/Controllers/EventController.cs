using Microsoft.AspNetCore.Mvc;

namespace EventManagmentSystem.Controllers
{
    public class EventController : Controller
    {
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
    }
}