using System; 

namespace WildWestBankApp.Models {
    public class Customer {
        public Int32 ID { get; set; }
        public String Name { get; set; }
        public String Address { get; set; }
        public DateTime BirthDay { get; set; }
    }
}