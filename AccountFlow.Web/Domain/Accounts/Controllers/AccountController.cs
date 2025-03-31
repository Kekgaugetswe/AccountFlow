using AccountFlow.Web.Domain.Accounts.Models;
using AccountFlow.Web.Domain.Accounts.Repositories;
using AccountFlow.Web.ViewModels.Accounts;
using Microsoft.AspNetCore.Mvc;

namespace AccountFlow.Web.Domain.Accounts.Controllers;

[Route("Account")]
public class AccountController : Controller
{
    private readonly IAccountRepository repository;

    public AccountController(IAccountRepository repository)
    {
        this.repository = repository;
    }

    [HttpGet]
    [Route("List")]
    public async Task<IActionResult> List()
    {
        var accounts = await repository.GetAllAccountsAsync();
        return View(accounts);
    }

    [HttpGet]
    [Route("Details/{code}")]
    public async Task<IActionResult> Details(int code)
    {
        var account = await repository.GetAccountByIdAsync(code);
        if (account == null)
        {
            return NotFound();
        }


        return View(account);
    }

    [HttpPost]
    [Route("CreateAccount")]
    public async Task<IActionResult> CreateAccount(Account account)
    {
        if (!ModelState.IsValid)
        {
            TempData["ErrorMessage"] = "Invalid account details.";
            return RedirectToAction("Details", "Person", new { id = account.PersonCode });
        }

        // Check if an account with the same AccountNumber already exists
        var existingAccount = await repository.GetAccountByAccountNumberAsync(account.AccountNumber);

        if (existingAccount != null)
        {
            TempData["ErrorMessage"] = "An account with the same AccountNumber already exists.";
            return RedirectToAction("Details", "Person", new { id = account.PersonCode });
        }

        await repository.CreateAccountAsync(account);
        TempData["SuccessMessage"] = "Account created successfully!";
        return RedirectToAction("Details", "Person", new { id = account.PersonCode });
    }

    [HttpGet]
    public async Task<IActionResult> EditAccount(int code)
    {
        var account = await repository.GetAccountByIdAsync(code);
        if (account == null)
        {
            return NotFound();
        }
        return PartialView("_AddOrEditAccountModal", account);

    }

    [HttpPost]
    [Route("EditAccount")]
    public async Task<IActionResult> EditAccount(Account account)
    {
        if (!ModelState.IsValid)
        {
            TempData["ErrorMessage"] = "Invalid account details.";
            return RedirectToAction("Details", "Person", new { id = account.PersonCode });
        }

        var accountDto = await repository.GetAccountByIdAsync(account.Code);
        if (accountDto == null)
        {
            TempData["ErrorMessage"] = "Account not found.";
            return RedirectToAction("Details", "Person", new { id = account.PersonCode });
        }

        accountDto.OutstandingBalance = account.OutstandingBalance;
        await repository.UpdateAccountAsync(accountDto);

        TempData["SuccessMessage"] = "Account updated successfully!";
        return RedirectToAction("Details", "Person", new { id = account.PersonCode });
    }


    [HttpPost]
    [Route("Delete")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            if (id == 0)
            {
                TempData["ErrorMessage"] = "Invalid account ID.";
                return RedirectToAction("List", "Person");
            }

            var success = await repository.DeleteAccountAsync(id);

            if (success)
            {
                TempData["SuccessMessage"] = "Account closed successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to close account.";
            }

            return RedirectToAction("List", "Person");
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction("List", "Person");
        }
    }

}