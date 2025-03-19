using System;
using System.ComponentModel.DataAnnotations;
using AccountFlow.Web.Domain.Persons.Models;

namespace AccountFlow.Web.ViewModels.Persons;

public class PersonViewModel
{

    public IEnumerable<Person> Persons { get; set; }
    public string SearchTerm { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int PageSize { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Surname { get; set; }

    [Required]
    [Display(Name = "ID Number")]
    public string IdNumber { get; set; }

}
