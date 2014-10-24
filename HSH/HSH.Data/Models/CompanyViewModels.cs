using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HSH.Data.Models;

namespace HSH.Data.Models
{
    public class CompanyViewModels
    {

        public double TransferGoldDep { get; set; }


        public double ImportExportGoldDep { get; set; }

        //[DisplayName("Member Cash Deposit")]
        //public double MemberCashDep { get; set; }


        [DisplayName("ยกมา")]
        public double ComGoldBegin { get; set; }

        [DisplayName("ทองเข้า")]
        public double ComGoldIn { get; set; }

        [DisplayName("ทองออก")]
        public double ComGoldOut { get; set; }

        [DisplayName("ทองคงเหลือ")]
        public double CompanyGold { get; set; }



        [DisplayName("เงินบริษัท")]
        public double CompanyCash { get; set; }


        [DisplayName("ยกมา")]
        public double ColGoldBegin { get; set; }

        [DisplayName("ทองเข้า")]
        public double ColGoldIn { get; set; }

        [DisplayName("ทองออก")]
        public double ColGoldOut { get; set; }

        [DisplayName("ทองหลักประกัน")]
        public double CollateralGold { get; set; }


        [DisplayName("เงินหลักประกัน")]
        public double CollateralCash { get; set; }

    }

    public class StockSummaryGoldViewModels
    {

        [DisplayName("ยกมา")]
        public double ComGoldBegin { get; set; }

        [DisplayName("ทองเข้า")]
        public double ComGoldIn { get; set; }

        [DisplayName("ทองออก")]
        public double ComGoldOut { get; set; }

        [DisplayName("ทองคงเหลือ")]
        public double ComGoldSum { get; set; }



        [DisplayName("ยกมา")]
        public double ColGoldBegin { get; set; }

        [DisplayName("ทองเข้า")]
        public double ColGoldIn { get; set; }

        [DisplayName("ทองออก")]
        public double ColGoldOut { get; set; }

        [DisplayName("ทองคงเหลือ")]
        public double ColGoldSum { get; set; }


        [DisplayName("ยกมา")]
        public double MemGoldBegin { get; set; }

        [DisplayName("ทองเข้า")]
        public double MemGoldIn { get; set; }

        [DisplayName("ทองออก")]
        public double MemGoldOut { get; set; }

        [DisplayName("ทองคงเหลือ")]
        public double MemGoldSum { get; set; }


        [DisplayName("ยกมา")]
        public double PhyGoldBegin { get; set; }

        [DisplayName("ทองเข้า")]
        public double PhyGoldIn { get; set; }

        [DisplayName("ทองออก")]
        public double PhyGoldOut { get; set; }

        [DisplayName("ทองคงเหลือ")]
        public double PhyGoldSum { get; set; }

    }

    public class CompanyAssetViewModels
    {
        public int AssetId { get; set; }
        public string AssetRef { get; set; }
        public string AssetType { get; set; }

        [DisplayName("Type")]
        public string Direction { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Range(1, 1000, ErrorMessage = "จำนวนทองระหว่าง 1 - 1,000 Kg")]
        public Nullable<double> Quantity { get; set; }
        public string Note { get; set; }
        public string Status { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string ApproveBy { get; set; }
        public Nullable<System.DateTime> ApproveDate { get; set; }
        public AspNetUsers UserCreated { get; set; }
        public List<CompanyAsset> CompanyAssetList { get; set; }

        public CompanyViewModels CompanySummary { get; set; }
    }

    //Model  CompanyAsset + Transfer For Sum Show Report (Inout)
    public class InoutViewModels
    {
        public string InoutRef { get; set; }
        public string Direction { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<double> Quantity { get; set; }
    }

    public class InoutAssetViewModels
    {
        public List<InoutViewModels> InoutAssetList { get; set; }
        public CompanyViewModels CompanySummary { get; set; }
    }


}