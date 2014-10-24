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
    public class TicketReceiveController : Controller
    {
        private HSHEntities db = new HSHEntities();

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
            var rec = db.TicketReceive.Include(t => t.UserCreated).Include(t => t.Ticket)
           .Where(q => q.Active == true && q.Type == EnumHelper.TicketReceiveType.Split.ToString())
           .Where(w => ! (w.Status == EnumHelper.TicketBuyStatus.WaitGold.ToString() && w.ApprovePayId == null && w.ApproveId == null));
           //.Where(w =>
           //         (w.Status == EnumHelper.TicketBuyStatus.Deposit.ToString()  && w.ApprovePayId == null) ||
           //         (w.Status == EnumHelper.TicketBuyStatus.WaitGold.ToString() && w.ApprovePayId == null && w.ApproveId != null)
           //     );

            if (!string.IsNullOrEmpty(keyword))
            {
                rec = rec.Where(w => w.Ticket.Member.FirstName.Contains(keyword) | w.Ticket.Member.MemberRef == keyword);
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
                rec = rec.Where(w => w.CreateDate >= df & w.CreateDate <= dt);
            }

            var recList = new List<TicketReceiveApproveViewModels>();
            foreach (var item in rec)
            {
                var rc = new TicketReceiveApproveViewModels();
                rc.ReceiveId = item.ReceiveId;
                rc.ReceiveRef = item.ReceiveRef;
                rc.ReceiveType = item.ReceiveType;
                rc.Ticket = item.Ticket;
                rc.MemberName = item.Ticket.Member.FirstName;
                rc.MemberRef = item.Ticket.Member.MemberRef;
                rc.Price = item.Ticket.Price;
                rc.Quantity = item.Quantity;
                rc.Amount = item.Amount;
                rc.CreateDate = item.CreateDate.Value;
                rc.Status = item.Status;
                rc.CreateBy = item.UserCreated.UserName;
                rc.TicketType = item.Ticket.TicketType;
                if (item.Status == EnumHelper.TicketBuyStatus.Deposit.ToString() && item.ApprovePayId == null)
                {
                    rc.Approved = true;
                }
                else if (item.Status == EnumHelper.TicketBuyStatus.WaitGold.ToString() && item.ApprovePayId == null && item.ApproveId != null)
                {
                    rc.Approved = true;
                }
                else
                {
                    rc.Approved = false;
                }

                recList.Add(rc);
            }
            return PartialView("FindApp", recList.OrderByDescending(t => t.CreateDate));
        }


        public ActionResult EditApp(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketReceive item = db.TicketReceive.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }

            var rc = new TicketReceiveApproveViewModels();
            rc.ReceiveId = item.ReceiveId;
            rc.ReceiveRef = item.ReceiveRef;
            rc.MemberRef = item.Ticket.Member.MemberRef ;
            rc.ReceiveType = item.ReceiveType;
            rc.Ticket = item.Ticket;
            rc.MemberName = item.Ticket.Member.FirstName;
            rc.MemberRef = item.Ticket.Member.MemberRef;
            rc.Price = item.Ticket.Price;
            rc.Quantity = item.Quantity;
            rc.Amount = item.Amount;
            rc.CreateDate = item.CreateDate.Value;
            rc.Status = item.Status;
            rc.CreateBy = item.UserCreated.UserName;
            rc.TicketType = item.Ticket.TicketType;
            return View(rc);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditApp(TicketReceiveApproveViewModels rec)
        {
            if (ModelState.IsValid)
            {
                var item = db.TicketReceive.Find(rec.ReceiveId);
                if (item != null)
                {
                    item.ApprovePayId = SessionHelper.CurrentUserInfo.Id;
                    item.ApprovePayDate = DateTime.Now;

                    //+Cash
                    var ca = new CompanyAsset();
                    ca.Active = true;
                    ca.CreateDate = DateTime.Now;
                    ca.CreateBy = SessionHelper.CurrentUserInfo.Id;
                    ca.Direction = EnumHelper.CompanyAssetType.Import.ToString();
                    ca.AssetType = EnumHelper.TransferType.Cash.ToString();
                    ca.Note = "Import Cash For Approved Order " + item.ReceiveRef;
                    ca.Amount = item.Amount;
                    ca.Status = item.Status;
                    db.CompanyAsset.Add(ca);

                    db.Entry(item).State = EntityState.Modified;
                    db.SaveChanges();

                    var tic = db.Ticket.Find(item.TicketId);
                    var ticketReceive = db.TicketReceive.Where(q => q.TicketId == item.TicketId && q.Active == true && 
                        q.Type == EnumHelper.TicketReceiveType.Split.ToString()
                        && q.ApprovePayId != null && q.ApproveId != null && q.Quantity != null);

                    double qty = 0;
                    if (ticketReceive.Count() > 0)
                    {
                        qty = ticketReceive.Sum(q => q.Quantity).Value;
                    }
                    if (tic.Quantity == qty)
                    {
                        tic.ApproveId = SessionHelper.CurrentUserInfo.Id;
                        tic.ApprovePayId = SessionHelper.CurrentUserInfo.Id;
                        tic.ApprovePayDate = DateTime.Now;
                        tic.ApproveDate = DateTime.Now;
                        db.SaveChanges();
                    }
                }

                return RedirectToAction("IndexApp");
            }
            return View(rec);
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


        public ActionResult IndexCom()
        {
            return View();
        }

        [HttpPost]
        public ActionResult IndexCom(string keyword, string DateFrom, string DateTo)
        {
            var rec = db.TicketReceive.Include(t => t.UserCreated).Include(t => t.Ticket)
                .Where(q => q.Active == true && q.Type == EnumHelper.TicketReceiveType.Split.ToString())
                .Where(w => !(w.Status == EnumHelper.TicketBuyStatus.Deposit.ToString() && w.ApprovePayId == null && w.ApproveId == null));
                //.Where(w =>
                //    (w.Status == EnumHelper.TicketBuyStatus.WaitGold.ToString() && w.ApproveId == null) ||
                //    (w.Status == EnumHelper.TicketBuyStatus.Deposit.ToString() && w.ApproveId == null && w.ApprovePayId != null) 
                //);

            

            if (!string.IsNullOrEmpty(keyword))
            {
                rec = rec.Where(w => w.Ticket.Member.FirstName.Contains(keyword));
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
                rec = rec.Where(w => w.CreateDate >= df & w.CreateDate <= dt);
            }

            var recList = new List<TicketReceiveApproveViewModels>();
            foreach (var item in rec)
            {
                var rc = new TicketReceiveApproveViewModels();
                rc.ReceiveId = item.ReceiveId;
                rc.ReceiveRef = item.ReceiveRef;
                rc.ReceiveType = item.ReceiveType;
                rc.Ticket = item.Ticket;
                rc.MemberName = item.Ticket.Member.FirstName;
                rc.MemberRef = item.Ticket.Member.MemberRef;
                rc.Price = item.Ticket.Price;
                rc.Quantity = item.Quantity;
                rc.Amount = item.Amount;
                rc.CreateDate = item.CreateDate.Value;
                rc.Status = item.Status;
                rc.CreateBy = item.UserCreated.UserName;
                rc.TicketType = item.Ticket.TicketType;
                if (item.Status == EnumHelper.TicketBuyStatus.WaitGold.ToString() && item.ApproveId == null)
                {
                    rc.Approved = true;
                }
                else if (item.Status == EnumHelper.TicketBuyStatus.Deposit.ToString() && item.ApproveId == null && item.ApprovePayId != null)
                {
                    rc.Approved = true;
                }
                else
                {
                    rc.Approved = false;
                }
                recList.Add(rc);
            }
            return PartialView("FindCom", recList.OrderByDescending(t => t.CreateDate));
        }



        public ActionResult EditCom(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketReceive item = db.TicketReceive.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }

            var rc = new TicketReceiveApproveViewModels();
            rc.ReceiveId = item.ReceiveId;
            rc.ReceiveRef = item.ReceiveRef;
            rc.MemberRef = item.Ticket.Member.MemberRef;
            rc.ReceiveType = item.ReceiveType;
            rc.Ticket = item.Ticket;
            rc.MemberName = item.Ticket.Member.FirstName;
            rc.MemberRef = item.Ticket.Member.MemberRef;
            rc.Price = item.Ticket.Price;
            rc.Quantity = item.Quantity;
            rc.Amount = item.Amount;
            rc.CreateDate = item.CreateDate.Value;
            rc.Status = item.Status;
            rc.CreateBy = item.UserCreated.UserName;
            rc.TicketType = item.Ticket.TicketType;
            return View(rc);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCom(TicketReceiveApproveViewModels rec)
        {
            if (ModelState.IsValid)
            {
                var item = db.TicketReceive.Find(rec.ReceiveId);
                if (item != null)
                {
                    item.ApproveId = SessionHelper.CurrentUserInfo.Id;
                    item.ApproveDate = DateTime.Now;

                    //-Gold Company,+Gold Member
                    var ca = new CompanyAsset();
                    ca.Active = true;
                    ca.CreateDate = DateTime.Now;
                    ca.CreateBy = SessionHelper.CurrentUserInfo.Id;
                    ca.Direction = EnumHelper.CompanyAssetType.Export.ToString();
                    ca.AssetType = EnumHelper.TransferType.Gold.ToString();
                    ca.Note = "Export Gold For Approved Order " + item.ReceiveRef;
                    ca.Quantity = item.Quantity;
                    ca.Status = item.Status;
                    db.CompanyAsset.Add(ca);

                    if (item.Status == EnumHelper.TicketBuyStatus.Deposit.ToString())
                    {
                        var tf = new Transfer();
                        tf.Active = true;
                        tf.MemberId = item.Ticket.MemberId;
                        tf.CreateDate = DateTime.Now;
                        tf.CreateBy = SessionHelper.CurrentUserInfo.Id;
                        tf.Direction = EnumHelper.DirectionAssetType.Deposit.ToString();
                        tf.TransferType = EnumHelper.TransferType.Gold.ToString();
                        tf.Note = "Deposit Gold For Approved Order " + item.ReceiveRef;
                        tf.Quantity = item.Quantity;
                        tf.Status = item.Status;
                        db.Transfer.Add(tf);
                    }

                    db.Entry(item).State = EntityState.Modified;
                    db.SaveChanges();

                    var tic = db.Ticket.Find(item.TicketId);
                    var ticketReceive = db.TicketReceive.Where(q => q.TicketId == item.TicketId && q.Active == true &&
                        q.Type == EnumHelper.TicketReceiveType.Split.ToString()
                        && q.ApprovePayId != null && q.ApproveId != null && q.Quantity != null);

                    double qty = 0;
                    if (ticketReceive.Count() > 0)
                    {
                        qty = ticketReceive.Sum(q => q.Quantity).Value;
                    }
                    if (tic.Quantity == qty)
                    {
                        tic.ApproveId = SessionHelper.CurrentUserInfo.Id;
                        tic.ApprovePayId = SessionHelper.CurrentUserInfo.Id;
                        tic.ApprovePayDate = DateTime.Now;
                        tic.ApproveDate = DateTime.Now;
                        db.SaveChanges();
                    }

                }

                return RedirectToAction("IndexCom");
            }
            return View(rec);
        }



        public ActionResult IndexSuc()
        {
            return View();
        }

        [HttpPost]
        public ActionResult IndexSuc(string keyword, string DateFrom, string DateTo)
        {
            var rec = db.TicketReceive.Include(t => t.UserCreated).Include(t => t.Ticket)
                .Where(q => q.Active == true && q.Type == EnumHelper.TicketReceiveType.Split.ToString())
                .Where(w =>
                    (w.Status == EnumHelper.TicketBuyStatus.WaitGold.ToString() && w.ApproveId != null && w.ApprovePayId != null) ||
                    (w.Status == EnumHelper.TicketBuyStatus.Deposit.ToString() && w.ApproveId != null && w.ApprovePayId != null)
                );



            if (!string.IsNullOrEmpty(keyword))
            {
                rec = rec.Where(w => w.Ticket.Member.FirstName.Contains(keyword));
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
                rec = rec.Where(w => w.CreateDate >= df & w.CreateDate <= dt);
            }

            var recList = new List<TicketReceiveApproveViewModels>();
            foreach (var item in rec)
            {
                var rc = new TicketReceiveApproveViewModels();
                rc.ReceiveId = item.ReceiveId;
                rc.ReceiveRef = item.ReceiveRef;
                rc.ReceiveType = item.ReceiveType;
                rc.Ticket = item.Ticket;
                rc.MemberName = item.Ticket.Member.FirstName;
                rc.MemberRef = item.Ticket.Member.MemberRef;
                rc.Price = item.Ticket.Price;
                rc.Quantity = item.Quantity;
                rc.Amount = item.Amount;
                rc.CreateDate = item.CreateDate.Value;
                rc.Status = item.Status;
                rc.CreateBy = item.UserCreated.UserName;
                rc.TicketType = item.Ticket.TicketType;
                recList.Add(rc);
            }
            return PartialView("FindSuc", recList.OrderByDescending(t => t.CreateDate));
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
