using System;
using System.IO;
using System.Collections.Generic; 

namespace WildWestBankApp.Models {
    public class TransactionRepository {
        private String transactionTypeFilePath = AppDomain.CurrentDomain.BaseDirectory + @"\data\TransactionType.csv";
        private List<TransactionType> transactionTypes;
        private List<Transaction> transactionList;

        public void Load() {
            LoadTransactionTypesFromFile();
            LoadTransactionListFromFile();
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
            using (StreamReader file = new StreamReader(transactionTypeFilePath)) {
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