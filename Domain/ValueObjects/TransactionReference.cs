namespace BankExercise.Domain.ValueObjects;

public readonly record struct TransactionReference(string Value)
{
  public override string ToString() => Value;

  public static TransactionReference Generate()
  {
    return new TransactionReference(Guid.NewGuid().ToString());
  }

}