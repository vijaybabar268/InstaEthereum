using InstaEthereum.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace InstaEthereum.Controllers
{
    public class BuyEthereumController : Controller
    {
        private readonly ApplicationDbContext _context;
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public BuyEthereumController()
        {
            _context = new ApplicationDbContext();
        }

        public BuyEthereumController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ActionResult StepStart()
        {
            Session.Remove("UserEmail");
            Session.Remove("EthereumQty");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StepStartProcess(UserLoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("StepStart", model);
            }

            var user = _context.AspNetUsers.FirstOrDefault(u => u.Email.ToLower().Trim() == model.Email.ToLower().Trim() && u.RoleId == 2);

            if (user == null)
            {
                return RedirectToAction("StepThree", new { Email = model.Email });
            }
                        
            Session["UserEmail"] = user.Email;
            return RedirectToAction("StepOne");
        }

        public ActionResult StepOne()
        {                        
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StepOneProcess(string ethereum_qty)
        {
            if (!string.IsNullOrWhiteSpace(ethereum_qty))
            {
                Session["EthereumQty"] = ethereum_qty;
            }

            return RedirectToAction("StepTwo");
        }

        public ActionResult StepTwo()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StepTwoProcess(string wallet_address)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Something went wrong.");
                return View("StepTwo");
            }

            var email = Session["UserEmail"].ToString().ToLower().Trim();
            var userInDb = _context.AspNetUsers.FirstOrDefault(u => u.RoleId == 2 && u.Email.ToLower().Trim() == email);

            if (userInDb == null)
            {
                return HttpNotFound();
            }

            userInDb.WalletAddress = wallet_address;
            _context.SaveChanges();        

            return RedirectToAction("StepFour");
        }

        #region User Registration
        public ActionResult StepThree(string email)
        {
            var model = new NewRegisterViewModel
            {
                Email = email
            };

            return View("StepThree", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StepThreeProcess(NewRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new AspNetUser
                {
                    UserName = model.UserName,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email,
                    RoleId = 2
                };

                _context.AspNetUsers.Add(user);
                _context.SaveChanges();

                Session["UserEmail"] = user.Email;
                return RedirectToAction("StepOne");
            }

            return View("StepThree", model);
        }
        #endregion

        public ActionResult StepFour()
        {
            return View();
        }

        public ActionResult StepFive()
        {
            return View();
        }

        public ActionResult Success()
        {
            return View();
        }

        public ActionResult Fail()
        {
            return View();
        }

        public ActionResult SearchTransaction()
        {
            return View();
        }

        public ActionResult Support()
        {
            return View();
        }
    }
}