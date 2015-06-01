using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Collections.Generic;
using System.IO;

namespace WildWestBankApp.Models {
    public class DataRepository {
        private String connName = "WWBankConnectionString";
        private DataSet dataSet;

        private String accountsFilePath = AppDomain.CurrentDomain.BaseDirectory + @"\data\Account.csv";
        private String transactionFilePath = AppDomain.CurrentDomain.BaseDirectory + @"\data\Transaction.csv";

        private List<Customer> customers;
        private List<Account> accounts;
        private List<Transaction> transactionList;
        public DataRepository() {
            dataSet = new DataSet();
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[connName];
            using (SqlConnection conn = new SqlConnection(settings.ConnectionString)) {
                conn.Open();
                String selectQuery = "SELECT ID, Name, Address, BirthDay FROM Customers";
                SqlDataAdapter da = new SqlDataAdapter(selectQuery, conn); 
                da.Fill(dataSet, "Customers");
                FillCustomerListWithData();

                selectQuery = "SELECT AccountID, CustomerID, Money FROM Accounts";
                da = new SqlDataAdapter(selectQuery, conn);
                da.Fill(dataSet, "Accounts");
                FillAccountListWithData();
                conn.Dispose();
            } 
        }

        private void FillCustomerListWithData() {
            customers = new List<Customer>();
            foreach (DataRow row in dataSet.Tables["Customers"].Rows) {
                var c = new Customer { ID = (Int32)row[0], Name = (String)row[1], Address = (String)row[2], BirthDay = (DateTime)row[3] };
                customers.Add(c);
            }
        }
        private void FillAccountListWithData() {
            accounts = new List<Account>();
            foreach (DataRow row in dataSet.Tables["Accounts"].Rows) {
                var a = new Account { AccountID = (Int32)row[0], CustomerID = (Int32)row[1], Money = (Decimal)row[2] };
                accounts.Add(a);
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
                    transaction.Type = (TransactionType) Convert.ToInt32(rowField[3]);
                    transaction.Amount = Convert.ToDecimal(rowField[4]);
                    transaction.DateTime = Convert.ToDateTime(rowField[5]);
                    transactionList.Add(transaction);
                }
                file.Dispose();
            }
        }
        public void Load() {
            LoadTransactionListFromFile();
        }

        public List<Account> Accounts {
            get { return accounts; }
        }
        public List<Customer> Customers {
            get { return customers; }
        }
        public List<Transaction> TransactionList {
            get {
                return transactionList;
            }
        }

        public void AddAccount(Account account) {
            Add(account, accounts, "Accounts", String.Format("INSERT INTO Accounts (AccountID, CustomerID, Money) VALUES ({0}, {1}, {2})",
                    account.AccountID, account.CustomerID, account.Money.ToString().Replace(',', '.')),
                new Object[] { account.AccountID, account.CustomerID, account.Money });
        }
        public void AddCustomer(Customer customer) {
            Add(customer, customers, "Customers", String.Format("INSERT INTO Customers (ID, Name, Address, BirthDay) VALUES ({0}, '{1}', '{2}', '{3}')",
                    customer.ID, customer.Name.Replace("'", "''"), customer.Address.Replace("'", "''"), customer.BirthDay.ToString("yyyyMMdd")),
                new Object[] { customer.ID, customer.Name, customer.Address, customer.BirthDay });
        }
        private void Add<T>(T obj, List<T> collection, String table, String insertQuery, Object[] row) {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[connName];
            using (SqlConnection conn = new SqlConnection(settings.ConnectionString)) {
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(insertQuery, conn);
                dataSet.Tables[table].Rows.Add(row);
                da.InsertCommand = new SqlCommand(insertQuery, conn);
                da.Update(dataSet, table);
                collection.Add(obj);
                conn.Dispose();
            } 
        }
        public void AddTransaction(Transaction transaction) {
            transactionList.Add(transaction);
            SaveTransactionInFile(transaction);
        }

        private String ConvertToString(Account account) {
            return String.Format("{0};{1};{2}", account.AccountID, account.CustomerID, account.Money);
        }
        private String ConvertToString(Transaction transaction) {
            return String.Format("{0};{1};{2};{3};{4};{5}", transaction.ID, transaction.FromAccountID,
                transaction.ToAccountID, (Int32)transaction.Type, transaction.Amount, transaction.DateTime);
        }

        private void SaveTransactionInFile(Transaction transaction) {
            File.AppendAllText(transactionFilePath, Environment.NewLine + ConvertToString(transaction));
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