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
    [HSH.Member.Attributes.SessionExpireAttribute]
    public class TradesController : Controller
    {
        private HSHEntities db = new HSHEntities();
        
        public ActionResult Index()
        {
            //var trade = db.Trade.ToList();
            return View();
        }

        public ActionResult GetTradeOrderList()
        {
            //return Json(new { bid = "330J", ask = "99" });
            return View("GetTradeOrderList", db.Trade.Where(t => t.Leave == false).OrderByDescending(t => t.CreateDate).ToList());
        }

        [HttpPost]
        public JsonResult GetTradeOrderJson()
        {
            List<Trade> resultList = db.Trade.Where(t =>
                t.CreateDate.Value.Year == DateTime.Now.Year &&
                t.CreateDate.Value.Month == DateTime.Now.Month &&
                t.CreateDate.Value.Day == DateTime.Now.Day &&
                t.CreateBy.Value ==  SessionHelper.CurrentUserInfo.UserId &&
                t.Leave == false
                ).OrderByDescending(t => t.CreateDate).ToList();

            var data = (from t in resultList
                        select new
                        {
                            id = t.TradeRef,
                            cell = new object[] 
                            { 
                                t.TradeRef, 
                                t.TradeType,
                                t.Quantity, 
                                t.Price,
                                t.Amount,
                                t.Leave,
                                t.Status,
                                t.CreateDate.Value.ToStringDateTimeFormat(),
                                t.UseDeposit
                            }
                        }).ToArray();


            var jsonData = new
            {
                total = 100,
                page = 1,
                records = resultList.Count(),
                rows = data

            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetTradeOrder()
        {
            List<Trade> resultList = db.Trade.Where(t =>
                t.CreateDate.Value.Year == DateTime.Now.Year &&
                t.CreateDate.Value.Month == DateTime.Now.Month &&
                t.CreateDate.Value.Day == DateTime.Now.Day &&
                t.CreateBy.Value == SessionHelper.CurrentUserInfo.UserId &&
                t.Leave == false
                ).OrderByDescending(t => t.CreateDate).ToList();


            var data = (from t in resultList
                        select new
                        {
                            id = t.TradeRef,
                            cell = new object[] 
                            { 
                                t.TradeRef, 
                                t.TradeType,
                                t.Quantity, 
                                t.Price,
                                t.Amount,
                                t.Leave,
                                t.Status,
                                t.CreateDate.Value.ToStringDateTimeFormat(),
                                t.UseDeposit
                            }
                        }).ToArray();


            var jsonData = new
            {
                total = 100,
                page = 1,
                records = resultList.Count(),
                rows = data

            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetTradePlace()
        {
            List<Trade> resultList = db.Trade.Where(t =>
                t.CreateDate.Value.Year == DateTime.Now.Year &&
                t.CreateDate.Value.Month == DateTime.Now.Month &&
                t.CreateDate.Value.Day == DateTime.Now.Day &&
                t.CreateBy.Value == SessionHelper.CurrentUserInfo.UserId &&
                t.Leave == true
                ).OrderByDescending(t => t.CreateDate).ToList();


            var data = (from t in resultList
                        select new
                        {
                            id = t.TradeRef,
                            cell = new object[] 
                            { 
                                t.TradeRef, 
                                t.TradeType,
                                t.Quantity, 
                                t.Price,
                                t.Amount,
                                t.Leave,
                                t.Status,
                                t.CreateDate.Value.ToStringDateTimeFormat(),
                                t.UseDeposit
                            }
                        }).ToArray();


            var jsonData = new
            {
                total = 100,
                page = 1,
                records = resultList.Count(),
                rows = data

            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetTradeAccept()
        {
            List<Trade> resultList = db.Trade.Where(t =>
                t.CreateDate.Value.Year == DateTime.Now.Year &&
                t.CreateDate.Value.Month == DateTime.Now.Month &&
                t.CreateDate.Value.Day == DateTime.Now.Day &&
                t.CreateBy.Value == SessionHelper.CurrentUserInfo.UserId &&
                t.Status == "Accept"
                ).OrderByDescending(t => t.CreateDate).ToList();


            var data = (from t in resultList
                        select new
                        {
                            id = t.TradeRef,
                            cell = new object[] 
                            { 
                                t.TradeRef, 
                                t.TradeType,
                                t.Quantity, 
                                t.Price,
                                t.Amount,
                                t.Leave,
                                t.Status,
                                t.CreateDate.Value.ToStringDateTimeFormat(),
                                t.UseDeposit
                            }
                        }).ToArray();


            var jsonData = new
            {
                total = 100,
                page = 1,
                records = resultList.Count(),
                rows = data

            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetTradeReject()
        {
            List<Trade> resultList = db.Trade.Where(t =>
                t.CreateDate.Value.Year == DateTime.Now.Year &&
                t.CreateDate.Value.Month == DateTime.Now.Month &&
                t.CreateDate.Value.Day == DateTime.Now.Day &&
                t.CreateBy.Value == SessionHelper.CurrentUserInfo.UserId &&
                t.Status == "Reject"
                ).OrderByDescending(t => t.CreateDate).ToList();


            var data = (from t in resultList
                        select new
                        {
                            id = t.TradeRef,
                            cell = new object[] 
                            { 
                                t.TradeRef, 
                                t.TradeType,
                                t.Quantity, 
                                t.Price,
                                t.Amount,
                                t.Leave,
                                t.Status,
                                t.CreateDate.Value.ToStringDateTimeFormat(),
                                t.UseDeposit
                            }
                        }).ToArray();


            var jsonData = new
            {
                total = 100,
                page = 1,
                records = resultList.Count(),
                rows = data

            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult TradeOrder(double Quantity, double Price, string TradeType, bool Leave, bool Deposit)
        {
            //Bid == Sell
            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    //var tic = new Ticket();
                    Trade tr = new Trade();
                    double refNum = db.Trade.Where(w => w.CreateDate.Value.Month == DateTime.Today.Month && w.CreateDate.Value.Year == DateTime.Today.Year).Count() + 1;
                    string ticketRef = "R" + DateTime.Now.ToString("ddMMyy/") + String.Format("{0:000}", refNum);

                    tr.TradeId = Guid.NewGuid();
                    tr.TradeRef = ticketRef;
                    tr.TradeType = TradeType;
                    tr.MemberId =  SessionHelper.CurrentUserInfo.MemberId;

                    tr.Leave = Leave;
                    tr.Quantity = Quantity;
                    tr.Price = Price;
                    tr.Amount = ((Quantity * Price) * 656) / 10;
                    tr.IpAddress = Request.UserHostAddress;
                    tr.CreateDate = DateTime.Now;
                    tr.CreateBy =  SessionHelper.CurrentUserInfo.UserId;
                    if (tr.TradeType == EnumHelper.TicketType.Sell.ToString())
                    {
                        tr.UseDeposit = Deposit;
                    }

                    if (Leave)
                    {
                        tr.Status = "Pending";
                    }
                    else
                    {
                        tr.Status = "Accept";
                        tr.AcceptType = "A";
                    }

                    db.Trade.Add(tr);
                    db.SaveChanges();
                    dbContextTransaction.Commit();
                    return Json(new { status = "Transaction success" });
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    return Json(new { status = ex.Message });
                }
            }
        }

        [HttpPost]
        public ActionResult getMemberAutoComplete(string term)
        {
            var result = (from f in db.Member
                          where f.Active == true && f.FirstName.ToLower().Contains(term.ToLower())
                          select new { f.FirstName, f.MemberRef }).Distinct().ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult getMemberRefKeyin(string MemberRef)
        {
            PortFolioViewModels pf = new BusinessService().getPortFolio(MemberRef);
            //return Json(new { MemberId = mem.MemberId, MemberLevel = mem.MemberLevel }, JsonRequestBehavior.AllowGet);
            if (pf == null)
            {

                return Json(null, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(pf, JsonRequestBehavior.AllowGet);
            }
        }

        //Json TradeSale Page For Search Profile Member
        [HttpPost]
        public JsonResult getMemberProfile(string MemberRef)
        {
            PortFolioViewModels pf = new BusinessService().getPortFolio(MemberRef);

            StringBuilder tbl = new StringBuilder();
            //tbl.Append("<table class='table table-condensed table-responsive table-hover table-striped'>");
            //tbl.Append("<table class='table table-condensed table-striped table-hover table-bordered table-responsive'>");


            if (pf.MemberRef == null)
            {
                tbl.Append("<div class='text-center text-danger'>ไม่พบข้อมูลลูกค้า<div>");
            }
            else
            {
                if (pf.MemberType ==  EnumHelper.MemberType.Company.ToString())
                {
                    pf.MemberType = "<span class='label label-primary'>" + EnumHelper.GetDescription(EnumHelper.MemberType.Company) + "</span>";
                }
                else if (pf.MemberType == EnumHelper.MemberType.Foreign.ToString())
                {
                     pf.MemberType = "<span class='label label-warning'>" + EnumHelper.GetDescription(EnumHelper.MemberType.Foreign) + "</span>";
                }
                else if (pf.MemberType == EnumHelper.MemberType.Normal.ToString())
                {
                     pf.MemberType = "<span class='label label-default'>" + EnumHelper.GetDescription(EnumHelper.MemberType.Normal) + "</span>";
                }
                else if (pf.MemberType == EnumHelper.MemberType.VIP.ToString())
                {
                     pf.MemberType = "<span class='label label-success'>" + EnumHelper.GetDescription(EnumHelper.MemberType.VIP) + "</span>";
                }
                else if (pf.MemberType == EnumHelper.MemberType.Walkin.ToString())
                {
                    pf.MemberType = "<span class='label label-danger'>" + EnumHelper.GetDescription(EnumHelper.MemberType.Walkin) + "</span>";
                }

                string pauseBuy = "";
                string pauseSell = "";
                if (pf.PauseBuy)
                {
                    pauseBuy = "<span class='label label-danger'>" + EnumHelper.PauseStatus.Pause.ToString() + "</span>";
                }
                else
                {
                    pauseBuy = "<span class='label label-default'>" + EnumHelper.PauseStatus.UnPause.ToString() + "</span>";
                }
                if (pf.PauseSell)
                {
                    pauseSell = "<span class='label label-danger'>" + EnumHelper.PauseStatus.Pause.ToString() + "</span>";
                }
                else
                {
                    pauseSell = "<span class='label label-default'>" + EnumHelper.PauseStatus.UnPause.ToString() + "</span>";
                }
                string CreditBuyBalance = pf.MarginValue == 0 ? "Unlimit" : StringHelper.ToStringCurrency(pf.CreditBuyBalance.ToString()) ;
                string CreditSellBalance = pf.MarginValue == 0 ? "Unlimit" : StringHelper.ToStringCurrency(pf.CreditSellBalance.ToString());


                tbl.Append(" <div class='panel-body' id='divMemberFound'>");
                tbl.Append("     <div class='row'>");
                tbl.Append("         <div class='col-md-3 text-right'>เลขที่บัญชีซื้อขายทองคำ :</div>");
                tbl.Append("         <div class='col-md-3'>" + pf.MemberRef + "</div>");
                tbl.Append("         <div class='col-md-3 text-right'>ประเภทสมาชิก:</div>");
                tbl.Append("         <div class='col-md-3'>" + pf.MemberType + "</div>");
                tbl.Append("     </div>");
                tbl.Append("     <div class='row'>");
                tbl.Append("         <div class='col-md-3 text-right'>ชื่อสมาชิก :</div>");
                tbl.Append("         <div class='col-md-3'>" + pf.FirstName + "</div>");
                tbl.Append("         <div class='col-md-3 text-right'>นามสกุล :</div>");
                tbl.Append("         <div class='col-md-3'>" + pf.LastName + "</div>");
                tbl.Append("     </div>");
                tbl.Append("     <div id='divProfileDetail'>");
                tbl.Append("         <div class='row'>");
                tbl.Append("             <div class='col-md-3 text-right'>เลเวล :</div>");
                tbl.Append("             <div class='col-md-3'>" + pf.MemberLevel + "</div>");
                tbl.Append("             <div class='col-md-3 text-right'>กลุ่มสมาชิก :</div>");
                tbl.Append("             <div class='col-md-3'>" + pf.MemberGroup + "</div>");
                tbl.Append("         </div>");
                tbl.Append("         <div class='row'>");
                tbl.Append("             <div class='col-md-3 text-right'>สถานะการซื้อ :</div>");
                tbl.Append("             <div class='col-md-3'>" + pauseBuy + "</div>");
                tbl.Append("             <div class='col-md-3 text-right'>สถานะการขาย :</div>");
                tbl.Append("             <div class='col-md-3'>" + pauseSell + "</div>");
                tbl.Append("         </div>");
                tbl.Append("         <div class='row'>");
                tbl.Append("             <div class='col-md-3 text-right'>ซื้อขายได้สูงสุดต่อครั้ง :</div>");
                tbl.Append("             <div class='col-md-3'>" + pf.MaxKg + "</div>");
                tbl.Append("             <div class='col-md-3 text-right'>CreditLimit :</div>");
                tbl.Append("             <div class='col-md-3'>" + pf.CreditLimit + "</div>");
                tbl.Append("         </div>");
                tbl.Append("         <div class='row'>");
                tbl.Append("             <div class='col-md-3 text-right'>Margin Type :</div>");
                tbl.Append("             <div class='col-md-3'>" + pf.MarginStatus + "</div>");
                tbl.Append("             <div class='col-md-3 text-right'>Duedate T+ :</div>");
                tbl.Append("             <div class='col-md-3'>" + pf.DuedateValue + "</div>");
                tbl.Append("         </div>");
                tbl.Append("         <div class='row'>");
                tbl.Append("             <div class='col-md-3 text-right'>เงินหลักประกัน (THB) :</div>");
                tbl.Append("             <div class='col-md-3'>" + StringHelper.ToStringCurrency(pf.AssetCash.ToString()) + "</div>");
                tbl.Append("             <div class='col-md-3 text-right'>ทองหลักประกัน (Kg) :</div>");
                tbl.Append("             <div class='col-md-3'>" + StringHelper.ToStringCurrency(pf.AssetGold.ToString()) + "</div>");
                tbl.Append("         </div>");
                tbl.Append("         <div class='row'>");
                tbl.Append("             <div class='col-md-3 text-right'>ปริมาณที่ซื้อ/ขายได้ (Kg) :</div>");
                tbl.Append("             <div class='col-md-3'>" + StringHelper.ToStringCurrency(pf.CreditLine.ToString()) + "</div>");
                tbl.Append("             <div class='col-md-3 text-right'>ทองฝาก (Kg) :</div>");
                tbl.Append("             <div class='col-md-3'>" + StringHelper.ToStringCurrency(pf.DepositGold.ToString()) + "</div>");
                tbl.Append("         </div>");
                tbl.Append("         <div class='row'>");
                tbl.Append("             <div class='col-md-3 text-right'>ปริมาณที่ลูกค้าซื้อได้คงเหลือ (Kg) :</div>");
                tbl.Append("             <div class='col-md-3'>" + CreditBuyBalance + "</div>");
                tbl.Append("             <div class='col-md-3 text-right'>ปริมาณที่ลูกค้าขายได้คงเหลือ (Kg) :</div>");
                tbl.Append("             <div class='col-md-3'>" + CreditSellBalance + "</div>");
                tbl.Append("         </div>");
                tbl.Append("         <div class='row'>");
                tbl.Append("             <div class='col-md-3 text-right'>จำนวนการซื้อทองคำล่วงหน้า (Kg) :</div>");
                tbl.Append("             <div class='col-md-3'>" + StringHelper.ToStringCurrency(pf.SumBuyPlaceOrder.ToString()) + "</div>");
                tbl.Append("             <div class='col-md-3 text-right'>จำนวนการขายทองคำล่วงหน้า (Kg) :</div>");
                tbl.Append("             <div class='col-md-3'>" + StringHelper.ToStringCurrency(pf.SumSellPlaceOrder.ToString()) + "</div>");
                tbl.Append("         </div>");
                tbl.Append("         <div class='row'>");
                tbl.Append("             <div class='col-md-3 text-right'>จำนวนการซื้อทองคำ (Kg) :</div>");
                tbl.Append("             <div class='col-md-3'>" + StringHelper.ToStringCurrency(pf.SumBuy.ToString()) + "</div>");
                tbl.Append("             <div class='col-md-3 text-right'>จำนวนการขายทองคำ (Kg) :</div>");
                tbl.Append("             <div class='col-md-3'>" + StringHelper.ToStringCurrency(pf.SumSell.ToString()) + "</div>");
                tbl.Append("         </div>");
                tbl.Append("     </div>");
                tbl.Append(" </div>");
            }
            return Json(new
            {
                result = tbl.ToString(),
                MemberLevel = pf.MemberRef == null ? "" : pf.MemberLevel.ToString(),
                MemberId = pf.MemberRef == null ? "" : pf.MemberId.ToString()
            });
        }


        [HttpPost]
        public JsonResult GetSpotPrice()
        {
            var sp = db.SpotPrice.Where(s => s.SpotId == 1).FirstOrDefault();
            //return Json(sp.Bid.ToString() +"|" + sp.Ask.ToString());
            return Json(new { bid = sp.Bid.ToString(), ask = sp.Ask.ToString() });
        }


        [HttpPost]
        public JsonResult CheckPortFolio(Guid MemberId, string TradeType, double Quantity, bool UseDeposit)
        {
            PortFolioViewModels pf = new BusinessService().getPortFolio(MemberId);
            bool TradeAble = false;
            double tradeBalance = 0;
            var msg = "";
            if (pf != null)
            {

                //1.check pause
                if (TradeType == EnumHelper.TicketType.Buy.ToString())
                {
                    if (pf.PauseBuy)
                    {
                        msg = "คุณถูกหยุดการซื้อชั่วคราว";
                        return Json(new { TradeAble = TradeAble, TradeBalance = tradeBalance, WarningMsg = msg });
                    }
                }
                else
                {
                    if (pf.PauseSell)
                    {
                        msg = "คุณถูกหยุดการขายชั่วคราว";
                        return Json(new { TradeAble = TradeAble, TradeBalance = tradeBalance, WarningMsg = msg });
                    }
                }

                //2check MaxKg
                if (Quantity > pf.MaxKg)
                {
                    msg = "เกินจำนวนสูงสุดในการซื้อขายต่อครั้ง";
                    return Json(new { TradeAble = TradeAble, TradeBalance = tradeBalance, WarningMsg = msg });
                }

                //3check CreditLimit
                double sumPending = 0;
                if (TradeType == EnumHelper.TicketType.Buy.ToString())
                {
                    sumPending = Convert.ToDouble(pf.SumBuy + pf.SumBuyPlaceOrder);
                }
                else
                {
                    sumPending = Convert.ToDouble(pf.SumSell + pf.SumSellPlaceOrder);
                }

                if (Quantity > pf.CreditLimit - sumPending)
                {
                    msg = "เกินจำนวน Credit Limit";
                    return Json(new { TradeAble = TradeAble, TradeBalance = tradeBalance, WarningMsg = msg });
                }

                //4check Margin
                if (pf.MarginValue != 0)
                {
                    //calculate collateral
                    if (TradeType == EnumHelper.TicketType.Buy.ToString())
                    {
                        tradeBalance = Convert.ToDouble(pf.CreditBuyBalance);
                    }
                    else
                    {
                        tradeBalance = Convert.ToDouble(pf.CreditSellBalance);
                    }

                    if (tradeBalance >= Quantity)
                    {
                        TradeAble = true;
                    }
                    else
                    {
                        msg = "หลักประกันไม่พอในการซื้อขาย";
                    }
                }
                else
                {
                    //Unlimit Trade ,Margin = 0, ไม่คิดหลักประกัน
                    TradeAble = true;
                }

                //case member sell and useDeposit(ขายทองฝาก) --> 
                if (TradeAble == true && UseDeposit == true && TradeType == EnumHelper.TicketType.Sell.ToString())
                {
                    if (Quantity > Convert.ToDouble(pf.QuantityBalanceSellGoldDep))
                    {
                        msg = "ทองฝากไม่พอสำหรับ ขายทองฝาก";
                        TradeAble = false;
                    }
                }
            }
            return Json(new { TradeAble = TradeAble, TradeBalance = tradeBalance, WarningMsg = msg });
        }

        //Method Json For Member getmarketprice
        [HttpPost]
        public JsonResult GetMarketPriceMember(Guid MemberId)
        {
            string price = "";
            string bidBg = "";
            string askBg = "";
            if (MemberId.ToString() != "")
            {
                char[] split = { '|' };
                price = new BusinessService().getMarketPriceMember(MemberId);
                if (price != "")
                {
                    bidBg = price.Split(split)[0];
                    askBg = price.Split(split)[1];
                }
            }

            return Json(new { bid = bidBg, ask = askBg }, JsonRequestBehavior.AllowGet);
        }

        //Method Json For Sale getmarketprice
        [HttpPost]
        public JsonResult GetMarketPriceSale()
        {
            string bidBg = "";
            string askBg = "";

            var mp = new MarketPriceViewModels();
            mp = new BusinessService().getMarketPrice();
            string priceFormat = "#,##";

            if (mp != null)
            {
                bidBg = mp.Bid99Bg1.ToString(priceFormat);
                askBg = mp.Ask99Bg1.ToString(priceFormat);
            }

            return Json(new
            {
                bid1 = mp.Bid99Bg1.ToString(priceFormat),
                ask1 = mp.Ask99Bg1.ToString(priceFormat),
                bid2 = mp.Bid99Bg2.ToString(priceFormat),
                ask2 = mp.Ask99Bg2.ToString(priceFormat),
                bid3 = mp.Bid99Bg3.ToString(priceFormat),
                ask3 = mp.Ask99Bg3.ToString(priceFormat),
                bid4 = mp.Bid99Bg4.ToString(priceFormat),
                ask4 = mp.Ask99Bg4.ToString(priceFormat)
            }, JsonRequestBehavior.AllowGet);
        }

        // GET: Trades/Create
        public ActionResult Create()
        {
            return View();
            //if ( SessionHelper.CurrentUserInfo != null)
            //{
            //    return View();
            //}
            //else
            //{
            //    return RedirectToAction("UserLogin", "Home");
            //}
        }

        #region "Sale"

        public ActionResult TradeSale()
        {
            if ( SessionHelper.CurrentUserInfo != null)
            {
                List<SelectListItem> items = new SelectList(db.Member.Where(m => m.Active == true).OrderByDescending(m => m.FirstName)
                .Select(m => new { MemberId = m.MemberId + "|" + m.MemberLevel.Value.ToString(), FirstName = m.FirstName + " (" + m.MemberRef + ")" }), "MemberId", "FirstName").ToList();

                //List<SelectListItem> items = new SelectList(db.Member, "MemberId", "FirstName").ToList();
                items.Insert(0, (new SelectListItem { Text = "-- โปรดเลือกรายชื่อสมาชิก --", Value = "" }));
                ViewBag.MemberId = items;

                return View();
            }
            else
            {
                return RedirectToAction("UserLogin", "Home");
            }

        }


        [HttpPost]
        public JsonResult TradeOrderSale(double Quantity, double Price, string TradeType, bool Deposit, bool Leave, string MemberId, bool Lot)
        {
            //Bid == Sell
            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    Trade tr = new Trade();
                    int refNum = db.Trade.Where(w => w.CreateDate.Value.Month == DateTime.Today.Month && w.CreateDate.Value.Year == DateTime.Today.Year).Count() + 1;
                    string ticketRef = "R" + DateTime.Now.ToString("ddMMyy/") + String.Format("{0:000}", refNum);

                    tr.TradeId = Guid.NewGuid();
                    tr.TradeRef = ticketRef;
                    tr.TradeType = TradeType;
                    tr.MemberId = Guid.Parse(MemberId);

                    tr.Leave = Leave;
                    tr.Quantity = Quantity;
                    tr.Price = Price;
                    tr.Amount = ((Quantity * Price) * 656) / 10;
                    tr.IpAddress = Request.UserHostAddress;
                    tr.CreateDate = DateTime.Now;
                    tr.CreateBy =  SessionHelper.CurrentUserInfo.UserId;
                    tr.UseDeposit = Deposit;
                    tr.SaleId =  SessionHelper.CurrentUserInfo.UserId;
                    tr.TradeLot = Lot;//Use Trigger update Quantity from Table TradeLot

                    if (Leave)
                    {
                        tr.Status = "Pending";
                    }
                    else
                    {
                        tr.Status = "Accept";
                        tr.AcceptType = "A";
                    }


                    db.Trade.Add(tr);
                    db.SaveChanges();
                    dbContextTransaction.Commit();
                    return Json(new { status = "Transaction success" });
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    return Json(new { status = ex.InnerException.InnerException.Message });
                }
            }
        }


        [HttpPost]
        public JsonResult GetTradeSaleLotOrder()
        {
            var tl = db.TradeLot.Where(w => w.Quantity > -1)
                .Include(t => t.UserOnline)
                .OrderByDescending(w => w.CreateDate);
            //var tl = db.TradeLot.Join(db.UserOnline,
            //    t => t.CreateBy,
            //    u => u.UserId,
            //    (t, u) => new { t.TradeLotId, t.TradeLotRef, t.CreateDate, u.UserName, t.TradeType, t.Quantity, t.Price }).OrderByDescending(o => o.CreateDate);

            StringBuilder tbl = new StringBuilder();
            //tbl.Append("<table class='table table-condensed table-responsive table-hover table-striped'>");
            tbl.Append("<table class='table table-condensed table-striped table-hover table-bordered table-responsive'>");
            tbl.Append("<tr class='info'>");
            tbl.Append("<th class='text-center'>BigLotRef</th>");
            tbl.Append("<th class='text-center'>CreateDate</th>");
            tbl.Append("<th class='text-center'>CreateBy</th>");
            tbl.Append("<th class='text-center'>Type</th>");
            tbl.Append("<th class='text-center'>Quantity</th>");
            tbl.Append("<th class='text-center'>Price</th>");
            tbl.Append("</tr>");


            if (tl.Count() == 0)
            {
                tbl.Append("<tr><td colspan='6' class='text-center text-danger'>" + StringHelper.stringNotFound + "</td></tr>");
            }
            else
            {
                foreach (var item in tl)
                {
                    if (item.TradeType == EnumHelper.TicketType.Buy.ToString())
                    {
                        tbl.Append(string.Format("<tr onclick=document.getElementById('txtAskLot').value='{0}';>", item.Price.Value.ToString(StringHelper.formatnumber0Digit)));
                    }
                    else
                    {
                        tbl.Append(string.Format("<tr onclick=document.getElementById('txtBidLot').value='{0}';>", item.Price.Value.ToString(StringHelper.formatnumber0Digit)));
                    }
                    tbl.Append(string.Format("<td class='text-center'>{0}</td>", item.TradeLotRef + item.TradeLotId));
                    tbl.Append(string.Format("<td class='text-center'>{0}</td>", StringHelper.ToFormatStringDate(item.CreateDate)));
                    tbl.Append(string.Format("<td class='text-left'>{0}</td>", item.UserOnline.UserName));
                    tbl.Append(string.Format("<td class='text-center'>{0}</td>", item.TradeType == EnumHelper.TicketType.Buy.ToString() ? "<span class='label label-primary'>Buy</span>" : "<span class='label label-danger'>Sell</span>"));
                    tbl.Append(string.Format("<td class='text-right'>{0}</td>", item.Price.Value.ToString(StringHelper.formatnumber0Digit)));
                    tbl.Append(string.Format("<td class='text-right'>{0}</td>", item.Quantity.Value.ToString(StringHelper.formatnumber0Digit)));
                    tbl.Append("</tr>");
                }
            }


            tbl.Append("</table>");
            return Json(new { result = tbl.ToString() });
        }

        #endregion




        [HttpPost]
        public JsonResult RejectTrade(string tradeRef)
        {
            try
            {
                var item = db.Trade.Where(t => t.TradeRef.Equals(tradeRef)).Single();
                if (item != null)
                {
                    //item.Status = EnumHelper.TradeStatus.Reject.ToString();
                    //item.RejectType = EnumHelper.RejectType.Client.ToString();

                    db.Entry(item).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { Result = true });
                }
            }
            catch (Exception)
            {
                return Json(new { Result = false });
            }
            return Json(new { Result = false });
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
