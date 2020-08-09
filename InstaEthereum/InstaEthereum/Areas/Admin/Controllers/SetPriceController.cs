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
    public class SetPriceController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SetPriceController()
        {
            _context = new ApplicationDbContext();
        }

        public ActionResult Index()
        {
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
            }

            var viewModel = new DisplaySetPriceViewModel()
            {
                SetPrices = _context.SetPrices.ToList(),
                BinanceETHPrice = _context.SetPrices.FirstOrDefault(p => p.Id == 1).Price,
                WazirXETHPrice = _context.SetPrices.FirstOrDefault(p => p.Id == 2).Price                
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SetPrice(string Id, decimal AddPercent)
        {
            if (!string.IsNullOrWhiteSpace(Id))
            {
                var priceId = int.Parse(Id);
                var priceInDb = _context.SetPrices.Find(priceId);

                if (priceInDb != null)
                {
                    var OriginalPrice = priceInDb.OriginalPrice;
                    var AddPercentAmt = (OriginalPrice * AddPercent) / 100;
                    var Price = OriginalPrice + AddPercentAmt;

                    priceInDb.AddPercent = AddPercent;
                    priceInDb.Price = Price;

                    _context.Entry(priceInDb).State = EntityState.Modified;
                    _context.SaveChanges();

                    TempData["Message"] = "Successfully Updated.";
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult ToggleDefault(int id)
        {            
            var prices = _context.SetPrices.ToList();

            for (int i = 0; i < prices.Count; i++)
            {
                prices[i].Status = false;
            }
            _context.SaveChanges();

            var price = _context.SetPrices.Find(id);
            price.Status = true;
            _context.SaveChanges();

            TempData["Message"] = "Successfully Updated.";
            return RedirectToAction("Index");
        }

    }
}