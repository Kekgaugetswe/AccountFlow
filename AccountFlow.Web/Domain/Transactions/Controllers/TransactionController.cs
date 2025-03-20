using AccountFlow.Web.Domain.Transactions.Models;
using AccountFlow.Web.Domain.Transactions.Repositories;
using AccountFlow.Web.ViewModels.Transactions;
using Microsoft.AspNetCore.Mvc;

namespace AccountFlow.Web.Domain.Transactions.Controllers
{
    [Route("Transaction")]
    public class TransactionController(ITransactionRepository repository) : Controller
    {
        public async Task<IActionResult> List()
        {
            var transactions = await repository.GetAllTransactionsAsync();
            var viewModel = new TransactionViewModel(transactions);
            return View(viewModel);

        }

        [HttpGet("Details/{code}")]
        public async Task<IActionResult> Details(int code)
        {
            var Transaction = await repository.GetTransactionByCodeAsync(code);
            return View(Transaction);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTransaction(Transaction transaction)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Details", "Acccount", new { code = transaction.AccountCode });
            
            await repository.CreateTransactionAsync(transaction);
            return RedirectToAction("Details", "Acccount", new { code = transaction.AccountCode });
        }

        [HttpGet]
        public async Task<IActionResult> EditTransaction(int code)
        {
            var transaction = await repository.GetTransactionByCodeAsync(code);

            return PartialView("_AddOrEditTransactionModal", transaction);
        }

        [HttpPost]
        public async Task<IActionResult> EditTransaction(Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Details", "Account", new { code = transaction.AccountCode });
            }

            var transactionDto = await repository.GetTransactionByCodeAsync(transaction.Code);
            if (transactionDto != null)
            {
                transactionDto.TransactionDate = transaction.TransactionDate;
                transactionDto.Description = transaction.Description;
                return RedirectToAction("Details", "Account", new { code = transaction.AccountCode });
            }
            return NotFound();
        }
    }
}
