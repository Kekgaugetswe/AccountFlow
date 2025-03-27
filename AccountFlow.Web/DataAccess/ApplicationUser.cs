using System;
using System.ComponentModel.DataAnnotations.Schema;
using AccountFlow.Web.Domain.Persons.Models;
using Microsoft.AspNetCore.Identity;

namespace AccountFlow.Web.DataAccess;

public class ApplicationUser : IdentityUser
{
        [ForeignKey("Person")]
        public int PersonCode { get; set; }
        public Person? Person { get; set; }

}
