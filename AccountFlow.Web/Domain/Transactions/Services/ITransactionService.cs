using System;
using AccountFlow.Web.Domain.Transactions.Models;

namespace AccountFlow.Web.Domain.Transactions.Services;

public interface ITransactionService
{
    Task<bool> CreateTransactionAsync(Transaction transaction);

    Task<bool> ReverseTransactionAsync(int transactionId);

}
