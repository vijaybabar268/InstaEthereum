using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using InstaEthereum.Areas.Admin.ViewModels;
using InstaEthereum.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace InstaEthereum.Areas.Admin.Controllers
{   
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController()
        {
            _context = new ApplicationDbContext();
        }

        public ActionResult Index()
        {   
            var viewModel = new DashboardViewModel()
            {
                TotalUsers = _context.AspNetUsers.Where(u => u.RoleId == 2).Count(),
                TotalOrders = _context.Orders.Count(),
                pendingOrders = _context.Orders.Where(s => s.Status == 0).Count(),
                CompleteOrders = _context.Orders.Where(s => s.Status == 1).Count(),
                InCompleteOrders = _context.Orders.Where(s => s.Status == 2).Count()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            var baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority;
            var url = baseUrl + "/Account/Login";
            return Redirect(url);
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
    }
}