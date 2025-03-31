using System;
using System.ComponentModel.DataAnnotations;

namespace AccountFlow.Web.Domain.Transactions.Models;

public class LoginModel
{
    [Required(ErrorMessage = "Username is required.")]
    public string Username { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    public bool RememberMe { get; set; }


}
