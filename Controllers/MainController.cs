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
        public ActionResult Accounts(Account account) {
            repository.AddAccount(account);
            return RedirectToAction("Accounts", new { id = account.CustomerID });
        }
    }
}
