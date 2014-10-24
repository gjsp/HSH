using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using HSH.Data.Models;

namespace HSH.Data.Models
{
    public class TicketViewModels
    {
    }

    public class TicketCreateViewModels
    {
        [DisplayName("สมาชิก *")]
        [Required(ErrorMessage = "Required")]
        public Guid MemberId { get; set; }

        [Required(ErrorMessage = "Required")]
        [DisplayName("ประเภท *")]
        public string TicketType { get; set; }

        [Required(ErrorMessage = "Required")]
        [DisplayName("จำนวน *")]
        [DataType(DataType.Currency)]
        [Range(1, 999, ErrorMessage = "จำนวนทองระหว่าง 1 - 999 Kg")]
        public double Quantity { get; set; }

        [Required(ErrorMessage = "Required")]
        [DisplayName("ราคา *")]
        //[StringLength(6, ErrorMessage = "Over Limit Price")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "ราคาไม่ถูกต้อง")]
        public string Price { get; set; }

        [Required(ErrorMessage = "Required")]
        [DisplayName("จำนวนเงิน")]
        public string Amount { get; set; }

        //[Required]
        [DisplayName("ประเภทการจ่ายเงิน")]
        public string Paytype { get; set; }

        [DisplayName("ขายทองที่ฝากในระบบ")]
        public bool UseDeposit { get; set; }

    }

    public class TicketReceiveDetailViewModels
    {
        public Guid TicketId { get; set; }

        [DisplayName("รหัสออเดอร์")]
        public string TicketRef { get; set; }

        [DisplayName("ประเภท")]
        public string TicketType { get; set; }

        [DisplayName("รหัสใบเทรด")]
        public string TradeRef { get; set; }

        [DisplayName("เลขที่บัญชีซื้อขายทองคำ")]
        public string MemberRef { get; set; }

        [DisplayName("ชื่อสมาชิก")]
        public string MemberName { get; set; }

        [DisplayName("ราคา")]
        public double Price { get; set; }

        [DisplayName("จำนวนทอง")]
        public double Quantity { get; set; }

        [DisplayName("จำนวนเงิน")]
        public double Amount { get; set; }

        [DisplayName("จำนวนทองที่ยังไม่ตัด")]
        public double QtyUnpaid { get; set; }

        [DisplayName("จำนวนทองที่ตัด")]
        public double QtyPaid { get; set; }

        [DisplayName("จำนวนเงินที่ยังไม่จ่าย")]
        public double AmtUnpaid { get; set; }

        [DisplayName("จำนวนเงินที่จ่าย")]
        public double AmountPaid { get; set; }
        public string MessageWarning { get; set; }

        public Nullable<bool> UseDeposit { get; set; }

    }

    public class TicketStatusViewModels
    {
        public Guid TicketId { get; set; }
        public string TicketRef { get; set; }
        public string MemberName { get; set; }
        public string Status { get; set; }
        public string Paytype { get; set; }

    }

    public class TicketPayViewModels
    {
        public Guid TicketId { get; set; }
        public string TicketRef { get; set; }

        public string MemberRef { get; set; }
        public string MemberName { get; set; }

        [DisplayName("ประเภทการจ่ายเงิน *")]
        [Required(ErrorMessage = "Required")]
        public string PayType { get; set; }


        [DisplayName("สถานะ *")]
        public string Status { get; set; }

        [DisplayName("เลขที่เช็ค")]
        public string PayChequeNo { get; set; }


        [DisplayName("ธนาคาร")]
        public string PayChequeBank { get; set; }


        [DisplayName("สาขา")]
        public string PayChequeBranch { get; set; }

        public Ticket TicketDetail { get; set; }

    }

    public class TicketAppViewModels
    {
        public Guid TicketId { get; set; }
        public string TicketRef { get; set; }
        public string MemberName { get; set; }
        public bool ReadyApprove { get; set; }
        public TicketReceiveDetailViewModels TicketDetail { get; set; }

        public List<TicketReceive> ReceiveList { get; set; }
    }

    public class TicketReceiveAppViewModels
    {
        public Guid TicketId { get; set; }
        public Guid ReceiveId { get; set; }
        public TicketReceiveDetailViewModels TicketDetail { get; set; }
        public TicketReceive TicketReceive { get; set; }

    }

