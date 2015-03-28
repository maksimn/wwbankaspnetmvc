using System;
using System.Collections.Generic;

namespace WildWestBankApp.Models {
    public class AccountsViewModel {
        public String CustomerName { get; set; }
        public List<AccountInfo> AccountsInfo { get; set; }

        public AccountsViewModel(Int32 id, DataRepository repository) {
            AccountsInfo = new List<AccountInfo>();
            foreach (var account in repository.Accounts) {
                if(account.CustomerID == id) {
                    AccountsInfo.Add(new AccountInfo() { Id = account.AccountID, Money = account.Money });
                }
            }
        }

        public class AccountInfo {
            public Int32 Id { get; set; }
            public Decimal Money { get; set; }
        }
    }
}