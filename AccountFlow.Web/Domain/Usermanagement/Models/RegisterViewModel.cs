using System;
using System.ComponentModel.DataAnnotations;

namespace AccountFlow.Web.Domain.Usermanagement.Models;

public class RegisterViewModel
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string IdNumber { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }


}
