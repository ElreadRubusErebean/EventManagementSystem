using Microsoft.AspNetCore.Mvc;

namespace EventManagmentSystem.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
    }
}
