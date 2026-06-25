using BankExercise.Domain.Entities;
using BankExercise.Domain.ValueObjects;

namespace BankExercise;

class Program
{
  static void Main(string[] args)
  {
    Bank bank = new();
    BankAccount account1 = bank.CreateAccount(new OwnerName("Mhmd"), new Money(Convert.ToDecimal(1000.5)));
    // BankAccount account2 = bank.CreateAccount(new OwnerName("yusuf"), new Money(1000));
    bank.Deposit(account1.AccountNumber, new Money(5000));
    // bank.Withdraw(account1.AccountNumber, new Money(2000));
    // bank.Withdraw(account2.AccountNumber, new Money(500));
    // bank.Transfer(fromAccountNumber: account1.AccountNumber, toAccountNumber: account2.AccountNumber, new Money(2000));
    System.Console.WriteLine(bank.GetAccountTransactions(account1.AccountNumber)[0].Date.ToString("dd/MM/yyyy HH:mm zz"));
    // System.Console.WriteLine(account2.Balance);

  }
}