using AccountFlow.Web.Domain.Transactions.Models;
using AccountFlow.Web.Domain.Transactions.Repositories;
using AccountFlow.Web.Domain.Transactions.Services;
using AccountFlow.Web.ViewModels.Transactions;
using Microsoft.AspNetCore.Mvc;

namespace AccountFlow.Web.Domain.Transactions.Controllers
{
    [Route("Transaction")]
    public class TransactionController(ITransactionRepository repository, ITransactionService transactionService) : Controller
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
        [Route("CreateTransaction")]
        
        public async Task<IActionResult> CreateTransaction(Transaction transaction, bool isDebit)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Invalid transaction data.";
                return RedirectToAction("Details", "Account", new { id = transaction.AccountCode });
            }

            try
            {
                await transactionService.CreateTransactionAsync(transaction, isDebit);
                TempData["SuccessMessage"] = "Transaction created successfully!";
            }
            catch (InvalidOperationException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "An error occurred while processing the transaction.";
            }

            return RedirectToAction("Details", "Account", new { id = transaction.AccountCode });
        }
        [HttpGet]
        public async Task<IActionResult> EditTransaction(int code)
        {
            var transaction = await repository.GetTransactionByCodeAsync(code);

            return PartialView("_AddOrEditTransactionModal", transaction);
        }

        [HttpPost]
        [Route("EditTransaction")]
        public async Task<IActionResult> EditTransaction(Transaction transaction)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Details", "Account", new { code = transaction.AccountCode });
            }

            var transactionDto = await repository.GetTransactionByCodeAsync(transaction.Code);
            if (transactionDto != null)
            {
                transactionDto.TransactionDate = transaction.TransactionDate;
                transactionDto.Description = transaction.Description;
                await repository.UpdateTransactionASync(transactionDto);
                return RedirectToAction("Details", "Account", new { code = transaction.AccountCode });
            }
            return NotFound();
        }



        [HttpPost]
        [Route("Reverse")]

        public async Task<IActionResult> Reverse(int id)
        {
            try
            {
                var transaction = await repository.GetTransactionByCodeAsync(id);
                if (transaction == null)
                {
                    TempData["ErrorMessage"] = "Transaction not found.";
                    return RedirectToAction("Details", "Account"); // Fallback if no transaction
                }

                int accountCode = transaction.AccountCode; // Get the account code before reversing

                var success = await transactionService.ReverseTransactionAsync(id);

                if (success)
                {
                    TempData["SuccessMessage"] = "Transaction reversed successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to reverse transaction.";
                }

                // Redirect to the specific account's details page
                return RedirectToAction("Details", "Account", new { code = accountCode });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("List", "Account"); // Fallback in case of error
            }
        }


    }
}
