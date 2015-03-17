using System;
using System.Web.Mvc;
using WildWestBankApp.Models;

namespace WildWestBankApp.Controllers {
    public class MainController : Controller {
        private DataRepository repository;
        public MainController() {
            repository = new DataRepository();
            repository.LoadCustomersFromDataFile();
        }
        public ActionResult Customers() {
            return View(repository.Customers);
        }
        [HttpPost]
        public ActionResult Customers(Customer cust) {
            String name = Request.Form["Name"];
            String address = Request.Form["Address"];
            String date = Request.Form["BirthDay"];
            Int32 id = repository.Customers[repository.Customers.Count - 1].ID + 1;
            repository.Customers.Add(new Customer() { ID = id, Name = name, Address = address });
            return View(repository.Customers);
        }
    }
}
