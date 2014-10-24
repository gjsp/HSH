using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace HSH.Data.Models
{
    public class PortFolioViewModels
    {
        public Guid MemberId { get; set; }

        [DisplayName("เลขที่บัญชีซื้อขายทองคำ")]
        public string MemberRef { get; set; }

        [DisplayName("ประเภทสมาชิก")]
        public string MemberType { get; set; }

        [DisplayName("กลุ่มสมาชิก")]
        public string MemberGroup { get; set; }

        [DisplayName("คำนำหน้าชื่อ")]
        public string MemberTitle { get; set; }

        [DisplayName("เลเวล")]
        public string MemberLevel { get; set; }

        [DisplayName("ชื่อสมาชิก")]
        public string FirstName { get; set; }

        [DisplayName("นามสกุล")]
        public string LastName { get; set; }

        [DisplayName("อีเมล์")]
        public string Email { get; set; }


        [DisplayName("Duedate T+")]
        public double DuedateValue { get; set; }


        [DisplayName("ซื้อขายได้สูงสุดต่อครั้ง (Kg)")]
        public double MaxKg { get; set; }


        [DisplayName("Credit Limit (Kg)")]
        public double CreditLimit { get; set; }


        [DisplayName("Margin Type")]
        public string MarginStatus { get; set; }

        public double MarginValue { get; set; }



        [DisplayName("ปริมาณที่ซื้อ/ขายได้ (Kg)")]
        public decimal CreditLine { get; set; }


        [DisplayName("ปริมาณที่ลูกค้าซื้อได้คงเหลือ (Kg)")]
        public decimal CreditBuyBalance { get; set; }


        [DisplayName("ปริมาณที่ลูกค้าขายได้คงเหลือ (Kg)")]
        public decimal CreditSellBalance { get; set; }


        [DisplayName("จำนวนการซื้อทองคำล่วงหน้า (Kg)")]
        public decimal SumBuyPlaceOrder { get; set; }


        [DisplayName("จำนวนการขายทองคำล่วงหน้า (Kg)")]
        public decimal SumSellPlaceOrder { get; set; }


        [DisplayName("จำนวนการซื้อทองคำ (Kg)")]
        public decimal SumBuy { get; set; }


        [DisplayName("จำนวนการขายทองคำ (Kg)")]
        public decimal SumSell { get; set; }


        [DisplayName("เงินหลักประกัน (THB)")]
        public decimal AssetCash { get; set; }


        [DisplayName("ทองหลักประกัน (Kg)")]
        public decimal AssetGold { get; set; }


        [DisplayName("ทองฝาก (Kg)")]
        public double DepositGold { get; set; }


        [DisplayName("สถานะการซื้อ")]
        public bool PauseBuy { get; set; }


        [DisplayName("สถานะการขาย")]
        public bool PauseSell { get; set; }

        public bool WithdrawAble { get; set; }

        //For Calculate,not display
        public decimal AssetTradeAble { get; set; }

        //For Calculate,not display ,for check case ขายทองฝาก
        public decimal QuantityBalanceSellGoldDep { get; set; }

        public List<Ticket> TicketPendingList { get; set; }
        public List<Ticket> TicketCompleteList { get; set; }
        public List<Transfer> TransferList { get; set; }

        public string MessageWarning { get; set; }
    }

}