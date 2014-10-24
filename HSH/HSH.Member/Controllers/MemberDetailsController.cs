using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HSH.Data.Models;
using HSH.Data.Business;
using HSH.Member.Helper;

namespace HSH.Member.Controllers
{
    [HSH.Member.Attributes.SessionExpireAttribute]
    public class MemberDetailsController : Controller
    {
        private HSHEntities db = new HSHEntities();

        public ActionResult Index()
        {
            PortFolioViewModels pf = new BusinessService().getPortFolio( SessionHelper.CurrentUserInfo.MemberId.Value);
            return View(pf);
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
