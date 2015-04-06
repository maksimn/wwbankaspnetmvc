using System;
using System.Collections.Generic;
using System.IO; 

namespace WildWestBankApp.Models {
    public class DataRepository { 
        private String customersFilePath = AppDomain.CurrentDomain.BaseDirectory + @"\data\Customer.csv";
        private String accountsFilePath = AppDomain.CurrentDomain.BaseDirectory + @"\data\Account.csv";
        private String transactionTypeFilePath = AppDomain.CurrentDomain.BaseDirectory + @"\data\TransactionType.csv";
        private List<Customer> customers;
        private List<Account> accounts;
        private List<TransactionType> transactionTypes;

        public void LoadTransactionTypesFromFile() {
            transactionTypes = new List<TransactionType>();
            using (StreamReader file = new StreamReader(transactionTypeFilePath)) {
                String line;
                String[] rowField;
                file.ReadLine();
                while ((line = file.ReadLine()) != null) {
                    rowField = line.Split(new Char[] { ';' });
                    TransactionType transactionType = new TransactionType();
                    transactionType.ID = Convert.ToInt32(rowField[0]);
                    transactionType.Name = rowField[1];
                    transactionTypes.Add(transactionType);
                }
            }
        }

        public void LoadCustomersFromDataFile() {            
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
            accounts = new List<Account>();
            using (StreamReader file = new StreamReader(accountsFilePath)) {
                String line;
                String[] rowField;
                file.ReadLine();
                while ((line = file.ReadLine()) != null) {
                    rowField = line.Split(new Char[] { ';' });
                    Account account = new Account() {
                        AccountID = Convert.ToInt32(rowField[0]),
                        CustomerID = Convert.ToInt32(rowField[1]),
                        Money = Convert.ToDecimal(rowField[2])
                    }; 
                    accounts.Add(account);
                }
            }
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

        public List<TransactionType> TransactionTypes {
            get {
                return transactionTypes;
            }
        }
        
        public void AddCustomer(Customer customer) {
            customers.Add(customer);
            File.AppendAllText(customersFilePath, Environment.NewLine + ConvertToString(customer));
        }
        
        private String ConvertToString(Customer customer) {
            String temp = "0" + customer.BirthDay.Day;
            String strDay = temp.Substring(temp.Length - 2);
            temp = "0" + customer.BirthDay.Month;
            String strMonth = temp.Substring(temp.Length - 2);
            return String.Format("{0};{1};{2};{3}{4}{5}", customer.ID, customer.Name, customer.Address,
                customer.BirthDay.Year, strMonth, strDay); 
        }
        
        public void AddAccount(Account account) {
            accounts.Add(account);
            File.AppendAllText(accountsFilePath, Environment.NewLine + ConvertToString(account));
        }
        
        private String ConvertToString(Account account) {
            return String.Format("{0};{1};{2}", account.AccountID, account.CustomerID, account.Money);
        }
    }
}