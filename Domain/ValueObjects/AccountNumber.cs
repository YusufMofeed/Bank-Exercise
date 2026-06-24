using System.Security.Cryptography;

namespace BankExercise.Domain.ValueObjects;

public readonly record struct AccountNumber(string Value)
{
  private static readonly HashSet<string> accountNumbers = [];
  private const int FromInclusive = 100000;
  private const int ToExclusive = 1000000;

  public override string ToString() => Value;

  public static AccountNumber Generate()
  {
    // Generate A 6-digit Unique Number.
    string accountNumber;
    do
    {
      accountNumber = RandomNumberGenerator.GetInt32(FromInclusive, ToExclusive).ToString();
    } while (accountNumbers.Contains(accountNumber));

    accountNumbers.Add(accountNumber);

    return new AccountNumber(accountNumber);
  }
}