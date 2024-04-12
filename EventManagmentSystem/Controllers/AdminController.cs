using Microsoft.AspNetCore.Mvc;

namespace EventManagmentSystem.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Admin()
        {
            return View();
        }

        public IActionResult Admin_EventOverview()
        {
            return View();
        }

        public IActionResult Admin_UserOverview()
        {
            return View();
        }
    }
}
