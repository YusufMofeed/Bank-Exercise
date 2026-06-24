using System.Text.RegularExpressions;

namespace BankExercise.Domain.ValueObjects;

public readonly record struct OwnerName
{
  public readonly string Name { get; }
  private readonly Regex OwnerNameRegex = new("^([A-Za-z])+$");

  public OwnerName(string name)
  {
    name = name.Trim().ToLower();
    // Check If Null or Empty.
    if (string.IsNullOrWhiteSpace(name))
      throw new ArgumentException("Name Cannot be Empty!", nameof(name));

    // Check If it Matches the Name Pattern.
    if (!OwnerNameRegex.IsMatch(name))
      throw new ArgumentException("Invalid Name! Only Use Alphabetical Characters.", nameof(name));

    Name = name;
  }

  public override string ToString() => Name;

}