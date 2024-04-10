using Microsoft.AspNetCore.Mvc;

namespace EventManagmentSystem.Controllers
{
    public class SellerController : Controller
    {
        public IActionResult Seller()
        {
            return View();
        }
    }
}
