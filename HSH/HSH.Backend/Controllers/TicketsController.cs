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
    [HSH.Backend.Attributes.SessionExpireAttribute]
    public class TicketsController : Controller
    {
        private HSHEntities db = new HSHEntities();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string keyword, string DateFrom, string DateTo)
        {
            var item = db.Ticket.Include(t => t.UserCreated).Include(t => t.Member);

            if (!string.IsNullOrEmpty(keyword))
            {
                item = item.Where(w => w.Member.FirstName.Contains(keyword));
            }

            if (!string.IsNullOrEmpty(DateFrom) && !string.IsNullOrEmpty(DateTo))
            {
                char spl = '/';
                int fYear = Convert.ToInt16(DateFrom.Split(spl)[2]);
                int fMonth = Convert.ToInt16(DateFrom.Split(spl)[0]);
                int fDay = Convert.ToInt16(DateFrom.Split(spl)[1]);
                int tYear = Convert.ToInt16(DateTo.Split(spl)[2]);
                int tMonth = Convert.ToInt16(DateTo.Split(spl)[0]);
                int tDay = Convert.ToInt16(DateTo.Split(spl)[1]);
                DateTime df = new DateTime(fYear, fMonth, fDay);
                DateTime dt = new DateTime(tYear, tMonth, tDay).AddDays(1).AddMilliseconds(-1);
                item = item.Where(w => w.CreateDate >= df & w.CreateDate <= dt);
            }
            return PartialView("Find", item.OrderByDescending(t => t.CreateDate));
        }

        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Ticket.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        public ActionResult Create()
        {

            List<SelectListItem> items = new SelectList(db.Member.Where(m => m.Active == true).OrderByDescending(m => m.FirstName)
                .Select(m => new { MemberId = m.MemberId, FirstName = m.FirstName + " (" + m.MemberRef + ")" }), "MemberId", "FirstName").ToList();
            items.Insert(0, (new SelectListItem { Text = "== Select ==", Value = "" }));
            ViewBag.MemberId = items;
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TicketCreateViewModels ticket)
        {
            if (ModelState.IsValid)
            {
                //check Limit trade
                PortFolioViewModels pf = new  BusinessService().getPortFolio(ticket.MemberId);
                bool TradeAble = true;
                //decimal tradeBalance = 0;
                //if (pf != null)
                //{
                //    if (ticket.TicketType == EnumHelper.TicketType.Buy.ToString())
                //    {
                //        tradeBalance = pf.CreditBuyBalance;//Swep from backend
                //    }
                //    else
                //    {
                //        tradeBalance = pf.CreditSellBalance;//Swep from backend
                //    }
                //    if (tradeBalance >= Convert.ToDecimal(ticket.Quantity))
                //    {
                //        TradeAble = true;
                //    }
                //}
                if (TradeAble == true)
                {
                    using (var dbContextTransaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            var tic = new Ticket();
                            int refNum = db.Ticket.Where(w => w.CreateDate.Value.Month == DateTime.Today.Month && w.CreateDate.Value.Year == DateTime.Today.Year && w.CreateDate.Value.Day == DateTime.Today.Day).Count() + 1;
                            string ticketRef = "T" + DateTime.Now.ToString("ddMMyy/") + String.Format("{0:000}", refNum);

                            tic.TicketId = Guid.NewGuid();
                            tic.MemberId = ticket.MemberId;
                            tic.TicketType = ticket.TicketType;
                            tic.TicketRef = ticketRef;
                            tic.Status = EnumHelper.TicketStatus.Pending.ToString();
                            tic.CreateBy = SessionHelper.CurrentUserInfo.Id;
                            tic.CreateDate = DateTime.Now;
                            tic.Active = true;
                            tic.UseDeposit = ticket.UseDeposit;
                            if (tic.TicketType == EnumHelper.TicketType.Sell.ToString())
                            {
                                tic.UseDeposit = ticket.UseDeposit;
                            }

                            tic.Quantity = Convert.ToDouble(ticket.Quantity);
                            tic.Price = Convert.ToDouble(ticket.Price);
                            tic.Amount = Convert.ToDouble(ticket.Amount);
                            tic.PayType = ticket.Paytype;
                            tic.DueDate = DateTime.Today.AddDays(pf.DuedateValue);


                            db.Ticket.Add(tic);
                            db.SaveChanges();
                            dbContextTransaction.Commit();
                            return RedirectToAction("Index");
                        }
                        catch (Exception ex)
                        {
                            dbContextTransaction.Rollback();
                            throw ex;
                        }
                    }
                }
                else
                {
                    //over trade limit
                }
            }

            List<SelectListItem> items = new SelectList(db.Member.Where(m => m.Active == true).OrderByDescending(m => m.FirstName)
                .Select(m => new { MemberId = m.MemberId, FirstName = m.FirstName + " (" + m.MemberRef + ")" }), "MemberId", "FirstName").ToList();
            items.Insert(0, (new SelectListItem { Text = "== Select ==", Value = "" }));
            ViewBag.MemberId = items;

            return View(ticket);
        }


        [HttpPost]
        //reference bsDialog in Create page
        public JsonResult ValidateCreate(TicketCreateViewModels ticket)
        {
            if (ModelState.IsValid)
            {
                PortFolioViewModels pf = new BusinessService().getPortFolio(ticket.MemberId);
                bool TradeAble = false;
                double tradeBalance = 0;

                //if (pf != null)
                //{
                //    if (ticket.TicketType == EnumHelper.TicketType.Buy.ToString())
                //    {
                //        tradeBalance = pf.CreditBuyBalance;
                //    }
                //    else
                //    {
                //        tradeBalance = pf.CreditSellBalance;
                //    }
                //    if (tradeBalance >= Convert.ToDecimal(ticket.Quantity))
                //    {
                //        TradeAble = true;
                //    }
                //}

                //return Json(new { ModelState = true, TradeAble = TradeAble, TradeBalance = tradeBalance, Msg = "หลักประกันไม่พอในการซื้อขาย" });


                var msg = "";
                if (pf != null)
                {

                    //1.check pause
                    if (ticket.TicketType == EnumHelper.TicketType.Buy.ToString())
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
                    if (ticket.Quantity > pf.MaxKg)
                    {
                        msg = "เกินจำนวนสูงสุดในการซื้อขายต่อครั้ง";
                        return Json(new { TradeAble = TradeAble, TradeBalance = tradeBalance, WarningMsg = msg });
                    }

                    //3check CreditLimit
                    double sumPending = 0;
                    if (ticket.TicketType == EnumHelper.TicketType.Buy.ToString())
                    {
                        sumPending = Convert.ToDouble(pf.SumBuy + pf.SumBuyPlaceOrder);
                    }
                    else
                    {
                        sumPending = Convert.ToDouble(pf.SumSell + pf.SumSellPlaceOrder);
                    }

                    if (ticket.Quantity > pf.CreditLimit - sumPending)
                    {
                        msg = "เกินจำนวน Credit Limit";
                        return Json(new { TradeAble = TradeAble, TradeBalance = tradeBalance, WarningMsg = msg });
                    }

                    //4check Margin
                    if (pf.MarginValue != 0)
                    {
                        //calculate collateral
                        if (ticket.TicketType == EnumHelper.TicketType.Buy.ToString())
                        {
                            tradeBalance = Convert.ToDouble(pf.CreditBuyBalance);
                        }
                        else
                        {
                            tradeBalance = Convert.ToDouble(pf.CreditSellBalance);
                        }

                        if (tradeBalance >= Convert.ToDouble(ticket.Quantity))
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
                    if (TradeAble == true && ticket.UseDeposit == true && ticket.TicketType == EnumHelper.TicketType.Sell.ToString())
                    {
                        if (ticket.Quantity > Convert.ToDouble(pf.QuantityBalanceSellGoldDep))
                        {
                            msg = "ทองฝากไม่พอสำหรับ ขายทองฝาก";
                            TradeAble = false;
                        }
                    }
                }

                return Json(new { TradeAble = TradeAble, TradeBalance = tradeBalance, WarningMsg = msg });
            }
            return Json(new { TradeAble = false, TradeBalance = 0, WarningMsg = "" });
        }

        [HttpPost]
        public JsonResult CheckPortFolio(Guid MemberId, string TradeType, decimal Quantity)
        {
            PortFolioViewModels pf = new BusinessService().getPortFolio(MemberId);
            bool TradeAble = false;
            decimal tradeBalance = 0;
            if (pf != null)
            {
                if (TradeType == EnumHelper.TicketType.Buy.ToString())
                {
                    tradeBalance = pf.CreditBuyBalance;
                }
                else
                {
                    tradeBalance = pf.CreditSellBalance;
                }
                if (tradeBalance >= Convert.ToDecimal(Quantity))
                {
                    TradeAble = true;
                }
            }
            return Json(new { TradeAble = TradeAble, TradeBalance = tradeBalance });
        }

        //public ActionResult Edit(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Ticket ticket = db.Tickets.Find(id);
        //    if (ticket == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.UpdateBy = new SelectList(db.AspNetUsers, "Id", "UserName", ticket.UpdateBy);
        //    ViewBag.CreateBy = new SelectList(db.AspNetUsers, "Id", "UserName", ticket.CreateBy);
        //    //ViewBag.MemberId = new SelectList(db.Members, "MemberId", "MemberRef", ticket.MemberId);
        //    //ViewBag.PaytypeId = new SelectList(db.Paytypes, "PaytypeId", "PaytypeName", ticket.PaytypeId);
        //    //ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "TicketStatusId", "TicketStatusName", ticket.TicketStatusId);
        //    ViewBag.Status = EnumHelper.SelectListFor<EnumHelper.TicketStatus>();
        //    return View(ticket);
        //}
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Ticket.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            var item = new TicketStatusViewModels();
            item.TicketId = ticket.TicketId;
            item.TicketRef = ticket.TicketRef;
            item.MemberName = ticket.Member.FirstName;
            item.Status = ticket.Status;
            item.Paytype = ticket.PayType;


            //ViewBag.UpdateBy = new SelectList(db.AspNetUsers, "Id", "UserName", ticket.UpdateBy);
            //ViewBag.CreateBy = new SelectList(db.AspNetUsers, "Id", "UserName", ticket.CreateBy);
            //ViewBag.MemberId = new SelectList(db.Members, "MemberId", "MemberRef", ticket.MemberId);
            //ViewBag.PaytypeId = new SelectList(db.Paytypes, "PaytypeId", "PaytypeName", ticket.PaytypeId);
            //ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "TicketStatusId", "TicketStatusName", ticket.TicketStatusId);


            //List<SelectListItem> list = new List<SelectListItem>();
            //list.Add(new SelectListItem { Value = "==Select==", Text = "" });
            //list.Add(new SelectListItem { Value = "Pending", Text = "Pending" });
            //list.Add(new SelectListItem { Value = "Complete", Text = "Complete" });
            //ViewBag.Status = new SelectList(list, "Value", "Text");


            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Value = "Pending", Text = "Pending" });
            items.Add(new SelectListItem { Value = "Complete", Text = "Complete" });
            ViewBag.TicketStatus = new SelectList(items, "Value", "Text");

            //List<SelectListItem> list = new List<SelectListItem>();
            //foreach (var value in Enum.GetValues(typeof(EnumHelper.TicketStatus)))
            //{
            //    list.Add(new SelectListItem { Value = value.ToString(), Text = value.ToString() });

            //}
            //ViewBag.Status = new SelectList(list, "Value", "Text");


            ViewBag.TicketStatus = EnumHelper.SelectListFor<EnumHelper.TicketStatus>();
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TicketStatusViewModels ticket)
        {

            if (ModelState.IsValid)
            {
                //db.Entry(ticket).State = EntityState.Modified;
                //db.SaveChanges();

                //var tk = db.Tickets.Where(w => w.TicketId == ticket.TicketId).SingleOrDefault();
                //if (tk != null)
                //{
                //    tk.Status = ticket.Status;
                //    tk.PayBy = SessionHelper.CurrentUserInfo.Id;
                //    tk.PayDate = DateTime.Now;

                //    db.SaveChanges();
                //}
                var item = db.Ticket.Find(ticket.TicketId);
                if (item != null)
                {
                    item.Status = ticket.Status;
                    item.PayType = ticket.Paytype;

                    db.Entry(item).State = EntityState.Modified;
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            ViewBag.TicketStatus = EnumHelper.SelectListFor<EnumHelper.TicketStatus>();
            return View(ticket);
        }


        #region "Finance and Casher"



        //Receive
        public ActionResult IndexRec(string pt)
        {
            if (pt == "ca")
            {
                SessionHelper.CurrentUserInfo.UserPageRole = EnumHelper.CashRoleType.Cashier.ToString();
            }
            else if (pt == "fi")
            {
                SessionHelper.CurrentUserInfo.UserPageRole = EnumHelper.CashRoleType.Finance.ToString();
            }
            if ((SessionHelper.CurrentUserInfo.UserType == EnumHelper.CashRoleType.Cashier.ToString() & SessionHelper.CurrentUserInfo.UserPageRole != EnumHelper.CashRoleType.Cashier.ToString()) || (SessionHelper.CurrentUserInfo.UserType == EnumHelper.CashRoleType.Finance.ToString() & SessionHelper.CurrentUserInfo.UserPageRole != EnumHelper.CashRoleType.Finance.ToString()))
            {
                return HttpNotFound();
            }
            return View();
        }

        [HttpPost]
        public ActionResult IndexRec(string keyword, string DateFrom, string DateTo)
        {
            var item = db.Ticket
                .Where(w => w.Active == true && w.TicketType == EnumHelper.TicketType.Buy.ToString() && w.PayStatus != EnumHelper.PayStatus.Paid.ToString())
                .Include(t => t.UserCreated).Include(t => t.Member);

            if (!string.IsNullOrEmpty(keyword))
            {
                item = item.Where(w => w.Member.FirstName.Contains(keyword));
            }

            if (!string.IsNullOrEmpty(DateFrom) && !string.IsNullOrEmpty(DateTo))
            {
                char spl = '/';
                int fYear = Convert.ToInt16(DateFrom.Split(spl)[2]);
                int fMonth = Convert.ToInt16(DateFrom.Split(spl)[0]);
                int fDay = Convert.ToInt16(DateFrom.Split(spl)[1]);
                int tYear = Convert.ToInt16(DateTo.Split(spl)[2]);
                int tMonth = Convert.ToInt16(DateTo.Split(spl)[0]);
                int tDay = Convert.ToInt16(DateTo.Split(spl)[1]);
                DateTime df = new DateTime(fYear, fMonth, fDay);
                DateTime dt = new DateTime(tYear, tMonth, tDay).AddDays(1).AddMilliseconds(-1);
                item = item.Where(w => w.CreateDate >= df & w.CreateDate <= dt);
            }
            return PartialView("FindRec", item.OrderByDescending(t => t.CreateDate));
        }

        public ActionResult EditRecPart(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Ticket.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            var item = new TicketReceivePartialViewModels();
            item.TicketId = ticket.TicketId;
            if ( SessionHelper.CurrentUserInfo.UserType == EnumHelper.UserType.Cashier.ToString() )
            {
                item.ReceiveType = EnumHelper.PayTypeCashier.Cash.ToString();
            }
            else
            {
                item.ReceiveType = "";
            }

            item.MemberName = ticket.Member.FirstName;
            item.TicketRef = ticket.TicketRef;
            item.Price = ticket.Price.Value.ToString();//for use calulate in script

            item.ReceiveList = db.TicketReceive.Where(w => w.Active == true && w.TicketId == id && w.Type == EnumHelper.TicketReceiveType.Partial.ToString())
                .OrderByDescending(w => w.CreateDate).ToList();

           
            item.TicketDetail = new BusinessService().getTicketReceiveDetails(ticket.TicketId);

            item.AmountUnPaid = (ticket.Amount.Value - item.ReceiveList.Sum(s => s.Amount).Value).ToString(StringHelper.formatnumber0Digit); // item.TicketDetail.AmtUnpaid.ToString(StringHelper.formatnumber0Digit);
            item.QtyUnpaid = item.TicketDetail.QtyUnpaid;
            item.Amount = item.AmountUnPaid;
            item.Quantity = item.QtyUnpaid;

            List<SelectListItem> items = new List<SelectListItem>();
            if (SessionHelper.CurrentUserInfo.UserPageRole == EnumHelper.CashRoleType.Finance.ToString())
            {
                items.Add(new SelectListItem { Text = "== Please select ==", Value = "", Selected = true });
                items.Add(new SelectListItem { Text = Data.Helper.EnumHelper.GetDescription(Data.Helper.EnumHelper.PayTypeFinance.Transfer), Value = Data.Helper.EnumHelper.PayTypeFinance.Transfer.ToString() });
                items.Add(new SelectListItem { Text = Data.Helper.EnumHelper.GetDescription(Data.Helper.EnumHelper.PayTypeFinance.ATS), Value = Data.Helper.EnumHelper.PayTypeFinance.ATS.ToString() });
                items.Add(new SelectListItem { Text = Data.Helper.EnumHelper.GetDescription(Data.Helper.EnumHelper.PayTypeFinance.DirectDebit), Value = Data.Helper.EnumHelper.PayTypeFinance.DirectDebit.ToString() });
                items.Add(new SelectListItem { Text = Data.Helper.EnumHelper.GetDescription(Data.Helper.EnumHelper.PayTypeFinance.Card), Value = Data.Helper.EnumHelper.PayTypeFinance.Card.ToString() });
                items.Add(new SelectListItem { Text = Data.Helper.EnumHelper.GetDescription(Data.Helper.EnumHelper.PayTypeFinance.Cheque), Value = Data.Helper.EnumHelper.PayTypeFinance.Cheque.ToString() });
            }
            else
            {
                items.Add(new SelectListItem { Text = Data.Helper.EnumHelper.GetDescription(Data.Helper.EnumHelper.PayTypeCashier.Cash), Value = Data.Helper.EnumHelper.PayTypeCashier.Cash.ToString() });
            }
            ViewBag.PayTypeList = new SelectList(items, "Value", "Text");


            items = new SelectList(db.CompanyBank.Where(b => b.Active == true).OrderByDescending(b => b.AccountName)
                    .Select(b => new { BankId = b.BankId, BankName = b.BankName + " (" + b.AccountNo + ")" }), "BankId", "BankName").ToList();
            items.Insert(0, (new SelectListItem { Text = "== Please select ==", Value = "" }));
            ViewBag.BankList = new SelectList(items, "Value", "Text");

            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditRecPart(TicketReceivePartialViewModels ticket)
        {
            if (ModelState.IsValid)
            {
                TicketReceive item = new TicketReceive();
                var tic = db.Ticket.Where(w => w.Active == true && w.TicketId == ticket.TicketId).SingleOrDefault();
                tic.PayStatus = EnumHelper.PayStatus.Partial.ToString();
                item.Ticket = tic;

                item.ReceiveId = Guid.NewGuid();
                item.TicketId = ticket.TicketId;
                int refNum = db.TicketReceive.Where(w => w.TicketId == ticket.TicketId).Count() + 1;
                item.ReceiveRef = item.Ticket.TicketRef + "-" + String.Format("{0:000}", refNum);
                item.ReceiveType = ticket.ReceiveType;
                item.Amount = Convert.ToDouble(ticket.Amount);
                item.CreateBy = SessionHelper.CurrentUserInfo.Id;
                item.CreateDate = DateTime.Now;
                item.Active = true;

                item.PayChequeNo = ticket.PayChequeNo;
                item.PayBankName = ticket.PayDetail.PayBankName;
                item.PayAccountNo = ticket.PayDetail.PayAccountNo;
                item.PayAccountName = ticket.PayDetail.PayAccountName;
                item.PayAccountType = ticket.PayDetail.PayAccountType;
                item.PayBankBranch = ticket.PayDetail.PayBankBranch;
                item.PayTransRef = ticket.PayDetail.PayTransRef;
                item.PayTime = ticket.PayDetail.PayTime;
                if (ticket.PayDetail.CompanyBankId == null)
                {
                    item.CompanyBankId = null;
                }
                else
                {
                    item.CompanyBankId = Convert.ToInt16(ticket.PayDetail.CompanyBankId);
                }
                if (!string.IsNullOrEmpty(ticket.PayDetail.PayDate))
                {
                    item.PayDate = Convert.ToDateTime(ticket.PayDetail.PayDate, Data.Helper.StringHelper.culture);
                }
                else
                {
                    item.PayDate = null;
                }


                if (item.ReceiveType == EnumHelper.MemberAssetPayTypeDetail.Cheque.ToString())
                {
                    item.PayChequeNo = ticket.PayDetail.PayChequeNo;
                    if (!string.IsNullOrEmpty(ticket.PayDetail.PayDuedate))
                    {
                        item.PayDuedate = Convert.ToDateTime(ticket.PayDetail.PayDuedate, Data.Helper.StringHelper.culture);
                    }
                    else
                    {
                        item.PayDuedate = null;
                    }
                }

                //item.Status = ticket.Status;
                item.Type = EnumHelper.TicketReceiveType.Partial.ToString();
                db.TicketReceive.Add(item);
                db.SaveChanges();

                ticket.ReceiveList = db.TicketReceive.Where(w => w.Active == true && w.TicketId == ticket.TicketId && w.Type == EnumHelper.TicketReceiveType.Partial.ToString())
                    .Include(w => w.UserCreated)
                    .OrderByDescending(w => w.CreateDate).ToList();

                ticket.TicketDetail = new BusinessService().getTicketReceiveDetails(ticket.TicketId);

                ticket.AmountUnPaid = ticket.TicketDetail.AmtUnpaid.ToString(StringHelper.formatnumber0Digit);
                ticket.QtyUnpaid = ticket.TicketDetail.QtyUnpaid;
                ticket.Amount = ticket.AmountUnPaid;
                ticket.Quantity = ticket.QtyUnpaid;

                //if (ticket.TicketDetail.AmtUnpaid == 0)
                //{
                //    var tk = db.Ticket.Find(ticket.TicketId);
                //    tk.PayStatus = EnumHelper.PayStatus.Paid.ToString();
                //    db.SaveChanges();
                //}
                //else
                //{
                //    var tk = db.Ticket.Find(ticket.TicketId);
                //    tk.PayStatus = EnumHelper.PayStatus.Partial.ToString();
                //    db.SaveChanges();
                //}
                return RedirectToAction("EditRecPart", new { id = ticket.TicketId });

            }
            return View(ticket);
        }

        public ActionResult EditPayPart(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Ticket.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            var item = new TicketPayPartialViewModels();
            item.TicketId = ticket.TicketId;
            if (SessionHelper.CurrentUserInfo.UserType == EnumHelper.UserType.Cashier.ToString())
            {
                item.PayType = EnumHelper.PayTypeCashier.Cash.ToString();
            }
            else
            {
                item.PayType = "";
            }

            item.MemberName = ticket.Member.FirstName;
            item.TicketRef = ticket.TicketRef;
            item.Price = ticket.Price.Value.ToString();//for use calulate in script

            item.PayList = db.TicketPay.Where(w => w.Active == true && w.TicketId == id && w.Type == EnumHelper.TicketPayType.Partial.ToString())
                .OrderByDescending(w => w.CreateDate).ToList();

            item.TicketDetail = new BusinessService().getTicketPayDetails(ticket.TicketId);

            item.AmountUnPaid = (ticket.Amount.Value - item.PayList.Sum(s => s.Amount).Value).ToString(StringHelper.formatnumber0Digit); // item.TicketDetail.AmtUnpaid.ToString(StringHelper.formatnumber0Digit);
            item.QtyUnpaid = item.TicketDetail.QtyUnpaid;
            item.Amount = item.AmountUnPaid;
            item.Quantity = item.QtyUnpaid;

          
            List<SelectListItem> items = new List<SelectListItem>();
            if (SessionHelper.CurrentUserInfo.UserPageRole == EnumHelper.CashRoleType.Finance.ToString())
            {
                items.Add(new SelectListItem { Text = "== Please select ==", Value = "", Selected = true });
                items.Add(new SelectListItem { Text = Data.Helper.EnumHelper.GetDescription(Data.Helper.EnumHelper.PayTypeFinance.Transfer), Value = Data.Helper.EnumHelper.PayTypeFinance.Transfer.ToString() });
                items.Add(new SelectListItem { Text = Data.Helper.EnumHelper.GetDescription(Data.Helper.EnumHelper.PayTypeFinance.ATS), Value = Data.Helper.EnumHelper.PayTypeFinance.ATS.ToString() });
                items.Add(new SelectListItem { Text = Data.Helper.EnumHelper.GetDescription(Data.Helper.EnumHelper.PayTypeFinance.DirectDebit), Value = Data.Helper.EnumHelper.PayTypeFinance.DirectDebit.ToString() });
                items.Add(new SelectListItem { Text = Data.Helper.EnumHelper.GetDescription(Data.Helper.EnumHelper.PayTypeFinance.Card), Value = Data.Helper.EnumHelper.PayTypeFinance.Card.ToString() });
                items.Add(new SelectListItem { Text = Data.Helper.EnumHelper.GetDescription(Data.Helper.EnumHelper.PayTypeFinance.Cheque), Value = Data.Helper.EnumHelper.PayTypeFinance.Cheque.ToString() });
            }
            else
            {
                items.Add(new SelectListItem { Text = Data.Helper.EnumHelper.GetDescription(Data.Helper.EnumHelper.PayTypeCashier.Cash), Value = Data.Helper.EnumHelper.PayTypeCashier.Cash.ToString() });
            }
            ViewBag.PayTypeList = new SelectList(items, "Value", "Text");

            items = new SelectList(db.CompanyBank.Where(b => b.Active == true).OrderByDescending(b => b.AccountName)
                    .Select(b => new { BankId = b.BankId, BankName = b.BankName + " (" + b.AccountNo + ")" }), "BankId", "BankName").ToList();
            items.Insert(0, (new SelectListItem { Text = "== Please select ==", Value = "" }));
            ViewBag.BankList = new SelectList(items, "Value", "Text");


            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPayPart(TicketPayPartialViewModels ticket)
        {
            if (ModelState.IsValid)
            {
                TicketPay item = new TicketPay();
                var tic = db.Ticket.Where(w => w.Active == true && w.TicketId == ticket.TicketId).SingleOrDefault();
                tic.PayStatus = EnumHelper.PayStatus.Partial.ToString();
                item.Ticket = tic;

                item.PayId = Guid.NewGuid();
                item.TicketId = ticket.TicketId;
                int refNum = db.TicketPay.Where(w => w.TicketId == ticket.TicketId).Count() + 1;
                item.PayRef = item.Ticket.TicketRef + "-" + String.Format("{0:000}", refNum);
                item.PayType = ticket.PayType;
                item.Amount = Convert.ToDouble(ticket.Amount);
                item.CreateBy = SessionHelper.CurrentUserInfo.Id;
                item.CreateDate = DateTime.Now;
                item.Active = true;

                item.PayChequeNo = ticket.PayChequeNo;
                item.PayBankName = ticket.PayDetail.PayBankName;
                item.PayAccountNo = ticket.PayDetail.PayAccountNo;
                item.PayAccountName = ticket.PayDetail.PayAccountName;
                item.PayAccountType = ticket.PayDetail.PayAccountType;
                item.PayBankBranch = ticket.PayDetail.PayBankBranch;
                item.PayTransRef = ticket.PayDetail.PayTransRef;
                item.PayTime = ticket.PayDetail.PayTime;
                if (ticket.PayDetail.CompanyBankId == null)
                {
                    item.CompanyBankId = null;
                }
                else
                {
                    item.CompanyBankId = Convert.ToInt16(ticket.PayDetail.CompanyBankId);
                }
                if (!string.IsNullOrEmpty(ticket.PayDetail.PayDate))
                {
                    item.PayDate = Convert.ToDateTime(ticket.PayDetail.PayDate, Data.Helper.StringHelper.culture);
                }
                else
                {
                    item.PayDate = null;
                }


                if (item.PayType == EnumHelper.MemberAssetPayTypeDetail.Cheque.ToString())
                {
                    item.PayChequeNo = ticket.PayDetail.PayChequeNo;
                    if (!string.IsNullOrEmpty(ticket.PayDetail.PayDuedate))
                    {
                        item.PayDuedate = Convert.ToDateTime(ticket.PayDetail.PayDuedate, Data.Helper.StringHelper.culture);
                    }
                    else
                    {
                        item.PayDuedate = null;
                    }
                }

                item.Type = EnumHelper.TicketPayType.Partial.ToString();
                db.TicketPay.Add(item);
                db.SaveChanges();

                ticket.PayList = db.TicketPay.Where(w => w.Active == true && w.TicketId == ticket.TicketId && w.Type == EnumHelper.TicketPayType.Partial.ToString())
                    .Include(w => w.UserCreated)
                    .OrderByDescending(w => w.CreateDate).ToList();

                ticket.TicketDetail = new BusinessService().getTicketPayDetails(ticket.TicketId);

                ticket.AmountUnPaid = ticket.TicketDetail.AmtUnpaid.ToString(StringHelper.formatnumber0Digit);
                ticket.QtyUnpaid = ticket.TicketDetail.QtyUnpaid;
                ticket.Amount = ticket.AmountUnPaid;
                ticket.Quantity = ticket.QtyUnpaid;

                
                return RedirectToAction("EditPayPart", new { id = ticket.TicketId });

            }
            return View(ticket);
        }


        public ActionResult EditRecSplit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Ticket.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            var item = new TicketReceiveSplitViewModels();
            item.TicketId = ticket.TicketId;
            item.Price = ticket.Price.Value.ToString();
            item.ReceiveList = db.TicketReceive.Where(t => t.TicketId == ticket.TicketId && t.Type == EnumHelper.TicketReceiveType.Split.ToString()).OrderByDescending(t => t.CreateDate).ToList();

            item.TicketDetail = new BusinessService().getTicketReceiveDetails(ticket.TicketId);
            item.AmountUnPaid = item.TicketDetail.AmtUnpaid.ToString(StringHelper.formatnumber0Digit);
            item.QtyUnpaid = item.TicketDetail.QtyUnpaid;
            item.Amount = item.AmountUnPaid;
            item.Quantity = item.QtyUnpaid;
            item.AmountPaid = item.TicketDetail.AmountPaid;
            item.MessageWarning = item.TicketDetail.MessageWarning;
            //item.AmtQty = item.TicketDetail.amtQty;
            //item.Amtq = item.TicketDetail.Amtq;

            List<SelectListItem> itemLists = new List<SelectListItem>();
            itemLists.Add(new SelectListItem { Text = EnumHelper.GetDescription(EnumHelper.TicketBuyStatus.Deposit), Value = EnumHelper.TicketBuyStatus.Deposit.ToString() });
            if (item.AmountUnPaid != "0")
            {
                itemLists.Add(new SelectListItem { Text = EnumHelper.GetDescription(EnumHelper.TicketBuyStatus.WaitGold), Value = EnumHelper.TicketBuyStatus.WaitGold.ToString() });
            }
           
            ViewBag.Status = new SelectList(itemLists, "Value", "Text");

            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditRecSplit(TicketReceiveSplitViewModels ticket)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    TicketReceive item = new TicketReceive();
                    item.Ticket = db.Ticket.Where(w => w.Active == true && w.TicketId == ticket.TicketId).SingleOrDefault();

                    item.ReceiveId = Guid.NewGuid();
                    item.TicketId = ticket.TicketId;
                    int refNum = db.TicketReceive.Where(w => w.TicketId == ticket.TicketId).Count() + 1;
                    item.ReceiveRef = item.Ticket.TicketRef + "-" + String.Format("{0:000}", refNum);
                    item.ReceiveType = ticket.ReceiveType;
                    item.Quantity = ticket.Quantity;
                    item.Amount = (ticket.Quantity * item.Ticket.Price) * 65.6;
                    item.CreateBy = SessionHelper.CurrentUserInfo.Id;
                    item.CreateDate = DateTime.Now;
                    item.Active = true;
                    item.Status = ticket.Status;
                    item.Type = EnumHelper.TicketReceiveType.Split.ToString();
                    db.TicketReceive.Add(item);
                    db.SaveChanges();

                    var rec = new TicketReceiveSplitViewModels();
                    rec.TicketId = ticket.TicketId;
                    rec.Price = ticket.Price;
                    rec.ReceiveList = db.TicketReceive.Where(t => t.TicketId == ticket.TicketId && t.Type == EnumHelper.TicketReceiveType.Split.ToString()).OrderByDescending(t => t.CreateDate).ToList();

                    rec.TicketDetail = new BusinessService().getTicketReceiveDetails(ticket.TicketId);
                    rec.AmountUnPaid = rec.TicketDetail.AmtUnpaid.ToString(StringHelper.formatnumber0Digit);
                    rec.QtyUnpaid = rec.TicketDetail.QtyUnpaid;
                    rec.Amount = rec.AmountUnPaid;
                    rec.Quantity = rec.QtyUnpaid;
                    rec.AmountPaid = rec.TicketDetail.AmountPaid;
                    rec.MessageWarning = rec.TicketDetail.MessageWarning;
                    //rec.AmtQty = rec.TicketDetail.amtQty;

                    if (rec.TicketDetail.QtyUnpaid == 0)
                    {
                        var tic = db.Ticket.Where(q => q.TicketId == ticket.TicketId).SingleOrDefault();
                        tic.PayStatus = EnumHelper.PayStatus.Paid.ToString();
                    }

                    db.SaveChanges();
                    return RedirectToAction("EditRecSplit", new { id = ticket.TicketId });
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            List<SelectListItem> itemLists = new List<SelectListItem>();
            itemLists.Add(new SelectListItem { Text = EnumHelper.GetDescription(EnumHelper.TicketBuyStatus.Deposit), Value = EnumHelper.TicketBuyStatus.Deposit.ToString() });
            itemLists.Add(new SelectListItem { Text = EnumHelper.GetDescription(EnumHelper.TicketBuyStatus.WaitGold), Value = EnumHelper.TicketBuyStatus.WaitGold.ToString() });
            ViewBag.Status = new SelectList(itemLists, "Value", "Text");

            return View(ticket);
        }

        public ActionResult EditPaySplit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Ticket.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            var item = new TicketPaySplitViewModels();
            item.TicketId = ticket.TicketId;
            item.Price = ticket.Price.Value.ToString();
            item.PayList = db.TicketPay.Where(t => t.TicketId == ticket.TicketId && t.Type == EnumHelper.TicketPayType.Split.ToString()).OrderByDescending(t => t.CreateDate).ToList();

            item.TicketDetail = new BusinessService().getTicketPayDetails(ticket.TicketId);
            item.AmountUnPaid = item.TicketDetail.AmtUnpaid.ToString(StringHelper.formatnumber0Digit);
            item.QtyUnpaid = item.TicketDetail.QtyUnpaid;
            item.Amount = item.AmountUnPaid;
            item.Quantity = item.QtyUnpaid;
            item.AmountPaid = item.TicketDetail.AmountPaid;
            item.MessageWarning = item.TicketDetail.MessageWarning;

            //List<SelectListItem> itemLists = new List<SelectListItem>();
            //itemLists.Add(new SelectListItem { Text = EnumHelper.GetDescription(EnumHelper.TicketBuyStatus.Deposit), Value = EnumHelper.TicketBuyStatus.Deposit.ToString() });
            //if (item.AmountUnPaid != "0")
            //{
            //    itemLists.Add(new SelectListItem { Text = EnumHelper.GetDescription(EnumHelper.TicketBuyStatus.WaitGold), Value = EnumHelper.TicketBuyStatus.WaitGold.ToString() });
            //}
            //ViewBag.Status = new SelectList(itemLists, "Value", "Text");
            List<SelectListItem> itemLists = new List<SelectListItem>();
            if (ticket.UseDeposit ?? false)
            {
                itemLists.Add(new SelectListItem { Text = EnumHelper.GetDescription(EnumHelper.TicketSellStatus.Cut), Value = EnumHelper.TicketSellStatus.Cut.ToString() });
            }
            else
            {
                itemLists.Add(new SelectListItem { Text = EnumHelper.GetDescription(EnumHelper.TicketSellStatus.Send), Value = EnumHelper.TicketSellStatus.Send.ToString() });
                itemLists.Add(new SelectListItem { Text = EnumHelper.GetDescription(EnumHelper.TicketSellStatus.WaitCash), Value = EnumHelper.TicketSellStatus.WaitCash.ToString() });
            }
            ViewBag.Status = new SelectList(itemLists, "Value", "Text");

            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPaySplit(TicketPaySplitViewModels ticket)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    TicketPay item = new TicketPay();
                    item.Ticket = db.Ticket.Where(w => w.Active == true && w.TicketId == ticket.TicketId).SingleOrDefault();

                    item.PayId = Guid.NewGuid();
                    item.TicketId = ticket.TicketId;
                    int refNum = db.TicketPay.Where(w => w.TicketId == ticket.TicketId).Count() + 1;
                    item.PayRef = item.Ticket.TicketRef + "-" + String.Format("{0:000}", refNum);
                    item.PayType = ticket.PayType;
                    item.Quantity = ticket.Quantity;
                    item.Amount = (ticket.Quantity * item.Ticket.Price) * 65.6;
                    item.CreateBy = SessionHelper.CurrentUserInfo.Id;
                    item.CreateDate = DateTime.Now;
                    item.Active = true;
                    item.Status = ticket.Status;
                    item.Type = EnumHelper.TicketPayType.Split.ToString();
                    db.TicketPay.Add(item);
                    db.SaveChanges();

                    var rec = new TicketPaySplitViewModels();
                    rec.TicketId = ticket.TicketId;
                    rec.Price = ticket.Price;
                    rec.PayList = db.TicketPay.Where(t => t.TicketId == ticket.TicketId && t.Type == EnumHelper.TicketPayType.Split.ToString()).OrderByDescending(t => t.CreateDate).ToList();

                    rec.TicketDetail = new BusinessService().getTicketPayDetails(ticket.TicketId);
                    rec.AmountUnPaid = rec.TicketDetail.AmtUnpaid.ToString(StringHelper.formatnumber0Digit);
                    rec.QtyUnpaid = rec.TicketDetail.QtyUnpaid;
                    rec.Amount = rec.AmountUnPaid;
                    rec.Quantity = rec.QtyUnpaid;
                    rec.AmountPaid = rec.TicketDetail.AmountPaid;
                    rec.MessageWarning = rec.TicketDetail.MessageWarning;
                    //rec.AmtQty = rec.TicketDetail.amtQty;

                    if (rec.TicketDetail.QtyUnpaid == 0)
                    {
                        var tic = db.Ticket.Where(q => q.TicketId == ticket.TicketId).SingleOrDefault();
                        tic.PayStatus = EnumHelper.PayStatus.Paid.ToString();
                    }

                    db.SaveChanges();
                    return RedirectToAction("EditPaySplit", new { id = ticket.TicketId });
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            List<SelectListItem> itemLists = new List<SelectListItem>();
            itemLists.Add(new SelectListItem { Text = EnumHelper.GetDescription(EnumHelper.TicketBuyStatus.Deposit), Value = EnumHelper.TicketBuyStatus.Deposit.ToString() });
            itemLists.Add(new SelectListItem { Text = EnumHelper.GetDescription(EnumHelper.TicketBuyStatus.WaitGold), Value = EnumHelper.TicketBuyStatus.WaitGold.ToString() });
            ViewBag.Status = new SelectList(itemLists, "Value", "Text");

            return View(ticket);
        }


        [HttpPost]
        public ActionResult checkSplitAble(Guid id, double qty,string Status)
        {
            JsonHelper.JsonResponse res = new JsonHelper.JsonResponse();
            if (ModelState.IsValid)
            {
                res.Message = new BusinessService().checkSplitAble(id, qty, Status);
                res.Status = JsonHelper.Status.Ok;
            }
            else
            {
                res.Status = JsonHelper.Status.Invalid;
                res.Message = "Data Invalid";
            }
            return Json(res);
        }

        [HttpPost]
        public ActionResult checkPaySplitAble(Guid id, double qty, string Status)
        {
            JsonHelper.JsonResponse res = new JsonHelper.JsonResponse();
            if (ModelState.IsValid)
            {
                res.Message = new BusinessService().checkPaySplitAble(id, qty, Status);
                res.Status = JsonHelper.Status.Ok;
            }
            else
            {
                res.Status = JsonHelper.Status.Invalid;
                res.Message = "Data Invalid";
            }
            return Json(res);
        }



        #region "Finance Payment"
        public ActionResult IndexPay(string pt)
        {
            if (pt == "ca")
            {
                SessionHelper.CurrentUserInfo.UserPageRole = EnumHelper.CashRoleType.Cashier.ToString();
            }
            else if (pt == "fi")
            {
                SessionHelper.CurrentUserInfo.UserPageRole = EnumHelper.CashRoleType.Finance.ToString();
            }
            if ((SessionHelper.CurrentUserInfo.UserType == EnumHelper.CashRoleType.Cashier.ToString() & SessionHelper.CurrentUserInfo.UserPageRole != EnumHelper.CashRoleType.Cashier.ToString()) || (SessionHelper.CurrentUserInfo.UserType == EnumHelper.CashRoleType.Finance.ToString() & SessionHelper.CurrentUserInfo.UserPageRole != EnumHelper.CashRoleType.Finance.ToString()))
            {
                return HttpNotFound();
            }
            return View();
        }

        [HttpPost]
        public ActionResult IndexPay(string keyword, string DateFrom, string DateTo)
        {
            var item = db.Ticket
                .Where(w => w.Active == true && w.TicketType == EnumHelper.TicketType.Sell.ToString() && w.PayStatus != EnumHelper.PayStatus.Paid.ToString())
                .Include(t => t.UserCreated).Include(t => t.Member);

            if (!string.IsNullOrEmpty(keyword))
            {
                item = item.Where(w => w.Member.FirstName.Contains(keyword));
            }

            if (!string.IsNullOrEmpty(DateFrom) && !string.IsNullOrEmpty(DateTo))
            {
                char spl = '/';
                int fYear = Convert.ToInt16(DateFrom.Split(spl)[2]);
                int fMonth = Convert.ToInt16(DateFrom.Split(spl)[0]);
                int fDay = Convert.ToInt16(DateFrom.Split(spl)[1]);
                int tYear = Convert.ToInt16(DateTo.Split(spl)[2]);
                int tMonth = Convert.ToInt16(DateTo.Split(spl)[0]);
                int tDay = Convert.ToInt16(DateTo.Split(spl)[1]);
                DateTime df = new DateTime(fYear, fMonth, fDay);
                DateTime dt = new DateTime(tYear, tMonth, tDay).AddDays(1).AddMilliseconds(-1);
                item = item.Where(w => w.CreateDate >= df & w.CreateDate <= dt);
            }
            return PartialView("FindPay", item.OrderByDescending(t => t.CreateDate));
        }

        public ActionResult EditPay(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Ticket.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            var item = new TicketPayViewModels();
            item.TicketId = ticket.TicketId;
            item.PayType = ticket.PayType;
            item.TicketDetail = ticket;

            List<SelectListItem> itemLists = new List<SelectListItem>();
            if (ticket.UseDeposit ?? false)
            {
                itemLists.Add(new SelectListItem { Text = EnumHelper.GetDescription(EnumHelper.TicketSellStatus.Cut), Value = EnumHelper.TicketSellStatus.Cut.ToString() });
            }
            else
            {
                itemLists.Add(new SelectListItem { Text = EnumHelper.GetDescription(EnumHelper.TicketSellStatus.Send), Value = EnumHelper.TicketSellStatus.Send.ToString() });
                itemLists.Add(new SelectListItem { Text = EnumHelper.GetDescription(EnumHelper.TicketSellStatus.WaitCash), Value = EnumHelper.TicketSellStatus.WaitCash.ToString() });
            }
            ViewBag.Status = new SelectList(itemLists, "Value", "Text");


            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Please select", Value = "", Selected = true });
            items.Add(new SelectListItem { Text = Data.Helper.EnumHelper.GetDescription(Data.Helper.EnumHelper.BankName.BAY), Value = Data.Helper.EnumHelper.BankName.BAY.ToString() });
            items.Add(new SelectListItem { Text = Data.Helper.EnumHelper.GetDescription(Data.Helper.EnumHelper.BankName.BBL), Value = Data.Helper.EnumHelper.BankName.BBL.ToString() });
            items.Add(new SelectListItem { Text = Data.Helper.EnumHelper.GetDescription(Data.Helper.EnumHelper.BankName.KBank), Value = Data.Helper.EnumHelper.BankName.KBank.ToString() });
            items.Add(new SelectListItem { Text = Data.Helper.EnumHelper.GetDescription(Data.Helper.EnumHelper.BankName.KTB), Value = Data.Helper.EnumHelper.BankName.KTB.ToString() });
            items.Add(new SelectListItem { Text = Data.Helper.EnumHelper.GetDescription(Data.Helper.EnumHelper.BankName.SCB), Value = Data.Helper.EnumHelper.BankName.SCB.ToString() });
            items.Add(new SelectListItem { Text = Data.Helper.EnumHelper.GetDescription(Data.Helper.EnumHelper.BankName.TBank), Value = Data.Helper.EnumHelper.BankName.TBank.ToString() });
            ViewBag.BankList = new SelectList(items, "Value", "Text");


            items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Please select", Value = "", Selected = true });
            if (SessionHelper.CurrentUserInfo.UserPageRole == EnumHelper.CashRoleType.Finance.ToString())
            {
                items.Add(new SelectListItem { Text = Data.Helper.EnumHelper.GetDescription(Data.Helper.EnumHelper.PayTypeFinance.ATS), Value = Data.Helper.EnumHelper.PayTypeFinance.ATS.ToString() });
                items.Add(new SelectListItem { Text = Data.Helper.EnumHelper.GetDescription(Data.Helper.EnumHelper.PayTypeFinance.Transfer), Value = Data.Helper.EnumHelper.PayTypeFinance.Transfer.ToString() });
                items.Add(new SelectListItem { Text = Data.Helper.EnumHelper.GetDescription(Data.Helper.EnumHelper.PayTypeFinance.Cheque), Value = Data.Helper.EnumHelper.PayTypeFinance.Cheque.ToString() });
            }
            else
            {
                items.Add(new SelectListItem { Text = Data.Helper.EnumHelper.GetDescription(Data.Helper.EnumHelper.PayTypeCashier.Cash), Value = Data.Helper.EnumHelper.PayTypeCashier.Cash.ToString() });
            }
            ViewBag.PayTypeList = new SelectList(items, "Value", "Text");

            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPay(TicketPayViewModels ticket)
        {
            if (ModelState.IsValid)
            {
                var item = db.Ticket.Find(ticket.TicketId);
                if (item != null)
                {
                    item.PayType = ticket.PayType;
                    item.PayId = SessionHelper.CurrentUserInfo.Id;
                    item.PayDate = DateTime.Now;
                    if (item.UseDeposit.Value)
                    {
                        item.PayStatus = EnumHelper.PayStatus.Paid.ToString();
                    }
                    else
                    {
                        item.PayStatus = EnumHelper.PayStatus.None.ToString();
                    }
                    item.Status = ticket.Status;

                    db.Entry(item).State = EntityState.Modified;
                    db.SaveChanges();
                }

                return RedirectToAction("IndexPay");
            }

            List<SelectListItem> itemLists = new List<SelectListItem>();
            if (ticket.TicketDetail.UseDeposit ?? false)
            {
                itemLists.Add(new SelectListItem { Text = EnumHelper.GetDescription(EnumHelper.TicketSellStatus.Cut), Value = EnumHelper.TicketSellStatus.Cut.ToString() });
            }
            else
            {
                itemLists.Add(new SelectListItem { Text = EnumHelper.GetDescription(EnumHelper.TicketSellStatus.Send), Value = EnumHelper.TicketSellStatus.Send.ToString() });
                itemLists.Add(new SelectListItem { Text = EnumHelper.GetDescription(EnumHelper.TicketSellStatus.WaitCash), Value = EnumHelper.TicketSellStatus.WaitCash.ToString() });
            }
            ViewBag.Status = new SelectList(itemLists, "Value", "Text");

            return View(ticket);
        }

        #endregion

        #region "Finance Approve"
        public ActionResult IndexApp(string pt)
        {
            if (pt == "ca")
            {
                SessionHelper.CurrentUserInfo.UserPageRole = EnumHelper.CashRoleType.Cashier.ToString();
            }
            else if (pt == "fi")
            {
                SessionHelper.CurrentUserInfo.UserPageRole = EnumHelper.CashRoleType.Finance.ToString();
            }
            if ((SessionHelper.CurrentUserInfo.UserType == EnumHelper.CashRoleType.Cashier.ToString() & SessionHelper.CurrentUserInfo.UserPageRole != EnumHelper.CashRoleType.Cashier.ToString()) || (SessionHelper.CurrentUserInfo.UserType == EnumHelper.CashRoleType.Finance.ToString() & SessionHelper.CurrentUserInfo.UserPageRole != EnumHelper.CashRoleType.Finance.ToString()))
            {
                return HttpNotFound();
            }
            return View();
        }

        [HttpPost]
        public ActionResult IndexApp(string keyword, string DateFrom, string DateTo)
        {
            var tickets = db.Ticket.Where(w => w.Active == true).Include(t => t.UserCreated).Include(t => t.Member)
                .Where(w =>
                    (w.Status == EnumHelper.TicketSellStatus.Cut.ToString() && w.ApprovePayId == null) ||
                    (w.Status == EnumHelper.TicketSellStatus.Send.ToString() && w.ApprovePayId == null && w.ApproveId != null) ||
                    (w.Status == EnumHelper.TicketSellStatus.WaitCash.ToString() && w.ApprovePayId == null && w.ApproveId != null) ||
                    (w.Status == EnumHelper.TicketBuyStatus.Deposit.ToString() && w.ApprovePayId == null) ||
                    (w.Status == EnumHelper.TicketBuyStatus.WaitGold.ToString() && w.ApprovePayId == null && w.ApproveId != null)
                );


            if (!string.IsNullOrEmpty(keyword))
            {
                tickets = tickets.Where(w => w.Member.FirstName.Contains(keyword));
            }

            if (!string.IsNullOrEmpty(DateFrom) && !string.IsNullOrEmpty(DateTo))
            {
                char spl = '/';
                int fYear = Convert.ToInt16(DateFrom.Split(spl)[2]);
                int fMonth = Convert.ToInt16(DateFrom.Split(spl)[0]);
                int fDay = Convert.ToInt16(DateFrom.Split(spl)[1]);
                int tYear = Convert.ToInt16(DateTo.Split(spl)[2]);
                int tMonth = Convert.ToInt16(DateTo.Split(spl)[0]);
                int tDay = Convert.ToInt16(DateTo.Split(spl)[1]);
                DateTime df = new DateTime(fYear, fMonth, fDay);
                DateTime dt = new DateTime(tYear, tMonth, tDay).AddDays(1).AddMilliseconds(-1);
                tickets = tickets.Where(w => w.CreateDate >= df & w.CreateDate <= dt);
            }

            var recList = db.ViewOfSumaryTicketReceive.ToList();
            var tvList = new List<TicketIndexApproveViewModels>();
            foreach (var item in tickets)
            {
                var tv = new TicketIndexApproveViewModels();
                tv.TicketId = item.TicketId;
                tv.MemberRef = item.Member.MemberRef;
                tv.MemberName = item.Member.FirstName;
                tv.TicketRef = item.TicketRef;
                tv.TradeRef = item.TradeRef;
                tv.TicketType = item.TicketType;
                tv.CreateDate = item.CreateDate.Value;
                tv.UseDeposit = item.UseDeposit;
                //if (item.UserCreated == null)
                //{
                //    tv.CreateByUser = item.UserCreated.UserName;
                //}
                //else
                //{
                //    tv.CreateByUser = item.UserCreated.UserName;
                //}
                tv.Price = item.Price.Value;
                tv.Quantity = item.Quantity.Value;
                tv.Amount = item.Amount.Value;
                tv.Status = item.Status;

                tv.QtyUnpaid = 0;
                tv.AmtUnpaid = 0;
                if (item.TicketType == EnumHelper.TicketType.Buy.ToString())
                {
                    tv.AmtPay = recList.Where(w => w.TicketId == item.TicketId).SingleOrDefault().Amount ?? 0;
                    tv.QtyPay = recList.Where(w => w.TicketId == item.TicketId).SingleOrDefault().Quantity ?? 0;
                    tv.QtyUnpaid = tv.Quantity - tv.QtyPay;
                    tv.AmtUnpaid = tv.Amount - tv.AmtPay;
                }
                tvList.Add(tv);
            }

            return PartialView("FindApp", tvList.OrderByDescending(t => t.CreateDate));
        }

        public ActionResult EditApp(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Ticket.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            var item = new TicketAppViewModels();
            item.TicketId = ticket.TicketId;
            item.TicketDetail = new BusinessService().getTicketReceiveDetails(ticket.TicketId);
            item.ReceiveList = db.TicketReceive.Where(w => w.Active == true && w.TicketId == id).OrderByDescending(w => w.CreateDate).ToList();

            //if sell and buy when ready approve will show approve button
            item.ReadyApprove = false;
            var allApprove = true;
            if (item.TicketDetail.AmtUnpaid <= 0)
            {
                foreach (var rec in item.ReceiveList)
                {
                    if (rec.ApprovePayId == null)
                    {
                        allApprove = false;
                    }
                }
            }
            if (ticket.TicketType == EnumHelper.TicketType.Sell.ToString() || allApprove == true)
            {
                item.ReadyApprove = true;
            }


            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditApp(TicketAppViewModels ticket)
        {
            if (ModelState.IsValid)
            {
                var item = db.Ticket.Find(ticket.TicketId);
                if (item != null)
                {
                    item.ApprovePayId = SessionHelper.CurrentUserInfo.Id;
                    item.ApprovePayDate = DateTime.Now;

                    //update stock
                    //var tf = new Transfer();
                    //tf.Active = true;
                    //tf.MemberId = item.MemberId;
                    //tf.CreateDate = DateTime.Now;
                    //tf.CreateBy = SessionHelper.CurrentUserInfo.Id;
                    //tf.Direction = EnumHelper.DirectionAssetType.Withdraw.ToString();
                    //tf.TransferType = EnumHelper.TransferType.Cash.ToString();
                    //tf.Note = "Withdraw Cash For Approved Order" + item.TicketRef;
                    //tf.Amount = item.Amount;
                    //tf.Status = item.Status;
                    var ca = new CompanyAsset();
                    ca.Active = true;
                    ca.CreateDate = DateTime.Now;
                    ca.CreateBy = SessionHelper.CurrentUserInfo.Id;
                    ca.Direction = EnumHelper.CompanyAssetType.Export.ToString();
                    ca.AssetType = EnumHelper.TransferType.Cash.ToString();
                    ca.Note = "Export Cash For Approved Order " + item.TicketRef;
                    ca.Amount = item.Amount;
                    ca.Status = item.Status;
                    db.CompanyAsset.Add(ca);

                    db.Entry(item).State = EntityState.Modified;
                    db.SaveChanges();
                }

                return RedirectToAction("IndexApp");
            }
            return View(ticket);
        }

        //Finance Approve Partial
        public ActionResult EditAppRec(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketReceive tr = db.TicketReceive.Find(id);
            if (tr == null)
            {
                return HttpNotFound();
            }
            var item = new TicketReceiveAppViewModels();
            item.TicketId = tr.TicketId.Value;
            item.ReceiveId = tr.ReceiveId;
            item.TicketDetail = new BusinessService().getTicketReceiveDetails(tr.TicketId.Value);
            item.TicketReceive = tr;
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAppRec(TicketReceiveAppViewModels tr)
        {
            if (ModelState.IsValid)
            {
                var item = db.TicketReceive.Find(tr.ReceiveId);
                if (item != null)
                {
                    item.ApprovePayId = SessionHelper.CurrentUserInfo.Id;
                    item.ApprovePayDate = DateTime.Now;

                    db.Entry(item).State = EntityState.Modified;
                    db.SaveChanges();
                }

                return RedirectToAction("EditApp", new { id = tr.TicketId });
            }
            return View(tr);
        }
        #endregion

        #region "Stock Approve"

        public ActionResult IndexCom()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Pending", Value = "Pending", Selected = true });
            items.Add(new SelectListItem { Text = "Complete", Value = "Complete" });
            ViewBag.Status = new SelectList(items, "Value", "Text");

            return View();
        }

        [HttpPost]
        public ActionResult IndexCom(string keyword, string DateFrom, string DateTo, string ddlStatus)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Pending", Value = "Pending", Selected = true });
            items.Add(new SelectListItem { Text = "Complete", Value = "Complete" });
            ViewBag.Status = new SelectList(items, "Value", "Text");

            var tickets = db.Ticket.Include(t => t.UserCreated).Include(t => t.Member).Where(q => q.Active == true)
                .Where(q =>
                    (q.Status == EnumHelper.TicketSellStatus.Cut.ToString() && q.ApproveId == null && q.ApprovePayId != null) ||
                    (q.Status == EnumHelper.TicketSellStatus.Send.ToString() && q.ApproveId == null) ||
                    (q.Status == EnumHelper.TicketSellStatus.WaitCash.ToString() && q.ApproveId == null) ||
                    (q.Status == EnumHelper.TicketBuyStatus.Deposit.ToString() && q.ApproveId == null) ||// && q.ApprovePayId != null) ||
                    (q.Status == EnumHelper.TicketBuyStatus.WaitGold.ToString() && q.ApproveId == null)
                );

            //if (ddlStatus == EnumHelper.TicketStatus.Pending.ToString())
            //{
            //    tickets = tickets.Where(w => w.ApprovePayId != null && w.ApproveId == null);
            //}
            //else
            //{
            //    tickets = tickets.Where(w => w.ApprovePayId != null && w.ApproveId != null);
            //}

            if (!string.IsNullOrEmpty(keyword))
            {
                tickets = tickets.Where(w => w.Member.FirstName.Contains(keyword));
            }

            if (!string.IsNullOrEmpty(DateFrom) && !string.IsNullOrEmpty(DateTo))
            {
                char spl = '/';
                int fYear = Convert.ToInt16(DateFrom.Split(spl)[2]);
                int fMonth = Convert.ToInt16(DateFrom.Split(spl)[0]);
                int fDay = Convert.ToInt16(DateFrom.Split(spl)[1]);
                int tYear = Convert.ToInt16(DateTo.Split(spl)[2]);
                int tMonth = Convert.ToInt16(DateTo.Split(spl)[0]);
                int tDay = Convert.ToInt16(DateTo.Split(spl)[1]);
                DateTime df = new DateTime(fYear, fMonth, fDay);
                DateTime dt = new DateTime(tYear, tMonth, tDay).AddDays(1).AddMilliseconds(-1);
                tickets = tickets.Where(w => w.CreateDate >= df & w.CreateDate <= dt);
            }
            var recList = db.ViewOfSumaryTicketReceive.ToList();
            var tvList = new List<TicketIndexApproveViewModels>();
            foreach (var item in tickets)
            {
                var tv = new TicketIndexApproveViewModels();
                tv.TicketId = item.TicketId;
                tv.MemberRef = item.Member.MemberRef;
                tv.MemberName = item.Member.FirstName;
                tv.TicketRef = item.TicketRef;
                tv.TradeRef = item.TradeRef;
                tv.TicketType = item.TicketType;
                tv.CreateDate = item.CreateDate.Value;
                //tv.CreateByUser = item.UserCreated.UserName;
                tv.Price = item.Price.Value;
                tv.Quantity = item.Quantity.Value;
                tv.Amount = item.Amount.Value;
                tv.Status = item.Status;
                tv.UseDeposit = item.UseDeposit;

                tv.QtyUnpaid = 0;
                tv.AmtUnpaid = 0;
                if (item.TicketType == EnumHelper.TicketType.Buy.ToString())
                {
                    tv.AmtPay = recList.Where(w => w.TicketId == item.TicketId).SingleOrDefault().Amount.Value;
                    tv.QtyPay = recList.Where(w => w.TicketId == item.TicketId).SingleOrDefault().Quantity.Value;
                    tv.QtyUnpaid = tv.Quantity - tv.QtyPay;
                    tv.AmtUnpaid = tv.Amount - tv.AmtPay;
                }

                tvList.Add(tv);
            }
            return PartialView("FindCom", tvList.OrderByDescending(t => t.CreateDate));
        }

        //Success
        public ActionResult IndexSuc()
        {
            return View();
        }

        [HttpPost]
        public ActionResult IndexSuc(string keyword, string DateFrom, string DateTo, string ddlStatus)
        {
            var tickets = db.Ticket.Where(t => t.Active == true && t.ApproveId != null)
                .Include(t => t.UserCreated).Include(t => t.Member);

            if (!string.IsNullOrEmpty(keyword))
            {
                tickets = tickets.Where(w => w.Member.FirstName.Contains(keyword));
            }

            if (!string.IsNullOrEmpty(DateFrom) && !string.IsNullOrEmpty(DateTo))
            {
                char spl = '/';
                int fYear = Convert.ToInt16(DateFrom.Split(spl)[2]);
                int fMonth = Convert.ToInt16(DateFrom.Split(spl)[0]);
                int fDay = Convert.ToInt16(DateFrom.Split(spl)[1]);
                int tYear = Convert.ToInt16(DateTo.Split(spl)[2]);
                int tMonth = Convert.ToInt16(DateTo.Split(spl)[0]);
                int tDay = Convert.ToInt16(DateTo.Split(spl)[1]);
                DateTime df = new DateTime(fYear, fMonth, fDay);
                DateTime dt = new DateTime(tYear, tMonth, tDay).AddDays(1).AddMilliseconds(-1);
                tickets = tickets.Where(w => w.CreateDate >= df & w.CreateDate <= dt);
            }
            return PartialView("FindSuc", tickets.OrderByDescending(t => t.CreateDate));
        }

        public ActionResult EditSuc(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Ticket.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }

            return View(ticket);
        }

        public ActionResult EditCom(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Ticket.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            var item = new TicketAppViewModels();
            item.TicketId = ticket.TicketId;
            item.TicketDetail = new BusinessService().getTicketReceiveDetails(ticket.TicketId);
            item.ReceiveList = db.TicketReceive.Where(w => w.Active == true && w.TicketId == id).OrderByDescending(w => w.CreateDate).ToList();

            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCom(TicketAppViewModels ticket)
        {
            if (ModelState.IsValid)
            {
                var item = db.Ticket.Find(ticket.TicketId);
                if (item != null)
                {
                    //+Gold Company,-Gold Member
                    var ca = new CompanyAsset();
                    ca.Active = true;
                    ca.CreateDate = DateTime.Now;
                    ca.CreateBy = SessionHelper.CurrentUserInfo.Id;
                    ca.Direction = EnumHelper.CompanyAssetType.Import.ToString();
                    ca.AssetType = EnumHelper.TransferType.Gold.ToString();
                    ca.Note = "Import Gold For Approved Order " + item.TicketRef;
                    ca.Quantity = item.Quantity;
                    ca.Status = item.Status;
                    db.CompanyAsset.Add(ca);

                    if (item.Status == EnumHelper.TicketSellStatus.Cut.ToString())
                    {
                        var tf = new Transfer();
                        tf.Active = true;
                        tf.MemberId = item.MemberId;
                        tf.CreateDate = DateTime.Now;
                        tf.CreateBy = SessionHelper.CurrentUserInfo.Id;
                        tf.Direction = EnumHelper.DirectionAssetType.Withdraw.ToString();
                        tf.TransferType = EnumHelper.TransferType.Gold.ToString();
                        tf.Note = "Withdraw Gold For Approved Order " + item.TicketRef;
                        tf.Quantity = item.Quantity;
                        tf.Status = item.Status;
                        db.Transfer.Add(tf);
                    }

                    item.ApproveId = SessionHelper.CurrentUserInfo.Id;
                    item.ApproveDate = DateTime.Now;

                    db.Entry(item).State = EntityState.Modified;
                    db.SaveChanges();
                }

                return RedirectToAction("IndexCom");
            }
            return View(ticket);
        }

        //Stock Approve Partial
        public ActionResult EditComRec(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketReceive tr = db.TicketReceive.Find(id);
            if (tr == null)
            {
                return HttpNotFound();
            }
            var item = new TicketReceiveAppViewModels();
            item.TicketId = tr.TicketId.Value;
            item.ReceiveId = tr.ReceiveId;
            item.TicketDetail = new BusinessService().getTicketReceiveDetails(tr.TicketId.Value);
            item.TicketReceive = tr;
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditComRec(TicketReceiveAppViewModels tr)
        {
            if (ModelState.IsValid)
            {
                var item = db.TicketReceive.Find(tr.ReceiveId);
                if (item != null)
                {
                    //update TicketReceive
                    item.ApproveId = SessionHelper.CurrentUserInfo.Id;
                    item.ApproveDate = DateTime.Now;

                    //Insert Transfer 
                    if (item.Status == EnumHelper.TicketBuyStatus.Deposit.ToString())
                    {
                        var ticket = db.Ticket.Find(tr.TicketId);
                        if (ticket.TicketType == EnumHelper.TicketType.Buy.ToString())
                        {
                            var tf = new Transfer();
                            tf.MemberId = ticket.MemberId;
                            tf.Direction = EnumHelper.DirectionAssetType.Deposit.ToString();
                            tf.CreateDate = DateTime.Now;
                            tf.CreateBy = SessionHelper.CurrentUserInfo.Id;
                            tf.Active = true;
                            tf.Note = "Deposit For Approved Order " + ticket.TicketRef;
                            tf.Quantity = ticket.Quantity;

                            db.Transfer.Add(tf);
                        }

                        //ticket.ApproveId = SessionHelper.CurrentUserInfo.Id;
                        //ticket.ApproveDate = DateTime.Now;
                    }

                    //var allApprove = true;
                    //var trList = db.TicketReceive.Where(w => w.TicketId == tr.TicketId);
                    //if (trList.Sum(q => q.Quantity).Value == tr.TicketDetail.Quantity)
                    //{

                    //}
                    //foreach (var rec in trList)
                    //{
                    //    if (rec.ReceiveId != tr.ReceiveId && rec.ApproveId == null)
                    //    {
                    //        allApprove = false;
                    //    }
                    //}
                    //if (allApprove == true)
                    //{
                    //}

                    db.Entry(item).State = EntityState.Modified;
                    db.SaveChanges();
                }

                return RedirectToAction("EditCom", new { id = tr.TicketId });
            }
            return View(tr);
        }

        #endregion
        #endregion
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
