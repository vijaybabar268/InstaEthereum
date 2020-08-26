using InstaEthereum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InstaEthereum.Areas.Admin.Controllers
{
    public class MarketTimesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MarketTimesController()
        {
            _context = new ApplicationDbContext();
        }

        public ActionResult Index()
        {
            var marketTimings = _context.MarketTimes.ToList();

            return View(marketTimings);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveMarketTime(string StartMarketTime, string CloseMarketTime, string Remarks)
        {
            var marketTimeInDb = _context.MarketTimes.FirstOrDefault();

            marketTimeInDb.StartMarketTime = DateTime.Parse(string.Format("{0:h:mm}", StartMarketTime));
            marketTimeInDb.CloseMarketTime = DateTime.Parse(string.Format("{0:h:mm}", CloseMarketTime));
            marketTimeInDb.Remarks = Remarks;

            _context.Entry(marketTimeInDb).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}