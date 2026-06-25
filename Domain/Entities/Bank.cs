using BankExercise.Domain.ValueObjects;
using BankExercise.Domain.Enums;
using System.Security.Cryptography;


namespace BankExercise.Domain.Entities;

public class Bank
{
  private readonly Dictionary<AccountNumber, BankAccount> _accounts = [];
  private readonly Dictionary<TransactionReference, Transaction> _transactions = [];

  private const int FromInclusive = 100000;
  private const int ToExclusive = 1000000;

  // Public API
  public BankAccount CreateAccount(OwnerName name, Money initialBalance)
  {
    AccountNumber accountNumber = GenerateAccountNumber();

    BankAccount createdAccount = new(accountNumber, name, initialBalance);

    _accounts.Add(accountNumber, createdAccount);

    return createdAccount;
  }


  private BankAccount FindAccount(AccountNumber accountNumber)
  {
    CheckAccountNumber(accountNumber);
    return _accounts[accountNumber];
  }
  public void Deposit(AccountNumber accountNumber, Money amount)
  {
    BankAccount account = FindAccount(accountNumber);

    account.ApplyDeposit(amount);

    Transaction transaction = new(TransactionType.Deposit, amount, accountNumber);

    AddToTransactions(transaction.ReferenceId, transaction);
  }

  public void Withdraw(AccountNumber accountNumber, Money amount)
  {
    BankAccount account = FindAccount(accountNumber);

    account.ApplyWithdraw(amount);

    Transaction transaction = new(TransactionType.Withdraw, amount, accountNumber);

    AddToTransactions(transaction.ReferenceId, transaction);
  }

  public void Transfer(AccountNumber fromAccountNumber, AccountNumber toAccountNumber, Money amount)
  {
    // Check Accounts
    BankAccount srcAccount = FindAccount(fromAccountNumber);
    BankAccount destAccount = FindAccount(toAccountNumber);
    Transaction transaction;

    if (srcAccount == destAccount)
      throw new ArgumentException("The Account Cannot Transfer For Itself.", nameof(toAccountNumber));

    try
    {
      // Confirm Transfer
      srcAccount.ApplyWithdraw(amount);
      destAccount.ApplyDeposit(amount);

      transaction = Transaction.CreateTransfer(amount, fromAccountNumber, toAccountNumber);
    }
    catch
    {
      srcAccount.ApplyDeposit(amount); // Rollback. Not Safe Yet.
      throw;
    }

    AddToTransactions(transaction.ReferenceId, transaction);
  }

  public IReadOnlyList<Transaction> GetAccountTransactions(AccountNumber accountNumber)
  {
    CheckAccountNumber(accountNumber);

    return [.. _transactions.Values.Where(t => t.AccountNumber == accountNumber || t.DestAccountNumber == accountNumber)];
  }

  public Transaction GetTransactionByRefId(TransactionReference refId)
  {
    _transactions.TryGetValue(refId, out Transaction? transaction);
    if (transaction is null)
      throw new ArgumentException("No Transaction Found For Provided Reference Id.", nameof(refId));
    return transaction;
  }

  public IReadOnlyList<Transaction> GetTransactionsByType(TransactionType type)
  {
    return [.. _transactions.Values.Where(t => t.Type == type)];
  }

  // Internal Operations
  private AccountNumber GenerateAccountNumber()
  {
    // Generate A 6-digit Unique Number.
    AccountNumber accountNumber;
    do
    {
      accountNumber = new AccountNumber(RandomNumberGenerator.GetInt32(FromInclusive, ToExclusive).ToString());
    } while (_accounts.ContainsKey(accountNumber));

    return accountNumber;
  }
  private void AddToTransactions(TransactionReference refId, Transaction transaction)
  {
    _transactions.Add(refId, transaction);
  }
  private void CheckAccountNumber(AccountNumber accountNumber)
  {
    if (!_accounts.ContainsKey(accountNumber))
      throw new ArgumentException("No Account Number Found.", nameof(accountNumber));
  }

}