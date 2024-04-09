using Microsoft.AspNetCore.Mvc;

namespace EventManagmentSystem.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Profile()
        {
            return View();
        }
    }
}
