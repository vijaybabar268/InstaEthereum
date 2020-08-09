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
            /*var allUsers = _context.AspNetUsers.ToList();            
            var userIds = new List<int>();
            var users = new List<AspNetUser>();

            foreach (var item in allUsers)
            {
                var userId = _context.AspNetUserRoles.Where(r => r.UserId == item.Id && r.RoleId == 2).Select(u => u.UserId).ToList();
                userIds.AddRange(userId);
            }

            users = _context.AspNetUsers.Where(u => userIds.Contains(u.Id)).ToList();

            return View(users);*/


            var users = _context.Users
                        .Where(r => r.Roles.Any(u => u.RoleId == "faa1d0c0-48b0-44de-8e1b-42f4f85e3c6e"))
                        .ToList();

            return View(users);
        }
    }
}