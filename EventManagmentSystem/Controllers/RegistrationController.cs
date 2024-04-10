using Microsoft.AspNetCore.Mvc;
using EventManagmentSystem.Models.ViewModel;
using EventManagmentSystem.Services;


namespace EventManagmentSystem.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly UserService _userService;

        public RegistrationController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registration(RegistrationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (_userService.TryRegisterUser(model, out string errorMessage))
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError(string.Empty, errorMessage);
                return View(model);
            }

        }

    }
}

