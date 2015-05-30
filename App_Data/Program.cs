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

public class Program {
    private static readonly String csvFolder = 
        @"D:\PrevRabStol\MyProgramming\ASP.NET MVC\WildWestBankApp\for_source_control\data\";
    private static readonly String customerCsvFileName = "Customer.csv";

    private static List<Customer> customers;

    private static String ConvertToString(Customer customer) {
        String temp = "0" + customer.BirthDay.Day;
        String strDay = temp.Substring(temp.Length - 2);
        temp = "0" + customer.BirthDay.Month;
        String strMonth = temp.Substring(temp.Length - 2);
        return String.Format("{0};{1};{2};{3}{4}{5}", customer.ID, customer.Name, customer.Address,
            customer.BirthDay.Year, strMonth, strDay);
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

    public static void Main() {
        LoadCustomersFromDataFile(); // Список клиентов из файла загружается успешно.        
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
}