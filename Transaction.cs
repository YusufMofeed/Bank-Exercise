namespace BankAccount;

public class Transaction
{
  private readonly string _referenceId;
  public string ReferenceId => _referenceId;
  private readonly string _date;
  public string Date => _date;
  private readonly TransactionType _type;
  public TransactionType Type => _type;

  private readonly double _amount;
  public double Amount => _amount;

  private readonly string _accountNumber;
  public string AccountNumber => _accountNumber;
  private readonly string _destAccountNumber;
  public string DestAccountNumber => _destAccountNumber;


  public Transaction(TransactionType type, double amount, string accountNumber, bool isTransfer = false, string destAccountNumber = "")
  {
    _referenceId = Guid.NewGuid().ToString();
    _date = DateTime.UtcNow.Ticks.ToString();
    _type = type;
    _amount = amount;
    _accountNumber = accountNumber;
    _destAccountNumber = destAccountNumber;


    if (isTransfer)
      if (String.IsNullOrWhiteSpace(destAccountNumber))
        throw new ArgumentException("Must Provide Destination Account Number When The Operation Is Transfer.", nameof(destAccountNumber));
      else
        _destAccountNumber = destAccountNumber;
  }

}
