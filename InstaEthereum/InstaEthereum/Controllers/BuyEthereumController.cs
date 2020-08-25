using InstaEthereum.Models;
using InstaEthereum.ViewModels;
using System;
using System.Linq;
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
                EthPrice = _context.SetPrices.FirstOrDefault(x => x.Status == true).Price,
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
                EthPrice = _context.SetPrices.FirstOrDefault(x => x.Status == true).Price,
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
                ModelState.AddModelError("EthereumQty", string.Format("Minimum purchase of {0} ETH and maximum {1} ETH", viewModel.MinEthBuy, viewModel.MaxEthBuy));

                return View("StepOne", viewModel);
            }

           
        }

        public ActionResult StepTwo()
        {
            var viewModel = new StepTwoViewModel()
            {
                EthPrice = _context.SetPrices.FirstOrDefault(x => x.Status == true).Price,
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
                EthPrice = _context.SetPrices.FirstOrDefault(x => x.Status == true).Price,
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
            var Segments = Request.UrlReferrer.Segments[2];

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
                        
            Random rnd = new Random();
            Int64 s1 = rnd.Next(000000, 999999);
            Int64 s2 = Convert.ToInt64(DateTime.Now.ToString("ddMMyyyyHHmmss"));
            var uniqueTransactionId = s2.ToString();

            var order = new Order
            {
                EthereumQty = model.EthereumQty,
                OrderDateTime = DateTime.Now,
                PurchasePrice = model.PayableAmount,
                Status = Helper.PaymentPending,
                TransactionId = uniqueTransactionId,
                UserId = userInDb.Id,
            };

            _context.Orders.Add(order);
            _context.SaveChanges();

            var result = Helper.SendEmail(email);
            Session["EmailResult"] = result;

            return RedirectToAction("EthereumBuyLink");
        }

        public ActionResult StepFive()
        {
            var userEmail = Request.QueryString["email"];
            var date = Request.QueryString["dt"].Split('|')[0];
            var time = Request.QueryString["dt"].Split('|')[1];
            var linkDateTime = DateTime.Parse(date + " " + time).AddMinutes(5); // Link Expire after 5 Mins
            var nowTime = DateTime.Now;

            var viewModel = new StepFiveViewModel()
            {
                EthPrice = _context.SetPrices.FirstOrDefault(x => x.Status == true).Price,
                MinEthBuy = _context.EthPurchaseRange.FirstOrDefault().Min,
                MaxEthBuy = _context.EthPurchaseRange.FirstOrDefault().Max
            };

            if (linkDateTime > nowTime)
            {
                return View(viewModel);
            }
            else
            {
                return Content("Payment Link has been Expired.");
            }                        
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StepFiveProcess(StepFiveViewModel model)
        {            
            if (!ModelState.IsValid)
            {
                return View("StepFive", model);
            }

            return RedirectToAction("Success");
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

        public ActionResult EthereumBuyLink()
        {
            return View();
        }

        // Perform Exception Handling
        //protected override void OnException(ExceptionContext filterContext)
        //{
        //    var controller = filterContext.RouteData.Values["controller"].ToString();
        //    var action = filterContext.RouteData.Values["action"].ToString();

        //    var ex = filterContext.Exception;
        //    filterContext.ExceptionHandled = true;

        //    filterContext.Result = new ViewResult()
        //    {
        //        ViewName = "Error"
        //    };
        //}

    }
}