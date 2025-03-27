using System;
using AccountFlow.Web.DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AccountFlow.Web.Domain.Usermanagement;

public class LinkPersonsToUsers
{
        private readonly DataContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public LinkPersonsToUsers(DataContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task LinkExistingPersonsToUsers()
    {
        // Get all Persons that do not have a linked ApplicationUser
        var persons = await _context.Persons.ToListAsync();

        foreach (var person in persons)
        {
            // Check if this person already has an associated user
            var userExists = await _userManager.Users
                                               .AnyAsync(u => u.PersonCode == person.Code);

            if (!userExists)
            {
                // If no user exists, create one

                // You can choose a default email or user-related information for the initial user
                var user = new ApplicationUser
                {
                    UserName = person.IdNumber, // You can use IdNumber or create a logic for email
                    Email = $"{person.IdNumber}@example.com", // Example email, or generate properly
                    PersonCode = person.Code // Link the person
                };

                var result = await _userManager.CreateAsync(user, "DefaultPassword123!"); // Set a default password

                if (result.Succeeded)
                {
                    // Optional: Assign roles or additional logic
                }
                else
                {
                    // Log errors or handle them accordingly
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"Error: {error.Description}");
                    }
                }
            }
        }
    }


}
