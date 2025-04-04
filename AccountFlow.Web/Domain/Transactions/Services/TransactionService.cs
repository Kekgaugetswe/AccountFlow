using System;
using AccountFlow.Web.Domain.Accounts.Repositories;
using AccountFlow.Web.Domain.Transactions.Models;
using AccountFlow.Web.Domain.Transactions.Repositories;

namespace AccountFlow.Web.Domain.Transactions.Services;

public class TransactionService(ITransactionRepository transactionRepository, IAccountRepository accountRepository) : ITransactionService
{

    private readonly ITransactionRepository _transactionRepository = transactionRepository;
    private readonly IAccountRepository _accountRepository = accountRepository;

    public async Task<bool> CreateTransactionAsync(Transaction transaction, bool isDebit)
    {
        if (transaction.Amount <= 0)
        {
            throw new ArgumentException("Transaction amount must be greater than zero.");
        }
        var account = await _accountRepository.GetAccountByIdAsync(transaction.AccountCode);
        if (account == null)
        {
            throw new ArgumentException("Account not found.");
        }

        if (isDebit)
        {
            if (transaction.Amount > account.OutstandingBalance)
            {
                throw new InvalidOperationException("Insufficient balance for this transaction.");
            }

            // Deduct the transaction amount from the account's outstanding balance
            account.OutstandingBalance -= transaction.Amount;
        }
        else
        {
            account.OutstandingBalance += transaction.Amount;
        }



        // Save the transaction and update the account
        await _transactionRepository.CreateTransactionAsync(transaction);
        await _accountRepository.UpdateAccountAsync(account);

        return true;
    }


    public async Task<bool> ReverseTransactionAsync(int transactionId)
    {
        var transaction = await _transactionRepository.GetTransactionByCodeAsync(transactionId);
        if (transaction == null)
        {
            throw new ArgumentException("Transaction not found.");
        }

        var account = await _accountRepository.GetAccountByIdAsync(transaction.AccountCode);
        if (account == null)
        {
            throw new ArgumentException("Account not found.");
        }
        if (transaction.IsDebit)
        {

            //Reverse debit  amount(add back)
            account.OutstandingBalance += transaction.Amount;
        }
        else
        {
            // Reverse credit amount( subtract)
            account.OutstandingBalance -= transaction.Amount;

        }

        // Remove transaction
        await _transactionRepository.DeleteAsync(transactionId);
        await _accountRepository.UpdateAccountAsync(account);

        return true;
    }

}
