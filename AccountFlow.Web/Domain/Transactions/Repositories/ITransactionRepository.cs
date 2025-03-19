using System;
using AccountFlow.Web.Domain.Transactions.Models;

namespace AccountFlow.Web.Domain.Transactions.Repositories;

public interface ITransactionRepository
{
    Task<IEnumerable<Transaction>> GetAllTransactionsAsync();
    Task<IEnumerable<Transaction>> GetTransactionsByAccountNumberAsync(string accountNumber);
    Task<Transaction> GetTransactionByCodeAsync(int code);

}
