using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;

using HSH.Data.Models;
using HSH.Data.Helper;
using HSH.Member.Helper;
using HSH.Data.Business;

namespace HSH.Member.Controllers
{
    [Attributes.SessionExpireAttribute]
    public class TradeLotsController : Controller
    {
        private HSHEntities db = new HSHEntities();
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetTradeLotOrder()
        {
            //var tl = db.TradeLot.OrderByDescending(w => w.CreateDate);
            var tl = db.TradeLot.Where(w => w.Quantity > -1).Join(db.UserOnline,
                t => t.CreateBy,
                u => u.UserId,
                (t, u) => new { t.TradeLotId, t.TradeLotRef, t.CreateDate, u.UserName, t.TradeType, t.Quantity, t.Price }).OrderByDescending(o => o.CreateDate);

            StringBuilder tbl = new StringBuilder();
            //tbl.Append("<table class='table table-condensed table-responsive table-hover table-striped'>");
            tbl.Append("<table class='table table-condensed table-striped table-hover table-bordered table-responsive'>");
            tbl.Append("<tr class='info'>");
            tbl.Append("<th class='text-center'>BigLotRef</th>");
            tbl.Append("<th class='text-center'>CreateDate</th>");
            tbl.Append("<th class='text-center'>CreateBy</th>");
            tbl.Append("<th class='text-center'>Type</th>");
            tbl.Append("<th class='text-center'>Price</th>");
            tbl.Append("<th class='text-center'>Quantity</th>");
            tbl.Append("</tr>");


            if (tl.Count() == 0)
            {
                tbl.Append("<tr><td colspan='6' class='text-center text-danger'>" + StringHelper.stringNotFound + "</td></tr>");
            }
            else
            {
                foreach (var item in tl)
                {
                    tbl.Append("<tr>");
                    tbl.Append(string.Format("<td class='text-center'>{0}</td>", item.TradeLotRef + item.TradeLotId));
                    tbl.Append(string.Format("<td class='text-center'>{0}</td>", StringHelper.ToFormatStringDate(item.CreateDate)));
                    tbl.Append(string.Format("<td class='text-left'>{0}</td>", item.UserName));
                    tbl.Append(string.Format("<td class='text-center'>{0}</td>", item.TradeType == EnumHelper.DealType.Buy.ToString() ? "<span class='label label-primary'>Buy</span>" : "<span class='label label-danger'>Sell</span>"));
                    tbl.Append(string.Format("<td class='text-right'>{0}</td>", item.Price.Value.ToString(StringHelper.formatnumber0Digit)));
                    tbl.Append(string.Format("<td class='text-right'>{0}</td>", item.Quantity.Value.ToString(StringHelper.formatnumber0Digit)));
                    tbl.Append("</tr>");
                }
            }


            tbl.Append("</table>");
            return Json(new { result = tbl.ToString() });
        }

        // GET: TradeLots/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TradeLots/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TradeLotCreateViewModels TradeLot)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var tl = new TradeLot();
                    tl.TradeLotRef = "BL";
                    tl.TradeType = TradeLot.TradeType;
                    tl.Price = Convert.ToDouble(TradeLot.Price);
                    tl.Quantity = TradeLot.Quantity;
                    tl.CreateDate = DateTime.Now;
                    tl.CreateBy = SessionHelper.CurrentUserInfo.UserId;
                    db.TradeLot.Add(tl);
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
