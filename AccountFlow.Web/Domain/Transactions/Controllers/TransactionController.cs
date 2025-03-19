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
    }
}
