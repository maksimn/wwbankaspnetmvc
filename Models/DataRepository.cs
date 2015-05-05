using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;

namespace WildWestBankApp.Models {
    public class DataRepository { 
        private String customersFilePath = AppDomain.CurrentDomain.BaseDirectory + @"\data\Customer.csv";
        private String accountsFilePath = AppDomain.CurrentDomain.BaseDirectory + @"\data\Account.csv";
        private String transactionFilePath = AppDomain.CurrentDomain.BaseDirectory + @"\data\Transaction.csv";
        private String transactionTypeFilePath = AppDomain.CurrentDomain.BaseDirectory + @"\data\TransactionType.csv";

        private List<Customer> customers;
        private List<Account> accounts;
        private List<TransactionType> transactionTypes;
        private List<Transaction> transactionList;

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
                file.Dispose();
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
                file.Dispose();
            }
        }
        private void LoadTransactionTypesFromFile() {
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
                file.Dispose();
            }
        }
        private void LoadTransactionListFromFile() {
            transactionList = new List<Transaction>();
            using (StreamReader file = new StreamReader(transactionFilePath)) {
                String line;
                String[] rowField;
                file.ReadLine();
                while ((line = file.ReadLine()) != null) {
                    rowField = line.Split(new Char[] { ';' });
                    Transaction transaction = new Transaction();
                    transaction.ID = Convert.ToInt32(rowField[0]);
                    transaction.FromAccountID = Convert.ToInt32(rowField[1]);
                    transaction.ToAccountID = Convert.ToInt32(rowField[2]);
                    transaction.TypeID = Convert.ToInt32(rowField[3]);
                    transaction.Amount = Convert.ToDecimal(rowField[4]);
                    transaction.DateTime = Convert.ToDateTime(rowField[5]);
                    transactionList.Add(transaction);
                }
                file.Dispose();
            }
        }
        public void Load() {
            LoadAccountsFromDataFile();
            LoadCustomersFromDataFile();
            LoadTransactionTypesFromFile();
            LoadTransactionListFromFile();
        }

        public List<Account> Accounts {
            get {
                return accounts;
            }
        }
        public List<Customer> Customers {
            get { 
                return customers; 
            }
        }
        public List<Transaction> TransactionList {
            get {
                return transactionList;
            }
        }

        public void AddAccount(Account account) {
            accounts.Add(account);
            SaveAccountInFile(account);
        }
        public void AddCustomer(Customer customer) {
            customers.Add(customer);
            SaveCustomerInFile(customer);
        }
        public void AddTransaction(Transaction transaction) {
            transactionList.Add(transaction);
            SaveTransactionInFile(transaction);
        }

        private String ConvertToString(Customer customer) {
            String temp = "0" + customer.BirthDay.Day;
            String strDay = temp.Substring(temp.Length - 2);
            temp = "0" + customer.BirthDay.Month;
            String strMonth = temp.Substring(temp.Length - 2);
            return String.Format("{0};{1};{2};{3}{4}{5}", customer.ID, customer.Name, customer.Address,
                customer.BirthDay.Year, strMonth, strDay);
        }       
        private String ConvertToString(Account account) {
            return String.Format("{0};{1};{2}", account.AccountID, account.CustomerID, account.Money);
        }
        private String ConvertToString(Transaction transaction) {
            return String.Format("{0};{1};{2};{3};{4};{5}", transaction.ID, transaction.FromAccountID,
                transaction.ToAccountID, transaction.TypeID, transaction.Amount, transaction.DateTime);
        }

        private void SaveTransactionInFile(Transaction transaction) {
            File.AppendAllText(transactionFilePath, Environment.NewLine + ConvertToString(transaction));
        }
        private void SaveAccountInFile(Account account) {
            File.AppendAllText(accountsFilePath, Environment.NewLine + ConvertToString(account));
        }
        private void SaveCustomerInFile(Customer customer) {
            File.AppendAllText(customersFilePath, Environment.NewLine + ConvertToString(customer));
        }

        public Account FindAccountById(Int32 id) {
            var accounts = Accounts.Where(acc => acc.AccountID == id);
            if (accounts.Count() == 0) {
                return null;
            }
            return accounts.First();
        }

        public void UpdateAccount(Account account) {
            String fileText = File.ReadAllText(accountsFilePath);
            String[] stringArray = fileText.Split(new String[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            for (Int32 i = 1; i < stringArray.Length; i++) {
                String strID = stringArray[i].Substring(0, stringArray[i].IndexOf(';'));
                Int32 id = Convert.ToInt32(strID);
                if (id == account.AccountID) {
                    stringArray[i] = ConvertToString(account);
                    break;
                }
            }
            File.WriteAllText(accountsFilePath, String.Join(Environment.NewLine, stringArray));
        }
    }
}