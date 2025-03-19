using System;
using AccountFlow.Web.Domain.Transactions.Models;

namespace AccountFlow.Web.ViewModels.Transactions;

public class TransactionViewModel
{
    public IEnumerable<Transaction> transactions {get;}

    public TransactionViewModel(IEnumerable<Transaction> transactions)
    {
        this.transactions = transactions;
    }

}
