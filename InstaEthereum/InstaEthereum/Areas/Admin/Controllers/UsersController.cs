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
            //var users = _context.Users
            //            .Where(r => r.Roles.Any(u => u.RoleId == "585e06f7-bbdd-4f85-b6ae-d0d0f50f21b4"))
            //            .ToList();

            var users = _context.AspNetUsers.Where(u => u.RoleId == 2).ToList();

            return View(users);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id)
        {
            var adminInDb = _context.Users.Find(id);

            _context.Users.Remove(adminInDb);
            _context.SaveChanges();

            return RedirectToAction("Index", "Users");
        }
    }
}