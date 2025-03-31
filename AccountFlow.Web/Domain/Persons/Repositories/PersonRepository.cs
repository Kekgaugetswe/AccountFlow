using System;
using AccountFlow.Web.DataAccess;
using AccountFlow.Web.Domain.Persons.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountFlow.Web.Domain.Persons.Repositories;

public class PersonRepository : IPersonRepository
{
    private readonly DataContext _context;
    public PersonRepository(DataContext context)
    {
        _context = context;

    }
    public async Task<(IEnumerable<Person> people, int totalCount)> GetAllPersonsAsync(string search, int pageNumber, int pageSize)
    {
        var query = _context.Persons.Include(p => p.Accounts).AsQueryable();
        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(p => p.Name.Contains(search) || p.Surname.Contains(search) || p.Name.Contains(search) || p.IdNumber.Contains(search) || p.Accounts.Any(a => a.AccountNumber.Contains(search)));
        }

        int totalCount = await query.CountAsync();

        var persons = await query.OrderByDescending(p => p.Code).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

        return (persons, totalCount);
    }


    public async Task<Person> GetPersonByCodeAsync(int code)
    {
        return await _context.Persons.Include(p => p.Accounts)
                               .FirstOrDefaultAsync(p => p.Code == code);
    }

    public async Task<Person?> GetPersonByIdNumberAsync(string idNumber)
    {
        var person = await _context.Persons.Include(p => p.Accounts)
                               .FirstOrDefaultAsync(p => p.IdNumber == idNumber);
        return person ?? null;
    }

    public async Task CreateAsync(Person person)
    {
        await _context.Persons.AddAsync(person);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Person person)
    {
        _context.Persons.Update(person);
        await _context.SaveChangesAsync();
    }


    public async Task<(bool Success, string Message)> DeleteAsync(int id)
    {
        var person = await _context.Persons
            .Include(p => p.Accounts)
            .FirstOrDefaultAsync(p => p.Code == id);

        if (person == null)
        {
            return (false, "Person not found.");
        }

        if (person.Accounts.Any())
        {
            return (false, "Person cannot be deleted because they have linked accounts.");
        }

        _context.Persons.Remove(person);
        await _context.SaveChangesAsync();
        return (true, "Person deleted successfully!");
    }


}
