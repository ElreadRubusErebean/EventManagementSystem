using Microsoft.AspNetCore.Mvc;

namespace EventManagmentSystem.Controllers
{
    public class RegistrationController : Controller
    {
        public IActionResult Registration()
        {
            return View();
        }
    }
}
