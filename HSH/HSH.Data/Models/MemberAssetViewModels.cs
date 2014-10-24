using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HSH.Data.Models;


namespace HSH.Data.Models
{
    public class MemberAssetViewModels
    {
        public Guid AssetId { get; set; }

        [DisplayName("Status *")]
        [Required(ErrorMessage = "Required")]
        public string Status { get; set; }

        public PortFolioViewModels MemberPortFolio { get; set; }

    }

    public class MemberAssetGoldWithdrawListModels
    {
        [Required]
        public Guid MemberId { get; set; }

        public string assetIdList { get; set; }
        public PortFolioViewModels MemberPortFolio { get; set; }
        public List<MemberAssetGoldWithdrawModels> MemberAssetWithdrawList { get; set; }
    }

 public class MemberAssetGoldWithdrawModels
    {
        public Nullable<System.Guid> MemberId { get; set; }
        public string AssetType { get; set; }

        public Guid AssetId { get; set; }
        public Nullable<decimal> AssetGold { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string CreateBy { get; set; }
        public string Direction { get; set; }
        public string GoldBrand { get; set; }
        public string GoldSerialNo { get; set; }
        public string Branch { get; set; }
        public string ApproveId { get; set; }
        public bool WithdrawCheck { get; set; }
        public string Status { get; set; }
        public string WithdrawStatus { get; set; }
    }
    public class MemberAssetGoldViewModels
    {
        public Guid MemberId { get; set; }

        [DisplayName("เลขที่บัญชีซื้อขายทองคำ")]
        public string MemberRef { get; set; }
        //public bool WithdrawCheck { get; set; }

        [DisplayName("ชื่อสมาชิก")]
        public string MemberName { get; set; }

        //[Required(ErrorMessage = "Required")]
        [DisplayName("จำนวนทองคำ")]
        //[Range(1, 999, ErrorMessage = "จำนวนทองระหว่าง 1 - 999 Kg")]
        public string AssetGold { get; set; }
        
        [Required(ErrorMessage = "Required")]
        [DisplayName("ประเภท")]
        public string Direction { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(50, ErrorMessage = "Over length character")]
        [DisplayName("ซีเรียลนัมเบอร์")]
        public string SerialNo { get; set; }


        [DisplayName("ยี่ห้อ")]
        [StringLength(50, ErrorMessage = "Over length character")]
        public string GoldBrand { get; set; }


        [DisplayName("สาขา")]
        [StringLength(50, ErrorMessage = "Over length character")]
        public string Branch { get; set; }

        public PortFolioViewModels MemberPortFolio { get; set; }
        public virtual List<MemberAsset> MemberAssetList { get; set; }
        public List<MemberAssetGoldWithdrawModels> MemberAssetWithdrawList { get; set; }


    }

    public class MemberAssetCashViewModels
    {
        public Guid MemberId { get; set; }

        [DisplayName("เลขที่บัญชีซื้อขายทองคำ")]
        public string MemberRef { get; set; }

        [DisplayName("ชื่อสมาชิก")]
        public string MemberName { get; set; }

        [Required(ErrorMessage = "Required")]
        [DataType(DataType.Text)]
        [DisplayName("จำนวนเงิน *")]
        [StringLength(11, MinimumLength=1, ErrorMessage="จำนวนเงินไม่เกิน 99ล้านบาท")]
        //[Range(10000, 1000000000, ErrorMessage = "จำนวนเงินอย่างน้อย 10,000 บาท")]
        public string AssetCash { get; set; }

        [Required(ErrorMessage = "Required")]
        [DisplayName("ประเภท *")]
        public string Direction { get; set; }

        [Required(ErrorMessage = "Required")]
        [DisplayName("ประเภทการจ่ายเงิน *")]
        public string PayType { get; set; }

        [Required(ErrorMessage = "Required")]
        [DisplayName("รายละเอียดการจ่ายเงิน *")]
        public string PayTypeDetail { get; set; }

        public Data.Models.PortFolioViewModels MemberPortFolio { get; set; }

        public virtual List<MemberAsset> MemberAssetList { get; set; }
        public MemberPayDetailViewModels PayDetail { get; set; }
    }

    public class MemberPayDetailViewModels
    {
        [DisplayName("ธนาคาร")]
        [StringLength(50, ErrorMessage = "Over length character")]
        public string PayBankName { get; set; }

        [DisplayName("เลขที่บัญชี")]
        [DataType(DataType.Currency)]
        [StringLength(13, MinimumLength = 10, ErrorMessage = "รูปแบบเลขที่บัญชีไม่ถูกต้อง")]
        public string PayAccountNo { get; set; }

        [DisplayName("ชื่อบัญชี")]
        [StringLength(50, ErrorMessage = "Over length character")]
        public string PayAccountName { get; set; }

        [DisplayName("ประเภทบัญชี")]
        [StringLength(50, ErrorMessage = "Over length character")]
        public string PayAccountType { get; set; }

        [DisplayName("สาขา")]
        [StringLength(50, ErrorMessage = "Over length character")]
        public string PayBankBranch { get; set; }

        [DisplayName("Refno")]
        [StringLength(50, ErrorMessage = "Over length character")]
        public string PayTransRef { get; set; }

        [DisplayName("เวลา")]
        [StringLength(50, ErrorMessage = "Over length character")]
        public string PayTime { get; set; }

        [DisplayName("วันที่")]
        public string PayDate { get; set; }

        [DisplayName("เลขที่เช็ค")]
        [StringLength(50, ErrorMessage = "Over length character")]
        public string PayChequeNo { get; set; }

        [DisplayName("Duedate")]
        public string PayDuedate { get; set; }

        [DisplayName("ธนาคารบริษัท")]
        public string CompanyBankId { get; set; }

    }
}