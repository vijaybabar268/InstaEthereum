using InstaEthereum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InstaEthereum.Areas.Admin.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsersController()
        {
            _context = new ApplicationDbContext();
        }

        public ActionResult Index()
        {
            var users = _context.AspNetUsers.ToList();

            return View(users);
        }
    }
}