using System; 
using System.Web.Mvc;
using WildWestBankApp.Models;

namespace WildWestBankApp.Controllers {
    public class TransactionController : Controller {
        private TransactionMaker transactionMaker;
        public TransactionController() {
            transactionMaker = new TransactionMaker();
        }

        public ActionResult Index() {
            return View(transactionMaker.Transactions);
        }

        public ActionResult Transfer() {
            return View(new Transaction { ID = transactionMaker.GetNewId() });
        }

        [HttpPost]
        public ActionResult Transfer(Transaction transaction) {
            if(!ModelState.IsValid) {
                return View(transaction);
            }
            MakeTransfer(transaction);
            return View(new Transaction { ID = transactionMaker.GetNewId() });
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

        private void MakeTransfer(Transaction transaction) {
            transaction.DateTime = DateTime.Now;
            Boolean result = false;
            try {
                result = transactionMaker.TransferMoneyBetweenAccounts(transaction, ModelState);
            } catch (OverflowException) {
                ViewBag.TransactionResult = "Overflow error while the transaction.";
            } catch (Exception) {
                throw;
            }
            if (result) {
                ViewBag.TransactionResult = "Last transaction was successfully committed.";
            }
        }
    }
}
