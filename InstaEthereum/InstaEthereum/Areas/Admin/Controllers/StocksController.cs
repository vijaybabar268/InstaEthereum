using InstaEthereum.Areas.Admin.ViewModels;
using InstaEthereum.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InstaEthereum.Areas.Admin.Controllers
{
    public class StocksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StocksController()
        {
            _context = new ApplicationDbContext();
        }

        public ActionResult Index()
        {
            var viewModel = new StockViewModel()
            {
                Id = 1,
                EthMinPurchaseLimit = _context.EthPurchaseRange.FirstOrDefault().Min,
                EthMaxPurchaseLimit = _context.EthPurchaseRange.FirstOrDefault().Max,
                Stocks = _context.Stocks.ToList()
            };

            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SetRange(int id, decimal EthMinPurchaseLimit, decimal EthMaxPurchaseLimit)
        {
            var ethMinMaxPurchaaseRangeInDb = _context.EthPurchaseRange.Find(id);

            ethMinMaxPurchaaseRangeInDb.Min = EthMinPurchaseLimit;
            ethMinMaxPurchaaseRangeInDb.Max = EthMaxPurchaseLimit;

            _context.Entry(ethMinMaxPurchaaseRangeInDb).State = EntityState.Modified;
            _context.SaveChanges();

            TempData["Message"] = "Successfully Updated.";

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveStock(StockViewModel viewModel)
        {            
            var stock = new Stock()
            {
                Datetime = DateTime.Now,
                PurchaseQty = viewModel.EthereumQty
            };

            _context.Stocks.Add(stock);
            _context.SaveChanges();            

            TempData["Message"] = "Successfully Updated.";

            return RedirectToAction("Index");
        }
    }
}