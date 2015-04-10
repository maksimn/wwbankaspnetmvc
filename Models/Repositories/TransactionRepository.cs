using System;
using System.IO;
using System.Collections.Generic; 

namespace WildWestBankApp.Models {
    public class TransactionRepository {
        private String transactionFilePath = AppDomain.CurrentDomain.BaseDirectory + @"\data\Transaction.csv";
        private String transactionTypeFilePath = AppDomain.CurrentDomain.BaseDirectory + @"\data\TransactionType.csv";
        private List<TransactionType> transactionTypes;
        private List<Transaction> transactionList;

        public void AddTransaction(Transaction transaction) {
            transactionList.Add(transaction);
            SaveTransactionInFile(transaction);
        }

        private void SaveTransactionInFile(Transaction transaction) {
            File.AppendAllText(transactionFilePath, Environment.NewLine + ConvertToString(transaction));
        }

        private String ConvertToString(Transaction transaction) {
            return String.Format("{0};{1};{2};{3};{4};{5}", transaction.ID, transaction.FromAccountID, 
                transaction.ToAccountID, transaction.TypeID, transaction.Amount, transaction.DateTime);
        }

        public void Load() {
            LoadTransactionTypesFromFile();
            LoadTransactionListFromFile();
        }

        public IEnumerable<Transaction> List {
            get {
                return transactionList;
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
            }
        }
    }
}