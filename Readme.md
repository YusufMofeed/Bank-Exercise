# Bank Exercise

## Purpose:
Practice OOP and backend thinking.

## Main Classes:
- Bank
- BankAccount
- Transaction

## Current Design:
- Bank stores accounts in Dictionary<AccountNumber, BankAccount>
- Bank stores transactions in Dictionary<ReferenceId, Transaction>
- Bank coordinates transfers
- BankAccount manages balance

## Known Questions:
- Transaction ownership
- History storage
- Transfer responsibility