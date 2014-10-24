using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HSH.Data.Models;

namespace HSH.Data.Models
{
   
    public class CollateralViewModels
    {
        [Required]
        [DisplayName("จำนวนเงินที่ซื้อขายได้ต่อ 1 kg")]
        [StringLength(7, ErrorMessage = "Over Limit")]
        public string CashPerKg { get; set; }

        [Required]
        [DisplayName("จำนวนเงินของทองคำต่อ 1 kg")]
        [StringLength(9, ErrorMessage = "Over Limit")]
        public string GoldPerKg { get; set; }

        [Required]
        [DisplayName("ตัวคูณ %")]
        [DataType(DataType.Currency)]
        [Range(0, 100, ErrorMessage = "Number must be between 1 and 100")]
        public double GoldPercent { get; set; }

    }


}