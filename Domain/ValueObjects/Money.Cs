using BankExercise.Domain.Enums;

namespace BankExercise.Domain.ValueObjects;

public readonly record struct Money
{
  public decimal Value { get; }
  public Money(decimal value)
  {
    if (value < 0)
      throw new ArgumentException("Money Must Be Greater Than Or Equals Zero.", nameof(value));
    Value = value;
  }
  public override string ToString() => Value.ToString();

  // Money operations.

  // Operator approach 
  // Usage: Balance += amount;
  public static Money operator +(Money left, Money right)
  {
    ValidateAmount(MathOperation.Addition, right);
    return new Money(left.Value + right.Value);
  }

  public static Money operator -(Money left, Money right)
  {
    ValidateAmount(MathOperation.Subtraction, right);
    return new Money(left.Value - right.Value);
  }

  public static bool operator <(Money left, Money right)
  {
    return left.Value < right.Value;
  }
  public static bool operator >(Money left, Money right)
  {
    return left.Value > right.Value;
  }
  public static bool operator >=(Money left, Money right)
  {
    return left.Value >= right.Value;
  }
  public static bool operator <=(Money left, Money right)
  {
    return left.Value <= right.Value;
  }

  private static void ValidateAmount(MathOperation operation, Money amount)
  {
    // We've already prevented Money from being < 0.
    // Prevent Add/Sub Amount from being == 0.
    if (amount.Value == 0)
      throw new ArgumentException($"{operation} Amount Must Be > Zero.", nameof(amount));
  }
  /*
    // Method approach 
    // Usage: Balance = Balance.Add(amount);
    public Money Add(Money amount)
    {
      return new Money(Value + amount.Value);
    }
    */
}