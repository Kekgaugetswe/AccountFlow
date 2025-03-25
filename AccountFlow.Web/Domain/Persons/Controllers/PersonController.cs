using System.Diagnostics;
using AccountFlow.Web.Domain.Persons.Models;
using AccountFlow.Web.Domain.Persons.Repositories;
using AccountFlow.Web.Domain.Persons.Services;
using AccountFlow.Web.ViewModels.Persons;
using Microsoft.AspNetCore.Mvc;

namespace AccountFlow.Web.Domain.Persons.Controllers
{
    public class PersonController(IPersonRepository personRepository, IPersonService service, IHttpContextAccessor httpContextAccessor) : Controller
    {

        public async Task<IActionResult> List(string searchTerm, int pageNumber = 1, int pageSize = 10)
        {
            if (httpContextAccessor.HttpContext.Session.GetString("LoggedIn") != "true")
            {
                // Redirect to the login page if the user is not logged in
                TempData["ErrorMessage"] = "You must log in to access this page.";
                return RedirectToAction("Login", "UserManagement");
            }
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
                TempData["ErrorMessage"] = "Person not found.";
                return RedirectToAction("List");
            }

            return View(person);
        }

        [HttpGet]
        public async Task<IActionResult> EditPerson(int id)
        {
            var person = await personRepository.GetPersonByCodeAsync(id);

            if (person == null)
            {
                TempData["ErrorMessage"] = "Person not found.";
                return RedirectToAction("List");
            }

            return PartialView("_PersonModal", person); // Return the modal with person data
        }


        [HttpPost]
        public async Task<IActionResult> EditPerson(Person model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Invalid data provided.";
                return RedirectToAction("List");
            }

            var person = await personRepository.GetPersonByCodeAsync(model.Code);

            if (person != null)
            {
                person.Name = model.Name;
                person.Surname = model.Surname;
                person.IdNumber = model.IdNumber;

                await personRepository.UpdateAsync(person);
                TempData["SuccessMessage"] = "Person updated successfully!";
                return RedirectToAction("List");
            }

            TempData["ErrorMessage"] = "Person not found.";
            return RedirectToAction("List");
        }

        [HttpPost]
        public async Task<IActionResult> CreatePerson(Person model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Invalid data provided.";
                return RedirectToAction("List");
            }

            try
            {
                // Call the service method to create the person
                await service.CreatePersonAsync(model);

                // If no exception is thrown, assume success
                TempData["SuccessMessage"] = "Person created successfully!";
            }
            catch (ArgumentException ex)
            {
                // Handle specific known exceptions, such as the ID number being invalid or already taken
                TempData["ErrorMessage"] = ex.Message;
            }
            catch (Exception ex)
            {
                // Handle any other unexpected exceptions
                TempData["ErrorMessage"] = "An unexpected error occurred: " + ex.Message;
            }

            return RedirectToAction("List");
        }


        [HttpPost]
        public async Task<IActionResult> DeletePerson(int id)
        {
            var (success, message) = await personRepository.DeleteAsync(id);

            if (success)
            {
                TempData["SuccessMessage"] = message;
            }
            else
            {
                TempData["ErrorMessage"] = message;
            }

            return RedirectToAction("List");
        }



    }
}
