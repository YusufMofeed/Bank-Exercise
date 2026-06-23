using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace BankAccount;

class Bank
{
  private string _accountNumber = "";
  private readonly Dictionary<string, BankAccount> _accounts = []; // <AccountNumber, BankAccount>
  private Dictionary<string, Transaction> _transactions = []; // <RefId, Transaction>

  private readonly Regex OwnerNameRegex = new("^([A-Za-z])+$");
  //  6-digit Random Number Boundaries.
  private const int FromInclusive = 100000;
  private const int ToExclusive = 1000000;

  public BankAccount CreateAccount(string ownerName, double initialBalance)
  {
    // // Check if can convert to decimal
    decimal _initialBalance = Convert.ToDecimal(initialBalance);
    // Validation
    ValidateOwnerName(ownerName);
    ValidateBalance(_initialBalance);

    // Generate & Store Account Id
    _accountNumber = GenerateAccountNumber();
    // _accountsNumbersSet.Add(_accountNumber);

    BankAccount createdAccount = new(_accountNumber, ownerName.Trim().ToLower(), _initialBalance);

    // Store the New Account
    _accounts.Add(_accountNumber, createdAccount);
    // return BankAccount Object.
    return createdAccount;
  }






  ///
  /// 
  // Public API
  public BankAccount FindAccount(string accountNumber)
  {
    if (!_accounts.ContainsKey(accountNumber))
      throw new ArgumentException("No Account Number Found.", nameof(accountNumber));

    _accounts.TryGetValue(accountNumber, out BankAccount account);

    return account;
  }
  public void Deposit(string accountNumber, decimal amount)
  {
    // 1. Find Account.
    BankAccount account = FindAccount(accountNumber);

    // 2. Account.ApplyDeposit.
    account.ApplyDeposit(amount);

    // 3. Generate Transaction.
    Transaction transaction = new(TransactionType.Deposit, amount, accountNumber);

    // 4. Store Transaction in Bank.
    AddToTransactions(transaction.ReferenceId, transaction);

    // 5. Add ReferenceID to Account.
    account.AddToTransactionsRefIds(transaction.ReferenceId);
  }

  public void Withdraw(string accountNumber, decimal amount)
  {
    BankAccount account = FindAccount(accountNumber);

    account.ApplyWithdraw(amount);

    Transaction transaction = new(TransactionType.Withdraw, amount, accountNumber);

    AddToTransactions(transaction.ReferenceId, transaction);

    account.AddToTransactionsRefIds(transaction.ReferenceId);

  }

  public void Transfer(string fromAccountNumber, string toAccountNumber, decimal amount)
  {
    // Check Accounts
    BankAccount srcAccount = FindAccount(fromAccountNumber);
    BankAccount destAccount = FindAccount(toAccountNumber);

    if (srcAccount == destAccount)
      throw new ArgumentException("The Account Cannot Transfer For Itself.", nameof(toAccountNumber));


    // Confirm Transfer
    srcAccount.ApplyWithdraw(amount);
    destAccount.ApplyDeposit(amount);


    // Add to Bank History
    Transaction transaction = new(amount, srcAccount.AccountNumber, destAccount.AccountNumber, TransactionType.Transfer);

    AddToTransactions(transaction.ReferenceId, transaction);

    // Add ReferenceID to Both Accounts.
    srcAccount.AddToTransactionsRefIds(transaction.ReferenceId);
    destAccount.AddToTransactionsRefIds(transaction.ReferenceId);
  }

  public List<Transaction> GetAccountTransactions(string accountNumber)
  {
    if (!_accounts.ContainsKey(accountNumber))
      throw new ArgumentException("No Account Number Found.", nameof(accountNumber));

    List<Transaction> transactions = [];
    foreach (KeyValuePair<string, Transaction> item in _transactions)
    {
      if (item.Value.AccountNumber == accountNumber || item.Value.DestAccountNumber == accountNumber)
        transactions.Add(item.Value);
    }
    // if(transactions.Count == 0)

    return transactions;
  }

  public Transaction GetTransactionByRefId(string refId)
  {
    _transactions.TryGetValue(refId, out Transaction transaction);
    if (transaction == null)
      throw new ArgumentException("No Transaction Found For Provided Reference Id.", nameof(refId));
    return transaction;
  }

  public Dictionary<string, Transaction> GetTransactionsByType(TransactionType type)
  {
    Dictionary<string, Transaction> transactions = [];
    foreach (KeyValuePair<string, Transaction> item in _transactions)
    {
      if (item.Value.Type == type)
        transactions.Add(item.Key, item.Value);
    }
    return transactions;
  }

  // Internal Operations
  private void ValidateOwnerName(string name)
  {
    name = name.Trim();
    // Check If Null or Empty.
    if (string.IsNullOrWhiteSpace(name))
      throw new ArgumentException("Name Cannot be Empty!", nameof(name));

    // Check If it Matches the Name Pattern.
    if (!OwnerNameRegex.IsMatch(name))
      throw new ArgumentException("Invalid Name! Only Use Alphabetical Characters.", nameof(name));
  }

  private void ValidateBalance(decimal balance)
  {
    // Check if Valid Balance.
    if (balance < 0)
      throw new ArgumentException("Not Valid Balance! balance should be a number greater than or equals zero.", nameof(balance));
  }
  private string GenerateAccountNumber()
  {
    // Generate A 6-digit Unique Number.
    int accountNumber;
    do
    {
      accountNumber = RandomNumberGenerator.GetInt32(FromInclusive, ToExclusive);

    } while (_accounts.ContainsKey(accountNumber.ToString()));

    return accountNumber.ToString();
  }
  private void AddToTransactions(string refId, Transaction transaction)
  {
    _transactions.Add(refId, transaction);
  }


}