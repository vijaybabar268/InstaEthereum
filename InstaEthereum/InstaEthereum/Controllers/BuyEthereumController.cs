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

            var user = _context.Users
                        .FirstOrDefault(r => r.Email.ToLower().Trim() == model.Email.ToLower().Trim() && r.Roles.Any(u => u.RoleId == "585e06f7-bbdd-4f85-b6ae-d0d0f50f21b4"));

            if (user == null)
            {
                return RedirectToAction("StepThree", new { Email = model.Email });
            }

            await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

            return RedirectToAction("StepOne");
        }

        public ActionResult StepOne()
        {
            Session.Remove("EthereumQty");
            
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
        public async Task<ActionResult> StepTwoProcess(string wallet_address)
        {
            //if (!ModelState.IsValid)
            //{
            //    ModelState.AddModelError("", "Something went wrong.");
            //    return View("StepTwo");
            //}
            
            //var userId = User.Identity.GetUserId();
            //var userInDb = _context.Users.SingleOrDefault(x => x.Id == userId);

            //if (userInDb == null)
            //{
            //    return HttpNotFound();
            //}

            //userInDb.WalletAddress = wallet_address;
            
            //var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            //var manager = new UserManager<ApplicationUser>(store);
            //await manager.UpdateAsync(userInDb);
            //_context.SaveChanges();
                        
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

                var result = await UserManager.CreateAsync(user, "Pass@12345");

                if (result.Succeeded)
                {                    
                    await UserManager.AddToRoleAsync(user.Id, "User");

                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    return RedirectToAction("StepOne");                    
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