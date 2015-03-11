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
    }
}
