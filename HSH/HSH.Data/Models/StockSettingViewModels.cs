using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HSH.Data.Models;

namespace HSH.Data.Models
{
    public class StockSettingViewModels
    {
        [Required(ErrorMessage = "Required")]
        [DisplayName("Spread Level 1")]
        public double Spread1 { get; set; }
        [Required(ErrorMessage = "Required")]
        [DisplayName("Spread Level 2")]
        public double Spread2 { get; set; }

        [Required(ErrorMessage = "Required")]
        [DisplayName("Spread Level 3")]
        public double Spread3 { get; set; }

        [Required(ErrorMessage = "Required")]
        [DisplayName("Spread Level 4")]
        public double Spread4 { get; set; }


        [Required(ErrorMessage = "Required")]
        [DisplayName("MaxKg Level 1")]
        public double MaxKg1 { get; set; }

        [Required(ErrorMessage = "Required")]
        [DisplayName("MaxKg Level 2")]
        public double MaxKg2{ get; set; }

        [Required(ErrorMessage = "Required")]
        [DisplayName("MaxKg Level 3")]
        public double MaxKg3 { get; set; }

        [Required(ErrorMessage = "Required")]
        [DisplayName("MaxKg Level 4")]
        public double MaxKg4 { get; set; }


        [Required(ErrorMessage="Required")]
        [DisplayName("Duedate Level 1")]
        public double Duedate1 { get; set; }

        [Required(ErrorMessage = "Required")]
        [DisplayName("Duedate Level 2")]
        public double Duedate2 { get; set; }

        [Required(ErrorMessage = "Required")]
        [DisplayName("Duedate Level 3")]
        public double Duedate3 { get; set; }

        [Required(ErrorMessage = "Required")]
        [DisplayName("Duedate Level 4")]
        public double Duedate4 { get; set; }


        [Required(ErrorMessage = "Required")]
        [DisplayName("CreditLimit Level 1")]
        public double CreditLimit1 { get; set; }

        [Required(ErrorMessage = "Required")]
        [DisplayName("CreditLimit Level 2")]
        public double CreditLimit2 { get; set; }

        [Required(ErrorMessage = "Required")]
        [DisplayName("CreditLimit Level 3")]
        public double CreditLimit3 { get; set; }

        [Required(ErrorMessage = "Required")]
        [DisplayName("CreditLimit Level 4")]
        public double CreditLimit4 { get; set; }


        [Required(ErrorMessage = "Required")]
        [DisplayName("Margin Level 1")]
        public double MarginType1 { get; set; }

        [Required(ErrorMessage = "Required")]
        [DisplayName("Margin Level 2")]
        public double MarginType2 { get; set; }

        [Required(ErrorMessage = "Required")]
        [DisplayName("Margin Level 3")]
        public double MarginType3 { get; set; }

        [Required(ErrorMessage = "Required")]
        [DisplayName("Margin Level 4")]
        public double MarginType4 { get; set; }

    }
}