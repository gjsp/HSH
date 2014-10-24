using System;
using System.Collections.Generic;
using System.Linq;
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
    public class CompanyController : Controller
    {
        private HSHEntities db = new HSHEntities();
        public ActionResult Index()
        {
            ////Company Gold
            //var stock = new StockSummaryGoldViewModels();
            //stock.ComGoldIn = db.Transfer.Where(q => q.Active == true && q.Direction == EnumHelper.DirectionAssetType.Deposit.ToString()).Sum(q => q.Quantity) ?? 0;
            //stock.ComGoldOut = db.Transfer.Where(q => q.Active == true && q.Direction == EnumHelper.DirectionAssetType.Withdraw.ToString()).Sum(q => q.Quantity) ?? 0;
            //stock.ComGoldSum = stock.ComGoldIn - stock.ComGoldOut;
            //stock.ComGoldBegin = 0 ;


            //stock.ComGoldIn = db.Transfer.Where(q => q.Active == true && q.Direction == EnumHelper.DirectionAssetType.Deposit.ToString()).Sum(q => q.Quantity) ?? 0;
            //stock.ComGoldOut = db.Transfer.Where(q => q.Active == true && q.Direction == EnumHelper.DirectionAssetType.Withdraw.ToString()).Sum(q => q.Quantity) ?? 0;
            //stock.ComGoldSum = stock.ComGoldIn - stock.ComGoldOut;
            //stock.ComGoldBegin = 0;

            ////Company Cash
            ////double sumCashIn = db.Ticket.Where(q => q.Active == true && q.ApproveId != null && q.ApprovePayId != null &&
            ////    q.TicketType == EnumHelper.TicketType.Buy.ToString()).Sum(q => q.Amount) ?? 0;

            ////double sumCashOut = db.Ticket.Where(q => q.Active == true && q.ApproveId != null && q.ApprovePayId != null &&
            ////   q.TicketType == EnumHelper.TicketType.Sell.ToString()).Sum(q => q.Amount) ?? 0;
            ////stock.CompanyCash = sumCashIn - sumCashOut;


            ////Gold Colleral
            //stock.ColGoldBegin = 0;
            //decimal sumGoldColIn = db.MemberAsset.Where(q => q.Active == true && q.ApproveId != null &&
            //    q.Direction == EnumHelper.DirectionAssetType.In.ToString() &&
            //    q.AssetType == EnumHelper.AssetType.Gold.ToString()).Sum(q => q.AssetGold) ?? 0;
            //stock.ColGoldIn = Convert.ToDouble(sumGoldColIn);

            //decimal sumGoldColOut = db.MemberAsset.Where(q => q.Active == true && q.ApproveId != null &&
            //   q.Direction == EnumHelper.DirectionAssetType.Out.ToString() &&
            //   q.AssetType == EnumHelper.AssetType.Gold.ToString()).Sum(q => q.AssetGold) ?? 0;
            //stock.ColGoldOut = Convert.ToDouble(sumGoldColOut);

            //stock.ColGoldSum = stock.ColGoldIn - stock.ColGoldOut;


            ////Cash Colleral
            ////decimal sumCashColIn = db.MemberAsset.Where(q => q.Active == true && q.ApproveId != null &&
            ////   q.Direction == EnumHelper.DirectionAssetType.In.ToString() &&
            ////   q.AssetType == EnumHelper.AssetType.Cash.ToString()).Sum(q => q.AssetCash) ?? 0;

            ////decimal sumCashColOut = db.MemberAsset.Where(q => q.Active == true && q.ApproveId != null &&
            ////   q.Direction == EnumHelper.DirectionAssetType.Out.ToString() &&
            ////   q.AssetType == EnumHelper.AssetType.Cash.ToString()).Sum(q => q.AssetCash) ?? 0;
            ////stock.CollateralCash = Convert.ToDouble(sumCashColIn - sumCashColOut);


            return View(new BusinessService().getSummaryStockGoldBalance());
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