using Microsoft.AspNetCore.Mvc;

namespace EventManagmentSystem.Controllers
{
    public class ValidationController : Controller
    {
        protected void SetSuccessMessage(string message)
        {
            TempData["SuccessMessage"] = message;
        }

        protected void SetErrorMessage(string message)
        {
            TempData["ErrorMessage"] = message;
        }

    }
}
