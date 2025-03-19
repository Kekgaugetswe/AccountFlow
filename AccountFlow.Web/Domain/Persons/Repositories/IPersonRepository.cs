using System;
using AccountFlow.Web.Domain.Persons.Models;

namespace AccountFlow.Web.Domain.Persons.Repositories;

public interface IPersonRepository
{
    Task<(IEnumerable<Person> people, int totalCount)> GetAllPersonsAsync(string search, int pageNumber, int pageSize);

    Task<Person> GetPersonByCodeAsync(int code);

    Task CreateAsync(Person person);
    Task UpdateAsync(Person person);
    Task DeleteAsync(int id);
}
