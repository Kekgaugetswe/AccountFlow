using System;
using System.ComponentModel.DataAnnotations;

namespace AccountFlow.Web.Domain.Transactions.Models;

public class LoginModel
{
    public string Username { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    public bool RememberMe { get; set; }


}
