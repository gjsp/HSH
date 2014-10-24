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
    public class CollateralsController : Controller
    {
        private HSHEntities db = new HSHEntities();
           
        public ActionResult Edit()
        {
            var Collateral = db.Collaterals.FirstOrDefault();
            var col = new CollateralViewModels();
            col.GoldPerKg = Collateral.GoldPerKg.Value.ToString(StringHelper.formatnumber0Digit);
            col.CashPerKg = Collateral.CashPerKg.Value.ToString(StringHelper.formatnumber0Digit);
            col.GoldPercent = Collateral.GoldPercent ?? 0;
            return View(col);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( CollateralViewModels collateral)
        {
            if (ModelState.IsValid)
            {
                var col = db.Collaterals.FirstOrDefault(); ;
                col.GoldPerKg = Convert.ToDecimal(collateral.GoldPerKg);
                col.CashPerKg = Convert.ToDecimal(collateral.CashPerKg);
                col.GoldPercent = collateral.GoldPercent;

                db.SaveChanges();
                Response.Redirect("Edit");
            }
            return View(collateral);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
