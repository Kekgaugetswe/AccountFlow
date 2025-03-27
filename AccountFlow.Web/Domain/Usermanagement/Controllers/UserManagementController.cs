using AccountFlow.Web.DataAccess;
using AccountFlow.Web.Domain.Persons.Models;
using AccountFlow.Web.Domain.Transactions.Models;
using AccountFlow.Web.Domain.Usermanagement.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AccountFlow.Web.Domain.Usermanagement.Controllers
{
    public class UserManagementController : Controller
    {


        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private DataContext _context;

        public UserManagementController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, DataContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
            if (result.Succeeded)
                return RedirectToAction("Index", "Home");

            ModelState.AddModelError("", "Invalid login attempt.");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check if the Person already exists based on the provided details (Name, Surname, etc.)
                var existingPerson = await _context.Persons
                                                    .FirstOrDefaultAsync(p => p.Name == model.Name && p.Surname == model.Surname && p.IdNumber == model.IdNumber);

                Person person;

                if (existingPerson != null)
                {
                    // If the person exists, use the existing person
                    person = existingPerson;
                }
                else
                {
                    // If no existing person, create a new one
                    person = new Person
                    {
                        Name = model.Name,
                        Surname = model.Surname,
                        IdNumber = model.IdNumber
                    };

                    // Add the new person and save to generate the PersonCode
                    _context.Persons.Add(person);
                    await _context.SaveChangesAsync();  // Save to the database to generate the PersonCode
                }

                // Create a new ApplicationUser and link it to the person
                var user = new ApplicationUser
                {
                    UserName = $"{person.Name}.{person.Surname}",  // Generate a username from Name and Surname
                    Email = model.Email,                          // Use the email entered by the user
                    PersonCode = person.Code                      // Link the ApplicationUser to the Person using the PersonCode
                };

                // Create the user in Identity
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Sign in the user after successful registration
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    // Redirect to the home page or a confirmation page
                    return RedirectToAction("Index", "Home");
                }

                // Add errors to the model if registration fails
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            // Return the view with the model in case of invalid data or errors
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}
