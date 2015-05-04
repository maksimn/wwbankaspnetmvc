using System;

namespace WildWestBankApp.Models {
    [Serializable]
    public sealed class AccountNotExistsException : Exception {
        private Int32 wrongId;
        public AccountNotExistsException(Int32 id) {
            wrongId = id;
        }
        public override String Message {
            get {
                return String.Format("Account with id = {0} does not exist", wrongId);
            }
        }
    }
}