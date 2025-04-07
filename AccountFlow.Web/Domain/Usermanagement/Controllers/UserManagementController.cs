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
        private readonly RoleManager<IdentityRole> _roleManager;

        private DataContext _context;

        public UserManagementController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, DataContext context, RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
            _roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var model = new List<UserViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                model.Add(new UserViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Roles = roles.ToList()
                });
            }

            return View(model);
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
                // Add error for incorrect username
                ModelState.AddModelError(nameof(model.Username), "Invalid username or password.");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
            if (result.Succeeded)
                return RedirectToAction("Index", "Home");

            // Add error for incorrect password
            ModelState.AddModelError(nameof(model.Password), "Invalid username or password.");
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> AssignToUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var allRoles = _roleManager.Roles.Select(r => r.Name).ToList();

            var model = new AssignRolesToUserViewModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                AvailableRoles = allRoles,
                SelectedRoles = userRoles.ToList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AssignToUser(AssignRolesToUserViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return NotFound();
            }

            var currentRoles = await _userManager.GetRolesAsync(user);

            var rolesToRemove = currentRoles.Except(model.SelectedRoles);
            var rolesToAdd = model.SelectedRoles.Except(currentRoles);

            await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
            await _userManager.AddToRolesAsync(user, rolesToAdd);

            return RedirectToAction("Index", "UserManagement");
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



        [HttpGet]
        public async Task<IActionResult> AssignRoles()
        {
            var users = await _userManager.Users.ToListAsync();
            var roles = _roleManager.Roles?.ToList() ?? new List<IdentityRole>(); // Ensure roles list is not null

            var userViewModels = new List<UserViewModel>();

            foreach (var user in users)
            {
                var userRoles = await _userManager.GetRolesAsync(user) ?? new List<string>(); // Ensure roles are retrieved
                userViewModels.Add(new UserViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Roles = userRoles.ToList() // Store assigned roles
                });
            }

            var model = new AssignRolesViewModel
            {
                Users = userViewModels,
                AvailableRoles = roles.Select(r => r.Name).ToList()
            };

            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> AssignRoles(string userId, List<string> selectedRoles)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);
            await _userManager.AddToRolesAsync(user, selectedRoles);

            return RedirectToAction("AssignRoles");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRoles(string userId, List<string> selectedRoles)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID is required.");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var currentRoles = await _userManager.GetRolesAsync(user);

            // Remove only roles that are not in the new selection
            var rolesToRemove = currentRoles.Except(selectedRoles);
            await _userManager.RemoveFromRolesAsync(user, rolesToRemove);

            // Add only roles that are newly selected
            var rolesToAdd = selectedRoles.Except(currentRoles);
            await _userManager.AddToRolesAsync(user, rolesToAdd);

            return RedirectToAction("AssignRoles");
        }


    }
}
