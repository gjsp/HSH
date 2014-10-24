using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HSH.Data.Models;

namespace HSH.Data.Models
{
    public class MarketPriceViewModels
    {
        public double MaxKg { get; set; }
        public double MaxKg1 { get; set; }
        public double MaxKg2 { get; set; }
        public double MaxKg3 { get; set; }
        public double MaxKg4 { get; set; }
        public bool SystemHalt { get; set; }
        public decimal Premium { get; set; }
        public decimal Discount { get; set; }
        
        [DisplayName("Spread")]
        public double Spread { get; set; }
        public double Spread1 { get; set; }
        public double Spread2 { get; set; }
        public double Spread3 { get; set; }
        public double Spread4 { get; set; }

        [DisplayName("Duedate")]
        public double Duedate1 { get; set; }
        public double Duedate2 { get; set; }
        public double Duedate3 { get; set; }
        public double Duedate4 { get; set; }

        [DisplayName("CreditLimit")]
        public decimal CreditLimit1 { get; set; }
        public double CreditLimit2 { get; set; }
        public double CreditLimit3 { get; set; }
        public double CreditLimit4 { get; set; }

        public double MarginType1 { get; set; }
        public double MarginType2 { get; set; }
        public double MarginType3 { get; set; }
        public double MarginType4 { get; set; }

        [DisplayName("ราคาในการคำนวน")]
        public string SpotCalculate { get; set; }//เลือก bid or ask
        public decimal SpotCalculateValue { get; set; }//bid ask value for calculate

        [DisplayName("อัตราแลกเปลี่ยน")]
        public double ThbCalculateValue { get; set; }//THB  for calculate
        
        public decimal BidSpot { get; set; }
        public decimal AskSpot { get; set; }
        public decimal BidThb { get; set; }
        public decimal AskThb { get; set; }

        public decimal Bid99Kg { get; set; }
        public decimal Ask99Kg { get; set; }
        public decimal Bid99Bg { get; set; }
        public decimal Ask99Bg { get; set; }


        public double Bid99Bg1 { get; set; }
        public double Ask99Bg1 { get; set; }
        public double Bid99Bg2 { get; set; }
        public double Ask99Bg2 { get; set; }
        public double Bid99Bg3 { get; set; }
        public double Ask99Bg3 { get; set; }
        public double Bid99Bg4 { get; set; }
        public double Ask99Bg4 { get; set; }

    }
}