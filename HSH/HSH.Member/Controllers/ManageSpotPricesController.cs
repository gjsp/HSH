using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HSH.Data.Models;
using HSH.Data.Business;
using HSH.Member.Helper;

namespace HSH.Member.Controllers
{
    [Attributes.SessionExpireAttribute]
    public class ManageSpotPricesController : Controller
    {
        private HSHEntities db = new HSHEntities();
        public ActionResult Index()
        {

            //set dropdownlist
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Ask", Value = "Ask" });
            items.Add(new SelectListItem { Text = "Bid", Value = "Bid" });
            ViewBag.UsePriceAsk = new SelectList(items, "Value", "Text");
            //BusinessService ps = new BusinessService();
            return View(new BusinessService().getMarketPrice());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index( MarketPriceViewModels model)
        {
            if (ModelState.IsValid)
            {
                var stock = db.StockOnline.FirstOrDefault();
                stock.Premium = model.Premium;
                stock.Discount = model.Discount;
                stock.Spread1 = model.Spread1;
                stock.Spread2 = model.Spread2;
                stock.Spread3 = model.Spread3;
                stock.Spread4 = model.Spread4;
                stock.AskThbSelf = Convert.ToDecimal( model.ThbCalculateValue);//THB
                stock.SpotCalculate = model.SpotCalculate;//Select spot for calculate
                
                //var spo = db.SpotPrice.FirstOrDefault();
                //spo.ask
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //set dropdownlist
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Ask", Value = "Ask" });
            items.Add(new SelectListItem { Text = "Bid", Value = "Bid" });
            ViewBag.UsePriceAsk = new SelectList(items, "Value", "Text");

            //return RedirectToAction("Index");
            return View(model);
        }


        [HttpPost]
        public JsonResult GetMarketPriceAdmin()
        {
            var mp = new MarketPriceViewModels();
            //mp = new ps.getMarketPrice();
            //BusinessService ps = new BusinessService();
            mp = new BusinessService().getMarketPrice();
            string priceFormat = "#,##";
            return Json(new
            {
                bidSpot = mp.BidSpot.ToString(priceFormat),
                askSpot = mp.AskSpot.ToString(priceFormat),
                bidThb = mp.BidThb.ToString(priceFormat),
                askThb = mp.AskThb.ToString(priceFormat),
                bid99Kg = mp.Bid99Kg.ToString(priceFormat),
                ask99Kg = mp.Ask99Kg.ToString(priceFormat),
                bid99Bg1 = mp.Bid99Bg1.ToString(priceFormat),
                ask99Bg1 = mp.Ask99Bg1.ToString(priceFormat),
                bid99Bg2 = mp.Bid99Bg2.ToString(priceFormat),
                ask99Bg2 = mp.Ask99Bg2.ToString(priceFormat),
                bid99Bg3 = mp.Bid99Bg3.ToString(priceFormat),
                ask99Bg3 = mp.Ask99Bg3.ToString(priceFormat),
                bid99Bg4 = mp.Bid99Bg4.ToString(priceFormat),
                ask99Bg4 = mp.Ask99Bg4.ToString(priceFormat)
            },JsonRequestBehavior.AllowGet);
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
