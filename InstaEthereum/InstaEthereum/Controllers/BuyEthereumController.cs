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
            Session.Remove("WalletAddress");

            var viewModel = new PageStartViewModel()
            {
                MinEthBuy = _context.EthPurchaseRange.FirstOrDefault().Min,
                MaxEthBuy = _context.EthPurchaseRange.FirstOrDefault().Max,
                EthPrice = _context.SetPrices.FirstOrDefault(x => x.Status == true).Price,                
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
                MinEthBuy = _context.EthPurchaseRange.FirstOrDefault().Min,
                MaxEthBuy = _context.EthPurchaseRange.FirstOrDefault().Max
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
                MinEthBuy = _context.EthPurchaseRange.FirstOrDefault().Min,
                MaxEthBuy = _context.EthPurchaseRange.FirstOrDefault().Max
            };

            if (!ModelState.IsValid)
                return View("StepOne", viewModel);

            if (model.EthereumQty >= viewModel.MinEthBuy && model.EthereumQty <= viewModel.MaxEthBuy)
            {
                Session["EthereumQty"] = model.EthereumQty;

                return RedirectToAction("StepTwo");
            }
            else
            {
                ModelState.AddModelError("", string.Format("Minimum purchase of {0} ETH and maximum {1} ETH", viewModel.MinEthBuy, viewModel.MaxEthBuy));

                return View("StepOne", viewModel);
            }

           
        }

        public ActionResult StepTwo()
        {
            var viewModel = new StepTwoViewModel()
            {
                SetPrice = _context.SetPrices.FirstOrDefault(x => x.Status == true).Price,
                MinEthBuy = _context.EthPurchaseRange.FirstOrDefault().Min,
                MaxEthBuy = _context.EthPurchaseRange.FirstOrDefault().Max
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
                MinEthBuy = _context.EthPurchaseRange.FirstOrDefault().Min,
                MaxEthBuy = _context.EthPurchaseRange.FirstOrDefault().Max
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
            Session["WalletAddress"] = model.WalletAddress;

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
            var payableAmount = _context.SetPrices.FirstOrDefault(x => x.Status == true).Price * (decimal)Session["EthereumQty"];

            var viewModel = new OrderViewModel()
            {
                OneEthPrice = _context.SetPrices.FirstOrDefault(x => x.Status == true).Price,
                EthereumQty = (decimal)Session["EthereumQty"],
                WalletAddress = (string)Session["WalletAddress"],
                PayableAmount = payableAmount
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StepFourProcess(OrderViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("StepFour", model);
            }

            var email = Session["UserEmail"].ToString().ToLower().Trim();
            var userInDb = _context.AspNetUsers.FirstOrDefault(u => u.RoleId == 2 && u.Email.ToLower().Trim() == email);

            if (userInDb == null)
            {
                ModelState.AddModelError("", "Something went wrong.");
                return View("StepFour", model);
            }

            var order = new Order
            {
                EthereumQty = model.EthereumQty,
                OrderDateTime = DateTime.Now,
                PurchasePrice = model.PayableAmount,
                Status = 0,
                TransactionId = "",
                UserId = userInDb.Id,
            };

            _context.Orders.Add(order);
            _context.SaveChanges();

            return RedirectToAction("StepFive");
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