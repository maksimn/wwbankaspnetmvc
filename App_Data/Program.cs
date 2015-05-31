using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;

public class Customer {
    public Int32 ID { get; set; }
    public String Name { get; set; }
    public String Address { get; set; }
    public DateTime BirthDay { get; set; }
}
public class Account { 
    public Int32 AccountID { get; set; } 
    public Int32 CustomerID { get; set; }
    public Decimal Money { get; set; }
}
public class Transaction {
    public Int32 ID { get; set; }
    public Int32 FromAccountID { get; set; }
    public Int32 ToAccountID { get; set; }
    public TransactionType Type { get; set; }
    public Decimal Amount { get; set; }
    public DateTime DateTime { get; set; }
}
public enum TransactionType { Transfer = 1, Add = 2, Withdraw = 3 }

public class Program {
    private static readonly String csvFolder = 
        @"D:\PrevRabStol\MyProgramming\ASP.NET MVC\WildWestBankApp\for_source_control\data\";
    private static readonly String customerCsvFileName = "Customer.csv";
    private static readonly String accountCsvFileName = "Account.csv", transactionCsvFileName = "Transaction.csv";


    private static List<Customer> customers;
    private static List<Account> accounts;
    private static List<Transaction> transactionList;

    private static String ConvertToString(Customer customer) {
        String temp = "0" + customer.BirthDay.Day;
        String strDay = temp.Substring(temp.Length - 2);
        temp = "0" + customer.BirthDay.Month;
        String strMonth = temp.Substring(temp.Length - 2);
        return String.Format("{0};{1};{2};{3}{4}{5}", customer.ID, customer.Name, customer.Address,
            customer.BirthDay.Year, strMonth, strDay);
    }

    private static String ConvertToString(Account account) {
        return String.Format("{0};{1};{2}", account.AccountID, account.CustomerID, account.Money);
    }

    private static String DateTimeToNeutralString(DateTime datetime) {
        String temp = "0" + datetime.Day;
        String strDay = temp.Substring(temp.Length - 2);
        temp = "0" + datetime.Month;
        String strMonth = temp.Substring(temp.Length - 2);
        return String.Format("{0}{1}{2}", datetime.Year, strMonth, strDay);
    }

    public static void LoadCustomersFromDataFile() {
        customers = new List<Customer>();
        using (StreamReader file = new StreamReader(csvFolder + customerCsvFileName)) {
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

    private static void InsertCustomersIntoDB() {               
        String connName = "WWBankConnectionString";
        ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[connName];
        DbProviderFactory factory = DbProviderFactories.GetFactory(settings.ProviderName);
        using (DbConnection conn = factory.CreateConnection()) {
            conn.ConnectionString = settings.ConnectionString;
            conn.Open();
            foreach (var c in customers) {
                String insertQuery =
                    String.Format("INSERT INTO [dbo].[Customers] (ID, Name, Address, BirthDay) VALUES ({0}, '{1}', '{2}', '{3}')",
                        c.ID, c.Name.Replace("'", "''"), c.Address.Replace("'", "''"), DateTimeToNeutralString(c.BirthDay));
                SqlCommand cmd = new SqlCommand(insertQuery, (SqlConnection)conn);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
            }
            conn.Dispose();
        } 
    }

    private static void LoadAccountsFromDataFile() {
        accounts = new List<Account>();
        using (StreamReader file = new StreamReader(csvFolder + accountCsvFileName)) {
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

    private static void InsertAccountsIntoDB() {
        String connName = "WWBankConnectionString";
        ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[connName];
        DbProviderFactory factory = DbProviderFactories.GetFactory(settings.ProviderName);
        using (DbConnection conn = factory.CreateConnection()) {
            conn.ConnectionString = settings.ConnectionString;
            conn.Open();
            foreach (var a in accounts) {
                String insertQuery =
                    String.Format("INSERT INTO [dbo].[Accounts] (AccountID, CustomerID, Money) VALUES ({0}, '{1}', '{2}')",
                        a.AccountID, a.CustomerID, a.Money.ToString().Replace(',', '.'));
                SqlCommand cmd = new SqlCommand(insertQuery, (SqlConnection)conn);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
            }
            conn.Dispose();
        }
    }

    private static void LoadTransactionListFromFile() {
        transactionList = new List<Transaction>();
        using (StreamReader file = new StreamReader(csvFolder + transactionCsvFileName)) {
            String line;
            String[] rowField;
            file.ReadLine();
            while ((line = file.ReadLine()) != null) {
                rowField = line.Split(new Char[] { ';' });
                Transaction transaction = new Transaction();
                transaction.ID = Convert.ToInt32(rowField[0]);
                transaction.FromAccountID = Convert.ToInt32(rowField[1]);
                transaction.ToAccountID = Convert.ToInt32(rowField[2]);
                transaction.Type = (TransactionType)Convert.ToInt32(rowField[3]);
                transaction.Amount = Convert.ToDecimal(rowField[4]);
                transaction.DateTime = Convert.ToDateTime(rowField[5]);
                transactionList.Add(transaction);
            }
            file.Dispose();
        }
    }
    private static void InsertTransactionsIntoDB() {
        String connName = "WWBankConnectionString";
        ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[connName];
        DbProviderFactory factory = DbProviderFactories.GetFactory(settings.ProviderName);
        using (DbConnection conn = factory.CreateConnection()) {
            conn.ConnectionString = settings.ConnectionString;
            conn.Open();
            foreach (var t in transactionList) {
                String insertQuery =
                    String.Format("INSERT INTO [dbo].[Transactions] (ID, AccountFromId, AccountToId, Type, Amount, DateTime) VALUES ({0}, {1}, {2}, {3}, {4}, '{5}')",
                        t.ID, t.FromAccountID, t.ToAccountID, (int)(t.Type), t.Amount.ToString().Replace(',', '.'), t.DateTime.ToString("yyyy-MM-dd HH:mm:ss"));
                SqlCommand cmd = new SqlCommand(insertQuery, (SqlConnection)conn);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
            }
            conn.Dispose();
        }
    }
    static private String ConvertToString(Transaction transaction) {
        return String.Format("{0};{1};{2};{3};{4};{5}", transaction.ID, transaction.FromAccountID,
            transaction.ToAccountID, (Int32)transaction.Type, transaction.Amount, transaction.DateTime);
    }

    public static void Main() {
        //LoadCustomersFromDataFile(); // Список клиентов из файла загружается успешно. 
        //InsertCustomersIntoDB();
        //LoadAccountsFromDataFile(); // SUCCESS
        //InsertAccountsIntoDB();
        //LoadTransactionListFromFile(); 
        //InsertTransactionsIntoDB();
    }
}