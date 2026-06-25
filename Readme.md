# Bank Exercise

This is a small C# learning project for practicing object-oriented programming, domain modeling, and backend design.

The project models a simple bank system where accounts can be created, money can be deposited or withdrawn, money can be transferred between accounts, and every financial operation is recorded as a transaction.

## Project Goals

The goal of this project is not to build a real banking system.

The goal is to practice:

- Thinking in domain objects
- Separating responsibilities between classes
- Using entities and value objects
- Protecting object state with encapsulation
- Modeling workflows such as deposit, withdraw, and transfer
- Choosing simple collections for in-memory storage
- Understanding what would need to change in a larger backend system

## Main Domain Concepts

### Bank

`Bank` is the main coordinator of the system.

It is responsible for:

- Creating accounts
- Finding accounts
- Handling deposits
- Handling withdrawals
- Handling transfers
- Storing all accounts in memory
- Storing all transactions in memory

In this project, `Bank` owns the transaction history. Because of that, all financial operations go through `Bank`.

### BankAccount

`BankAccount` represents a customer account.

It is responsible for:

- Holding the account number
- Holding the owner name
- Holding the current balance
- Applying balance changes internally

`BankAccount` does not create transactions by itself. It only knows how to update its own balance when asked by the `Bank`.

### Transaction

`Transaction` represents a recorded financial operation.

A transaction stores:

- A unique transaction reference
- The transaction date
- The transaction type
- The amount
- The related account number
- The destination account number for transfers

Transactions are intended to represent history, so they should not be changed after they are created.

## Value Objects

This project uses value objects for important domain values.

### AccountNumber

`AccountNumber` represents the identity of a bank account.

Two account numbers with the same value are considered equal.

### Money

`Money` represents a money amount.

It wraps a `decimal` value instead of using raw numbers everywhere in the domain.

This makes the code more expressive:

```csharp
new Money(100)
```

is clearer than passing a plain `decimal` without context.

### OwnerName

`OwnerName` represents the name of the account owner.

It validates that the name is not empty and only contains alphabetical characters.

### TransactionReference

`TransactionReference` represents the unique reference of a transaction.

It is used to find a specific transaction later.

## Important Design Decision

Deposit and withdraw operations conceptually belong to a bank account.

However, this project intentionally makes `Bank` responsible for the full deposit, withdraw, and transfer workflows.

The reason is transaction history.

If developers were allowed to call a method like this directly:

```csharp
account.Deposit(amount);
```

they might update the balance but forget to record the transaction.

To avoid that, the public workflow is:

```csharp
bank.Deposit(accountNumber, amount);
bank.Withdraw(accountNumber, amount);
bank.Transfer(fromAccountNumber, toAccountNumber, amount);
```

Each operation follows this pattern:

```text
Bank receives the request
Bank finds the account
Bank asks the account to apply the balance change
Bank creates a transaction
Bank stores the transaction in history
```

This keeps transaction recording centralized.

## Example Workflow

```csharp
Bank bank = new();

BankAccount account1 = bank.CreateAccount(
    new OwnerName("yusuf"),
    new Money(1000)
);

BankAccount account2 = bank.CreateAccount(
    new OwnerName("ahmad"),
    new Money(500)
);

bank.Deposit(account1.AccountNumber, new Money(200));
bank.Withdraw(account2.AccountNumber, new Money(100));
bank.Transfer(account1.AccountNumber, account2.AccountNumber, new Money(300));
```

## Current Storage

This project stores data in memory.

Accounts are stored in:

```csharp
Dictionary<AccountNumber, BankAccount>
```

Transactions are stored in:

```csharp
Dictionary<TransactionReference, Transaction>
```

This is simple and useful for learning.

It also makes lookups easy:

- Find an account by account number
- Find a transaction by transaction reference
- Filter transactions by account or type

## What Is Good About The Current Design

- The code uses domain concepts instead of only primitive types.
- `BankAccount` protects its balance from direct public modification.
- `Money`, `AccountNumber`, `OwnerName`, and `TransactionReference` make the model more expressive.
- `Bank` centralizes financial workflows.
- Transactions are recorded in one place.
- Dictionaries are a reasonable collection choice for in-memory lookup.
- The design is small enough to understand but still useful for practicing domain thinking.

## Current Limitations

This is a learning project, so some things are intentionally simple.

Current limitations:

- Data is not saved after the program ends.
- There is no database.
- There is no real concurrency handling.
- There is no database transaction to guarantee all-or-nothing updates.
- Transfer rollback is simple and not production-level.
- Account number generation is handled inside `Bank`.
- There are no automated tests yet.

## What Would Change In A Larger System

If this project grew into a real backend application, the design would likely be split into more parts.

Possible future design:

- `BankAccount` remains the account entity.
- `Transaction` remains the transaction history record.
- `Bank` may become a domain service or application service.
- Account storage would move to an `AccountRepository`.
- Transaction storage would move to a `TransactionRepository`.
- Account number generation may move to a separate generator service.
- A database would enforce account number uniqueness.
- A database transaction would make transfers atomic.
- Tests would cover deposits, withdrawals, transfers, invalid amounts, and transaction history.

## Possible Future Improvements

Good next steps for this project:

- Add unit tests
- Validate `AccountNumber`
- Improve transfer failure handling
- Make it harder to bypass `Bank` and mutate accounts directly
- Add better transaction queries
- Add a simple repository layer
- Add persistence later using a database
- Separate application workflow code from domain model code

## Learning Notes

This project is mainly about practicing design tradeoffs.

The current `Bank`-as-coordinator approach is reasonable for this exercise because it protects the rule:

> Every financial operation should be recorded in transaction history.

In a larger system, this same rule would probably be protected using services, repositories, and database transactions.

For now, keeping the design simple makes the important ideas easier to see.