    public class TicketReceivePartialViewModels
    {
        public Guid TicketId { get; set; }
        public string TicketRef { get; set; }
        public string MemberName { get; set; }

        public string Type { get; set; }

        [DisplayName("ประเภทการจ่ายเงิน *")]
        [Required(ErrorMessage = "Required")]
        public string ReceiveType { get; set; }


        [DisplayName("จำนวนทองที่ค้างจ่าย")]
        public double QtyUnpaid { get; set; }

        [DisplayName("จำนวนเงินที่ค้างจ่าย")]
        public string AmountUnPaid { get; set; }

        public Nullable<double> Quantity { get; set; }
        public string Price { get; set; }


        [DisplayName("จำนวนเงิน *")]
        [Required(ErrorMessage = "Required")]
        [StringLength(11, MinimumLength = 1, ErrorMessage = "จำนวนเงินไม่เกิน 1,000 ล้านบาท")]
        public string Amount { get; set; }

        public string Status { get; set; }


        [DisplayName("เลขที่เช็ค")]
        public string PayChequeNo { get; set; }


        [DisplayName("ธนาคาร")]
        public string PayChequeBank { get; set; }


        [DisplayName("สาขา")]
        public string PayChequeBranch { get; set; }

        public TicketReceiveDetailViewModels TicketDetail { get; set; }

        public virtual List<TicketReceive> ReceiveList { get; set; }

        public MemberPayDetailViewModels PayDetail { get; set; } 
    }

    public class TicketReceiveSplitViewModels
    {
        public Guid TicketId { get; set; }
        public string TicketRef { get; set; }
        public string MemberName { get; set; }
        public string Type { get; set; }

        public string ReceiveType { get; set; }

        [DisplayName("จำนวนทองที่ค้างจ่าย")]
        public double QtyUnpaid { get; set; }

        [DisplayName("จำนวนเงินที่ค้างจ่าย")]
        public string AmountUnPaid { get; set; }

        [Required]
        [DisplayName("จำนวนทอง")]
        [DataType(DataType.Currency)]
        [Range(1, 999, ErrorMessage = "จำนวนทองระหว่าง 1 - 999 Kg")]
        public Nullable<double> Quantity { get; set; }
        public string Price { get; set; }
        public string Amount { get; set; }
        public Nullable<double> AmountPaid { get; set; }

        [DisplayName("สถานะ")]
        public string Status { get; set; }

        public string MessageWarning { get; set; }

        //public double AmtQty { get; set; }

        //public double Amtq { get; set; }

        public List<TicketReceive> ReceiveList { get; set; }
        public TicketReceiveDetailViewModels TicketDetail { get; set; }

    }

    public class ReceiveViewModels
    {
        public int ReceiveId { get; set; }
        public string ReceiveRef { get; set; }
        public Guid TicketId { get; set; }
        public string TicketRef { get; set; }
        public string MemberName { get; set; }

        [DisplayName("ยอดเงินทั้งหมด")]
        public double AmountFull { get; set; }

        [DisplayName("ยอดเงินคงเหลือ")]
        public double AmountBalance { get; set; }

        [DisplayName("ยอดเงินที่จ่ายไปแล้ว")]
        public double AmountPaid { get; set; }

        [DisplayName("Receive Type")]
        [Required(ErrorMessage = "Please select Receive type")]
        public string ReceiveType { get; set; }

        [DisplayName("Amount")]
        [Required(ErrorMessage = "Please select payment type")]
        public double Amount { get; set; }

        public string Note { get; set; }
    }

    //for IndexApp -> Finance Order Approve
    public class TicketIndexApproveViewModels
    {
        public Guid TicketId { get; set; }
        public string MemberRef { get; set; }
        public string MemberName { get; set; }
        public string TicketRef { get; set; }
        public string TradeRef { get; set; }
        public string TicketType { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateByUser { get; set; }
        public double Price { get; set; }
        public double Quantity { get; set; }
        public double Amount { get; set; }

        [DisplayName("Quantity Unpaid")]
        public double QtyUnpaid { get; set; }
        public double QtyPay { get; set; }

        [DisplayName("Amount Unpaid")]
        public double AmtUnpaid { get; set; }
        public double AmtPay { get; set; }

        public string Status { get; set; }

        public Nullable<bool> UseDeposit { get; set; }

    }






