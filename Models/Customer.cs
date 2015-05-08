using System;
using System.ComponentModel.DataAnnotations;

namespace WildWestBankApp.Models {
    public class Customer {
        [Required]
        public Int32 ID { get; set; }
        [Required]
        public String Name { get; set; }
        [Required]
        public String Address { get; set; }
        [Required]
        public DateTime BirthDay { get; set; }
    }
}