using System.Security.Cryptography;

namespace BankAccount;

class Program
{
  static void Main(string[] args)
  {
    Bank bank = new();
    BankAccount account1 = bank.CreateAccount("Mhmd", 100.50);
    BankAccount account2 = bank.CreateAccount("yusuf", 1000);
    bank.Deposit(account1.AccountNumber, 5000);
    bank.Withdraw(account1.AccountNumber, 2000);
    bank.Withdraw(account2.AccountNumber, 500);
    bank.Transfer(fromAccountNumber: account1.AccountNumber, toAccountNumber: account2.AccountNumber, 2000);
    System.Console.WriteLine(account1.Balance);
    System.Console.WriteLine(account2.Balance);

  }
}