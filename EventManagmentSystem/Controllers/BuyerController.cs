using Microsoft.AspNetCore.Mvc;

namespace EventManagmentSystem.Controllers
{
    public class BayerController : Controller
    {
        public IActionResult Profile()
        {
            return View();
        }
    }
}
