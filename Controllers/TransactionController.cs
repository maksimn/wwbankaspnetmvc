using System; 
using System.Web.Mvc;
using System.Linq;
using WildWestBankApp.Models;

namespace WildWestBankApp.Controllers {
    public class TransactionController : Controller {
        public TransactionController() {
            repository = new TransactionRepository();
            repository.Load();
        }

        public ActionResult Index() {
            return View();
        }

        public ActionResult Transfer() {
            var transaction = new Transaction();
            if (repository.List.Count() == 0) {
                transaction.ID = 1;
            } else {
                transaction.ID = repository.List.Max(tr => tr.ID) + 1;
            }            
            return View(transaction);
        }

        [HttpPost]
        public ActionResult Transfer(Transaction transaction) {
            transaction.DateTime = DateTime.Now;
            return RedirectToAction("Index");
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
