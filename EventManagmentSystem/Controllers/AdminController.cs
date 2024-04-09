using Microsoft.AspNetCore.Mvc;

namespace EventManagmentSystem.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Admin()
        {
            return View();
        }
    }
}
