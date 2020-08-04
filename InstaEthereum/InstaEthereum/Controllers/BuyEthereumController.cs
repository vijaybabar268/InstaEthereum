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
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public BuyEthereumController()
        {

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
        public async Task<ActionResult> StepStartProcess(UserLoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("StepStart", model);
            }

            var user = UserManager.FindByEmail(model.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "Email ID is not registered.");
                return View("StepStart", model);
            }

            var result = await SignInManager.PasswordSignInAsync(user.UserName, "Pass123!@#", true, shouldLockout: false);

            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToAction("StepOne");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View("StepStart", model);
            }
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
        public ActionResult StepThree()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> StepThreeProcess(NewRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.UserName,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email                    
                };

                try
                {
                    var result = await UserManager.CreateAsync(user, "Pass123!@#");

                    if (result.Succeeded)
                    {
                        await UserManager.AddToRoleAsync(user.Id, "User");

                        return RedirectToAction("StepStart");
                    }
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