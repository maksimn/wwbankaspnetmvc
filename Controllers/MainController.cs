using System;
using System.Web.Mvc;

namespace WildWestBankApp.Controllers {
    public class MainController : Controller {
        public ActionResult Customers() {
            return View();
        }
    }
}
