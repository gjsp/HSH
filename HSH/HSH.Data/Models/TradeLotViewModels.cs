using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HSH.Data.Models;

namespace HSH.Data.Models
{
    public class TradeLotViewModels
    {
    }

    public class TradeLotCreateViewModels
    {

        [Required]
        [DisplayName("Trade Type")]
        public string TradeType { get; set; }


        [Required]
        [StringLength(6, ErrorMessage = "Over Limit Price")]
        public string Price { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Range(1, 999, ErrorMessage = "Quantity must be between 1 and 999")]
        public double Quantity { get; set; }

    }
}