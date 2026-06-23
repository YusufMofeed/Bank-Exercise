namespace BankAccount;

public class Transaction
{
  private readonly string _referenceId;
  public string ReferenceId => _referenceId;
  private readonly string _date;
  public string Date => _date;
  private readonly TransactionType _type;
  public TransactionType Type => _type;

  private readonly decimal _amount;
  public decimal Amount => _amount;

  private readonly string _accountNumber;
  public string AccountNumber => _accountNumber;
  private readonly string _destAccountNumber;
  public string DestAccountNumber => _destAccountNumber;


  public Transaction(TransactionType type, decimal amount, string accountNumber)
  {
    _referenceId = Guid.NewGuid().ToString();
    _date = DateTime.UtcNow.Ticks.ToString();
    _type = type;
    _amount = amount;
    _accountNumber = accountNumber;
    _destAccountNumber = "";
  }
  public Transaction(decimal amount, string srcAccountNumber, string destAccountNumber, TransactionType type = TransactionType.Transfer)
  {
    _referenceId = Guid.NewGuid().ToString();
    _date = DateTime.UtcNow.Ticks.ToString();
    _type = type;
    _amount = amount;
    _accountNumber = srcAccountNumber;
    _destAccountNumber = destAccountNumber;
  }

}
