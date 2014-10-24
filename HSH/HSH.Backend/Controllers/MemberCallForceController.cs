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
    public class MemberCallForceController : Controller
    {
        private HSHEntities db = new HSHEntities();

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Index()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "ประเภทสมาชิก", Value = "", Selected = true });
            items.Add(new SelectListItem { Text = Data.Helper.EnumHelper.GetDescription(Data.Helper.EnumHelper.MemberType.Normal), Value = Data.Helper.EnumHelper.MemberType.Normal.ToString() });
            items.Add(new SelectListItem { Text = Data.Helper.EnumHelper.GetDescription(Data.Helper.EnumHelper.MemberType.Company), Value = Data.Helper.EnumHelper.MemberType.Company.ToString() });
            items.Add(new SelectListItem { Text = Data.Helper.EnumHelper.GetDescription(Data.Helper.EnumHelper.MemberType.Foreign), Value = Data.Helper.EnumHelper.MemberType.Foreign.ToString() });
            items.Add(new SelectListItem { Text = Data.Helper.EnumHelper.GetDescription(Data.Helper.EnumHelper.MemberType.Walkin), Value = Data.Helper.EnumHelper.MemberType.Walkin.ToString() });
            items.Add(new SelectListItem { Text = Data.Helper.EnumHelper.GetDescription(Data.Helper.EnumHelper.MemberType.VIP), Value = Data.Helper.EnumHelper.MemberType.VIP.ToString() });
            ViewBag.MemberTypeList = new SelectList(items, "Value", "Text");

            return View();
        }

        [HttpPost]
        public ActionResult Index(string keyword, string ddlMemberType)
        {
            try
            {
                var member = db.Member.Where(w => w.Active == true && w.ApproveRegisterId != null);

                if (!string.IsNullOrEmpty(keyword))
                {
                    member = member.Where(w => w.FirstName.Contains(keyword) | w.MemberRef == keyword);
                }

                if (ddlMemberType != "")
                {
                    member = member.Where(w => w.MemberType == ddlMemberType);
                }

                var mcf = new List<MemberCallForceViewModels>();
                var pf = new BusinessService();
                foreach (var mb in member)
                {
                    var mc = new MemberCallForceViewModels();
                    mc.MemberId = mb.MemberId;
                    mc.MemberDetail = mb;
                    mc.PortFolio = pf.getPortFolio(mb.MemberId);
                    mc.CallForce = pf.getMarketPrice().Ask99Bg.ToString();
                    var cf = pf.getCallForce(mb.MemberId);
                    mc.CallForce = cf.CallForce;
                    mc.ForceBuy = cf.ForceBuy;
                    mc.ForceSell = cf.ForceSell;
                    mcf.Add(mc);
                }
                
                return PartialView("Find", mcf);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public ActionResult Edit(Guid id,string PauseType,string Pause)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var mem = db.Member.Find(id);
            if (mem == null)
            {
                return HttpNotFound();
            }
            var item = new MemberCallForcePauseViewModels();
            item.MemberId = mem.MemberId;
            item.PauseType = PauseType;
            item.MemberDetail = mem;
            if (Pause=="Pause")
            {
                item.Paused = false;
            }
            else
            {
                item.Paused = true ;
            }
            item.PortFolio = new BusinessService().getPortFolio(mem.MemberId);
            ViewBag.Title = "Member Pause " + PauseType;
            return View(item);
        }

        [HttpPost]
        public ActionResult Edit(MemberCallForcePauseViewModels mem)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var item = db.Member.Find(mem.MemberId);
                    if (item != null)
                    {
                        if (mem.PauseType == EnumHelper.TicketType.Buy.ToString())
                        {
                            item.PauseBuy = mem.Paused ;
                        }
                        else
                        {
                            item.PauseSell = mem.Paused ;
                        }

                        db.Entry(item).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                catch (Exception)
                {
                    return View();
                }
                return RedirectToAction("Index");
            }
            return View(mem);
        }
    
    }
}