    public class TicketPayPartialViewModels
    {
        public Guid TicketId { get; set; }
        public string TicketRef { get; set; }
        public string MemberName { get; set; }

        public string Type { get; set; }

        [DisplayName("ประเภทการจ่ายเงิน *")]
        [Required(ErrorMessage = "Required")]
        public string PayType { get; set; }


        [DisplayName("จำนวนทองที่ค้างจ่าย")]
        public double QtyUnpaid { get; set; }

        [DisplayName("จำนวนเงินที่ค้างจ่าย")]
        public string AmountUnPaid { get; set; }

        public Nullable<double> Quantity { get; set; }
        public string Price { get; set; }


        [DisplayName("จำนวนเงิน *")]
        [Required(ErrorMessage = "Required")]
        [StringLength(11, MinimumLength = 1, ErrorMessage = "จำนวนเงินไม่เกิน 1,000 ล้านบาท")]
        public string Amount { get; set; }

        public string Status { get; set; }


        [DisplayName("เลขที่เช็ค")]
        public string PayChequeNo { get; set; }


        [DisplayName("ธนาคาร")]
        public string PayChequeBank { get; set; }


        [DisplayName("สาขา")]
        public string PayChequeBranch { get; set; }

        public TicketPayDetailViewModels TicketDetail { get; set; }

        public virtual List<TicketPay> PayList { get; set; }

        public MemberPayDetailViewModels PayDetail { get; set; }
    }

    public class TicketPaySplitViewModels
    {
        public Guid TicketId { get; set; }
        public string TicketRef { get; set; }
        public string MemberName { get; set; }
        public string Type { get; set; }

        public string PayType { get; set; }

        [DisplayName("จำนวนทองที่ค้างจ่าย")]
        public double QtyUnpaid { get; set; }

        [DisplayName("จำนวนเงินที่ค้างจ่าย")]
        public string AmountUnPaid { get; set; }

        [Required]
        [DisplayName("จำนวนทอง")]
        [DataType(DataType.Currency)]
        [Range(1, 999, ErrorMessage = "จำนวนทองระหว่าง 1 - 999 Kg")]
        public Nullable<double> Quantity { get; set; }
        public string Price { get; set; }
        public string Amount { get; set; }
        public Nullable<double> AmountPaid { get; set; }

        [DisplayName("สถานะ")]
        public string Status { get; set; }

        public string MessageWarning { get; set; }

        //public double AmtQty { get; set; }

        //public double Amtq { get; set; }

        public List<TicketPay> PayList { get; set; }
        public TicketPayDetailViewModels TicketDetail { get; set; }

    }

    public class TicketPayDetailViewModels
    {
        public Guid TicketId { get; set; }

        [DisplayName("รหัสออเดอร์")]
        public string TicketRef { get; set; }

        [DisplayName("ประเภท")]
        public string TicketType { get; set; }

        [DisplayName("รหัสใบเทรด")]
        public string TradeRef { get; set; }

        [DisplayName("เลขที่บัญชีซื้อขายทองคำ")]
        public string MemberRef { get; set; }

        [DisplayName("ชื่อสมาชิก")]
        public string MemberName { get; set; }

        [DisplayName("ราคา")]
        public double Price { get; set; }

        [DisplayName("จำนวนทอง")]
        public double Quantity { get; set; }

        [DisplayName("จำนวนเงิน")]
        public double Amount { get; set; }

        [DisplayName("จำนวนทองที่ยังไม่ตัด")]
        public double QtyUnpaid { get; set; }

        [DisplayName("จำนวนทองที่ตัด")]
        public double QtyPaid { get; set; }

        [DisplayName("จำนวนเงินที่ยังไม่จ่าย")]
        public double AmtUnpaid { get; set; }

        [DisplayName("จำนวนเงินที่จ่าย")]
        public double AmountPaid { get; set; }
        public string MessageWarning { get; set; }

        public Nullable<bool> UseDeposit { get; set; }

    }

}