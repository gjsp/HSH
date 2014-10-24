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
    public class TransfersController : Controller
    {
        private HSHEntities db = new HSHEntities();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string keyword)
        {
            var member = db.Member.Where(w => w.Active == true);

            if (!string.IsNullOrEmpty(keyword))
            {
                member = member.Where(w => w.FirstName.Contains(keyword));
            }
            var tmList = new List<TransferMemberViewModels>();
            var pf = new BusinessService();
            foreach (var mb in member)
            {
                var tm = new TransferMemberViewModels();
                tm.MemberId = mb.MemberId;
                tm.MemberDetail = mb;
                var tf = pf.getSumTransfer(mb.MemberId);
                tm.Quantity = tf.Quantity;
                tmList.Add(tm);
            }

            return PartialView("Find", tmList);
        }


        public ActionResult Edit(Guid? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Member.Find(Id);
            var ma = new TransferGoldViewModels();

            if (member == null)
            {
                return HttpNotFound();
            }
            else
            {
                ma.MemberId = member.MemberId;
                ma.Direction = EnumHelper.DirectionAssetType.Withdraw.ToString();
                ma.TransferList = db.Transfer.Where(a => a.Active == true && a.MemberId == member.MemberId)
                                    .OrderByDescending(a => a.CreateDate).ToList();
                ma.MemberPortFolio = new BusinessService().getPortFolio(Id.Value);
                ViewBag.WithdrawAble = ma.MemberPortFolio.WithdrawAble;
                ViewBag.AssetValue = 10;
            }

            //set dropdownlist
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = EnumHelper.DirectionAssetType.Withdraw.ToString(), Value = EnumHelper.DirectionAssetType.Withdraw.ToString() });
            items.Add(new SelectListItem { Text = EnumHelper.DirectionAssetType.Deposit.ToString(), Value = EnumHelper.DirectionAssetType.Deposit.ToString() });
            ViewBag.DirectionList = new SelectList(items, "Value", "Text");

            items = new List<SelectListItem>();
            for (int i = 0; i < 10; i++)
            {
                items.Add(new SelectListItem { Text = "B" + i.ToString(), Value = "B" + i.ToString() });
            }
            ViewBag.GoldBrandList = new SelectList(items, "Value", "Text");

            return View(ma);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TransferGoldViewModels tf)
        {
            if (ModelState.IsValid)
            {
                var item = new Transfer();
                if (tf.MemberId != null)
                {
                    item.MemberId = tf.MemberId;
                    item.Direction = tf.Direction;
                    item.CreateDate = DateTime.Now;
                    item.CreateBy = SessionHelper.CurrentUserInfo.Id;
                    item.Active = true;
                    item.Quantity = 1;

                    db.Transfer.Add(item);
                    db.SaveChanges();

                    //var member = db.Member.Find(tf.MemberId);
                    //tf.MemberId = member.MemberId;
                    //tf.Direction = EnumHelper.DirectionAssetType.Withdraw.ToString();
                    //tf.TransferList = db.Transfer.Where(a => a.Active == true && a.MemberId == member.MemberId)
                    //                    .Include(a=>a.Member).Include(a=>a.UserCreated)
                    //                    .OrderByDescending(a => a.CreateDate).ToList();
                    //tf.MemberPortFolio = new Business.BusinessService().getPortFolio(tf.MemberId.Value);
                    //ViewBag.WithdrawAble = tf.MemberPortFolio.WithdrawAble;
                    //ViewBag.AssetValue = 0;

                    ////set dropdownlist
                    //List<SelectListItem> items = new List<SelectListItem>();
                    //items.Add(new SelectListItem { Text = EnumHelper.DirectionAssetType.Deposit.ToString(), Value = EnumHelper.DirectionAssetType.Deposit.ToString(), Selected = true });
                    //items.Add(new SelectListItem { Text = EnumHelper.DirectionAssetType.Withdraw.ToString(), Value = EnumHelper.DirectionAssetType.Withdraw.ToString() });
                    //ViewBag.Direction = new SelectList(items, "Value", "Text");
                    return RedirectToAction("Edit", new { id = tf.MemberId });
                }
            }
            return View(tf);
        }

        [HttpPost]
        public ActionResult checkWithdrawAble(Guid id, double qty)
        {
            JsonHelper.JsonResponse res = new JsonHelper.JsonResponse();
            if (ModelState.IsValid)
            {
                res.Message = "";
                PortFolioViewModels pf = new BusinessService().getPortFolio(id);
                if (pf.WithdrawAble == false)
                {
                    res.Message = "ไม่สามารถถอนได้ เนื่องจากมีการซื้อขายค้างไว้";
                }
                else
                {
                    if (pf.DepositGold < qty)
                    {
                        res.Message = "ไม่สามารถถอนได้ เนื่องจากถอนเกินจำนวนที่ฝากไว้";
                    }
                }

                res.Status = JsonHelper.Status.Ok;
            }
            else
            {
                res.Status = JsonHelper.Status.Invalid;
                res.Message = "Data Invalid";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }


        //Company Asset

        public ActionResult IndexAsset()
        {
            CompanyAssetViewModels ca = new CompanyAssetViewModels();
            ca.CompanySummary = new BusinessService().getSummaryStockBalance();
            return View(ca);
        }

        [HttpPost]
        public ActionResult IndexAsset(string keyword, string DateFrom, string DateTo)
        {
            var item = db.CompanyAsset.Where(q => q.Active == true && q.AssetType == EnumHelper.AssetType.Gold.ToString()).Include(q => q.UserCreated);
            if (!string.IsNullOrEmpty(keyword))
            {
                item = item.Where(w => w.AssetRef.Contains(keyword));
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
            CompanyAssetViewModels ca = new CompanyAssetViewModels();
            ca.CompanyAssetList = item.OrderByDescending(o => o.CreateDate).ToList();
            //ca.CompanySummary = new Business.BusinessService().getSummaryCompany();
            return PartialView("FindAsset", ca);
        }


        public ActionResult EditAsset()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = EnumHelper.CompanyAssetType.Import.ToString(), Value = EnumHelper.CompanyAssetType.Import.ToString(), Selected = true });
            items.Add(new SelectListItem { Text = EnumHelper.CompanyAssetType.Export.ToString(), Value = EnumHelper.CompanyAssetType.Export.ToString() });
            ViewBag.Direction = new SelectList(items, "Value", "Text");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAsset(CompanyAssetViewModels ca)
        {
            if (ModelState.IsValid)
            {
                var item = new CompanyAsset();
                item.Direction = ca.Direction;
                item.CreateDate = DateTime.Now;
                item.CreateBy = SessionHelper.CurrentUserInfo.Id;
                item.Active = true;
                item.Quantity = ca.Quantity.Value;
                item.AssetType = EnumHelper.AssetType.Gold.ToString();
                db.CompanyAsset.Add(item);
                db.SaveChanges();

                return RedirectToAction("IndexAsset");
            }
            return View(ca);
        }



        //Inout Asset

        public ActionResult IndexInout()
        {
            InoutAssetViewModels io = new InoutAssetViewModels();
            io.CompanySummary = new BusinessService().getSummaryStockBalance();
            return View(io);
        }

        [HttpPost]
        public ActionResult IndexInout(string keyword, string DateFrom, string DateTo)
        {
            //Find Company Asset
            try
            {
                var itemCom = db.CompanyAsset.Where(q => q.Active == true && q.AssetType == EnumHelper.TransferType.Gold.ToString()).Include(q => q.UserCreated);
                if (!string.IsNullOrEmpty(keyword))
                {
                    itemCom = itemCom.Where(w => w.AssetRef.Contains(keyword));
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
                    itemCom = itemCom.Where(w => w.CreateDate >= df & w.CreateDate <= dt);
                }
                //Find Transfer
                var itemTrs = db.Transfer.Where(q => q.Active == true);
                if (!string.IsNullOrEmpty(keyword))
                {
                    itemTrs = itemTrs.Where(w => w.TransferRef.Contains(keyword));
                }
                if (!string.IsNullOrEmpty(DateFrom) && !string.IsNullOrEmpty(DateTo))
                {
                    char spl = '/';
                    int fYear = Convert.ToInt16(DateFrom.Split(spl)[2]);
                    int fMonth = Convert.ToInt16(DateFrom.Split(spl)[1]);
                    int fDay = Convert.ToInt16(DateFrom.Split(spl)[0]);
                    int tYear = Convert.ToInt16(DateTo.Split(spl)[2]);
                    int tMonth = Convert.ToInt16(DateTo.Split(spl)[1]);
                    int tDay = Convert.ToInt16(DateTo.Split(spl)[0]);
                    DateTime df = new DateTime(fYear, fMonth, fDay);
                    DateTime dt = new DateTime(tYear, tMonth, tDay);

                    itemTrs = itemTrs.Where(w => w.CreateDate.Value.Year >= df.Year && w.CreateDate.Value.Year <= dt.Year);
                    itemTrs = itemTrs.Where(w => w.CreateDate.Value.Month >= df.Month && w.CreateDate.Value.Month <= dt.Month);
                    itemTrs = itemTrs.Where(w => w.CreateDate.Value.Day >= df.Day && w.CreateDate.Value.Day <= dt.Day);
                }

                InoutAssetViewModels ioAsset = new InoutAssetViewModels();
                var inoutList = new List<InoutViewModels>();
                InoutViewModels io;

                foreach (var i in itemCom)
                {
                    io = new InoutViewModels();
                    io.InoutRef = i.AssetRef;
                    io.Direction = i.Direction == EnumHelper.CompanyAssetType.Import.ToString() ? EnumHelper.DirectionAssetType.In.ToString() : EnumHelper.DirectionAssetType.Out.ToString();
                    io.CreateDate = i.CreateDate;
                    io.CreateBy = i.UserCreated.UserName;
                    io.Quantity = i.Quantity;
                    inoutList.Add(io);
                }
                foreach (var i in itemTrs)
                {
                    io = new InoutViewModels();
                    io.InoutRef = i.TransferRef;
                    io.Direction = i.Direction == EnumHelper.DirectionAssetType.Deposit.ToString() ? EnumHelper.DirectionAssetType.In.ToString() : EnumHelper.DirectionAssetType.Out.ToString();
                    io.CreateDate = i.CreateDate;
                    io.CreateBy = i.UserCreated.UserName;
                    io.Quantity = i.Quantity;
                    inoutList.Add(io);
                }
                ioAsset.InoutAssetList = inoutList;
                ioAsset.CompanySummary = new BusinessService().getSummaryStockBalance();
                return PartialView("FindInout", ioAsset);
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
