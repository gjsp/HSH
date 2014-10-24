using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HSH.Data.Models;
using HSH.Member.Helper;
using HSH.Data.Helper;

namespace HSH.Member.Controllers
{
    public class HomeController : Controller
    {
        HSHEntities db = new HSHEntities();
        [AllowAnonymous]
        public ActionResult UserLogin()
        {
            return View();
        }

        [HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public ActionResult UserLogin(UserOnline model)
        {
            if (ModelState.IsValid)
            {
                //string username = db.UserOnline.SingleOrDefault().UserName;
                var userOnline = db.UserOnline.Where(u => u.Active == true && u.UserName == model.UserName && u.Password == model.Password).FirstOrDefault();
                if (userOnline != null)
                {
                    SessionHelper.CurrentUserInfo = userOnline;
                    if (userOnline.Role == "Admin")
                    {
                        return RedirectToAction("Index", "ManageSpotPrices");
                    }
                    else if (userOnline.Role == "Member")
                    {
                        return RedirectToAction("Create", "Trades");
                    }
                    else if (userOnline.Role == EnumHelper.RoleOnline.Sale.ToString())
                    {
                        return RedirectToAction("TradeSale", "Trades");
                    }
                    else if (userOnline.Role == EnumHelper.RoleOnline.Trader.ToString())
                    {
                        return RedirectToAction("Index", "Settles");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }

            return View(model);
        }

        public ActionResult Index()
        {
            return View();
        }

      

     
    }
}