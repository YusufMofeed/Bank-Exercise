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
  internal void ApplyDeposit(Money amount)
  {
    Balance += amount;
  }
  internal void ApplyWithdraw(Money amount)
  {
    ValidateWithdrawAmount(amount);
    Balance -= amount;
  }

  // System Operations (Private)
  private void ValidateWithdrawAmount(Money amount)
  {
    if (amount > Balance || amount.Value == 0)
      throw new InvalidOperationException("Withdrawal Amount Must Be >= Zero And <= The Account Balance.");
  }
}