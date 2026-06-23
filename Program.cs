using System.Security.Cryptography;

namespace BankAccount;

class Program
{
  static void Main(string[] args)
  {
    Bank bank = new();
    BankAccount account1 = bank.CreateAccount("Mhmd", 1000);
    BankAccount account2 = bank.CreateAccount("yusuf", 1000);
    bank.Deposit(account1.AccountNumber, 5000);
    bank.Withdraw(account1.AccountNumber, 2000);
    bank.Withdraw(account2.AccountNumber, 500);
    bank.Transfer(fromAccount: account1, toAccount: account2, 2000);
  }
}