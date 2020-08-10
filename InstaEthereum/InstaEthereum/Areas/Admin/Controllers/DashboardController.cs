using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
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
            /*var role = _context.Roles.SingleOrDefault(u => u.Name == "User");
            var totalUsers = _context.Users.Where(u => u.Roles.Any(r => (r.RoleId == role.Id))).Count();*/

            var totalUsers = _context.AspNetUsers.Where(u => u.RoleId == 2).Count();
            ViewBag.TotalUsers = totalUsers;

            return View();
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