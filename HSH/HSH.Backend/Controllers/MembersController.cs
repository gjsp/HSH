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
using HSH.Data.Helper;


namespace HSH.Backend.Controllers
{
    [SessionExpireAttribute]
    public class MembersController : Controller
    {
        private HSHEntities db = new HSHEntities();

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
        public ActionResult Index(string keyword, string DateFrom, string DateTo, string ddlMemberType)
        {
            try
            {
                var item = from i in db.Member select i;
                if (!string.IsNullOrEmpty(keyword))
                {
                    item = item.Where(i => i.FirstName.Contains(keyword) | i.MemberRef == keyword);
                }
                if (!string.IsNullOrEmpty(DateFrom) && !string.IsNullOrEmpty(DateTo))
                {
                    if (keyword.Length != 7) //if find member ref don't search date
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
                }
                if (ddlMemberType != "")
                {
                    item = item.Where(w => w.MemberType == ddlMemberType);
                }
                return PartialView("Find", item.OrderByDescending(m => m.CreateDate));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public ActionResult IndexColCash(string pt)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "ประเภทสมาชิก", Value = "", Selected = true });
            items.Add(new SelectListItem { Text = Data.Helper.EnumHelper.GetDescription(Data.Helper.EnumHelper.MemberType.Normal), Value = Data.Helper.EnumHelper.MemberType.Normal.ToString() });
            items.Add(new SelectListItem { Text = Data.Helper.EnumHelper.GetDescription(Data.Helper.EnumHelper.MemberType.Company), Value = Data.Helper.EnumHelper.MemberType.Company.ToString() });
            items.Add(new SelectListItem { Text = Data.Helper.EnumHelper.GetDescription(Data.Helper.EnumHelper.MemberType.Foreign), Value = Data.Helper.EnumHelper.MemberType.Foreign.ToString() });
            items.Add(new SelectListItem { Text = Data.Helper.EnumHelper.GetDescription(Data.Helper.EnumHelper.MemberType.Walkin), Value = Data.Helper.EnumHelper.MemberType.Walkin.ToString() });
            items.Add(new SelectListItem { Text = Data.Helper.EnumHelper.GetDescription(Data.Helper.EnumHelper.MemberType.VIP), Value = Data.Helper.EnumHelper.MemberType.VIP.ToString() });
            ViewBag.MemberTypeList = new SelectList(items, "Value", "Text");

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
        public ActionResult IndexColCash(string keyword, string DateFrom, string DateTo, string ddlMemberType)
        {
            var item = from i in db.Member where i.Active == true select i;
            if (!string.IsNullOrEmpty(keyword))
            {
                item = item.Where(i => i.FirstName.Contains(keyword) | i.MemberRef == keyword);
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
            if (ddlMemberType != "")
            {
                item = item.Where(w => w.MemberType == ddlMemberType);
            }
            return PartialView("FindColCash", item.OrderByDescending(m => m.CreateDate));
        }


        public ActionResult IndexColGold()
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
        public ActionResult IndexColGold(string keyword, string DateFrom, string DateTo, string ddlMemberType)
        {
            var item = from i in db.Member where i.Active == true select i;
            if (!string.IsNullOrEmpty(keyword))
            {
                item = item.Where(i => i.FirstName.Contains(keyword) | i.MemberRef == keyword);
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
            if (ddlMemberType != "")
            {
                item = item.Where(w => w.MemberType == ddlMemberType);
            }
            return PartialView("FindColGold", item.OrderByDescending(m => m.CreateDate));
        }



        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Member.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        public ActionResult Detail(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Member.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        public ActionResult Create()
        {
            //set dropdownlist
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "บุคคลธรรมดา", Value = Data.Helper.EnumHelper.MemberClass.Individual.ToString(), Selected = true });
            items.Add(new SelectListItem { Text = "นิติบุคคล", Value = Data.Helper.EnumHelper.MemberClass.Corperate.ToString() });
            ViewBag.MemberClass = new SelectList(items, "Value", "Text");

            items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "บุคคลธรรมดา", Value = "Normal", Selected = true });
            items.Add(new SelectListItem { Text = "ต่างประเทศ", Value = "Foreign" });
            items.Add(new SelectListItem { Text = "ร้านทอง", Value = "Shop" });
            items.Add(new SelectListItem { Text = "บริษัท", Value = "Company" });
            items.Add(new SelectListItem { Text = "ห้างหุ้นส่วน", Value = "Partner" });
            ViewBag.MemberType = new SelectList(items, "Value", "Text");

            items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Mr", Value = "Mr", Selected = true });
            items.Add(new SelectListItem { Text = "MRS", Value = "MRS" });
            items.Add(new SelectListItem { Text = "MISS", Value = "MISS" });
            ViewBag.MemberTitle = new SelectList(items, "Value", "Text");


            items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Group 1", Value = "1", Selected = true });
            items.Add(new SelectListItem { Text = "Group 2", Value = "2" });
            items.Add(new SelectListItem { Text = "Group 3", Value = "3" });
            ViewBag.MemberGroup = new SelectList(items, "Value", "Text");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MemberViewModels item)
        {
            if (ModelState.IsValid)
            {
                var mb = new Member();
                mb.MemberId = Guid.NewGuid();
                mb.MemberClass = item.MemberClass;
                mb.MemberType = item.MemberType;


                string MemberRefNum;
                if (item.MemberType == Data.Helper.EnumHelper.MemberType.Normal.ToString())
                {
                    MemberRefNum = "03";
                }
                else if (item.MemberType == Data.Helper.EnumHelper.MemberType.VIP.ToString())
                {
                    MemberRefNum = "09";
                }
                else if (item.MemberType == Data.Helper.EnumHelper.MemberType.Walkin.ToString())
                {
                    MemberRefNum = "06";
                }
                else
                {
                    MemberRefNum = "00";
                }

                mb.MemberRef = MemberRefNum + "-" + String.Format("{0:0000}", db.Member.Count() + 1);
                mb.MemberName = item.MemberName;
                mb.MemberGroup = item.MemberGroup;
                mb.MemberTitle = item.MemberTitle;
                //mb.CollateralCash = item.CollateralCash;
                //mb.CollateralGold = item.CollateralGold;
                mb.FirstName = item.FirstName;
                mb.LastName = item.LastName;
                mb.BirthDate = item.BirthDate;
                // mb.BirthDate = Convert.ToDateTime(item.BirthDate, new System.Globalization.CultureInfo("en-US", false));
                mb.PassportId = item.PassportId;
                mb.DateOfIssue = item.DateOfIssue;
                mb.DateOfExpire = item.DateOfExpire;
                mb.Political = item.Political;
                mb.HomeNo = item.HomeNo;
                mb.HomeSoi = item.HomeSoi;
                mb.HomeStreet = item.HomeStreet;
                mb.HomeSubDistrict = item.HomeSubDistrict;
                mb.HomeDistrict = item.HomeDistrict;
                mb.HomeProvince = item.HomeProvince;
                mb.HomePostcode = item.HomePostcode;
                mb.PresentAddress = item.PresentAddress;
                //mb.Mobile = item.Mobile;
                mb.Telephone = item.Telephone;
                mb.Fax = item.Fax;
                mb.Email = item.Email;
                mb.WorkType = item.WorkType;
                mb.WorkPlace = item.WorkPlace;
                mb.WorkBusinessType = item.WorkBusinessType;
                mb.WorkTelephone = item.WorkTelephone;
                mb.WorkFax = item.WorkFax;
                mb.Status = item.Status;
                mb.MateName = item.MateName;
                mb.MateCareer = item.MateCareer;
                mb.MatePolitical = item.MatePolitical;
                mb.MateTelephone = item.MateTelephone;
                mb.MateWorkplace = item.MateWorkplace;
                mb.MateBusinessType = item.MateBusinessType;
                mb.EmergencyPersonName = item.EmergencyPersonName;
                mb.EmergencyRelationship = item.EmergencyRelationship;
                mb.EmergencyAddress = item.EmergencyAddress;
                mb.EmergencyTelephone = item.EmergencyTelephone;
                mb.CorporateType = item.CorporateType;
                mb.CorporateName = item.CorporateName;
                mb.CorporateBusinessType = item.CorporateBusinessType;
                mb.CorporateTradeRegNo = item.CorporateTradeRegNo;
                mb.CorporateTaxNo = item.CorporateTaxNo;
                mb.CorporateHomeNo = item.CorporateHomeNo;
                mb.CorporateHomeSoi = item.CorporateHomeSoi;
                mb.CorporateHomeStreet = item.CorporateHomeStreet;
                mb.CorporateHomeSubDistrict = item.CorporateHomeSubDistrict;
                mb.CorporateHomeDistrict = item.CorporateHomeDistrict;
                mb.CorporateHomeProvince = item.CorporateHomeProvince;
                mb.CorporateHomePostcode = item.CorporateHomePostcode;
                mb.CorporateTelephone = item.CorporateTelephone;
                mb.CorporateFax = item.CorporateFax;
                mb.CorporatePresidentSigned = item.CorporatePresidentSigned;
                mb.CorporateContactName = item.CorporateContactName;
                mb.CorporateContactPassportId = item.CorporateContactPassportId;
                mb.CorporateContactTelephone = item.CorporateContactTelephone;
                mb.CorporateContactEmail = item.CorporateContactEmail;
                mb.PayType = item.PayType;
                mb.PayBank = item.PayBank;
                mb.PayBankBanch = item.PayBankBanch;
                mb.PayAccountType = item.PayAccountType;
                mb.PayAccountNo = item.PayAccountNo;
                mb.PayAccountName = item.PayAccountName;
                mb.PayBankApprove = item.PayBankApprove;
                mb.PayApproveId = item.PayApproveId;
                mb.PayApproveBy = item.PayApproveBy;
                mb.PayApporveDate = item.PayApporveDate;
                mb.Active = false;

                db.Member.Add(mb);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ////set dropdownlist
            //List<SelectListItem> items = new List<SelectListItem>();
            //items.Add(new SelectListItem { Text = "บุคคลธรรมดา", Value = "individuals", Selected = true });
            //items.Add(new SelectListItem { Text = "นิติบุคคล", Value = "corporation" });
            //ViewBag.MemberClass = new SelectList(items, "Value", "Text");

            //items = new List<SelectListItem>();
            //items.Add(new SelectListItem { Text = "บุคคลธรรมดา", Value = "Normal", Selected = true });
            //items.Add(new SelectListItem { Text = "ต่างประเทศ", Value = "Foreign" });
            //items.Add(new SelectListItem { Text = "ร้านทอง", Value = "Shop" });
            //items.Add(new SelectListItem { Text = "บริษัท", Value = "Company" });
            //items.Add(new SelectListItem { Text = "ห้างหุ้นส่วน", Value = "Partner" });
            //ViewBag.MemberType = new SelectList(items, "Value", "Text");

            //items = new List<SelectListItem>();
            //items.Add(new SelectListItem { Text = "Mr", Value = "Mr", Selected = true });
            //items.Add(new SelectListItem { Text = "MRS", Value = "MRS" });
            //items.Add(new SelectListItem { Text = "MISS", Value = "MISS" });
            //ViewBag.MemberTitle = new SelectList(items, "Value", "Text");


            //items = new List<SelectListItem>();
            //items.Add(new SelectListItem { Text = "Group 1", Value = "1", Selected = true });
            //items.Add(new SelectListItem { Text = "Group 2", Value = "2" });
            //items.Add(new SelectListItem { Text = "Group 3", Value = "3" });
            //ViewBag.MemberGroup = new SelectList(items, "Value", "Text");

            return View(item);
        }


        public ActionResult CreateMemberIndi()
        {
            //set dropdownlist

            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = Data.Helper.EnumHelper.GetDescription(Data.Helper.EnumHelper.MemberType.Normal), Value = Data.Helper.EnumHelper.MemberType.Normal.ToString(), Selected = true });
            //items.Add(new SelectListItem { Text = Data.Helper.EnumHelper.GetDescription(Data.Helper.EnumHelper.MemberType.Shop), Value = Data.Helper.EnumHelper.MemberType.Shop.ToString() });
            //items.Add(new SelectListItem { Text = Data.Helper.EnumHelper.GetDescription(Data.Helper.EnumHelper.MemberType.Company), Value = Data.Helper.EnumHelper.MemberType.Company.ToString() });
            items.Add(new SelectListItem { Text = Data.Helper.EnumHelper.GetDescription(Data.Helper.EnumHelper.MemberType.Foreign), Value = Data.Helper.EnumHelper.MemberType.Foreign.ToString() });
            items.Add(new SelectListItem { Text = Data.Helper.EnumHelper.GetDescription(Data.Helper.EnumHelper.MemberType.Walkin), Value = Data.Helper.EnumHelper.MemberType.Walkin.ToString() });
            items.Add(new SelectListItem { Text = Data.Helper.EnumHelper.GetDescription(Data.Helper.EnumHelper.MemberType.VIP), Value = Data.Helper.EnumHelper.MemberType.VIP.ToString() });
            ViewBag.MemberTypeList = new SelectList(items, "Value", "Text");

