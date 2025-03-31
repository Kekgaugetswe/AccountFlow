using AccountFlow.Web.DataAccess;
using AccountFlow.Web.Domain.Usermanagement;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AccountFlow.Web.Domain
{
    public class AdminController : Controller
    {


        private readonly DataContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(DataContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }


        [HttpGet]
        public async Task<IActionResult> LinkPersonsToUsers()
        {
            var persons = await _context.Persons.ToListAsync();

            foreach (var person in persons)
            {
                // Check if the person already has a user linked by matching their ID or Code
                var user = await _userManager.Users
                                             .FirstOrDefaultAsync(u => u.PersonCode == person.Code);

                if (user == null)
                {
                    // If no user exists for this person, create and link one
                    var newUser = new ApplicationUser
                    {
                        UserName = $"{person.Name}.{person.Surname}",
                        Email = $"{person.Name.ToLower()}@example.com", // You can customize this.
                        PersonCode = person.Code  // Link with the existing person's code
                    };

                    var result = await _userManager.CreateAsync(newUser, "Password123!"); // Set a default password here

                    if (result.Succeeded)
                    {
                        // Optionally assign roles or other operations here
                    }
                }
            }

            return RedirectToAction("Index", "Home"); // Redirect to the homepage or wherever you'd like
        }

        [HttpGet]
        public async Task<IActionResult> AssignAdminRoleToUsers()
        {
            // Ensure the "Admin" role exists
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            var users = await _userManager.Users.ToListAsync();

            foreach (var user in users)
            {
                if (!await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    await _userManager.AddToRoleAsync(user, "Admin");
                }
            }

            // Redirect to home page after updating roles
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> EnsureRolesExist()
        {
            // Ensure "Admin" and "User" roles exist
            await EnsureRoleExists("Admin");
            await EnsureRoleExists("User");

            // Redirect to home page after ensuring roles exist
            return RedirectToAction("Index", "Home");
        }

        private async Task EnsureRoleExists(string roleName)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }
}
