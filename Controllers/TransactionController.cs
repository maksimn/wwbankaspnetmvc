using System; 
using System.Web.Mvc;
using WildWestBankApp.Models;

namespace WildWestBankApp.Controllers {
    public class TransactionController : Controller {
        public TransactionController() {
            repository = new TransactionRepository();
        }

        public ActionResult Index() {
            return View();
        }

        public ActionResult Transfer() {
            return View();
        }

        [HttpPost]
        public ActionResult Transfer(Transaction transaction) {
            return View();
        }

        public ActionResult Add() {
            return View();
        }

        [HttpPost]
        public ActionResult Add(Transaction transaction) {
            return View();
        }

        public ActionResult Withdraw() {
            return View();
        }

        [HttpPost]
        public ActionResult Withdraw(Transaction transaction) {
            return View();
        }

        private TransactionRepository repository;
    }
}
