﻿using EventManagmentSystem.Models.DbModel;
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
            //check ob der user admin ist
            //ich verwende hier die Methode IsAdmin() aus dem UserService
            if (!_userService.IsAdmin())
            {
                TempData["ErrorMessage"] = "Sie haben nicht die erforderlichen Berechtigungen";
                return RedirectToAction("Index", "Home");
            }
            //wenn der User Admin ist, dann alle User anzeigen
            //die methode GetAllUsersAsync() ist asynchron und ist in UserService definiert
            var users = await _userService.GetAllUsersAsync();
            if (users == null)//
            {
                // wenn keine Benutzer abgerufen werden können
                users = new List<User>();
            }
            var model = new AdminViewModel
                {
                    Users = users
                };
                return View(model);
        }
        //Methode zum löschen eines Benutzers
        [HttpPost]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            //check ob der user admin ist
            if (!_userService.IsAdmin())
            {
                TempData["ErrorMessage"] = "Sie haben nicht die erforderlichen Berechtigungen";
                return RedirectToAction("Index", "Home");
            }
            //löschen des Benutzers
            //die Methode DeleteUserAsync() ist asynchron und ist in UserService definiert
            var success = await _userService.DeleteUserAsync(userId);
            if (!success)
            {
                TempData["ErrorMessage"] = "Benutzer konnte nicht gelöscht werden";
            }
            return RedirectToAction("Admin");
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