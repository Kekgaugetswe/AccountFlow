using System;
using AccountFlow.Web.DataAccess;
using AccountFlow.Web.Domain.Transactions.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountFlow.Web.Domain.Transactions.Repositories;

public class TransactionRepository(DataContext context) : ITransactionRepository
{
    public async Task<IEnumerable<Transaction>> GetAllTransactionsAsync()
    {
        var transactions = await context.Transactions.Include(t => t.Account).ToListAsync();

        return transactions.AsEnumerable();
    }

    public async Task<Transaction> GetTransactionByCodeAsync(int code)
    {
        return await context.Transactions.Include(t => t.Account).FirstOrDefaultAsync(t => t.Code == code);
    }

    public async Task<IEnumerable<Transaction>> GetTransactionsByAccountNumberAsync(string accountNumber)
    {
        var transactions = await context.Transactions
            .Include(t => t.Account)
            .Where(t => t.Account.AccountNumber == accountNumber)  // Filter by account number
            .ToListAsync();

        return transactions.AsEnumerable();
    }

    public async Task CreateTransactionAsync(Transaction transaction)
    {

        if (transaction.CaptureDate == DateTime.MinValue)
        {
            transaction.CaptureDate = DateTime.Now;

        }

        await context.Transactions.AddAsync(transaction);
        await context.SaveChangesAsync();
    }

    public async Task UpdateTransactionASync(Transaction transaction)
    {
        context.Transactions.Update(transaction);
        await context.SaveChangesAsync();
    }


    public async Task DeleteAsync(int id)
    {
        var transaction = await context.Transactions.FindAsync(id);
        if (transaction != null)
        {
            context.Transactions.Remove(transaction);
            await context.SaveChangesAsync();
        }
    }
}


