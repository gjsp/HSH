//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HSH.Data.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class CompanyAsset
    {
        public int AssetId { get; set; }
        public string AssetRef { get; set; }
        public string AssetType { get; set; }
        public string Direction { get; set; }
        public Nullable<double> Quantity { get; set; }
        public Nullable<double> Amount { get; set; }
        public string Note { get; set; }
        public string Status { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string ApproveBy { get; set; }
        public Nullable<System.DateTime> ApproveDate { get; set; }
        public Nullable<bool> Active { get; set; }
    
        public virtual AspNetUsers AspNetUsers { get; set; }
        public virtual AspNetUsers AspNetUsers1 { get; set; }
    }
}