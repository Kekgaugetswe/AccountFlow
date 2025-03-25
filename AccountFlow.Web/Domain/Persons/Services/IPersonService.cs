using System;
using AccountFlow.Web.Domain.Persons.Models;

namespace AccountFlow.Web.Domain.Persons.Services;

public interface IPersonService
{

    Task CreatePersonAsync(Person person);

}
