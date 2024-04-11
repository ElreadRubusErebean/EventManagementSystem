using EventManagmentSystem.Models.ViewModel;
using EventManagmentSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventManagmentSystem.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserService _userService;

        public AdminController(UserService userService)
        {
            _userService = userService;
        }
        [HttpGet]
        public async Task<IActionResult> Admin()
        {
            var users = await _userService.GetAllUsersAsync();
            var model = new AdminViewModel
            {
                Users = users
            };
            return View(model);
        }


    }
}
