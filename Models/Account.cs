using System; 

namespace WildWestBankApp.Models {
    public class Account {
        public Int32 AccountID { get; set; }
        public Int32 CustomerID { get; set; }
        public Decimal Money { get; set; }
    }
}