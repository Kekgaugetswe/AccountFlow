using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AccountFlow.Web.DataAccess;
using AccountFlow.Web.Domain.Accounts.Models;

namespace AccountFlow.Web.Domain.Persons.Models;

public class Person
{
    [Key]
    public int Code { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    [Column("id_number")]
    [Required]
    public string IdNumber { get; set; } = string.Empty;

    public virtual ApplicationUser? ApplicationUser { get; set; }
    public ICollection<Account>? Accounts { get; set; }

}
