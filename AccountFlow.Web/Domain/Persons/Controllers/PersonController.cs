using AccountFlow.Web.Domain.Persons.Models;
using AccountFlow.Web.Domain.Persons.Repositories;
using AccountFlow.Web.ViewModels.Persons;
using Microsoft.AspNetCore.Mvc;

namespace AccountFlow.Web.Domain.Persons.Controllers
{
    public class PersonController : Controller
    {
        private readonly IPersonRepository personRepository;

        public PersonController(IPersonRepository personRepository)
        {
            this.personRepository = personRepository;
        }

        public async Task<IActionResult> List(string searchTerm, int pageNumber = 1, int pageSize = 10)
        {
            var (persons, totalCount) = await personRepository.GetAllPersonsAsync(searchTerm, pageNumber, pageSize);

            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            // Pass the data to the view
            var viewModel = new PersonViewModel
            {
                Persons = persons,
                SearchTerm = searchTerm,
                CurrentPage = pageNumber,
                TotalPages = totalPages,
                PageSize = pageSize
            };

            return View(viewModel);

        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var person = await personRepository.GetPersonByCodeAsync(id);

            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        [HttpGet]
        public async Task<IActionResult> EditPerson(int id)
        {
            var person = await personRepository.GetPersonByCodeAsync(id);

            if (person == null)
            {
                return NotFound();
            }

            return PartialView("_EditPersonModal", person); // Return the modal with person data
        }


        [HttpPost]
        public async Task<IActionResult> EditPerson(Person model)
        {
            if (!ModelState.IsValid)
            {
                return View(model); // Return view with errors if validation fails
            }

            var person = await personRepository.GetPersonByCodeAsync(model.Code);

            if (person != null)
            {
                person.Name = model.Name;
                person.Surname = model.Surname;
                person.IdNumber = model.IdNumber;

                await personRepository.UpdateAsync(person);
                return RedirectToAction("List"); // Redirect back to list after save
            }

            return NotFound(); // Return NotFound if person is not found
        }

        [HttpPost]
        public async Task<IActionResult> CreatePerson(Person model)
        {
            if (ModelState.IsValid)
            {

                var newPerson = new Person
                {
                    Name = model.Name,
                    Surname = model.Surname,
                    IdNumber = model.IdNumber
                };

                await personRepository.CreateAsync(newPerson);

                return RedirectToAction("List");
            }
            return RedirectToAction("List");

        }


    }
}
