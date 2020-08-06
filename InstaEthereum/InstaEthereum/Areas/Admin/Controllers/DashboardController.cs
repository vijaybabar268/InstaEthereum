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
        public ActionResult Index()
        {
            return View();
        }

        // Admin Log Off
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            Session.RemoveAll();

            var baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority;
            var url = baseUrl + "/Admin/Login/SignIn";
            return Redirect(url);
        }               
    }
}