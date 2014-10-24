using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
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
    public class SettlesController : Controller
    {
        private HSHEntities db = new HSHEntities();
        public ActionResult Index()
        {
            return View();
        }



        [HttpPost]
        public JsonResult GetInfoSettleTrade()
        {
            var mp = new MarketPriceViewModels();
            mp = new BusinessService().getMarketPrice();
            string formatPrice = "#,##0";
            string formatFx = "#,##0.00";


            var sett = db.TicketSettleSummary.SingleOrDefault();
            double tradeBuy = db.TicketSettle.Where(w => w.TicketType ==EnumHelper.DealType.Buy.ToString()).Sum(w => w.Quantity) ?? 0;
            double tradeSell= db.TicketSettle.Where(w => w.TicketType ==EnumHelper.DealType.Sell.ToString()).Sum(w => w.Quantity) ?? 0;
            double comBuy = tradeBuy - sett.CompanyBuy.Value;
            double comSell = tradeSell- sett.CompanySell.Value;
            double net = comBuy - comSell;

            return Json(new
            {
                bidBg = mp.Bid99Bg.ToString(formatPrice),
                askBg = mp.Ask99Bg.ToString(formatPrice),
                bidSpot = mp.BidSpot.ToString(formatFx),
                askSpot = mp.AskSpot.ToString(formatFx),
                fx = mp.ThbCalculateValue.ToString("#,##0.000"),
                memBuy = tradeBuy.ToString(formatPrice),
                memSell = tradeSell.ToString(formatPrice),
                comBuy = comBuy.ToString(formatPrice),
                comSell = comSell.ToString(formatPrice),
                net = net.ToString(formatPrice)
            });
        }

        [HttpPost]
        public JsonResult GetSettleTicket()
        {
            StringBuilder listBuy = new StringBuilder();
            StringBuilder listSell = new StringBuilder();

            var sett = db.TicketSettle.Where(w=>w.Quantity > 0).OrderByDescending(o => o.Price);
            
            if (sett.Count() > 0)
            {
                listBuy.Append("<div id='listBuy' style='text-align:left;margin-left:20px'>");
                listSell.Append("<div id='listSell' style='text-align:left;margin-left:20px'>");
                //string scriptOnchange = "onchange= if(true){alert('1');}";//document.getElementById('hiddenRealtime').value='0'
                foreach (var item in sett)
                {
                    if (item.TicketType == EnumHelper.DealType.Buy.ToString())
                    {
                        listBuy.Append(string.Format("<div><input  type='checkbox' value='{1}' disabled  class='chk'/>&nbsp;&nbsp;{0}</div>", 
                            item.Price + " * " + item.Quantity,
                            item.Price + "|" + item.Quantity + "|" + EnumHelper.DealType.Buy.ToString()
                            ));
                    }
                    else
                    {
                        listSell.Append(string.Format("<div><input type='checkbox' value='{1}' disabled  class='chk'/>&nbsp;&nbsp;{0}</div>", 
                            item.Price + " * " + item.Quantity,
                            item.Price + "|" + item.Quantity + "|" + EnumHelper.DealType.Sell.ToString()
                            ));
                    }
                }
                listBuy.Append("<div>");
                listSell.Append("<div>");

            }

            return Json(new
            {
                listBuy = listBuy.ToString(),
                listSell = listSell.ToString()
            },JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddSpot(double spot, string type)
        {
            JsonHelper.JsonResponse res = new JsonHelper.JsonResponse();
            try
            {
                var sett = db.TicketSettleSummary.SingleOrDefault();
                if (sett != null)
                {
                    if (type == EnumHelper.DealType.Buy.ToString())
                    {
                        sett.CompanyBuy += spot;
                    }
                    else
                    {
                        sett.CompanySell += spot;
                    }
                    db.Entry(sett).State = EntityState.Modified;
                    db.SaveChanges();
                    res.Status = JsonHelper.Status.Ok;
                }
            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
                res.Status = JsonHelper.Status.Error;
            }
            return Json(res);
        }

        [HttpPost]
        public JsonResult AddSettle(string arrData)
        {
            double qty = 0;
            double price = 0;
            string type;
            double sumBuy = 0;
            double sumSell = 0;
            double maxBuy = 0; double maxSell = 0;

            JsonHelper.JsonResponse res = new JsonHelper.JsonResponse();
            TicketSettle sett;
            try
            {
                foreach (var arr in arrData.Split(','))
                {
                    if (arr != "")
                    {
                        price = Convert.ToDouble(arr.Split('|')[0]);
                        qty = Convert.ToDouble(arr.Split('|')[1]);
                        type = arr.Split('|')[2];
                        if (type == EnumHelper.DealType.Buy.ToString())
                        {
                            sumBuy += qty;
                        }
                        else
                        {
                            sumSell += qty;
                        }
                    }
                }
                maxBuy = sumBuy < sumSell ? sumBuy : sumSell;//choose sum minimum 
                maxSell = maxBuy;//look like twice part

                //update
                if (sumBuy != 0 && sumSell != 0)
                {
                    foreach (var arr in arrData.Split(','))
                    {
                        if (arr != "")
                        {
                            price = Convert.ToDouble(arr.Split('|')[0]);
                            qty = Convert.ToDouble(arr.Split('|')[1]);
                            type = arr.Split('|')[2];
                            if (type == EnumHelper.DealType.Buy.ToString())
                            {
                                if (qty <= maxBuy)
                                {
                                    sett = db.TicketSettle.Where(w => w.Price == price).SingleOrDefault();
                                    sett.Quantity = 0;
                                    db.SaveChanges();
                                    maxBuy -= qty;
                                }
                                else
                                {
                                    sett = db.TicketSettle.Where(w => w.Price == price).SingleOrDefault();
                                    sett.Quantity = qty - maxBuy;
                                    db.SaveChanges();
                                    maxBuy -= qty;
                                }
                            }
                            else
                            {
                                if (qty <= maxSell)
                                {
                                    sett = db.TicketSettle.Where(w => w.Price == price).SingleOrDefault();
                                    sett.Quantity = 0;
                                    db.SaveChanges();
                                    maxSell -= qty;
                                }
                                else
                                {
                                    sett = db.TicketSettle.Where(w => w.Price == price).SingleOrDefault();
                                    sett.Quantity = qty - maxSell;
                                    db.SaveChanges();
                                    maxSell -= qty;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                res.Status = JsonHelper.Status.Error;
                res.Message = ex.Message;
            }
            res.Status = JsonHelper.Status.Ok;
            return Json(res,JsonRequestBehavior.AllowGet);
        }

    }
}