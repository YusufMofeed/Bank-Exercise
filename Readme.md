# Bank Exercise

## Purpose:
Practice OOP and backend thinking.

## Main Classes:
- Bank
- BankAccount
- Transaction

## Current Design:
### Bank
- Stores accounts in Dictionary<AccountNumber, BankAccount>
- Stores transactions in Dictionary<ReferenceId, Transaction>
- Coordinates transfers

### BankAccount
- Holds balance
- Performs deposits and withdrawals

### Transaction
- Represents a banking operation

## Known Questions:
- Transaction ownership
- History storage
- Transfer responsibility
