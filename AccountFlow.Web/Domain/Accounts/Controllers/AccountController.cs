using AccountFlow.Web.Domain.Accounts.Models;
using AccountFlow.Web.Domain.Accounts.Repositories;
using AccountFlow.Web.Domain.Persons.Repositories;
using AccountFlow.Web.Domain.Transactions.Repositories;
using AccountFlow.Web.ViewModels.Accounts;
using AccountFlow.Web.ViewModels.Transactions;
using Microsoft.AspNetCore.Mvc;

namespace AccountFlow.Web.Domain.Accounts.Controllers
{
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

            var viewModel = new AccountViewModel(account);

            return View(viewModel);
        }

        [HttpPost]
        [Route("CreateAccount")]

        public async Task<IActionResult> CreateAccount(Account account)
        {
            if (!ModelState.IsValid)
            {
                
                return RedirectToAction("Details", "Person", new { id = account.PersonCode });
            }
            await repository.CreateAccountAsync(account);
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
                return View(account);
            }
            var accountDto = await repository.GetAccountByIdAsync(account.Code);
            if (account != null)
            {
                accountDto.AccountNumber = account.AccountNumber;

                await repository.UpdateAccountAsync(accountDto);
                return RedirectToAction("Details", "Person", new { id = account.Code });
            }

            return NotFound();
        }

    }

}