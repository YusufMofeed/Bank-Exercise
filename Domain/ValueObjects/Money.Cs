namespace BankExercise.Domain.ValueObjects;

public readonly record struct Money
{
  public decimal Value { get; }
  public Money(double value)
  {
    if (value < 0)
      throw new ArgumentException("Money Must Be Greater Than Or Equals Zero.", nameof(value));
    Value = Convert.ToDecimal(value);
  }
  public override string ToString() => Value.ToString();

}