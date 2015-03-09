using System;
using System.Collections.Generic;

namespace WildWestBankApp.Models {
    public class DataRepository {
        private List<Customer> customers;
        private List<Account> accounts;
        public void LoadCustomersFromDataFile() {

        }
        public void LoadAccountsFromDataFile() {

        }
        public List<Customer> Customers {
            get { 
                return customers; 
            }
        }
        public List<Account> Accounts {
            get {
                return accounts;
            }
        }

    }
}