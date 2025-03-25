using System;
using AccountFlow.Web.Domain.Persons.Models;
using AccountFlow.Web.Domain.Persons.Repositories;
using AccountFlow.Web.Domain.Validators;

namespace AccountFlow.Web.Domain.Persons.Services;

public class PersonService(IPersonRepository personRepository) : IPersonService
{

    public async Task CreatePersonAsync(Person person)
    {
        try
        {
            var existingPerson = await personRepository.GetPersonByIdNumberAsync(person.IdNumber);

            if (existingPerson != null)
            {
                throw new ArgumentException("A person with this ID number already exists.");
            }

            if (!SaIdValidator.IsValidSouthAfricanId(person.IdNumber))
            {
                throw new ArgumentException("The provided ID number is invalid.");
            }

            await personRepository.CreateAsync(person);
        }
        catch (Exception ex)
        {
            // You can throw the exception to be handled by the controller
            throw new Exception("An unexpected error occurred: " + ex.Message);
        }
    }



}
