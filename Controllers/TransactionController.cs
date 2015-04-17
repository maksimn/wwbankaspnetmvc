using System; 
using System.Web.Mvc;
using System.Linq;
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
            transaction.ID = GetNewTransactionId();
            return View(transaction);
        }

        [HttpPost]
        public ActionResult Transfer(Transaction transaction) {
            transaction.DateTime = DateTime.Now;
            repository.AddTransaction(transaction);
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

        private Int32 GetNewTransactionId() {
            if (repository.TransactionList.Count() == 0) {
                return 1;
            } else {
                return repository.TransactionList.Max(tr => tr.ID) + 1;
            }   
        }

        private DataRepository repository;
    }
}
