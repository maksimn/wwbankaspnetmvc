using System;
using System.Collections.Generic;
using System.IO;

namespace WildWestBankApp.Models {
    public class DataRepository {
        private String rootFolder = AppDomain.CurrentDomain.BaseDirectory;
        private List<Customer> customers;
        private List<Account> accounts;
        public void LoadCustomersFromDataFile() {            
            String customersFilePath = rootFolder + @"\data\Customer.csv";
            customers = new List<Customer>();
            using(StreamReader file = new StreamReader(customersFilePath)) {
                String line;
                String[] rowField;
                file.ReadLine();
                while ((line = file.ReadLine()) != null) {
                    rowField = line.Split(new Char[] { ';' });
                    Customer customer = new Customer();
                    customer.ID = Convert.ToInt32(rowField[0]);
                    customer.Name = rowField[1];
                    customer.Address = rowField[2];
                    Int32 year = Convert.ToInt32(rowField[3].Substring(0, 4));
                    Int32 month = Convert.ToInt32(rowField[3].Substring(4, 2));
                    Int32 day = Convert.ToInt32(rowField[3].Substring(6, 2));
                    customer.BirthDay = new DateTime(year, month, day);
                    customers.Add(customer);
                }
            }
        }
        public void LoadAccountsFromDataFile() {
            String _accountsFilePath = rootFolder + @"\data\Account.csv";
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