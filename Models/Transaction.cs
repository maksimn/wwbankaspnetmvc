using System;
using System.ComponentModel.DataAnnotations;

namespace WildWestBankApp.Models {
    public class Transaction {
        public Int32 ID { get; set; }
        [Required]
        public Int32 FromAccountID { get; set; }
        [Required]
        public Int32 ToAccountID { get; set; }
        public TransactionType Type { get; set; }
        [Required]
        [PositiveNumber(ErrorMessage="The Value must be positive")]
        public Decimal Amount { get; set; }
        public DateTime DateTime { get; set; }
    }

    public enum TransactionType { Transfer = 1, Add = 2, Withdraw = 3 }
}