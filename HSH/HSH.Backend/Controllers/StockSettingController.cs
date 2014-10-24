using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HSH.Backend.Helper;
using HSH.Data.Models;
using HSH.Backend.Attributes;
using HSH.Data.Business;
using HSH.Data.Helper;

namespace HSH.Backend.Controllers
{
    [SessionExpireAttribute]
    public class StockSettingController : Controller
    {
        private HSHEntities db = new HSHEntities();
        public ActionResult Edit()
        {
            var stock = db.StockOnline.FirstOrDefault();
            var sto = new StockSettingViewModels();
            sto.Spread1 = stock.Spread1 ?? 0;
            sto.Spread2 = stock.Spread2 ?? 0;
            sto.Spread3 = stock.Spread3 ?? 0;
            sto.Spread4 = stock.Spread4 ?? 0;
           
            sto.MaxKg1 = stock.MaxKg1 ?? 0;
            sto.MaxKg2 = stock.MaxKg2 ?? 0;
            sto.MaxKg3 = stock.MaxKg3 ?? 0;
            sto.MaxKg4 = stock.MaxKg4 ?? 0;

            sto.Duedate1 = stock.Duedate1 ?? 0;
            sto.Duedate2 = stock.Duedate2 ?? 0;
            sto.Duedate3 = stock.Duedate3 ?? 0;
            sto.Duedate4 = stock.Duedate4 ?? 0;

            sto.CreditLimit1 = stock.CreditLimit1 ?? 0;
            sto.CreditLimit2 = stock.CreditLimit2 ?? 0;
            sto.CreditLimit3 = stock.CreditLimit3 ?? 0;
            sto.CreditLimit4 = stock.CreditLimit4 ?? 0;

            sto.MarginType1 = stock.MarginType1 ?? 0;
            sto.MarginType2 = stock.MarginType2 ?? 0;
            sto.MarginType3 = stock.MarginType3 ?? 0;
            sto.MarginType4 = stock.MarginType4 ?? 0;

            return View(sto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(StockSettingViewModels sto)
        {
            if (ModelState.IsValid)
            {
                var stock = db.StockOnline.FirstOrDefault();
                stock.Spread1 = sto.Spread1;
                stock.Spread2 = sto.Spread2;
                stock.Spread3 = sto.Spread3;
                stock.Spread4 = sto.Spread4;

                stock.MaxKg1 = sto.MaxKg1;
                stock.MaxKg2 = sto.MaxKg2;
                stock.MaxKg3 = sto.MaxKg3;
                stock.MaxKg4 = sto.MaxKg4;

                stock.Duedate1 = sto.Duedate1;
                stock.Duedate2 = sto.Duedate2;
                stock.Duedate3 = sto.Duedate3;
                stock.Duedate4 = sto.Duedate4;

                stock.CreditLimit1 = sto.CreditLimit1;
                stock.CreditLimit2 = sto.CreditLimit2;
                stock.CreditLimit3 = sto.CreditLimit3;
                stock.CreditLimit4 = sto.CreditLimit4;

                stock.MarginType1 = sto.MarginType1;
                stock.MarginType2 = sto.MarginType2;
                stock.MarginType3 = sto.MarginType3;
                stock.MarginType4 = sto.MarginType4;

                db.SaveChanges();
                //Response.Redirect("Edit");
                return RedirectToAction("Edit");
            }
            return View(sto);
        }

        public JsonResult GetMarketPrice()
        {
            var mp = new MarketPriceViewModels();
            mp = new BusinessService().getMarketPrice();
            var spd = db.StockOnline.SingleOrDefault();
            string formatPrice = "#,##0";

            return Json(new
            {
                askBg1 = mp.Ask99Bg1.ToString(formatPrice),
                askBg2 = mp.Ask99Bg2.ToString(formatPrice),
                askBg3 = mp.Ask99Bg3.ToString(formatPrice),
                askBg4 = mp.Ask99Bg4.ToString(formatPrice),
                bidBg1 = mp.Bid99Bg1.ToString(formatPrice),
                bidBg2 = mp.Bid99Bg2.ToString(formatPrice),
                bidBg3 = mp.Bid99Bg3.ToString(formatPrice),
                bidBg4 = mp.Bid99Bg4.ToString(formatPrice)
            },JsonRequestBehavior.AllowGet);
        }

    }
}
