using System;
using System.ComponentModel.DataAnnotations;

namespace WildWestBankApp.Models {
    public class Transaction {
        public Int32 ID { get; set; } 
        public Int32 FromAccountID { get; set; }
        public Int32 ToAccountID { get; set; }
        public Int32 TypeID { get; set; }
        public Decimal Amount { get; set; }
        public DateTime DateTime { get; set; }
    }
}