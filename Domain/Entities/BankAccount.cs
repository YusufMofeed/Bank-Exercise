using BankExercise.Domain.ValueObjects;

namespace BankExercise.Domain.Entities;

public class BankAccount
{
  private readonly string _ownerName;
  public string OwnerName => _ownerName;

  private decimal _balance;
  public decimal Balance => _balance;

  private readonly AccountNumber _accountNumber;
  public AccountNumber AccountNumber => _accountNumber;


  public BankAccount(AccountNumber accountNumber, OwnerName name, decimal initialBalance)
  {
    // Assign Account Number, OwnerName & Balance.
    _accountNumber = accountNumber;
    _ownerName = name.Name;
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
  private static void ValidateDepositAmount(decimal amount)
  {
    if (amount <= 0)
      throw new ArgumentException("Deposit Amount Must Be Greater Than Zero.", nameof(amount));
  }

  private void ValidateWithdrawAmount(decimal amount)
  {
    if (amount <= 0 || amount > _balance)
      throw new ArgumentException("Withdrawl Amount Must Be Greater Than Zero and Less Than Or Equals The Account Balance.", nameof(amount));
  }
}