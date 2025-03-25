using System;
using AccountFlow.Web.DataAccess;
using AccountFlow.Web.Domain.Accounts.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountFlow.Web.Domain.Accounts.Repositories;

public class AccountRepository(DataContext context) : IAccountRepository
{
    public async Task<IEnumerable<Account>> GetAllAccountsAsync()
    {
        var accounts = await context.Accounts.Include(a => a.Person).ToListAsync();
        return accounts.AsEnumerable();
    }

    public async Task<Account> GetAccountByIdAsync(int code)
    {
        return await context.Accounts
            .Include(a => a.Person)
            .Include(a => a.Transactions)
            .FirstOrDefaultAsync(a => a.Code == code);
    }

    public async Task CreateAccountAsync(Account account)
    {
        await context.Accounts.AddAsync(account);
        await context.SaveChangesAsync();

    }

    public async Task UpdateAccountAsync(Account account)
    {
        context.Accounts.Update(account);
        await context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAccountAsync(int accountId)
    {
        var account = await context.Accounts.FindAsync(accountId);
        if (account == null)
        {
            return false;
        }

        context.Accounts.Remove(account);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<Account> GetAccountByAccountNumberAsync(string accountNumber)
    {
        return await context.Accounts
                             .FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);
    }



}
