using System;
using Microsoft.AspNetCore.Identity;

namespace AccountFlow.Web.DataAccess;

public class ApplicationUser : IdentityUser
{
     // Custom properties for your user
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }

}
