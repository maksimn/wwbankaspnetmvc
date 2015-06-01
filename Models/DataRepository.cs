using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Collections.Generic;

namespace WildWestBankApp.Models {
    public class DataRepository {
        private readonly String connName = "WWBankConnectionString";
        private DataSet dataSet;

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
                DataTable accountTable = dataSet.Tables["Accounts"];
                DataColumn[] pk = new DataColumn[1];
                pk[0] = accountTable.Columns["AccountID"];
                accountTable.Constraints.Add(new UniqueConstraint("PK_Account", pk[0]));
                accountTable.PrimaryKey = pk;

                selectQuery = "SELECT ID, AccountFromId, AccountToId, Type, Amount, DateTime FROM Transactions";
                da = new SqlDataAdapter(selectQuery, conn);
                da.Fill(dataSet, "Transactions");
                FillTransactionListWithData();
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
        private void FillTransactionListWithData() {
            transactionList = new List<Transaction>();
            foreach (DataRow row in dataSet.Tables["Transactions"].Rows) {
                var a = new Transaction { ID = (Int32)row[0], FromAccountID = (Int32)row[1], ToAccountID = (Int32)row[2],
                    Type = (TransactionType)row[3], Amount = (Decimal)row[4], DateTime = (DateTime)row[5] };
                transactionList.Add(a);
            }
        }

        public List<Account> Accounts {
            get { return accounts; }
        }
        public List<Customer> Customers {
            get { return customers; }
        }
        public List<Transaction> TransactionList {
            get { return transactionList; }
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
        public void AddTransaction(Transaction transaction) {
            Add(transaction, transactionList, "Transactions", String.Format("INSERT INTO Transactions (ID, AccountFromId, AccountToId, Type, Amount, DateTime) VALUES ({0}, {1}, {2}, {3}, {4}, '{5}')",
                        transaction.ID, transaction.FromAccountID, transaction.ToAccountID, (Int32)(transaction.Type), transaction.Amount.ToString().Replace(',', '.'), transaction.DateTime.ToString("yyyy-MM-dd HH:mm:ss")),
                new Object[] { transaction.ID, transaction.FromAccountID, transaction.ToAccountID, transaction.Type, transaction.Amount, transaction.DateTime });
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

        public Account FindAccountById(Int32 id) {
            var accounts = Accounts.Where(acc => acc.AccountID == id);
            if (accounts.Count() == 0) {
                return null;
            }
            return accounts.First();
        }

        public void UpdateAccount(Account account) {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[connName];
            using (SqlConnection conn = new SqlConnection(settings.ConnectionString)) {
                conn.Open();
                String updateQuery = String.Format("UPDATE Accounts SET Money = {0} WHERE AccountID = {1}", 
                    account.Money.ToString().Replace(',', '.'), account.AccountID);
                SqlDataAdapter da = new SqlDataAdapter(updateQuery, conn);
                DataRow row = dataSet.Tables["Accounts"].Rows.Find(new Object[] { account.AccountID });
                row["Money"] = account.Money;
                da.UpdateCommand = new SqlCommand(updateQuery, conn);
                da.Update(dataSet, "Accounts");
                FindAccountById(account.AccountID).Money = account.Money;
                conn.Dispose();
            } 
        }
    }
}