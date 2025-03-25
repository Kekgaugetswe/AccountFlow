using System;
using AccountFlow.Web.Domain.Accounts.Models;

namespace AccountFlow.Web.Domain.Accounts.Repositories;

public interface IAccountRepository
{

    Task<IEnumerable<Account>> GetAllAccountsAsync();
    Task<Account> GetAccountByIdAsync(int code);

    Task CreateAccountAsync(Account account);
    Task UpdateAccountAsync(Account account);
    Task<bool>  DeleteAccountAsync(int accountId);
    Task<Account> GetAccountByAccountNumberAsync(string accountNumber);

}
