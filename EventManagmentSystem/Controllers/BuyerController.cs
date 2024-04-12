using Microsoft.AspNetCore.Mvc;

namespace EventManagmentSystem.Controllers
{
    public class BuyerController : Controller
    {
        public IActionResult Profile()
        {
            return View();
        }

        public IActionResult Buyer_EventOverview()
        {
            return View();
        }
    }
}
