using System;
using System.Linq;
using System.Collections.Generic;

namespace WildWestBankApp.Models {
    public class AccountsViewModel {
        public String CustomerName { get; set; }
        public Int32 CustomerID { get; set; }
        public IEnumerable<AccountInfo> AccountsInfo { get; set; }

        public AccountsViewModel(Int32 id, DataRepository repository) {
            CustomerID = id;
            CustomerName = repository.Customers.Where(customer => customer.ID == id).First().Name;
            AccountsInfo = repository.Accounts.Where(account => account.CustomerID == id)
                .Select(account => new AccountInfo { Id = account.AccountID, Money = account.Money });
            newAccountId = repository.Accounts.Max(a => a.AccountID) + 1;
        }

        public class AccountInfo {
            public Int32 Id { get; set; }
            public Decimal Money { get; set; }
        }

        private readonly Int32 newAccountId;
        public Int32 NewAccountID {
            get {
                return newAccountId;
            }
        }
    }
}