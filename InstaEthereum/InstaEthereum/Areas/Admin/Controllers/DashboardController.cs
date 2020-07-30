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
    [Authorize(Roles = RoleName.Admin )]
    public class DashboardController : Controller
    {        
        public ActionResult Index()
        {
            return View();
        }

        // Admin Logout
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