using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using HSH.Data.Models;

namespace HSH.Data.Models
{
    public class MemberApproveRegisterViewModels
    {
        public Guid MemberId { get; set; }

        [DisplayName("เลขที่บัญชีซื้อขายทองคำ")]
        public string MemberRef { get; set; }

        [DisplayName("ชื่อสมาชิก")]
        public string MemberName { get; set; }
        public Guid UserApproveId { get; set; }


        [DisplayName("เลเวล *")]
        [Required(ErrorMessage = "Required")]
        public string MemberLevel { get; set; }
        public bool TradeUnlimit { get; set; }
        public int TradeMaximum { get; set; }
    }
    public class MemberApproveRegisterOutViewModels
    {
        public Guid MemberId { get; set; }

        [DisplayName("เลขที่บัญชีซื้อขายทองคำ")]
        public string MemberRef { get; set; }

        [DisplayName("ชื่อสมาชิก")]
        public string MemberName { get; set; }
        public Guid UserApproveId { get; set; }

        public string MemberLevel { get; set; }
        public bool TradeUnlimit { get; set; }
        public int TradeMaximum { get; set; }
    }

}