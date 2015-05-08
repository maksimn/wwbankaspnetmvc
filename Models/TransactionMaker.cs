using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace WildWestBankApp.Models {
    public class TransactionMaker {
        private DataRepository repository;

        public TransactionMaker() {
            repository = new DataRepository();
            repository.Load();
        }

        public List<Transaction> Transactions {
            get {
                return repository.TransactionList;
            }
        }

        public Int32 GetNewId() {
            if (this.Transactions.Count() == 0) {
                return 1;
            } else {
                return this.Transactions.Max(tr => tr.ID) + 1;
            }
        }

        private void ValidateIfAccountExists(Account a, ModelStateDictionary msd) {
            var newModelState = new ModelStateDictionary();
            if (a == null) {
                newModelState.AddModelError(
                    "FromAccountID",
                    String.Format("Account with id = {0} does not exist.", a.AccountID)
                );
            }
            msd.Merge(newModelState);
        }

        private Boolean TransferMoneyBetweenAccounts(Transaction transaction, ModelStateDictionary modelState) {
            ShowErrorMessageIfFromAccountAndToAccountAreEqual(transaction, modelState);

            Account fromAccount = repository.FindAccountById(transaction.FromAccountID);
            Account toAccount = repository.FindAccountById(transaction.ToAccountID);

            ValidateIfAccountExists(fromAccount, modelState);
            ValidateIfAccountExists(toAccount, modelState);

            if(!modelState.IsValid) {
                return false;
            }
            if (!ValidateIfAccountHasEnougnMoneyForTransaction(fromAccount, transaction)) {
                var newMsd = new ModelStateDictionary();
                newMsd.AddModelError("Amount",
                    String.Format("Money on Account with id = {0} is not enough for the transaction", fromAccount.AccountID));
                modelState.Merge(newMsd);
                return false;
            }
            checked {
                fromAccount.Money -= transaction.Amount;
                toAccount.Money += transaction.Amount; // Can generate OverflowException
            }
            repository.UpdateAccount(fromAccount);
            repository.UpdateAccount(toAccount);
            repository.AddTransaction(transaction);
            modelState.Clear();
            return true;
        }

        private Boolean AddMoneyToAccount(Transaction transaction, ModelStateDictionary modelState) {
            Account toAccount = repository.FindAccountById(transaction.ToAccountID);
            ValidateIfAccountExists(toAccount, modelState);
            if (!modelState.IsValid) {
                return false;
            }            
            checked {
                toAccount.Money += transaction.Amount; // Can generate OverflowException
            }
            repository.UpdateAccount(toAccount);
            repository.AddTransaction(transaction);
            modelState.Clear();
            return true;
        }

        private Boolean WithdrawFromAccount(Transaction transaction, ModelStateDictionary modelState) {
            Account fromAccount = repository.FindAccountById(transaction.FromAccountID);
            ValidateIfAccountExists(fromAccount, modelState);
            if (!modelState.IsValid) {
                return false;
            }
            if (!ValidateIfAccountHasEnougnMoneyForTransaction(fromAccount, transaction)) {
                var newMsd = new ModelStateDictionary();
                newMsd.AddModelError("Amount",
                    String.Format("Money on Account with id = {0} is not enough for the transaction", fromAccount.AccountID));
                modelState.Merge(newMsd);
                return false;
            }
            checked {
                fromAccount.Money -= transaction.Amount; // Can generate OverflowException
            }
            repository.UpdateAccount(fromAccount);
            repository.AddTransaction(transaction);
            modelState.Clear();
            return true;
        }

        private Boolean ValidateIfAccountHasEnougnMoneyForTransaction(Account a, Transaction t) {
            if(a.Money >= t.Amount) {
                return true;
            } else {
                return false;
            }
        }

        private void ShowErrorMessageIfFromAccountAndToAccountAreEqual(Transaction t, ModelStateDictionary msd) {
            var newModelState = new ModelStateDictionary();
            if (t.FromAccountID == t.ToAccountID) {
                newModelState.AddModelError(
                    "FromAccountID",
                    "'From Account' and 'To Account' fields must be different for transfering money"
                );
                newModelState.AddModelError(
                    "ToAccountID",
                    "'From Account' and 'To Account' fields must be different for transfering money"
                );
                msd.Merge(newModelState);
            }
        }

        public String TryMakeTransfer(Transaction transaction, ModelStateDictionary modelState) {
            return TryCommitTransaction(transaction, modelState, TransferMoneyBetweenAccounts);
        }

        public String TryAddMoneyToAccount(Transaction transaction, ModelStateDictionary modelState) {
            return TryCommitTransaction(transaction, modelState, AddMoneyToAccount);
        }

        public String TryWithdrawFromAccount(Transaction transaction, ModelStateDictionary modelState) {
            return TryCommitTransaction(transaction, modelState, WithdrawFromAccount);
        }

        private String TryCommitTransaction(Transaction transaction, ModelStateDictionary modelState, 
                                            Func<Transaction, ModelStateDictionary, Boolean> operation) {
            transaction.DateTime = DateTime.Now;
            Boolean result = false;
            try {
                result = operation(transaction, modelState);
            } catch (OverflowException) {
                return "Overflow error while the transaction.";
            } catch (Exception) {
                throw;
            }
            if (result) {
                return "Last transaction was successfully committed.";
            }
            return "Try to re-enter your information and repeat transaction.";
        }
    }
}