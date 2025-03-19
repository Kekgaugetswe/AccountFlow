using System;
using AccountFlow.Web.Domain.Accounts.Models;
using AccountFlow.Web.Domain.Persons.Models;
using AccountFlow.Web.Domain.Transactions.Models;

namespace AccountFlow.Web.ViewModels.Accounts;

public class AccountViewModel
{
   public string AccountNumber { get; }
    public decimal Balance { get; }
    public string PersonName { get; }
    public IEnumerable<Transaction> Transactions { get; }

    public AccountViewModel(Account account)
    {
        AccountNumber = account.AccountNumber;
        Balance = account.OutstandingBalance;
        PersonName = $"{account.Person.Name} {account.Person.Surname}";
        Transactions = account.Transactions ?? new List<Transaction>();
    }
    
}
