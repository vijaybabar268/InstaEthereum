using InstaEthereum.Models;
using Microsoft.AspNet.Identity;
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

            var user = _context.AspNetUsers.FirstOrDefault(u => u.Email.ToLower().Trim() == model.Email.ToLower().Trim());
                        
            if (user == null)
            {
                //ModelState.AddModelError("", "Email ID is not registered.");
                //return View("StepStart", model);

                return RedirectToAction("StepThree", new { Email = model.Email });
            }

            var userRole = _context.AspNetUserRoles.FirstOrDefault(r => r.UserId == user.Id && r.RoleId == 2);

            if (userRole == null)
            {
                ModelState.AddModelError("", "Invalid login attempts.");
                return View("StepStart", model);
            }

            return RedirectToAction("StepOne");                      
        }

        public ActionResult StepOne()
        {
            Session.Remove("EthereumQty");
            Session.Remove("WalletAddress");

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
            if (!string.IsNullOrWhiteSpace(wallet_address))
            {
                Session["WalletAddress"] = wallet_address;
            }

            //return RedirectToAction("StepThree");
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
                try
                {
                    var user = new AspNetUser
                    {
                        UserName = model.UserName,
                        PhoneNumber = model.PhoneNumber,
                        Email = model.Email
                    };

                    _context.AspNetUsers.Add(user);
                    _context.SaveChanges();

                    var userRoleAssigned = new AspNetUserRole() {  RoleId = 2, UserId = user.Id };

                    _context.AspNetUserRoles.Add(userRoleAssigned);
                    _context.SaveChanges();


                    //return RedirectToAction("StepStart");
                    return RedirectToAction("StepOne");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }                
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