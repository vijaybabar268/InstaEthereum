using InstaEthereum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InstaEthereum.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoginController()
        {
            _context = new ApplicationDbContext();
        }

        // Admin Sign In
        public ActionResult SignIn()
        {
            Session.RemoveAll();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignInProcess(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("SignIn", model);
            }

            var user = _context.AspNetUsers.FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid Credentials, Try Again.");
                return View("SignIn", model);
            }

            var adminRole = _context.AspNetUserRoles.FirstOrDefault(r => r.UserId == user.Id && r.RoleId == 1);

            if (adminRole == null)
            {
                ModelState.AddModelError("", "Not authorized on Admin.");
                return View("SignIn", model);
            }

            Session["AdminId"] = user.Id;
            Session["AdminEmail"] = user.Email;            

            return RedirectToAction("Index", "Dashboard");
        }


        // Admin Sign Up
        public ActionResult SignUp()
        {            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUpProcess(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = new AspNetUser
                    {
                        Email = model.Email,
                        Password = model.Password
                    };

                    _context.AspNetUsers.Add(user);
                    _context.SaveChanges();

                    var userRoleAssigned = new AspNetUserRole() { UserId = user.Id, RoleId = 1 };

                    _context.AspNetUserRoles.Add(userRoleAssigned);
                    _context.SaveChanges();

                    ModelState.AddModelError("", "Successfully Created.");
                    return View("SignUp", model);                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }            
                        
            return View("SignUp", model);
        }

    }
}