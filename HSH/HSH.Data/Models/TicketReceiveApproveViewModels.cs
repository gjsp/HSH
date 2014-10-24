using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HSH.Data.Models;

namespace HSH.Data.Models
{
    public class TicketReceiveApproveViewModels
    {
        public System.Guid ReceiveId { get; set; }
        public string ReceiveRef { get; set; }
        public string MemberName { get; set; }
        public string MemberRef { get; set; }

        public string TicketType { get; set; }
        public string ReceiveType { get; set; }
        public string Type { get; set; }
        public Nullable<double> Price { get; set; }
        public Nullable<double> Quantity { get; set; }
        public Nullable<double> Amount { get; set; }
        public string Status { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public virtual Ticket Ticket { get; set; }

        //For Show/Hide in approve receive page
        public bool Approved { get; set; }
    }


    public class TicketPayApproveViewModels
    {
        public System.Guid PayId { get; set; }
        public string PayRef { get; set; }
        public string MemberName { get; set; }
        public string MemberRef { get; set; }

        public string TicketType { get; set; }
        public string PayType { get; set; }
        public string Type { get; set; }
        public Nullable<double> Price { get; set; }
        public Nullable<double> Quantity { get; set; }
        public Nullable<double> Amount { get; set; }
        public string Status { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public virtual Ticket Ticket { get; set; }

        //For Show/Hide in approve receive page
        public bool Approved { get; set; }
    }


}