using BankExercise.Domain.ValueObjects;
using BankExercise.Domain.Enums;

namespace BankExercise.Domain.Entities;

public class Transaction
{
  public TransactionReference ReferenceId { get; }
  public DateTimeOffset Date { get; }
  public TransactionType Type { get; }
  public Money Amount { get; }
  public AccountNumber AccountNumber { get; }
  public AccountNumber? DestAccountNumber { get; }
  // The ? communicates: This value may not exist for all transaction types.


  public Transaction(TransactionType type, Money amount, AccountNumber accountNumber)
  {
    ReferenceId = TransactionReference.Generate();
    Date = DateTimeOffset.UtcNow;
    Type = type;
    Amount = amount;
    AccountNumber = accountNumber;
  }
  private Transaction(Money amount, AccountNumber srcAccountNumber, AccountNumber destAccountNumber)
  {
    ReferenceId = TransactionReference.Generate();
    Date = DateTimeOffset.UtcNow;
    Type = TransactionType.Transfer;
    Amount = amount;
    AccountNumber = srcAccountNumber;
    DestAccountNumber = destAccountNumber;
  }

  // Factory Method "Force Transfer Type"
  public static Transaction CreateTransfer(Money amount, AccountNumber srcAccountNumber, AccountNumber destAccountNumber)
  {
    return new Transaction(amount, srcAccountNumber, destAccountNumber);
  }
}
