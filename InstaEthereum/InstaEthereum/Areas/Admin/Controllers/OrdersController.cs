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
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController()
        {
            _context = new ApplicationDbContext();
        }

        public ActionResult Index()
        {
            var viewModel = new DisplayOrderViewModel()
            {
                Orders = _context.Orders
            };

            return View(viewModel);
        }
    }
}