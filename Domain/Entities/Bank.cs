using BankExercise.Domain.ValueObjects;
using BankExercise.Domain.Enums;
using System.Security.Cryptography;


namespace BankExercise.Domain.Entities;

public class Bank
{
  private AccountNumber _accountNumber;
  private readonly Dictionary<AccountNumber, BankAccount> _accounts = [];
  private readonly Dictionary<TransactionReference, Transaction> _transactions = [];

  private const int FromInclusive = 100000;
  private const int ToExclusive = 1000000;

  public BankAccount CreateAccount(OwnerName name, Money initialBalance)
  {
    // Generate Account Id
    _accountNumber = GenerateAccountNumber();

    BankAccount createdAccount = new(_accountNumber, name, initialBalance);

    // Store the New Account
    _accounts.Add(_accountNumber, createdAccount);

    return createdAccount;
  }

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
  // Public API

  private BankAccount FindAccount(AccountNumber accountNumber)
  {
    CheckAccountNumber(accountNumber);
    return _accounts[accountNumber];
  }
  public void Deposit(AccountNumber accountNumber, Money amount)
  {
    // 1. Find Account.
    BankAccount account = FindAccount(accountNumber);

    // 2. Account.ApplyDeposit.
    account.ApplyDeposit(amount);

    // 3. Generate Transaction.
    Transaction transaction = new(TransactionType.Deposit, amount, accountNumber);

    // 4. Store Transaction in Bank.
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

      // Create Transfer Transaction. 
      transaction = Transaction.CreateTransfer(amount, fromAccountNumber, toAccountNumber);
    }
    catch
    {
      srcAccount.ApplyDeposit(amount); // Rollback
      throw;
    }

    // Add to Bank History.
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