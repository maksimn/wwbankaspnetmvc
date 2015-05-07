using System;
using System.ComponentModel.DataAnnotations;

namespace WildWestBankApp.Models {
    [AttributeUsage(AttributeTargets.Property)]
    public class PositiveDecimalAttribute : ValidationAttribute {
        public override Boolean IsValid(Object value) {
            if (value == null) {
                return false;
            }
            Decimal x = -1.00m;
            try {
                x = (Decimal)value;
            } catch {
                return false;
            }
            if (x <= 0) {
                return false;
            }
            return true;
        }
    }
}