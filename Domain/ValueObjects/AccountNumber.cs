namespace BankExercise.Domain.ValueObjects;

public readonly record struct AccountNumber(string Value)
{
  public override string ToString() => Value;
}