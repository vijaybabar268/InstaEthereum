﻿using InstaEthereum.Areas.Admin.ViewModels;
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
                
        public ActionResult GetPrice()
        {            
            // WazirX ETH Price
            using (var webClient = new WebClient())
            {
                string rawJson = webClient.DownloadString("https://api.wazirx.com/api/v2/tickers");                
                var jsonData = JObject.Parse(rawJson);                
                var wazirxETHPrice = decimal.Parse(jsonData.Value<JObject>("ethinr").Properties()
                                    .FirstOrDefault(x => x.Name == "buy").Value.ToString());

                var priceInDb = _context.SetPrices.Find(2);
                priceInDb.OriginalPrice = wazirxETHPrice;
                
                _context.SaveChanges();
            }

            TempData["Message"] = "Wazirx ETH Price Set.";
            return RedirectToAction("Index");
        }

    }
}