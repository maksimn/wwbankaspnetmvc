using System;

namespace WildWestBankApp.Models {
    public class OperationViewModel {
        private String[] operations;
        public OperationViewModel(DataRepository repository) {
            Int32 sz = repository.TransactionTypes.Count;
            operations = new String[sz];
            for (Int32 i = 0; i < repository.TransactionTypes.Count; i++) {
                operations[i] = repository.TransactionTypes[i].Name;
            }
        }
        public String[] OperationTypes {
            get {
                return operations;
            }
        }
    }
}