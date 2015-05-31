using System;
using System.Linq;
using System.Web.Mvc;
using WildWestBankApp.Models;

namespace WildWestBankApp.Controllers {
    public class MainController : Controller {
        private DataRepository repository;
        
        public MainController() {
            repository = new DataRepository(); 
        }
        
        public ActionResult Customers() {
            return View(repository.Customers);
        }
        
        [HttpPost]
        public ActionResult Customers(Customer cust) {
            repository.AddCustomer(cust);        
            return View(repository.Customers);
        }

        public ActionResult Accounts(Int32 id) {
            AccountsViewModel viewModel = new AccountsViewModel(id, repository);
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Accounts() {
            Int32 customerId = Convert.ToInt32(Request.Form["ID"]);
            Int32 newAccountId = repository.Accounts.Max(a => a.AccountID) + 1;
            repository.AddAccount(new Account() { AccountID = newAccountId, CustomerID = customerId, Money = 0.0m });
            return RedirectToAction("Accounts", new { id = customerId });
        }
    }
}
