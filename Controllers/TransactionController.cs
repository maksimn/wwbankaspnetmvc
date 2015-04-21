using System; 
using System.Web.Mvc;
using WildWestBankApp.Models;

namespace WildWestBankApp.Controllers {
    public class TransactionController : Controller {
        public TransactionController() {
            repository = new DataRepository();
            repository.Load();
        }

        public ActionResult Index() {
            return View(repository.TransactionList);
        }

        public ActionResult Transfer() {
            var transaction = new Transaction();
            transaction.ID = repository.GetNewTransactionId();
            return View(transaction);
        }

        [HttpPost]
        public ActionResult Transfer(Transaction transaction) {
            transaction.DateTime = DateTime.Now;
            repository.TransferMoneyBetweenAccounts(transaction);
            return View("index", repository.TransactionList);
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

        private DataRepository repository;
    }
}
