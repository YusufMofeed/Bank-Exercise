using BankExercise.Domain.ValueObjects;

namespace BankExercise.Domain.Entities;

class Bank
{
  private AccountNumber _accountNumber;
  private readonly Dictionary<AccountNumber, BankAccount> _accounts = [];
  private readonly Dictionary<TransactionReference, Transaction> _transactions = [];

  public BankAccount CreateAccount(OwnerName name, Money initialBalance)
  {
    // Generate Account Id
    _accountNumber = AccountNumber.Generate();

    BankAccount createdAccount = new(_accountNumber, name, initialBalance.Value);

    // Store the New Account
    _accounts.Add(_accountNumber, createdAccount);

    return createdAccount;
  }


  // Public API

  public BankAccount FindAccount(AccountNumber accountNumber)
  {
    CheckAccountNumber(accountNumber);

    _accounts.TryGetValue(accountNumber, out BankAccount account);

    return account;
  }
  public void Deposit(AccountNumber accountNumber, Money amount)
  {
    // 1. Find Account.
    BankAccount account = FindAccount(accountNumber);

    // 2. Account.ApplyDeposit.
    account.ApplyDeposit(amount.Value);

    // 3. Generate Transaction.
    Transaction transaction = new(TransactionType.Deposit, amount.Value, accountNumber);

    // 4. Store Transaction in Bank.
    AddToTransactions(transaction.ReferenceId, transaction);
  }

  public void Withdraw(AccountNumber accountNumber, Money amount)
  {
    BankAccount account = FindAccount(accountNumber);

    account.ApplyWithdraw(amount.Value);

    Transaction transaction = new(TransactionType.Withdraw, amount.Value, accountNumber);

    AddToTransactions(transaction.ReferenceId, transaction);
  }

  public void Transfer(AccountNumber fromAccountNumber, AccountNumber toAccountNumber, Money amount)
  {
    // Check Accounts
    BankAccount srcAccount = FindAccount(fromAccountNumber);
    BankAccount destAccount = FindAccount(toAccountNumber);

    if (srcAccount == destAccount)
      throw new ArgumentException("The Account Cannot Transfer For Itself.", nameof(toAccountNumber));


    // Confirm Transfer
    srcAccount.ApplyWithdraw(amount.Value);
    destAccount.ApplyDeposit(amount.Value);


    // Create Transfer Transaction. 
    Transaction transaction = Transaction.CreateTransfer(amount.Value, fromAccountNumber, toAccountNumber);

    // Add to Bank History.
    AddToTransactions(transaction.ReferenceId, transaction);
  }

  public List<Transaction> GetAccountTransactions(AccountNumber accountNumber)
  {
    CheckAccountNumber(accountNumber);

    List<Transaction> transactions = [.. _transactions.Values.Where(t => t.AccountNumber == accountNumber || t.DestAccountNumber == accountNumber)];
    return transactions;
  }

  public Transaction GetTransactionByRefId(TransactionReference refId)
  {
    _transactions.TryGetValue(refId, out Transaction? transaction);
    if (transaction is null)
      throw new ArgumentException("No Transaction Found For Provided Reference Id.", nameof(refId));
    return transaction;
  }

  public Dictionary<TransactionReference, Transaction> GetTransactionsByType(TransactionType type)
  {
    Dictionary<TransactionReference, Transaction> transactions = new(_transactions.Where(t => t.Value.Type == type));
    return transactions;
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