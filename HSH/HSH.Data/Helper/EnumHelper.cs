using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Collections;
using System.Web.Mvc;
using System.Reflection;

namespace HSH.Data.Helper
{
   
    public static class EnumHelper
    {
        public enum TradeStatus
        {
            Pending = 1,
            Accept = 2,
            Reject = 3
        }

        public enum RejectType
        {
            Client = 1,
            Admin = 2,
            System = 3
        }

        public enum SpotCalculate
        {
            Bid = 1,
            Ask = 2
        }

        public enum DealType
        {
            Buy,
            Sell
        }

        public enum RoleOnline
        {
            Member,
            Admin,
            Sale,
            Trader
        }

        public enum PauseStatus
        {
            Pause, UnPause
        }

        public enum CashRoleType
        {
            Cashier, Finance
        }
        public enum UserTypeRole
        {
            Maker, Checker
        }

        public enum MemberType
        {
            [Description("บุคคลธรรมดา")]
            Normal,
            //[Description("ร้านทอง")]
            //Shop,
            [Description("นิติบุคคล/ร้านทอง")]
            Company,
            [Description("ต่างประเทศ")]
            Foreign,
            [Description("Walkin")]
            Walkin,
            [Description("VIP")]
            VIP
        }

        public enum TransferType
        {
            Cash, Gold
        }

        public enum CallForce
        {
            Normal, Call, Force
        }

        public enum TicketReceiveType { Partial, Split }

        public enum TicketPayType { Partial, Split }

        public enum UserType
        {
            Admin = 0,
            Cashier = 1,
            Finance = 2,
            Operation = 3,
            Risk = 4,
            Stock = 5,
            Trader = 6
        }

        public enum CompanyAssetType
        {
            Import, Export
        }
        public enum TransactionType
        {
            [Description("ฝาก")]
            In ,
            [Description("ถอน")]
            Out 
        }

        public enum DirectionAssetType
        {
            In = 1,
            Out = 2,
            Deposit = 3,
            Withdraw = 4
        }
        public enum AssetType
        {
            Cash = 1,
            Gold = 2
        }

        public enum PayTypeFinance
        {
            [Description("ATS")]
            ATS,
            [Description("Transfer")]
            Transfer,
            [Description("Cheque")]
            Cheque,
            [Description("Card")]
            Card,
            [Description("InternetBanking")]
            DirectDebit
        }

        public enum PayTypeCashier
        {
            [Description("Cash")]
            Cash
        }

        public enum PayStatus
        {
            Partial = 1,
            Paid = 2,
            None = 3
        }


        public enum MemberLevel
        {
            [Description("เลเวล 1")]
            Level1 = 1,
            [Description("เลเวล 2")]
            Level2 = 2,
            [Description("เลเวล 3")]
            Level3 = 3,
            [Description("เลเวล 4")]
            Level4 = 4
        }

        public enum TicketType
        {
            [Description("Buy")]
            Buy,
            [Description("Sell")]
            Sell
        }
        public enum TicketStatus
        {
            Pending,
            [Description("ฝากทอง")]
            Deposit,
            [Description("เงินออก-รอทอง")]
            WaitCash,
            [Description("ตัดทองฝาก")]
            Cut,
            [Description("รับทอง")]
            Send,
            [Description("ทองออก-รอเงิน")]
            WaitGold,
            Complete
        }
        public enum TicketSellStatus
        {
            [Description("ตัดทองฝาก")]
            Cut,
            [Description("รับทอง")]
            Send,
            [Description("เงินออก-รอทอง")]
            WaitCash
        }
        public enum TicketBuyStatus
        {
            [Description("ฝากทอง")]
            Deposit,
            [Description("ทองออก-รอเงิน")]
            WaitGold
        }

        public enum MemberClass
        {
            [Description("Individual")]
            Individual,
            [Description("Corperate")]
            Corperate
        }

        public enum MemberIndiType
        {
            [Description("บุคคลธรรมดา")]
            Normal,
            //[Description("ร้านทอง")]
            //Shop,
            [Description("ต่างประเทศ")]
            Foreign,
            [Description("Walkin")]
            Walkin,
            [Description("VIP")]
            VIP
        }

        public enum MemberAssetPayType
        {
            [Description("Cashier")]
            Cashier,
            [Description("Bank")]
            Bank
        }
        public enum MemberAssetStatus
        {
            Wait, Approved, DisApproved
        }

        public enum MemberAssetPayTypeDetail
        {
            [Description("Cash")]
            Cash,
            [Description("Card")]
            Card,
            [Description("Cheque")]
            Cheque,
            [Description("Transfer")]
            Transfer,
            [Description("ATS")]
            ATS,
            [Description("InternetBanking")]
            DirectDebit
        }

        public enum BankName
        {
            [Description("กรุงเทพ")]
            BBL,
            [Description("ไทยพาณิชย์")]
            SCB,
            [Description("กสิกรไทย")]
            KBank,
            [Description("กรุงไทย")]
            KTB,
            [Description("กรุงศรี")]
            BAY,
            [Description("ธนชาติ")]
            TBank
        }

        public static string GetDescription<TEnum>(this TEnum value)
        {
            var fi = value.GetType().GetField(value.ToString());

            if (fi != null)
            {
                var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attributes.Length > 0)
                {
                    return attributes[0].Description;
                }
            }

            return value.ToString();
        }

        /// <summary>
        /// Build a select list for an enum
        /// </summary>
        public static SelectList SelectListFor<T>() where T : struct
        {
            Type t = typeof(T);
            return !t.IsEnum ? null
                             : new SelectList(BuildSelectListItems(t), "Value", "Text");
        }

        /// <summary>
        /// Build a select list for an enum with a particular value selected 
        /// </summary>
        public static SelectList SelectListFor<T>(T selected) where T : struct
        {
            Type t = typeof(T);
            return !t.IsEnum ? null
                             : new SelectList(BuildSelectListItems(t), "Value", "Text", selected.ToString());
        }

        private static IEnumerable<SelectListItem> BuildSelectListItems(Type t)
        {
            return Enum.GetValues(t)
                       .Cast<Enum>()
                       .Select(e => new SelectListItem { Value = e.ToString(), Text = e.GetDescription() });
        }
       

    }
}