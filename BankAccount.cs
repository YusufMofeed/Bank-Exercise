using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace BankAccount;

public class BankAccount
{
  private readonly string _ownerName;
  public string OwnerName => _ownerName;

  private decimal _balance;
  public decimal Balance => _balance;

  private readonly string _accountNumber;
  public string AccountNumber => _accountNumber;
  private readonly HashSet<string> _transactionsRefIds = []; // could be a List
  // public HashSet<string> TransactionsRefIds => _transactionsRefIds;


  public BankAccount(string accountNumber, string ownerName, decimal initialBalance)
  {
    // Assign Account Number, OwnerName & Balance.
    _accountNumber = accountNumber;
    _ownerName = ownerName;
    _balance = initialBalance;
  }
  // Public API
  public void ApplyDeposit(decimal amount)
  {
    ValidateDepositAmount(amount);
    _balance += amount;
  }
  public void ApplyWithdraw(decimal amount)
  {
    ValidateWithdrawAmount(amount);
    _balance -= amount;
  }

  // System Operations (Private)

  private void ValidateDepositAmount(decimal amount)
  {
    if (amount <= 0)
      throw new ArgumentException("Deposit Amount Must Be Greater Than Zero.", nameof(amount));
  }

  private void ValidateWithdrawAmount(decimal amount)
  {
    if (amount <= 0 || amount > _balance)
      throw new ArgumentException("Withdrawl Amount Must Be Greater Than Zero and Less Than Or Equals The Account Balance.", nameof(amount));
  }

  public void AddToTransactionsRefIds(string refID)
  {
    _transactionsRefIds.Add(refID);
  }
}