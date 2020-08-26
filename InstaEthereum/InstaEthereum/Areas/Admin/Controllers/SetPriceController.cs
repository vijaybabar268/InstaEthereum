using InstaEthereum.Areas.Admin.ViewModels;
using InstaEthereum.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

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
                
        public ActionResult GetPrice(int id)
        {
            switch (id)
            {
                case 1:
                    SetBinancePrice();
                    break;
                case 2:
                    SetWazirXPrice();
                    break;
                default:
                    break;
            }
            
            TempData["Message"] = "ETH Price Set.";

            return RedirectToAction("Index");
        }

        private void SetBinancePrice()
        {
            string usdEthPrice;
            string OneUsdInrRate;

            using (var webClient = new WebClient())
            {
                string rawJson = webClient.DownloadString("https://api.binance.com/api/v3/ticker/price?symbol=ETHUSDT");
                var jsonData = (JObject)JsonConvert.DeserializeObject(rawJson);
                usdEthPrice = jsonData["price"].Value<string>();
            }

            using (var webClient = new WebClient())
            {
                string rawJson = webClient.DownloadString("https://v6.exchangerate-api.com/v6/ef2a90f42c9893f7864d5177/latest/USD");
                var jsonData = (JObject)JsonConvert.DeserializeObject(rawJson);
                var conversionRates = jsonData["conversion_rates"];
                OneUsdInrRate = conversionRates["INR"].Value<string>();
            }

            decimal binanceETHPrice = decimal.Parse(usdEthPrice) * decimal.Parse(OneUsdInrRate);

            var priceInDb = _context.SetPrices.Find(1);

            priceInDb.OriginalPrice = binanceETHPrice;

            /*Update add percentage to price*/
            var OriginalPrice = priceInDb.OriginalPrice;
            var AddPercentAmt = (OriginalPrice * priceInDb.AddPercent) / 100;
            var Price = OriginalPrice + AddPercentAmt;                        
            priceInDb.Price = Price;

            _context.SaveChanges();
        }

        private void SetWazirXPrice()
        {
            string wazirxETHPrice;

            using (var webClient = new WebClient())
            {
                string rawJson = webClient.DownloadString("https://api.wazirx.com/api/v2/tickers");
                var jsonData = (JObject)JsonConvert.DeserializeObject(rawJson);
                var ethinr = jsonData["ethinr"];
                wazirxETHPrice = ethinr["buy"].ToString();
            }

            var priceInDb = _context.SetPrices.Find(2);
            priceInDb.OriginalPrice = decimal.Parse(wazirxETHPrice);

            /*Update add percentage to price*/
            var OriginalPrice = priceInDb.OriginalPrice;
            var AddPercentAmt = (OriginalPrice * priceInDb.AddPercent) / 100;
            var Price = OriginalPrice + AddPercentAmt;
            priceInDb.Price = Price;

            _context.SaveChanges();
        }

    }
}