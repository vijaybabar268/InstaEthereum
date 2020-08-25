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
            var users = _context.AspNetUsers.Where(u => u.RoleId == 2).ToList();
                        
            return View(users.OrderByDescending(x => x.Id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var adminInDb = _context.AspNetUsers.Find(id);

            _context.AspNetUsers.Remove(adminInDb);
            _context.SaveChanges();

            return RedirectToAction("Index", "Users");
        }                
    }
}