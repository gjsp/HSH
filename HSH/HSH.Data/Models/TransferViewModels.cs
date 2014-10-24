using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HSH.Data.Models;

namespace HSH.Data.Models
{
    public class TransferViewModels
    {
        public int TransferId { get; set; }
        public string TransferRef { get; set; }
        public string TransferType { get; set; }
        public Nullable<System.Guid> MemberId { get; set; }
        public Nullable<double> Quantity { get; set; }
        public Nullable<double> Amount { get; set; }
        public string Note { get; set; }
        public string Status { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string ApproveBy { get; set; }
        public Nullable<System.DateTime> ApproveDate { get; set; }
        public string Direction { get; set; }
        public Member MemberDetail { get; set; }
    }
    public class TransferGoldViewModels
    {
        public string TransferRef { get; set; }


        [Required(ErrorMessage = "Required")]
        [DisplayName("ประเภท *")]
        public string Direction { get; set; }
        public Nullable<System.Guid> MemberId { get; set; }

        [DisplayName("จำนวนทอง")]
        //[Required(ErrorMessage = "Required")]
        [DataType(DataType.Currency)]
        [Range(1, 999, ErrorMessage = "จำนวนทองระหว่าง 1 - 999")]
        public Nullable<double> Quantity { get; set; }

        //[Required(ErrorMessage = "Required")]
        //[DisplayName("จำนวนทองคำ")]
        //[Range(1, 999, ErrorMessage = "จำนวนทองระหว่าง 1 - 999 Kg")]
        //public string AssetGold { get; set; }

      
        [Required(ErrorMessage = "Required")]
        [DisplayName("ซีเรียลนัมเบอร์ *")]
        public string SerialNo { get; set; }


        [DisplayName("ยี่ห้อ")]
        public string GoldBrand { get; set; }


        [DisplayName("สาขา")]
        public string Branch { get; set; }

        public PortFolioViewModels MemberPortFolio { get; set; }

        public  List<Transfer> TransferList { get; set; }

    }

    public class TransferMemberViewModels
    {
        public System.Guid MemberId { get; set; }
        public Nullable<double> Quantity { get; set; }

        public Member MemberDetail { get; set; }
    }



}