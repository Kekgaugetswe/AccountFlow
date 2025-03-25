using AccountFlow.Web.Domain.Transactions.Models;
using AccountFlow.Web.Domain.Usermanagement.Models;
using Microsoft.AspNetCore.Mvc;

namespace AccountFlow.Web.Domain.Usermanagement.Controllers
{
    public class UserManagementController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        // Inject IHttpContextAccessor to access session
        public UserManagementController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        // GET: /Account/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // var user = new ApplicationUser { UserName = model.UserName, Email = model.Email };
                // var result = await _userManager.CreateAsync(user, model.Password);

                // if (result.Succeeded)
                // {
                //     // await _signInManager.SignInAsync(user, isPersistent: false);
                //     return RedirectToAction("Index", "Home");
                // }
                // foreach (var error in result.Errors)
                // {
                //     ModelState.AddModelError(string.Empty, error.Description);
                // }
            }
            return  RedirectToAction("Index", "Home");
        }

        // GET: /Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            // Set a session flag indicating the user is logged in
            _httpContextAccessor.HttpContext.Session.SetString("LoggedIn", "true");
            TempData["SuccessMessage"] = "You have successfully logged in!";
            return RedirectToAction("List", "Person");
        }

        // Logout action
        public async Task<IActionResult> Logout()
        {
            // Remove the session flag indicating the user is logged out
            // Remove the session flag indicating the user is logged out
            _httpContextAccessor.HttpContext.Session.Remove("LoggedIn");
            TempData["SuccessMessage"] = "You have successfully logged out!";
            return RedirectToAction("Index", "Home");

        }
    }
}
