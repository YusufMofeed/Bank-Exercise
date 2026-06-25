using BankExercise.Domain.Enums;
using BankExercise.Domain.ValueObjects;

namespace BankExercise.Domain.Entities;

public class BankAccount
{
  public OwnerName OwnerName { get; }
  public Money Balance { get; private set; }
  public AccountNumber AccountNumber { get; }


  public BankAccount(AccountNumber accountNumber, OwnerName name, Money initialBalance)
  {
    OwnerName = name;
    Balance = initialBalance;
    AccountNumber = accountNumber;
  }
  // Public API
  public void ApplyDeposit(Money amount)
  {
    Balance += amount;
  }
  public void ApplyWithdraw(Money amount)
  {
    ValidateWithdrawAmount(amount);
    Balance -= amount;
  }

  // System Operations (Private)
  private void ValidateWithdrawAmount(Money amount)
  {
    if (amount > Balance)
      throw new InvalidOperationException("Withdrawal Amount Must Be <= The Account Balance.");
  }
}