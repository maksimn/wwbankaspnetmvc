using System;
using System.ComponentModel.DataAnnotations;

namespace WildWestBankApp.Models {
    public class Account {
        [Required]
        public Int32 AccountID { get; set; }
        [Required]
        public Int32 CustomerID { get; set; }
        public Decimal Money { get; set; }
    }
}