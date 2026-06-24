using BankExercise.Domain.ValueObjects;

namespace BankExercise.Domain.Entities;

public class Transaction
{
  private readonly TransactionReference _referenceId;
  public TransactionReference ReferenceId => _referenceId;
  private readonly string _date;
  public string Date => _date;
  private readonly TransactionType _type;
  public TransactionType Type => _type;

  private readonly decimal _amount;
  public decimal Amount => _amount;

  private readonly AccountNumber _accountNumber;
  public AccountNumber AccountNumber => _accountNumber;

  // The ? communicates:
  // This value may not exist for all transaction types.
  private readonly AccountNumber? _destAccountNumber;
  public AccountNumber? DestAccountNumber => _destAccountNumber;


  public Transaction(TransactionType type, decimal amount, AccountNumber accountNumber)
  {
    _referenceId = TransactionReference.Generate();
    _date = DateTime.UtcNow.Ticks.ToString();
    _type = type;
    _amount = amount;
    _accountNumber = accountNumber;
  }
  private Transaction(decimal amount, AccountNumber srcAccountNumber, AccountNumber destAccountNumber)
  {
    _referenceId = TransactionReference.Generate();
    _date = DateTime.UtcNow.Ticks.ToString();
    _type = TransactionType.Transfer;
    _amount = amount;
    _accountNumber = srcAccountNumber;
    _destAccountNumber = destAccountNumber;
  }

  // Factory Method "Force Transfer Type"
  public static Transaction CreateTransfer(decimal amount, AccountNumber srcAccountNumber, AccountNumber destAccountNumber)
  {
    return new Transaction(amount, srcAccountNumber, destAccountNumber);
  }
}
