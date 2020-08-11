using InstaEthereum.Models;
using InstaEthereum.ViewModels;
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

        public BuyEthereumController()
        {
            _context = new ApplicationDbContext();
        }

        public ActionResult StepStart()
        {
            Session.Remove("UserEmail");
            Session.Remove("EthereumQty");

            var viewModel = new PageStartViewModel()
            {
                MinEthBuy = _context.EthPurchaseRange.FirstOrDefault().Min,
                MaxEthBuy = _context.EthPurchaseRange.FirstOrDefault().Max,
                EthPrice = _context.SetPrices.FirstOrDefault(x => x.Status == true).Price
            };
                        
            return View("StepStart", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StepStartProcess(PageStartViewModel model)
        {
            var viewModel = new PageStartViewModel()
            {
                MinEthBuy = _context.EthPurchaseRange.FirstOrDefault().Min,
                MaxEthBuy = _context.EthPurchaseRange.FirstOrDefault().Max,
                EthPrice = _context.SetPrices.FirstOrDefault(x => x.Status == true).Price
            };

            if (!ModelState.IsValid)
            {
                return View("StepStart", viewModel);
            }

            Session["UserEmail"] = model.Email;

            var user = _context.AspNetUsers.FirstOrDefault(u => u.Email.ToLower().Trim() == model.Email.ToLower().Trim() && u.RoleId == 2 && u.RoleId != 1);

            if (user == null)
            {
                return RedirectToAction("StepThree");
            }
            
            return RedirectToAction("StepOne");
        }

        public ActionResult StepOne()
        {
            var viewModel = new StepOneViewModel()
            {
                SetPrice = _context.SetPrices.FirstOrDefault(x => x.Status == true).Price,
                MinEthBuy = 1,
                MaxEthBuy = 20
            };
                        
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StepOneProcess(StepOneViewModel model)
        {
            var viewModel = new StepOneViewModel()
            {
                SetPrice = _context.SetPrices.FirstOrDefault(x => x.Status == true).Price,
                MinEthBuy = 1,
                MaxEthBuy = 20
            };

            if (!ModelState.IsValid)
                return View("StepOne", viewModel);                
                        
            if (model.EthereumQty < 1 || model.EthereumQty > 21)
            {
                ModelState.AddModelError("", "Minimum purchase of 1 ETH and maximum 20 ETH");
                return View("StepOne", viewModel);
            }

            Session["EthereumQty"] = model.EthereumQty;
                        
            return RedirectToAction("StepTwo");
        }

        public ActionResult StepTwo()
        {
            var viewModel = new StepTwoViewModel()
            {
                SetPrice = _context.SetPrices.FirstOrDefault(x => x.Status == true).Price,
                MinEthBuy = 1,
                MaxEthBuy = 20
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StepTwoProcess(StepTwoViewModel model)
        {
            var viewModel = new StepTwoViewModel()
            {
                SetPrice = _context.SetPrices.FirstOrDefault(x => x.Status == true).Price,
                MinEthBuy = 1,
                MaxEthBuy = 20                
            };

            if (!ModelState.IsValid)
                return View("StepTwo", viewModel);

            var email = Session["UserEmail"].ToString().ToLower().Trim();
            var userInDb = _context.AspNetUsers.FirstOrDefault(u => u.RoleId == 2 && u.Email.ToLower().Trim() == email);

            if (userInDb == null)
            {
                ModelState.AddModelError("", "Something went wrong.");
                return View("StepTwo", viewModel);
            }

            userInDb.WalletAddress = model.WalletAddress;

            _context.SaveChanges();        

            return RedirectToAction("StepFour");
        }

        #region User Registration
        public ActionResult StepThree()
        {
            var model = new NewRegisterViewModel
            {
                Email = Session["UserEmail"].ToString()
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