            //items = new List<SelectListItem>();
            //items.Add(new SelectListItem { Text = "นาย", Value = "นาย", Selected = true });
            //items.Add(new SelectListItem { Text = "นาง", Value = "นาง" });
            //items.Add(new SelectListItem { Text = "นางสาว", Value = "นางสาว" });
            //ViewBag.MemberTitle = new SelectList(items, "Value", "Text");

            //items = new List<SelectListItem>();
            //items.Add(new SelectListItem { Text = "Group 1", Value = "1", Selected = true });
            //items.Add(new SelectListItem { Text = "Group 2", Value = "2" });
            //items.Add(new SelectListItem { Text = "Group 3", Value = "3" });
            //ViewBag.MemberGroup = new SelectList(items, "Value", "Text");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateMemberIndi(MemberIndividualViewModels item)
        {
            if (ModelState.IsValid)
            {
                bool IsValidate = true;
                string memPassId = checkDupplicatePassportId(item.PassportId);
                string MemEmail = checkDupplicateEmail(item.Email);
                if (memPassId != "")
                {
                    IsValidate = false;
                    ModelState.AddModelError("PassportId", memPassId);
                }
                if (MemEmail != "")
                {
                    IsValidate = false;
                    ModelState.AddModelError("Email", MemEmail);
                }

                if (IsValidate)
                {
                    var mb = new Member();
                    mb.MemberId = Guid.NewGuid();
                    mb.MemberClass = Data.Helper.EnumHelper.MemberClass.Individual.ToString();
                    mb.MemberType = item.MemberType;

                    string MemberRefNum = "";
                    //Change Requirment shop --> merge to normal is normal/shop
                    //if (item.MemberType == Data.Helper.EnumHelper.MemberType.Shop.ToString())
                    //{
                    //    MemberRefNum = "05";
                    //}
                    //else 
                    if (item.MemberType == Data.Helper.EnumHelper.MemberType.Normal.ToString())
                    {
                        MemberRefNum = "03";
                    }
                    else if (item.MemberType == Data.Helper.EnumHelper.MemberType.VIP.ToString())
                    {
                        MemberRefNum = "09";
                    }
                    else if (item.MemberType == Data.Helper.EnumHelper.MemberType.Walkin.ToString())
                    {
                        MemberRefNum = "00";
                    }
                    else if (item.MemberType == Data.Helper.EnumHelper.MemberType.Foreign.ToString())
                    {
                        MemberRefNum = "07";
                    }


                    double memNumber = db.Member.Count();
                    mb.MemberRef = MemberRefNum + String.Format("{0:00000}", memNumber + 1);
                    //mb.MemberName = item.MemberName;
                    mb.MemberGroup = int.Parse(item.MemberGroup);
                    mb.MarketingName = item.MarketingName;
                    mb.MemberTitle = item.MemberTitle;
                    mb.FirstName = item.FirstName;
                    mb.LastName = item.LastName;
                    mb.PassportId = item.PassportId;

                    if (!string.IsNullOrEmpty(item.BirthDate))
                    {
                        mb.BirthDate = Convert.ToDateTime(item.BirthDate, Data.Helper.StringHelper.culture);
                    }
                    else
                    {
                        mb.BirthDate = null;
                    }
                    if (!string.IsNullOrEmpty(item.DateOfIssue))
                    {
                        mb.DateOfIssue = Convert.ToDateTime(item.DateOfIssue, Data.Helper.StringHelper.culture);
                    }
                    else
                    {
                        mb.DateOfIssue = null;
                    }
                    if (!string.IsNullOrEmpty(item.DateOfExpire))
                    {
                        mb.DateOfExpire = Convert.ToDateTime(item.DateOfExpire, Data.Helper.StringHelper.culture);
                    }
                    else
                    {
                        mb.DateOfExpire = null;
                    }

                    mb.Political = item.Political;
                    mb.PassportNo = item.PassportNo;
                    mb.GovernmentCard = item.GovernmentCard;
                    mb.Sex = item.Sex;
                    mb.Nation = item.Nation;
                    mb.Ethnicity = item.Ethnicity;
                    mb.Religion = item.Religion;
                    mb.Education = item.Education;
                    mb.IncomeMonth = item.IncomeMonth;


                    //mb.HomeNo = item.HomeNo;
                    //mb.HomeSoi = item.HomeSoi;
                    //mb.HomeStreet = item.HomeStreet;
                    //mb.HomeSubDistrict = item.HomeSubDistrict;
                    //mb.HomeDistrict = item.HomeDistrict;
                    //mb.HomeProvince = item.HomeProvince;
                    //mb.HomePostcode = item.HomePostcode;
                    mb.HomeAddress = item.HomeAddress;
                    mb.PresentAddress = item.PresentAddress;
                    //mb.Mobile = item.Mobile;
                    mb.Telephone = item.Telephone;
                    mb.Fax = item.Fax;
                    mb.Email = item.Email;

                    //bank
                    mb.BankName = item.BankName;
                    mb.BankAccountNo = item.BankAccountNo;
                    mb.BankAccountName = item.BankAccountName;
                    mb.BankAccountType = item.BankAccountType;
                    mb.BankBranch = item.BankBranch;
                    mb.BankPayType = item.BankPayType;

                    //work
                    mb.WorkType = item.WorkType;
                    mb.WorkPlace = item.WorkPlace;
                    mb.WorkBusinessType = item.WorkBusinessType;
                    mb.WorkTelephone = item.WorkTelephone;
                    mb.WorkFax = item.WorkFax;
                    mb.Status = item.Status;
                    mb.MateName = item.MateName;
                    mb.MateCareer = item.MateCareer;
                    mb.MatePolitical = item.MatePolitical;
                    mb.MateTelephone = item.MateTelephone;
                    mb.MateWorkplace = item.MateWorkplace;
                    mb.MateBusinessType = item.MateBusinessType;
                    mb.EmergencyPersonName = item.EmergencyPersonName;
                    mb.EmergencyRelationship = item.EmergencyRelationship;
                    mb.EmergencyAddress = item.EmergencyAddress;
                    mb.EmergencyTelephone = item.EmergencyTelephone;

                    mb.AuthorizerName = item.AuthorizerName;
                    mb.AuthorizerId = item.AuthorizerId;
                    mb.AuthorizerAddress = item.AuthorizerAddress;
                    mb.AuthorizerTel = item.AuthorizerTel;


                    mb.CreateDate = DateTime.Now;
                    mb.CreateBy = Helper.SessionHelper.CurrentUserInfo.Id;
                    //mb.Active = false;

                    db.Member.Add(mb);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            //List<SelectListItem> items = new List<SelectListItem>();
            //items.Add(new SelectListItem { Text = Data.Helper.EnumHelper.GetDescription(Data.Helper.EnumHelper.MemberType.Normal), Value = Data.Helper.EnumHelper.MemberType.Normal.ToString(), Selected = true });
            ////items.Add(new SelectListItem { Text = Data.Helper.EnumHelper.GetDescription(Data.Helper.EnumHelper.MemberType.Shop), Value = Data.Helper.EnumHelper.MemberType.Shop.ToString() });
            ////items.Add(new SelectListItem { Text = Data.Helper.EnumHelper.GetDescription(Data.Helper.EnumHelper.MemberType.Company), Value = Data.Helper.EnumHelper.MemberType.Company.ToString() });
            //items.Add(new SelectListItem { Text = Data.Helper.EnumHelper.GetDescription(Data.Helper.EnumHelper.MemberType.Foreign), Value = Data.Helper.EnumHelper.MemberType.Foreign.ToString() });
            //items.Add(new SelectListItem { Text = Data.Helper.EnumHelper.GetDescription(Data.Helper.EnumHelper.MemberType.Walkin), Value = Data.Helper.EnumHelper.MemberType.Walkin.ToString() });
            //items.Add(new SelectListItem { Text = Data.Helper.EnumHelper.GetDescription(Data.Helper.EnumHelper.MemberType.VIP), Value = Data.Helper.EnumHelper.MemberType.VIP.ToString() });
            //ViewBag.MemberType = new SelectList(items, "Value", "Text");

            //items = new List<SelectListItem>();
            //items.Add(new SelectListItem { Text = "Group 1", Value = "1", Selected = true });
            //items.Add(new SelectListItem { Text = "Group 2", Value = "2" });
            //items.Add(new SelectListItem { Text = "Group 3", Value = "3" });
            //ViewBag.MemberGroup = new SelectList(items, "Value", "Text");

            return View(item);
        }

        [HttpPost]
        public JsonResult ValidateCreateIndi(MemberIndividualViewModels item)
        {
            //JsonHelper.JsonResponse res = new JsonHelper.JsonResponse();
            //res.Message = "";
            //res.ModelState = false;
            if (ModelState.IsValid)
            {
                //if (ModelState.IsValid)
                //{
                //    res.Message = new Business.BusinessService().checkSplitAble(id, qty);
                //    res.Status = JsonHelper.Status.Ok;
                //}
                //else
                //{
                //    res.Status = JsonHelper.Status.Invalid;
                //    res.Message = "Data Invalid";
                //}

                var mb = db.Member.Where(q => q.PassportId == item.PassportId);
                if (mb.Count() > 0)
                {
                    //res.Message = "เลขบัตรประชาชนซ้ำ"; 
                }
                //res.Status = JsonHelper.Status.Ok;
                //res.ModelState = true;
                return Json(new { ModelState = true, Msg = "" });
            }
            return Json(new { ModelState = false });
            //// return Json(res);
        }

        public ActionResult EditMemberIndi(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member item = db.Member.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            var indi = new MemberIndividualViewModels();

            indi.MemberId = item.MemberId;
            indi.MemberRef = item.MemberRef;
            indi.MemberLevel = item.MemberLevel;
            indi.MemberType = item.MemberType;
            indi.MemberGroup = item.MemberGroup.Value.ToString();
            indi.MarketingName = item.MarketingName;
            indi.MemberTitle = item.MemberTitle;
            indi.FirstName = item.FirstName;
            indi.LastName = item.LastName;
            indi.BirthDate = item.BirthDate == null ? "" : item.BirthDate.Value.ToString(Data.Helper.StringHelper.formatdate);
            indi.DateOfIssue = item.DateOfIssue == null ? "" : item.DateOfIssue.Value.ToString(Data.Helper.StringHelper.formatdate);
            indi.DateOfExpire = item.DateOfExpire == null ? "" : item.DateOfExpire.Value.ToString(Data.Helper.StringHelper.formatdate);

            indi.PassportId = item.PassportId;
            indi.Political = item.Political;
            indi.PassportNo = item.PassportNo;
            indi.GovernmentCard = item.GovernmentCard;
            indi.Sex = item.Sex;
            indi.Nation = item.Nation;
            indi.Ethnicity = item.Ethnicity;
            indi.Religion = item.Religion;
            indi.Education = item.Education;
            indi.IncomeMonth = item.IncomeMonth;

            //bank
            indi.BankName = item.BankName;
            indi.BankAccountNo = item.BankAccountNo;
            indi.BankAccountName = item.BankAccountName;
            indi.BankAccountType = item.BankAccountType;
            indi.BankBranch = item.BankBranch;
            indi.BankPayType = item.BankPayType;

            //indi.HomeNo = item.HomeNo;
            //indi.HomeSoi = item.HomeSoi;
            //indi.HomeStreet = item.HomeStreet;
            //indi.HomeSubDistrict = item.HomeSubDistrict;
            //indi.HomeDistrict = item.HomeDistrict;
            //indi.HomeProvince = item.HomeProvince;
            //indi.HomePostcode = item.HomePostcode;
            indi.HomeAddress = item.HomeAddress;
            indi.PresentAddress = item.PresentAddress;
            //indi.Mobile = item.Mobile;
            indi.Telephone = item.Telephone;
            indi.Fax = item.Fax;
            indi.Email = item.Email;
            indi.WorkType = item.WorkType;
            indi.WorkPlace = item.WorkPlace;
            indi.WorkBusinessType = item.WorkBusinessType;
            indi.WorkTelephone = item.WorkTelephone;
            indi.WorkFax = item.WorkFax;
            indi.Status = item.Status;
            indi.MateName = item.MateName;
            indi.MateCareer = item.MateCareer;
            indi.MatePolitical = item.MatePolitical;
            indi.MateTelephone = item.MateTelephone;
            indi.MateWorkplace = item.MateWorkplace;
            indi.MateBusinessType = item.MateBusinessType;
            indi.EmergencyPersonName = item.EmergencyPersonName;
            indi.EmergencyRelationship = item.EmergencyRelationship;
            indi.EmergencyAddress = item.EmergencyAddress;
            indi.EmergencyTelephone = item.EmergencyTelephone;

            indi.AuthorizerName = item.AuthorizerName;
            indi.AuthorizerId = item.AuthorizerId;
            indi.AuthorizerAddress = item.AuthorizerAddress;
            indi.AuthorizerTel = item.AuthorizerTel;
            //set dropdownlist
            //List<SelectListItem> items = new List<SelectListItem>();
            //if (indi.MemberType == Data.Helper.EnumHelper.MemberIndiType.Normal.ToString())
            //{
            //    ViewBag.MemberType = Data.Helper.EnumHelper.SelectListFor(Data.Helper.EnumHelper.MemberIndiType.Normal);
            //}
            //else if (indi.MemberType == Data.Helper.EnumHelper.MemberIndiType.Foreign.ToString())
            //{
            //    ViewBag.MemberType = Data.Helper.EnumHelper.SelectListFor(Data.Helper.EnumHelper.MemberIndiType.Foreign);
            //}
            //else if (indi.MemberType == Data.Helper.EnumHelper.MemberIndiType.Walkin.ToString())
            //{
            //    ViewBag.MemberType = Data.Helper.EnumHelper.SelectListFor(Data.Helper.EnumHelper.MemberIndiType.Walkin);
            //}
            //else if (indi.MemberType == Data.Helper.EnumHelper.MemberIndiType.VIP.ToString())
            //{
            //    ViewBag.MemberType = Data.Helper.EnumHelper.SelectListFor(Data.Helper.EnumHelper.MemberIndiType.VIP);
            //}

            //items = new List<SelectListItem>();
            //items.Add(new SelectListItem { Text = "นาย", Value = "นาย" });
            //items.Add(new SelectListItem { Text = "นาง", Value = "นาง" });
            //items.Add(new SelectListItem { Text = "นางสาว", Value = "นางสาว" });
            //ViewBag.MemberTitleList = new SelectList(items, "Value", "Text");

            //items = new List<SelectListItem>();
            //items.Add(new SelectListItem { Text = "Group 1", Value = "1" });
            //items.Add(new SelectListItem { Text = "Group 2", Value = "2" });
            //items.Add(new SelectListItem { Text = "Group 3", Value = "3" });
            //ViewBag.MemberGroupList = new SelectList(items, "Value", "Text", item.MemberGroup);


            return View(indi);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditMemberIndi(MemberIndividualViewModels item)
        {
            if (ModelState.IsValid)
            {
                var mb = db.Member.Find(item.MemberId);
                if (item != null)
                {
                    //bool IsValidate = true;
                    //if (item.PassportId != mb.PassportId)
                    //{
                    //    var memPassId = db.Member.Where(q => q.Active != false && q.PassportId == item.PassportId);
                    //    if (memPassId.Count() > 0)
                    //    {
                    //        IsValidate = false;
                    //        ModelState.AddModelError("PassportId", "เลขที่บัตรประชาชนของคุณ ไม่สามารถใช้งานได้");
                    //    }
                    //}
                    //if (item.Email != mb.Email)
                    //{
                    //    var MemEmail = db.Member.Where(q => q.Active != false && q.Email == item.Email);
                    //    if (MemEmail.Count() > 0)
                    //    {
                    //        IsValidate = false;
                    //        ModelState.AddModelError("Email", "อีเมล์ของคุณ นี้ไม่สามารถใช้งานได้");
                    //    }
                    //}
                    bool IsValidate = true;
                    string memPassId = checkDupplicatePassportId(item.PassportId);
                    string MemEmail = checkDupplicateEmail(item.Email);
                    if (memPassId != "")
                    {
                        IsValidate = false;
                        ModelState.AddModelError("PassportId", memPassId);
                    }
                    if (MemEmail != "")
                    {
                        IsValidate = false;
                        ModelState.AddModelError("Email", MemEmail);
                    }

                    if (IsValidate)
                    {
                        mb.MemberId = item.MemberId;
                        mb.MemberRef = item.MemberRef;
                        mb.MemberType = item.MemberType;
                        mb.MemberGroup = int.Parse(item.MemberGroup);
                        mb.MarketingName = item.MarketingName;
                        mb.FirstName = item.FirstName;
                        mb.LastName = item.LastName;
                        if (!string.IsNullOrEmpty(item.BirthDate))
                        {
                            mb.BirthDate = Convert.ToDateTime(item.BirthDate, Data.Helper.StringHelper.culture);
                        }
                        else
                        {
                            mb.BirthDate = null;
                        }
                        if (!string.IsNullOrEmpty(item.DateOfIssue))
                        {
                            mb.DateOfIssue = Convert.ToDateTime(item.DateOfIssue, Data.Helper.StringHelper.culture);
                        }
                        else
                        {
                            mb.DateOfIssue = null;
                        }
                        if (!string.IsNullOrEmpty(item.DateOfExpire))
                        {
                            mb.DateOfExpire = Convert.ToDateTime(item.DateOfExpire, Data.Helper.StringHelper.culture);
                        }
                        else
                        {
                            mb.DateOfExpire = null;
                        }

                        mb.PassportId = item.PassportId;
                        mb.Political = item.Political;
                        mb.PassportNo = item.PassportNo;
                        mb.GovernmentCard = item.GovernmentCard;
                        mb.Sex = item.Sex;
                        mb.Nation = item.Nation;
                        mb.Ethnicity = item.Ethnicity;
                        mb.Religion = item.Religion;
                        mb.Education = item.Education;

                        //bank
                        mb.BankName = item.BankName;
                        mb.BankAccountNo = item.BankAccountNo;
                        mb.BankAccountName = item.BankAccountName;
                        mb.BankAccountType = item.BankAccountType;
                        mb.BankBranch = item.BankBranch;
                        mb.BankPayType = item.BankPayType;

                        //mb.HomeNo = item.HomeNo;
                        //mb.HomeSoi = item.HomeSoi;
                        //mb.HomeStreet = item.HomeStreet;
                        //mb.HomeSubDistrict = item.HomeSubDistrict;
                        //mb.HomeDistrict = item.HomeDistrict;
                        //mb.HomeProvince = item.HomeProvince;
                        //mb.HomePostcode = item.HomePostcode;
                        mb.HomeAddress = item.HomeAddress;
                        mb.PresentAddress = item.PresentAddress;
                        //mb.Mobile = item.Mobile;
                        mb.Telephone = item.Telephone;
                        mb.Fax = item.Fax;
                        mb.Email = item.Email;
                        mb.WorkType = item.WorkType;
                        mb.WorkPlace = item.WorkPlace;
                        mb.WorkBusinessType = item.WorkBusinessType;
                        mb.WorkTelephone = item.WorkTelephone;
                        mb.WorkFax = item.WorkFax;
                        mb.Status = item.Status;
                        mb.MateName = item.MateName;
                        mb.MateCareer = item.MateCareer;
                        mb.MatePolitical = item.MatePolitical;
                        mb.MateTelephone = item.MateTelephone;
                        mb.MateWorkplace = item.MateWorkplace;
                        mb.MateBusinessType = item.MateBusinessType;
                        mb.EmergencyPersonName = item.EmergencyPersonName;
                        mb.EmergencyRelationship = item.EmergencyRelationship;
                        mb.EmergencyAddress = item.EmergencyAddress;
                        mb.EmergencyTelephone = item.EmergencyTelephone;

                        mb.AuthorizerName = item.AuthorizerName;
                        mb.AuthorizerId = item.AuthorizerId;
                        mb.AuthorizerAddress = item.AuthorizerAddress;
                        mb.AuthorizerTel = item.AuthorizerTel;

                        db.Entry(mb).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }

            }

            return View(item);
        }


        public ActionResult DetailMemberIndi(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member item = db.Member.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            var indi = new MemberIndividualViewModels();

            indi.MemberId = item.MemberId;
            indi.MemberRef = item.MemberRef;
            indi.MemberLevel = item.MemberLevel;
            indi.MemberType = item.MemberType;
            indi.MemberGroup = item.MemberGroup.Value.ToString();
            indi.MemberTitle = item.MemberTitle;
            indi.FirstName = item.FirstName;
            indi.LastName = item.LastName;
            indi.BirthDate = item.BirthDate == null ? "" : item.BirthDate.Value.ToString(Data.Helper.StringHelper.formatdate);
            indi.PassportId = item.PassportId;
            indi.DateOfIssue = item.DateOfIssue == null ? "" : item.DateOfIssue.Value.ToString(Data.Helper.StringHelper.formatdate);
            indi.DateOfExpire = item.DateOfExpire == null ? "" : item.DateOfExpire.Value.ToString(Data.Helper.StringHelper.formatdate);

            indi.Political = item.Political; //== null ? "" : item.Political.Value == true ? "" : "";

            indi.HomeAddress = item.HomeAddress;
            indi.PresentAddress = item.PresentAddress;
            //indi.Mobile = item.Mobile;
            indi.Telephone = item.Telephone;
            indi.Fax = item.Fax;
            indi.Email = item.Email;
            indi.WorkType = item.WorkType;
            indi.WorkPlace = item.WorkPlace;
            indi.WorkBusinessType = item.WorkBusinessType;
            indi.WorkTelephone = item.WorkTelephone;
            indi.WorkFax = item.WorkFax;
            indi.Status = item.Status;
            indi.MateName = item.MateName;
            indi.MateCareer = item.MateCareer;
            indi.MatePolitical = item.MatePolitical;
            indi.MateTelephone = item.MateTelephone;
            indi.MateWorkplace = item.MateWorkplace;
            indi.MateBusinessType = item.MateBusinessType;
            indi.EmergencyPersonName = item.EmergencyPersonName;
            indi.EmergencyRelationship = item.EmergencyRelationship;
            indi.EmergencyAddress = item.EmergencyAddress;
            indi.EmergencyTelephone = item.EmergencyTelephone;

            //bank
            indi.BankName = item.BankName;
            indi.BankAccountNo = item.BankAccountNo;
            indi.BankAccountName = item.BankAccountName;
            indi.BankAccountType = item.BankAccountType;
            indi.BankBranch = item.BankBranch;
            indi.BankPayType = item.BankPayType;

            indi.AuthorizerName = item.AuthorizerName;
            indi.AuthorizerId = item.AuthorizerId;
            indi.AuthorizerAddress = item.AuthorizerAddress;
            indi.AuthorizerTel = item.AuthorizerTel;

            //set dropdownlist
            List<SelectListItem> items = new List<SelectListItem>();
            //items.Add(new SelectListItem { Text = Data.Helper.EnumHelper.GetDescription(Data.Helper.EnumHelper.MemberType.Normal), Value = Data.Helper.EnumHelper.MemberType.Normal.ToString() });
            //items.Add(new SelectListItem { Text = Data.Helper.EnumHelper.GetDescription(Data.Helper.EnumHelper.MemberType.Shop), Value = Data.Helper.EnumHelper.MemberType.Shop.ToString() });
            ////items.Add(new SelectListItem { Text = Data.Helper.EnumHelper.GetDescription(Data.Helper.EnumHelper.MemberType.Company), Value = Data.Helper.EnumHelper.MemberType.Company.ToString() });
            //items.Add(new SelectListItem { Text = Data.Helper.EnumHelper.GetDescription(Data.Helper.EnumHelper.MemberType.Foreign), Value = Data.Helper.EnumHelper.MemberType.Foreign.ToString() });
            //items.Add(new SelectListItem { Text = Data.Helper.EnumHelper.GetDescription(Data.Helper.EnumHelper.MemberType.Walkin), Value = Data.Helper.EnumHelper.MemberType.Walkin.ToString() });
            //items.Add(new SelectListItem { Text = Data.Helper.EnumHelper.GetDescription(Data.Helper.EnumHelper.MemberType.VIP), Value = Data.Helper.EnumHelper.MemberType.VIP.ToString()});
            //ViewBag.MemberType = new SelectList(items, "Value", "Text", indi.MemberType);
            //ViewBag.MemberType = Data.Helper.EnumHelper.SelectListFor<Data.Helper.EnumHelper.MemberType>();

            if (indi.MemberType == Data.Helper.EnumHelper.MemberIndiType.Normal.ToString())
            {
                ViewBag.MemberType = Data.Helper.EnumHelper.SelectListFor(Data.Helper.EnumHelper.MemberIndiType.Normal);
            }
            else if (indi.MemberType == Data.Helper.EnumHelper.MemberIndiType.Foreign.ToString())
            {
                ViewBag.MemberType = Data.Helper.EnumHelper.SelectListFor(Data.Helper.EnumHelper.MemberIndiType.Foreign);
            }
            else if (indi.MemberType == Data.Helper.EnumHelper.MemberIndiType.Walkin.ToString())
            {
                ViewBag.MemberType = Data.Helper.EnumHelper.SelectListFor(Data.Helper.EnumHelper.MemberIndiType.Walkin);
            }
            else if (indi.MemberType == Data.Helper.EnumHelper.MemberIndiType.VIP.ToString())
            {
                ViewBag.MemberType = Data.Helper.EnumHelper.SelectListFor(Data.Helper.EnumHelper.MemberIndiType.VIP);
            }

            items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "นาย", Value = "นาย" });
            items.Add(new SelectListItem { Text = "นาง", Value = "นาง" });
            items.Add(new SelectListItem { Text = "นางสาว", Value = "นางสาว" });
            ViewBag.MemberTitleList = new SelectList(items, "Value", "Text");

            items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Group 1", Value = "1" });
            items.Add(new SelectListItem { Text = "Group 2", Value = "2" });
            items.Add(new SelectListItem { Text = "Group 3", Value = "3" });
            ViewBag.MemberGroupList = new SelectList(items, "Value", "Text", item.MemberGroup);

            return View(indi);

        }




        public ActionResult CreateMemberCorp()
        {
            //set dropdownlist
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Group 1", Value = "1", Selected = true });
            items.Add(new SelectListItem { Text = "Group 2", Value = "2" });
            items.Add(new SelectListItem { Text = "Group 3", Value = "3" });
            ViewBag.MemberGroup = new SelectList(items, "Value", "Text");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateMemberCorp(MemberCorporateViewModels item)
        {
            if (ModelState.IsValid)
            {
                bool IsValidate = true;
                string memPassId = checkDupplicatePassportId(item.PassportId);
                string MemEmail = checkDupplicateEmail(item.Email);
                if (memPassId != "")
                {
                    IsValidate = false;
                    ModelState.AddModelError("PassportId", memPassId);
                }
                if (MemEmail != "")
                {
                    IsValidate = false;
                    ModelState.AddModelError("Email", MemEmail);
                }

                if (IsValidate)
                {
                    var mb = new Member();
                    mb.MemberId = Guid.NewGuid();
                    mb.MemberType = Data.Helper.EnumHelper.MemberType.Company.ToString();

                    string MemberRefNum = "05";
                    double memNumber = db.Member.Count();
                    mb.MemberRef = MemberRefNum + String.Format("{0:00000}", memNumber + 1);
                    //mb.CorporateType = item.CorporateType;
                    mb.MemberGroup = item.MemberGroup;
                    mb.MarketingName = item.MarketingName;
                    mb.FirstName = item.FirstName;
                    mb.PassportId = item.PassportId;

                    mb.PresentAddress = item.PresentAddress;
                    mb.Telephone = item.Telephone;
                    mb.Fax = item.Fax;
                    mb.Email = item.Email;

                    mb.CorporateBusinessType = item.CorporateBusinessType;
                    mb.CorporateTradeRegNo = item.CorporateTradeRegNo;
                    mb.CorporateTaxNo = item.CorporateTaxNo;
                    mb.CorporatePresidentSigned = item.CorporatePresidentSigned;
                    mb.CorporateContactName = item.CorporateContactName;

                    //mb.HasAuthorizer  =   item.HasAuthorizer ;
                    mb.Nation = item.Nation;
                    mb.IncomeMonth = item.IncomeMonth;

                    //Share Holder
                    mb.CorporateShareHolder1 = item.CorporateShareHolder1;
                    mb.CorporateShareHolderRate1 = item.CorporateShareHolderRate1;
                    mb.CorporateShareHolder2 = item.CorporateShareHolder2;
                    mb.CorporateShareHolderRate2 = item.CorporateShareHolderRate2;
                    mb.CorporateShareHolder3 = item.CorporateShareHolder3;
                    mb.CorporateShareHolderRate3 = item.CorporateShareHolderRate3;
                    mb.CorporateShareHolder4 = item.CorporateShareHolder4;
                    mb.CorporateShareHolderRate4 = item.CorporateShareHolderRate4;
                    mb.CorporateShareHolder5 = item.CorporateShareHolder5;
                    mb.CorporateShareHolderRate5 = item.CorporateShareHolderRate5;

                    //authorizer
                    mb.AuthorizerName = item.AuthorizerName;
                    mb.AuthorizerId = item.AuthorizerId;
                    mb.AuthorizerAddress = item.AuthorizerAddress;
                    mb.AuthorizerTel = item.AuthorizerTel;
                    mb.AuthorizerEmail = item.AuthorizerEmail;

                    //Attorney
                    mb.AttorneyName = item.AttorneyName;
                    mb.AttorneyId = item.AttorneyId;
                    mb.AttorneyAddress = item.AttorneyAddress;
                    mb.AttorneyTel = item.AttorneyTel;
                    mb.AttorneyEmail = item.AttorneyEmail;

                    //Bank
                    mb.BankName = item.BankName;
                    mb.BankAccountName = item.BankAccountName;
                    mb.BankAccountNo = item.BankAccountNo;
                    mb.BankAccountType = item.BankAccountType;
                    mb.BankBranch = item.BankBranch;
                    mb.BankPayType = item.BankPayType;


                    mb.CreateDate = DateTime.Now;
                    mb.CreateBy = Helper.SessionHelper.CurrentUserInfo.Id;
                    //mb.Active = false;

                    db.Member.Add(mb);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(item);
        }

        public string checkDupplicatePassportId(string txt)
        {
            try
            {
                var memPassId = db.Member.Where(q => q.Active != false && q.PassportId == txt);
                if (memPassId.Count() > 0)
                {
                    return "เลขที่บัตรประชาชนของคุณ ไม่สามารถใช้งานได้";
                }
                return "";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string checkDupplicateEmail(string txt)
        {
            try
            {
                var memPassId = db.Member.Where(q => q.Active != false && q.Email == txt);
                if (memPassId.Count() > 0)
                {
                    return "อีเมล์ของคุณ นี้ไม่สามารถใช้งานได้";
                }
                return "";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public ActionResult EditMemberCorp(Guid? id)
        {
            //set dropdownlist
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Group 1", Value = "1", Selected = true });
            items.Add(new SelectListItem { Text = "Group 2", Value = "2" });
            items.Add(new SelectListItem { Text = "Group 3", Value = "3" });
            ViewBag.MemberGroupList = new SelectList(items, "Value", "Text");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member item = db.Member.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            var corp = new MemberCorporateViewModels();
            corp.MemberId = item.MemberId;
            corp.MemberRef = item.MemberRef;
            corp.MemberLevel = item.MemberLevel;
            corp.MemberGroup = item.MemberGroup;
            corp.MarketingName = item.MarketingName;
            corp.FirstName = item.FirstName;
            corp.PresentAddress = item.PresentAddress;
            corp.PassportId = item.PassportId;

            corp.Telephone = item.Telephone;
            corp.Fax = item.Fax;
            corp.Email = item.Email;
            corp.CorporateBusinessType = item.CorporateBusinessType;
            corp.CorporateTradeRegNo = item.CorporateTradeRegNo;
            corp.CorporateTaxNo = item.CorporateTaxNo;
            corp.CorporatePresidentSigned = item.CorporatePresidentSigned;
            corp.CorporateContactName = item.CorporateContactName;

            corp.Nation = item.Nation;
            corp.IncomeMonth = item.IncomeMonth;

            //Share Holder
            corp.CorporateShareHolder1 = item.CorporateShareHolder1;
            corp.CorporateShareHolderRate1 = item.CorporateShareHolderRate1;
            corp.CorporateShareHolder2 = item.CorporateShareHolder2;
            corp.CorporateShareHolderRate2 = item.CorporateShareHolderRate2;
            corp.CorporateShareHolder3 = item.CorporateShareHolder3;
            corp.CorporateShareHolderRate3 = item.CorporateShareHolderRate3;
            corp.CorporateShareHolder4 = item.CorporateShareHolder4;
            corp.CorporateShareHolderRate4 = item.CorporateShareHolderRate4;
            corp.CorporateShareHolder5 = item.CorporateShareHolder5;
            corp.CorporateShareHolderRate5 = item.CorporateShareHolderRate5;

            //authorizer
            corp.AuthorizerName = item.AuthorizerName;
            corp.AuthorizerId = item.AuthorizerId;
            corp.AuthorizerAddress = item.AuthorizerAddress;
            corp.AuthorizerTel = item.AuthorizerTel;
            corp.AuthorizerEmail = item.AuthorizerEmail;

            //Attorney
            corp.AttorneyName = item.AttorneyName;
            corp.AttorneyId = item.AttorneyId;
            corp.AttorneyAddress = item.AttorneyAddress;
            corp.AttorneyTel = item.AttorneyTel;
            corp.AttorneyEmail = item.AttorneyEmail;

            //Bank
            corp.BankName = item.BankName;
            corp.BankAccountName = item.BankAccountName;
            corp.BankAccountNo = item.BankAccountNo;
            corp.BankAccountType = item.BankAccountType;
            corp.BankBranch = item.BankBranch;
            corp.BankPayType = item.BankPayType;

            return View(corp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditMemberCorp(MemberCorporateViewModels item)
        {
            if (ModelState.IsValid)
            {
                var mb = db.Member.Find(item.MemberId);
                if (item != null)
                {
                    mb.MemberId = item.MemberId;
                    mb.MemberRef = item.MemberRef;
                    mb.MemberGroup = item.MemberGroup;
                    //mb.MemberType = item.MemberType;
                    mb.FirstName = item.FirstName;
                    mb.PresentAddress = item.PresentAddress;
                    mb.PassportId = item.PassportId;
                    mb.Telephone = item.Telephone;
                    mb.Fax = item.Fax;
                    mb.Email = item.Email;
                    mb.CorporateBusinessType = item.CorporateBusinessType;
                    mb.CorporateTradeRegNo = item.CorporateTradeRegNo;
                    mb.CorporateTaxNo = item.CorporateTaxNo;
                    mb.CorporatePresidentSigned = item.CorporatePresidentSigned;
                    mb.CorporateContactName = item.CorporateContactName;

                    db.Entry(mb).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

            }
            //set dropdownlist
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Group 1", Value = "1", Selected = true });
            items.Add(new SelectListItem { Text = "Group 2", Value = "2" });
            items.Add(new SelectListItem { Text = "Group 3", Value = "3" });
            ViewBag.MemberGroup = new SelectList(items, "Value", "Text");
            return View(item);
        }

        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Member.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MemberId,MemberRef,MemberClass,MemberType,MemberName,MemberGroup,MemberTitle,FirstName,LastName,BirthDate,PassportId,DateOfIssue,DateOfExpire,Political,HomeNo,HomeSoi,HomeStreet,HomeSubDistrict,HomeDistrict,HomeProvince,HomePostcode,PresentAddress,Mobile,Telephone,Fax,Email,WorkType,WorkPlace,WorkBusinessType,WorkTelephone,WorkFax,Status,MateName,MateCareer,MatePolitical,MateTelephone,MateWorkplace,MateBusinessType,EmergencyPersonName,EmergencyRelationship,EmergencyAddress,EmergencyTelephone,CorporateType,CorporateName,CorporateBusinessType,CorporateTradeRegNo,CorporateTaxNo,CorporateHomeNo,CorporateHomeSoi,CorporateHomeStreet,CorporateHomeSubDistrict,CorporateHomeDistrict,CorporateHomeProvince,CorporateHomePostcode,CorporateTelephone,CorporateFax,CorporatePresidentSigned,CorporateContactName,CorporateContactPassportId,CorporateContactTelephone,CorporateContactEmail,PayType,PayBank,PayBankBanch,PayAccountType,PayAccountNo,PayAccountName,PayBankApprove,PayApproveBy,PayApporveDate,GuaranteeAmount,GuaranteeApprovedAmount")] Member member)
        {
            if (ModelState.IsValid)
            {
                db.Entry(member).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(member);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }



        #region [Risk]


        public ActionResult IndexApproveCollateral()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "ประเภทหลักประกัน", Value = "", Selected = true });
            items.Add(new SelectListItem { Text = Data.Helper.EnumHelper.GetDescription(Data.Helper.EnumHelper.AssetType.Gold), Value = Data.Helper.EnumHelper.AssetType.Gold.ToString() });
            items.Add(new SelectListItem { Text = Data.Helper.EnumHelper.GetDescription(Data.Helper.EnumHelper.AssetType.Cash), Value = Data.Helper.EnumHelper.AssetType.Cash.ToString() });
            ViewBag.AssetTypeList = new SelectList(items, "Value", "Text");
            return View();
        }

        [HttpPost]
        public ActionResult IndexApproveCollateral(string keyword, string DateFrom, string DateTo, string ddlAssetType)
        {
            //var itemWD = db.MemberAsset.Where(q => q.AssetType == "Gold" && q.Direction == "Out").Select(q => q.AssetId).ToArray();
            var item = db.MemberAsset.Include(q => q.Member);
            if (!string.IsNullOrEmpty(keyword))
            {
                item = item.Where(i => i.Member.FirstName.Contains(keyword) | i.Member.MemberRef == keyword);
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
            if (ddlAssetType != "")
            {
                item = item.Where(w => w.AssetType == ddlAssetType);
            }
            return PartialView("FindApproveCollateralView", item.OrderByDescending(i => i.CreateDate));
        }


        public ActionResult IndexApproveCollateralCash(string pt)
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
        public ActionResult IndexApproveCollateralCash(string keyword, string DateFrom, string DateTo)
        {
            var item = db.MemberAsset.Include(q => q.Member).Where(q => q.AssetType == Data.Helper.EnumHelper.AssetType.Cash.ToString());
            if (!string.IsNullOrEmpty(keyword))
            {
                item = item.Where(i => i.Member.FirstName.Contains(keyword) | i.Member.MemberRef == keyword);
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

            Session[Data.Helper.StringHelper.stringCurrentPageIndex] = this.ControllerContext.RouteData.Values["action"].ToString();
            return PartialView("FindApproveCollateralCash", item.OrderByDescending(i => i.CreateDate));
        }
        public ActionResult IndexApproveCollateralGold()
        {
            return View();
        }

        [HttpPost]
        public ActionResult IndexApproveCollateralGold(string keyword, string DateFrom, string DateTo)
        {
            var item = db.MemberAsset.Include(q => q.Member).Where(q => q.AssetType == Data.Helper.EnumHelper.AssetType.Gold.ToString());
            if (!string.IsNullOrEmpty(keyword))
            {
                item = item.Where(i => i.Member.FirstName.Contains(keyword) | i.Member.MemberRef == keyword);
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

            Session[Data.Helper.StringHelper.stringCurrentPageIndex] = this.ControllerContext.RouteData.Values["action"].ToString();
            return PartialView("FindApproveCollateralGold", item.OrderByDescending(i => i.CreateDate));
        }





        public ActionResult ApproveCollateral(Guid? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var ma = db.MemberAsset.Find(Id);
            if (ma == null)
            {
                return HttpNotFound();
            }
            var mav = new MemberAssetViewModels();
            mav.AssetId = ma.AssetId;
            mav.MemberPortFolio = new Data.Business.BusinessService().getPortFolio(ma.MemberId.Value);//  new busins BusinessService().getPortFolio(ma.MemberId.Value);
            //if (type == HSH.Data.Helper.EnumHelper.DirectionAssetType.Withdraw.ToString())
            //{
            //    ViewBag.Title = "ยืนยันอนุมัติ การถอนหลักประกัน";
            //}
            //else
            //{
            //    ViewBag.Title = "ยืนยันอนุมัติ การฝากหลักประกัน";
            //}

            return View(mav);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ApproveCollateral(MemberAssetViewModels ma)
        {
            if (ModelState.IsValid)
            {
                var item = db.MemberAsset.Find(ma.AssetId);
                if (item != null)
                {
                    item.ApproveDate = DateTime.Now;
                    item.ApproveId = Helper.SessionHelper.CurrentUserInfo.Id;
                    item.Status = ma.Status;

                    if (item.Direction == EnumHelper.DirectionAssetType.Out.ToString())
                    {
                        //View Select Portfolio
                        //SUM(CASE WHEN (AssetType = 'Cash' AND Direction = 'Out' AND NOT [Status] = 'DisApproved') THEN - AssetCash WHEN (AssetType = 'Cash' AND Direction = 'In' AND [Status] = 'Approved') THEN AssetCash ELSE 0 END) AS AssetCash, 
                        //SUM(CASE WHEN (AssetType = 'Gold' AND Direction = 'Out' AND NOT [Status] = 'DisApproved') THEN - AssetGold WHEN (AssetType = 'Gold' AND Direction = 'In' AND [Status] = 'Approved') THEN AssetGold ELSE 0 END) AS AssetGold

                        //Withdraw
                        var depItem = db.MemberAsset.Where(q => q.WithdrawId == ma.AssetId).SingleOrDefault(); //get deposit for update
                        if (depItem != null)
                        {
                            if (ma.Status == EnumHelper.MemberAssetStatus.Approved.ToString())
                            {   //Approve Withdraw
                                depItem.WithdrawStatus = EnumHelper.MemberAssetStatus.Approved.ToString();
                                depItem.WithdrawId = ma.AssetId;
                            }
                            else
                            {   //Cancel Withdraw
                                depItem.WithdrawStatus = null;
                                depItem.WithdrawId = null;
                                item.WithdrawId = depItem.AssetId; //stap id for view later
                            }
                        }
                    }

                    db.SaveChanges();
                }
                return RedirectToAction(Session[Data.Helper.StringHelper.stringCurrentPageIndex].ToString());
            }

            return View(ma);
        }

        //public ActionResult ApproveCollateralOut(Guid? Id)
        //{
        //    if (Id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    var ma = db.MemberAsset.Find(Id);
        //    if (ma == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    var mav = new MemberAssetViewModels();
        //    mav.AssetId = ma.AssetId;
        //    mav.MemberPortFolio = new Data.Business.BusinessService().getPortFolio(ma.MemberId.Value);//  new busins BusinessService().getPortFolio(ma.MemberId.Value);
        //    return View(mav);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult ApproveCollateralOut(MemberAssetViewModels ma)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var item = db.MemberAsset.Find(ma.AssetId);
        //        if (item != null)
        //        {
        //            item.ApproveDate = DateTime.Now;
        //            item.ApproveId = Helper.SessionHelper.CurrentUserInfo.Id;
        //            item.Status = ma.Status;
        //            db.SaveChanges();
        //        }
        //        return RedirectToAction(Session[Data.Helper.StringHelper.stringCurrentPageIndex].ToString());
        //    }

        //    return View(ma);
        //}


        public ActionResult IndexApproveRegister()
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
        public ActionResult IndexApproveRegister(string keyword, string DateFrom, string DateTo, string ddlMemberType)
        {
            var item = from i in db.Member select i;

            if (!string.IsNullOrEmpty(keyword))
            {
                item = item.Where(i => i.FirstName.Contains(keyword) | i.MemberRef == keyword);
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
            if (ddlMemberType != "")
            {
                item = item.Where(w => w.MemberType == ddlMemberType);
            }

            Session[Data.Helper.StringHelper.stringCurrentPageIndex] = this.ControllerContext.RouteData.Values["action"].ToString();
            return PartialView("FindApproveRegister", item.OrderByDescending(i => i.CreateDate));
        }

        public ActionResult ApproveRegister(Guid? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Member.Find(Id);
            if (member == null)
            {
                return HttpNotFound();
            }
            else
            {
                var mb = new MemberApproveRegisterViewModels();
                mb.MemberId = member.MemberId;
                mb.MemberName = member.FirstName;
                mb.MemberRef = member.MemberRef;
                //mb.MemberLevel = member.MemberLevel;
                if (member.MemberLevel != null)
                {
                    if (member.MemberLevel.Value == 1)
                    {
                        mb.MemberLevel = "Level1";
                    }
                    else if (member.MemberLevel.Value == 2)
                    {
                        mb.MemberLevel = "Level2";
                    }
                    else if (member.MemberLevel.Value == 3)
                    {
                        mb.MemberLevel = "Level3";
                    }
                    else if (member.MemberLevel.Value == 4)
                    {
                        mb.MemberLevel = "Level4";
                    }
                }


                //mb.MemberGroup = member.MemberGroup.Value;

                //List<SelectListItem> items = new List<SelectListItem>();
                //items.Add(new SelectListItem { Value = "1", Text = "Level 1" });
                //items.Add(new SelectListItem { Value = "2", Text = "Level 2" });
                //items.Add(new SelectListItem { Value = "3", Text = "Level 3" });
                //ViewBag.MemberGroup = new SelectList(items, "Value", "Text");

                return View(mb);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ApproveRegister(MemberApproveRegisterViewModels member)
        {
            if (ModelState.IsValid)
            {
                Member item = db.Member.Find(member.MemberId);
                if (item != null)
                {
                    item.ApproveRegisterDate = DateTime.Now;
                    item.ApproveRegisterId = Helper.SessionHelper.CurrentUserInfo.Id;
                    item.Active = true;

                    if (member.MemberLevel == "Level1")
                    {
                        item.MemberLevel = 1;
                    }
                    else if (member.MemberLevel == "Level2")
                    {
                        item.MemberLevel = 2;
                    }
                    else if (member.MemberLevel == "Level3")
                    {
                        item.MemberLevel = 3;
                    }
                    else if (member.MemberLevel == "Level4")
                    {
                        item.MemberLevel = 4;
                    }
                    else if (member.MemberLevel == "Level5")
                    {
                        item.MemberLevel = 5;
                    }
                    db.SaveChanges();
                }
                return RedirectToAction("IndexApproveRegister");
            }

            return View(member);
        }

        public ActionResult ApproveRegisterOut(Guid? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Member.Find(Id);
            if (member == null)
            {
                return HttpNotFound();
            }
            else
            {
                var mb = new MemberApproveRegisterOutViewModels();
                mb.MemberId = member.MemberId;
                mb.MemberName = member.FirstName;
                mb.MemberRef = member.MemberRef;

                return View(mb);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ApproveRegisterOut(MemberApproveRegisterOutViewModels member)
        {
            if (ModelState.IsValid)
            {
                Member item = db.Member.Find(member.MemberId);
                if (item != null)
                {
                    item.ApproveRegisterDate = DateTime.Now;
                    item.ApproveRegisterId = Helper.SessionHelper.CurrentUserInfo.Id;
                    item.Active = false;

                    db.SaveChanges();
                }
                return RedirectToAction("IndexApproveRegister");
            }

            return View(member);
        }



        public ActionResult AddAssetGoldOut(Guid? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Member.Find(Id);
            var ma = new MemberAssetGoldWithdrawListModels();

            if (member == null)
            {
                return HttpNotFound();
            }
            else
            {
                ma.MemberId = Id.Value;
                var maList = db.MemberAsset.Where(a => a.Active == true && a.MemberId == Id && a.AssetGold != null).Include(q => q.UserApproved)
                                      .OrderByDescending(a => a.CreateDate).ToList();
                //MemberAsset =  db.MemberAsset.Where(a => a.Active == true && a.MemberId == Id && a.AssetGold != null).OrderByDescending(a => a.CreateDate).ToList();

                var MemberAssetGoldWithdrawList = new List<MemberAssetGoldWithdrawModels>();
                foreach (var item in maList)
                {
                    var MemberAssetGoldWithdraw = new MemberAssetGoldWithdrawModels();
                    MemberAssetGoldWithdraw.AssetId = item.AssetId;
                    MemberAssetGoldWithdraw.WithdrawCheck = false;
                    MemberAssetGoldWithdraw.AssetGold = item.AssetGold;
                    MemberAssetGoldWithdraw.CreateDate = item.CreateDate;
                    MemberAssetGoldWithdraw.Direction = item.Direction;
                    MemberAssetGoldWithdraw.ApproveId = item.ApproveId;
                    MemberAssetGoldWithdraw.CreateBy = item.UserCreated != null ? item.UserCreated.UserName : "";
                    MemberAssetGoldWithdraw.ApproveId = item.UserApproved != null ? item.UserApproved.UserName : "";
                    MemberAssetGoldWithdraw.GoldBrand = item.GoldBrand;
                    MemberAssetGoldWithdraw.GoldSerialNo = item.GoldSerialNo;
                    MemberAssetGoldWithdraw.Branch = item.Branch;
                    MemberAssetGoldWithdraw.Status = item.Status;
                    MemberAssetGoldWithdraw.WithdrawStatus = item.WithdrawStatus;
                    MemberAssetGoldWithdrawList.Add(MemberAssetGoldWithdraw);
                }
                ma.MemberAssetWithdrawList = MemberAssetGoldWithdrawList;
                ma.MemberPortFolio = new Data.Business.BusinessService().getPortFolio(Id.Value);// new Business.BusinessService().getPortFolio(Id.Value);

            }

            var items = new List<SelectListItem>();
            for (int i = 0; i < 10; i++)
            {
                items.Add(new SelectListItem { Text = "B" + i.ToString(), Value = "B" + i.ToString() });
            }
            ViewBag.GoldBrandList = new SelectList(items, "Value", "Text");

            return View(ma);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult AddAssetGoldOut(MemberAssetGoldWithdrawListModels ma)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if (ma.MemberId != null)
        //        {
        //            try
        //            {
        //                foreach (var wd in ma.MemberAssetWithdrawList)
        //                {
        //                    if (wd.WithdrawCheck == true)
        //                    {
        //                        var maDep = db.MemberAsset.Where(q => q.Active == true).SingleOrDefault();
        //                        maDep.Active = false;

        //                        MemberAsset item = new MemberAsset();
        //                        item.AssetId = Guid.NewGuid();
        //                        item.MemberId = ma.MemberId;
        //                        item.Direction = EnumHelper.DirectionAssetType.Withdraw.ToString();
        //                        item.CreateDate = DateTime.Now;
        //                        item.CreateBy = Helper.SessionHelper.CurrentUserInfo.Id;
        //                        item.Active = true;
        //                        item.AssetType = Data.Helper.EnumHelper.AssetType.Gold.ToString();
        //                        db.MemberAsset.Add(item);
        //                        db.SaveChanges();
        //                    }
        //                }
        //                return RedirectToAction("AddAssetGold", new { id = ma.MemberId });
        //            }
        //            catch (Exception ex)
        //            {
        //                throw ex;
        //            }
        //        }
        //    }
        //    return View(ma);
        //}

        [HttpPost]
        public JsonResult AddAssetGoldWithdraw(string arrData, string MemberId)
        {
            JsonHelper.JsonResponse res = new JsonHelper.JsonResponse();
            try
            {
                ////count qty
                //double qtyWD = arrData.Split(',').Count()-1;
                //PortFolioViewModels pf = new Data.Business.BusinessService().CheckWithdrawAble(Guid.Parse(MemberId), EnumHelper.AssetType.Gold.ToString(), qtyWD);
                //if (pf.WithdrawAble == false)
                //{
                //    res.Message = "ไม่สามารถถอนทองหลักประกันได้";
                //    return Json(res, JsonRequestBehavior.AllowGet);
                //}
                foreach (var AssetId in arrData.Split(','))
                {
                    if (AssetId != "")
                    {
                        //validate withdraw able

                        var ma = db.MemberAsset.Find(Guid.Parse(AssetId));
                        if (ma != null)
                        {
                            //add withdraw
                            var item = new MemberAsset();
                            item.AssetId = Guid.NewGuid();
                            item.MemberId = ma.MemberId;
                            item.Direction = EnumHelper.DirectionAssetType.Out.ToString();
                            item.CreateDate = DateTime.Now;
                            item.CreateBy = Helper.SessionHelper.CurrentUserInfo.Id;
                            item.Active = true;
                            item.AssetType = EnumHelper.AssetType.Gold.ToString();
                            item.AssetGold = ma.AssetGold;
                            item.GoldSerialNo = ma.GoldSerialNo;
                            item.GoldBrand = ma.GoldBrand;
                            item.Branch = ma.Branch;
                            item.Status = EnumHelper.MemberAssetStatus.Wait.ToString();
                            db.MemberAsset.Add(item);

                            //update depost
                            ma.WithdrawStatus = EnumHelper.MemberAssetStatus.Wait.ToString();
                            ma.WithdrawId = item.AssetId;

                            db.SaveChanges();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                res.Status = JsonHelper.Status.Error;
                res.Message = ex.Message;

            }
            //res.Status = JsonHelper.Status.Ok;
            res.Message = "";
            return Json(res, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult CheckAssetGoldWithdrawAble(string arrData, string MemberId)
        {
            JsonHelper.JsonResponse res = new JsonHelper.JsonResponse();
            try
            {
                //count qty
                double qtyWD = arrData.Split(',').Count() - 1;
                PortFolioViewModels pf = new Data.Business.BusinessService().CheckWithdrawAble(Guid.Parse(MemberId), EnumHelper.AssetType.Gold.ToString(), qtyWD);
                if (pf.WithdrawAble == false)
                {
                    res.Message = pf.MessageWarning;
                    return Json(res, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                res.Status = JsonHelper.Status.Error;
                res.Message = ex.Message;

            }
            //res.Status = JsonHelper.Status.Ok;
            res.Message = "";
            return Json(res, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult CheckAssetCashWithdrawAble(double Amount, string MemberId)
        {
            JsonHelper.JsonResponse res = new JsonHelper.JsonResponse();
            try
            {
                PortFolioViewModels pf = new Data.Business.BusinessService().CheckWithdrawAble(Guid.Parse(MemberId), EnumHelper.AssetType.Cash.ToString(), Amount);
                if (pf.WithdrawAble == false)
                {
                    res.Message = pf.MessageWarning;
                    return Json(res, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                res.Status = JsonHelper.Status.Error;
                res.Message = ex.Message;

            }
            //res.Status = JsonHelper.Status.Ok;
            res.Message = "";
            return Json(res, JsonRequestBehavior.AllowGet);

        }

        public ActionResult AddAssetGold(Guid? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Member.Find(Id);
            var ma = new MemberAssetGoldViewModels();

            if (member == null)
            {
                return HttpNotFound();
            }
            else
            {
                ma.MemberId = Id.Value;
                //ma.MemberName = member.FirstName;
                // ma.MemberRef = member.MemberRef;
                //ma.AssetGold = string.Empty;
                //ma.Direction = Data.Helper.EnumHelper.DirectionAssetType.In.ToString();
                //ma.MemberAssetList = db.MemberAsset.Where(a => a.Active == true && a.MemberId == Id && a.AssetGold != null)
                //.Include(a => a.UserCreated)
                //.OrderByDescending(a => a.CreateDate).ToList();
                ma.MemberPortFolio = new Data.Business.BusinessService().getPortFolio(Id.Value);// new Business.BusinessService().getPortFolio(Id.Value);
                //ViewBag.WithdrawAble = ma.MemberPortFolio.WithdrawAble;
                //ViewBag.AssetValue = ma.MemberPortFolio.AssetGold;
            }

            //set dropdownlist
            List<SelectListItem> items = new List<SelectListItem>();
            //items.Add(new SelectListItem { Text = "Please Select", Value = "", Selected = true });
            items.Add(new SelectListItem { Text = Data.Helper.EnumHelper.DirectionAssetType.Deposit.ToString(), Value = Data.Helper.EnumHelper.DirectionAssetType.In.ToString() });
            //items.Add(new SelectListItem { Text = Data.Helper.EnumHelper.DirectionAssetType.Withdraw.ToString(), Value = Data.Helper.EnumHelper.DirectionAssetType.Out.ToString() });
            ViewBag.Direction = new SelectList(items, "Value", "Text");

            items = new List<SelectListItem>();
            for (int i = 0; i < 10; i++)
            {
                items.Add(new SelectListItem { Text = "B" + i.ToString(), Value = "B" + i.ToString() });
            }
            ViewBag.GoldBrandList = new SelectList(items, "Value", "Text");


            return View(ma);
        }

        //Only Deposit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddAssetGold(MemberAssetGoldViewModels ma)
        {
            if (ModelState.IsValid)
            {
                MemberAsset item = new MemberAsset();
                if (ma.MemberId != null)
                {
                    try
                    {
                        item.AssetId = Guid.NewGuid();
                        item.MemberId = ma.MemberId;
                        item.Direction = ma.Direction;
                        item.CreateDate = DateTime.Now;
                        item.CreateBy = Helper.SessionHelper.CurrentUserInfo.Id;
                        item.Active = true;
                        item.AssetGold = 1;//1 per time
                        item.GoldBrand = ma.GoldBrand;
                        item.GoldSerialNo = ma.SerialNo;
                        item.Branch = ma.Branch;
                        item.AssetType = EnumHelper.AssetType.Gold.ToString();
                        item.Status = EnumHelper.MemberAssetStatus.Wait.ToString();
                        db.MemberAsset.Add(item);
                        db.SaveChanges();
                        return RedirectToAction("IndexAssetGold", new { id = ma.MemberId });
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            return View(ma);
        }

        public ActionResult IndexAssetGold(Guid? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Member.Find(Id);
            var ma = new MemberAssetGoldViewModels();

            if (member == null)
            {
                return HttpNotFound();
            }
            else
            {
                ma.MemberId = Id.Value;
                //ma.MemberName = member.FirstName;
                //ma.MemberRef = member.MemberRef;
                //ma.AssetGold = string.Empty;
                //ma.Direction = Data.Helper.EnumHelper.DirectionAssetType.In.ToString();
                ma.MemberAssetList = db.MemberAsset.Where(a => a.Active == true && a.AssetType == EnumHelper.AssetType.Gold.ToString() && a.MemberId == Id && a.AssetGold != null)
                                    .OrderByDescending(a => a.CreateDate).ToList();

                ma.MemberPortFolio = new Data.Business.BusinessService().getPortFolio(Id.Value);
                //ViewBag.WithdrawAble = ma.MemberPortFolio.WithdrawAble;
                //ViewBag.AssetValue = ma.MemberPortFolio.AssetGold;
            }
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "ประเภทธุรกรรม", Value = "", Selected = true });
            items.Add(new SelectListItem { Text = Data.Helper.EnumHelper.DirectionAssetType.Deposit.ToString(), Value = Data.Helper.EnumHelper.DirectionAssetType.In.ToString() });
            items.Add(new SelectListItem { Text = Data.Helper.EnumHelper.DirectionAssetType.Withdraw.ToString(), Value = Data.Helper.EnumHelper.DirectionAssetType.Out.ToString() });
            ViewBag.DirectionList = new SelectList(items, "Value", "Text");


            return View(ma);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult IndexAssetGold(Guid MemberId, string DateFrom, string DateTo, string ddlDirection)
        {
            var item = db.MemberAsset.Where(q => q.Active == true && q.AssetType == EnumHelper.AssetType.Gold.ToString() && q.MemberId == MemberId);

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
            if (ddlDirection != "")
            {
                item = item.Where(w => w.Direction == ddlDirection);
            }

            return PartialView("FindAssetGold", item.OrderByDescending(i => i.CreateDate));
        }



        public ActionResult AddAssetCash(Guid? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Member.Find(Id);
            var ma = new MemberAssetCashViewModels();

            if (member == null)
            {
                return HttpNotFound();
            }
            else
            {
                ma.MemberId = Id.Value;
                ma.MemberName = member.FirstName;
                ma.MemberRef = member.MemberRef;
                ma.AssetCash = string.Empty;

                //ma.Direction = Data.Helper.EnumHelper.DirectionAssetType.In.ToString();
                //ma.MemberAssetList = db.MemberAsset.Where(a => a.Active == true && a.MemberId == Id && a.AssetCash != null)
                //.OrderByDescending(a => a.CreateDate).ToList();
                ma.MemberPortFolio = new Data.Business.BusinessService().getPortFolio(Id.Value);
                ViewBag.WithdrawAble = ma.MemberPortFolio.WithdrawAble;
                ViewBag.AssetValue = ma.MemberPortFolio.AssetCash;

            }

            //set dropdownlist
            List<SelectListItem> items = new List<SelectListItem>();
            //items.Add(new SelectListItem { Text = Data.Helper.EnumHelper.DirectionAssetType.Deposit.ToString(), Value = Data.Helper.EnumHelper.DirectionAssetType.In.ToString(), Selected = true });
            //items.Add(new SelectListItem { Text = Data.Helper.EnumHelper.DirectionAssetType.Withdraw.ToString(), Value = Data.Helper.EnumHelper.DirectionAssetType.Out.ToString() });
            //ViewBag.DirectionList = new SelectList(items, "Value", "Text");

            items = new List<SelectListItem>();
            if (SessionHelper.CurrentUserInfo.UserPageRole == EnumHelper.CashRoleType.Finance.ToString())
            {
                items.Add(new SelectListItem { Text = Data.Helper.EnumHelper.GetDescription(Data.Helper.EnumHelper.MemberAssetPayType.Bank), Value = Data.Helper.EnumHelper.MemberAssetPayType.Bank.ToString() });
            }
            else
            {
                items.Add(new SelectListItem { Text = Data.Helper.EnumHelper.GetDescription(Data.Helper.EnumHelper.MemberAssetPayType.Cashier), Value = Data.Helper.EnumHelper.MemberAssetPayType.Cashier.ToString() });
            }
            ViewBag.PayTypeList = new SelectList(items, "Value", "Text");

            items = new List<SelectListItem>();

            if (SessionHelper.CurrentUserInfo.UserPageRole == EnumHelper.CashRoleType.Finance.ToString())
            {
                items.Add(new SelectListItem { Text = "Please select", Value = "", Selected = true });
                items.Add(new SelectListItem { Text = Data.Helper.EnumHelper.GetDescription(Data.Helper.EnumHelper.MemberAssetPayTypeDetail.Transfer), Value = Data.Helper.EnumHelper.MemberAssetPayTypeDetail.Transfer.ToString() });
                items.Add(new SelectListItem { Text = Data.Helper.EnumHelper.GetDescription(Data.Helper.EnumHelper.MemberAssetPayTypeDetail.ATS), Value = Data.Helper.EnumHelper.MemberAssetPayTypeDetail.ATS.ToString() });
                items.Add(new SelectListItem { Text = Data.Helper.EnumHelper.GetDescription(Data.Helper.EnumHelper.MemberAssetPayTypeDetail.DirectDebit), Value = Data.Helper.EnumHelper.MemberAssetPayTypeDetail.DirectDebit.ToString() });
                items.Add(new SelectListItem { Text = Data.Helper.EnumHelper.GetDescription(Data.Helper.EnumHelper.MemberAssetPayTypeDetail.Card), Value = Data.Helper.EnumHelper.MemberAssetPayTypeDetail.Card.ToString() });
                items.Add(new SelectListItem { Text = Data.Helper.EnumHelper.GetDescription(Data.Helper.EnumHelper.MemberAssetPayTypeDetail.Cheque), Value = Data.Helper.EnumHelper.MemberAssetPayTypeDetail.Cheque.ToString() });
            }
            else
            {
                items.Add(new SelectListItem { Text = Data.Helper.EnumHelper.GetDescription(Data.Helper.EnumHelper.MemberAssetPayTypeDetail.Cash), Value = Data.Helper.EnumHelper.MemberAssetPayTypeDetail.Cash.ToString() });
            }
            ViewBag.PayTypeDetailList = new SelectList(items, "Value", "Text");

            //items = new List<SelectListItem>();
            //items.Add(new SelectListItem { Text = "Please select", Value = "", Selected = true });
            //items.Add(new SelectListItem { Text = Data.Helper.EnumHelper.GetDescription(Data.Helper.EnumHelper.BankName.BAY), Value = Data.Helper.EnumHelper.BankName.BAY.ToString() });
            //items.Add(new SelectListItem { Text = Data.Helper.EnumHelper.GetDescription(Data.Helper.EnumHelper.BankName.BBL), Value = Data.Helper.EnumHelper.BankName.BBL.ToString() });
            //items.Add(new SelectListItem { Text = Data.Helper.EnumHelper.GetDescription(Data.Helper.EnumHelper.BankName.KBank), Value = Data.Helper.EnumHelper.BankName.KBank.ToString() });
            //items.Add(new SelectListItem { Text = Data.Helper.EnumHelper.GetDescription(Data.Helper.EnumHelper.BankName.KTB), Value = Data.Helper.EnumHelper.BankName.KTB.ToString() });
            //items.Add(new SelectListItem { Text = Data.Helper.EnumHelper.GetDescription(Data.Helper.EnumHelper.BankName.SCB), Value = Data.Helper.EnumHelper.BankName.SCB.ToString() });
            //items.Add(new SelectListItem { Text = Data.Helper.EnumHelper.GetDescription(Data.Helper.EnumHelper.BankName.TBank), Value = Data.Helper.EnumHelper.BankName.TBank.ToString() });

            //ViewBag.BankList = new SelectList(items, "Value", "Text");
            items = new SelectList(db.CompanyBank.Where(b => b.Active == true).OrderByDescending(b => b.AccountName)
                    .Select(b => new { BankId = b.BankId, BankName = b.BankName + " (" + b.AccountNo + ")" }), "BankId", "BankName").ToList();
            items.Insert(0, (new SelectListItem { Text = "== Please select ==", Value = "" }));
            ViewBag.BankList = new SelectList(items, "Value", "Text");

            return View(ma);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddAssetCash(MemberAssetCashViewModels ma)
        {
            if (ModelState.IsValid)
            {
                MemberAsset item = new MemberAsset();
                if (ma.MemberId != null)
                {
                    try
                    {
                        item.AssetId = Guid.NewGuid();
                        item.MemberId = ma.MemberId;
                        item.AssetCash = Convert.ToDecimal(ma.AssetCash);
                        item.CreateDate = DateTime.Now;
                        item.CreateBy = Helper.SessionHelper.CurrentUserInfo.Id;
                        item.Active = true;
                        item.AssetType = Data.Helper.EnumHelper.AssetType.Cash.ToString();
                        item.PayType = ma.PayType;
                        item.PayTypeDetail = ma.PayTypeDetail;

                        item.PayBankName = ma.PayDetail.PayBankName;
                        item.PayAccountNo = ma.PayDetail.PayAccountNo;
                        item.PayAccountName = ma.PayDetail.PayAccountName;
                        item.PayAccountType = ma.PayDetail.PayAccountType;
                        item.PayBankBranch = ma.PayDetail.PayBankBranch;
                        item.PayTransRef = ma.PayDetail.PayTransRef;
                        item.PayTime = ma.PayDetail.PayTime;
                        if (ma.PayDetail.CompanyBankId == null)
                        {
                            item.CompanyBankId = null;
                        }
                        else
                        {
                            item.CompanyBankId = Convert.ToInt16(ma.PayDetail.CompanyBankId);
                        }
                        if (!string.IsNullOrEmpty(ma.PayDetail.PayDate))
                        {
                            item.PayDate = Convert.ToDateTime(ma.PayDetail.PayDate, Data.Helper.StringHelper.culture);
                        }
                        else
                        {
                            item.PayDate = null;
                        }


                        if (item.PayTypeDetail == EnumHelper.MemberAssetPayTypeDetail.Cheque.ToString())
                        {
                            item.PayChequeNo = ma.PayDetail.PayChequeNo;
                            if (!string.IsNullOrEmpty(ma.PayDetail.PayDuedate))
                            {
                                item.PayDuedate = Convert.ToDateTime(ma.PayDetail.PayDuedate, Data.Helper.StringHelper.culture);
                            }
                            else
                            {
                                item.PayDuedate = null;
                            }
                        }
                        item.Direction = ma.Direction;
                        item.Status = EnumHelper.MemberAssetStatus.Wait.ToString();
                        item.PayBankName = ma.PayDetail.PayBankName;

                        db.MemberAsset.Add(item);
                        db.SaveChanges();

                        return RedirectToAction("IndexAssetCash", new { id = ma.MemberId });
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            return View(ma);
        }

        public ActionResult IndexAssetCash(Guid? Id, string pt)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Member.Find(Id);
            var ma = new MemberAssetCashViewModels();

            if (member == null)
            {
                return HttpNotFound();
            }
            else
            {
                ma.MemberId = Id.Value;
                ma.MemberAssetList = db.MemberAsset.Where(a => a.Active == true && a.MemberId == Id && a.AssetCash != null)
                                    .OrderByDescending(a => a.CreateDate).ToList();
                ma.MemberPortFolio = new Data.Business.BusinessService().getPortFolio(Id.Value);
            }

            //set dropdownlist
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "ประเภทธุรกรรม", Value = "", Selected = true });
            items.Add(new SelectListItem { Text = Data.Helper.EnumHelper.DirectionAssetType.Deposit.ToString(), Value = Data.Helper.EnumHelper.DirectionAssetType.In.ToString() });
            items.Add(new SelectListItem { Text = Data.Helper.EnumHelper.DirectionAssetType.Withdraw.ToString(), Value = Data.Helper.EnumHelper.DirectionAssetType.Out.ToString() });
            ViewBag.DirectionList = new SelectList(items, "Value", "Text");

            return View(ma);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult IndexAssetCash(Guid MemberId, string DateFrom, string DateTo, string ddlDirection)
        {
            //var item = from i in db.MemberAsset select i;
            var item = db.MemberAsset.Where(q => q.Active == true && q.AssetType == EnumHelper.AssetType.Cash.ToString() && q.MemberId == MemberId);

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
            if (ddlDirection != "")
            {
                item = item.Where(w => w.Direction == ddlDirection);
            }

            return PartialView("FindAssetCash", item.OrderByDescending(i => i.CreateDate));
        }

        #endregion


        public ActionResult EditCollateral(Guid? Id)
        {
            if (Helper.SessionHelper.CurrentUserInfo != null)
            {
                if (Id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Member member = db.Member.Find(Id);
                if (member == null)
                {
                    return HttpNotFound();
                }
                return View(member);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }


        #region [PortFolio]

        public ActionResult ViewPortFolio(Guid Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            var port = new PortFolioViewModels();// PortFolioViewModels();


            port = new Data.Business.BusinessService().getPortFolio(Id);
            port.TicketPendingList = db.Ticket.Where(q => q.Active == true && q.MemberId == Id)
                                               .Where(q => q.ApproveId == null || q.ApprovePayId == null).ToList();
            port.TicketCompleteList = db.Ticket.Where(q => q.Active == true && q.MemberId == Id && q.ApprovePayId != null && q.ApproveId != null).ToList();
            port.TransferList = db.Transfer.Where(a => a.Active == true && a.MemberId == Id)
                                        .Include(a => a.Member).Include(a => a.UserCreated)
                                        .OrderByDescending(a => a.CreateDate).ToList();
            return View(port);
        }
        #endregion
    }
}
