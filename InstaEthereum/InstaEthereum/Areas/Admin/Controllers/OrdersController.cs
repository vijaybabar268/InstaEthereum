using InstaEthereum.Areas.Admin.ViewModels;
using InstaEthereum.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

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
                Orders = _context.Orders.OrderByDescending(x => x.OrderDateTime)
            };

            return View(viewModel);
        }

        public ActionResult Update(int id)
        {
            var viewModel = new UpdateOrderViewModel()
            {
                OrderId = id,
                TransactionStatus = Helper.OrderStatus,
                TransactionStatusId = _context.Orders.FirstOrDefault(x=>x.Id == id).Status
            };

            return View("Update", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateProcess(UpdateOrderViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.TransactionStatus = Helper.OrderStatus;

                return View("Update", viewModel);
            }

            var orderInDb = _context.Orders.Find(viewModel.OrderId);

            if (orderInDb == null)
            {
                ModelState.AddModelError("", "Something went wrong.");
                viewModel.TransactionStatus = Helper.OrderStatus;

                return View("Update", viewModel);
            }

            orderInDb.Status = viewModel.TransactionStatusId;
            orderInDb.ETHTXNNo = viewModel.EthTxnNo;

            _context.Entry(orderInDb).State = EntityState.Modified;
            _context.SaveChanges();

            return RedirectToAction("Index", "Orders");
        }
    }
}