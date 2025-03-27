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

        public AdminController(DataContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
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
    }
}
