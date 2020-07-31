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
        private ApplicationUserManager _userManager;

        public BuyEthereumController()
        {

        }

        public BuyEthereumController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
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

            return RedirectToAction("StepThree");
        }

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
                    Email = model.Email,
                    WalletAddress = Session["WalletAddress"].ToString()
                };

                var result = await UserManager.CreateAsync(user, "Pass123!@#");

                if (result.Succeeded)
                {
                    // Assign role to register user
                    await UserManager.AddToRoleAsync(user.Id, "User");

                    return RedirectToAction("StepFour");
                }
            }

            return RedirectToAction("StepThree");
        }

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