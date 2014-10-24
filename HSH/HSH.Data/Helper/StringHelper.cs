using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Security.Cryptography;
using System.Globalization;

namespace HSH.Data.Helper
{
    public static class StringHelper
    {
        public static string stringNotFound = "=== Data Not Found ===";
        public static string stringPartialViewKey = "_PartialView";
        public static string stringLocationHrefLogin = "../Account/login";
        public static string stringCurrentPageIndex = "CurrentPageIndex";


        public static string formatnumber2Digit = "#,##0.#0";
        public static string formatnumber0Digit = "#,##0";
        public static string formatdate = "MM/dd/yyyy";//"dd/MM/yyyy";
        public static string formatdateBootstrap = "MM/DD/YYYY";//"dd/MM/yyyy";

        public static IFormatProvider culture = new CultureInfo("en-US", true);

        public static string ToStringFormat(this DateTime date)
        {
            return date.ToString(formatdate, culture);
        }
        public static string ToFormatStringDate(this DateTime? data)
        {
            if (data.HasValue == false)
            {
                return "";
            }
            else
            {
                return data.Value.ToString(formatdate, culture);
            }
        }

        public static string ToStringCurrency(string values)
        {
            if (values == string.Empty)
            {
                return string.Empty;
            }
            else
            {
                return Convert.ToDouble(values).ToString(formatnumber0Digit);
            }
        }






        public static string ToMD5Hash(this string strConvert)
        {
            // Use input string to calculate MD5 hash
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(strConvert);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // Convert the byte array to hexadecimal string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }
            return sb.ToString();
        }



        public static DateTime? ToDateFormat(this string strConvert)
        {
            if (string.IsNullOrEmpty(strConvert))
                return null;

            //dd-MM-yyyy
            string[] strDate = strConvert.Split('-');
            return new DateTime(int.Parse(strDate[2]), int.Parse(strDate[1]), int.Parse(strDate[0]));
        }

        public static DateTime? ToDateFormatUpdateForm(this string strConvert)
        {
            if (string.IsNullOrEmpty(strConvert))
                return null;

            //dd-MM-yyyy
            //IFormatProvider culture = new CultureInfo("en-US", true);
            //return DateTime.Parse(strConvert, culture, System.Globalization.DateTimeStyles.None);
            return DateTime.ParseExact(strConvert, "MM/dd/yyyy", culture);

        }



        public static string ToStringTimeFormat(this DateTime date)
        {
            //return date.Hour.ToString("0#") + ":" + date.Minute.ToString("0#") + ":" + date.Second.ToString("0#");// date.ToString("hh:mm:ss", culture);
            //return "";// date.ToString("hh:mm:ss", new CultureInfo("th-TH", true));
            return date.ToString("hh:mm:ss", culture);
        }

        public static string ToStringFormatExcel(this DateTime date)
        {
            return date.ToString("MM/dd/yyyy", culture);
        }

        public static string ToStringDateTimeFormat(this DateTime date)
        {
            return date.ToString("MM/dd/yyyy hh:mm:ss", culture);
        }

        public static string ToStringFormat(this DateTime? date)
        {
            if (date != null)
                return date.Value.ToString("MM/dd/yyyy", culture);
            else
                return "";
        }

        public static string ToStringDateTimeFormat(this DateTime? date)
        {
            if (date != null)
                return date.Value.ToString("MM/dd/yyyy hh:mm", culture);
            else
                return "";
        }

        public static Int32? ToInt32(this string obj)
        {
            if (string.IsNullOrEmpty(obj) || obj == "-1")
                return null;
            else
                return Convert.ToInt32(obj);
        }

        public static bool? ToBool(this string obj)
        {
            if (string.IsNullOrEmpty(obj) || obj == "-1")
                return null;
            else
                return Convert.ToBoolean(Convert.ToInt32(obj));
        }

        public static string GenerateCreateOrderNo()
        {
            //return "ORD" + DateTime.Now.ToString("yyyyMMdd") + "XXX";
            return "ORD" + CheckDateRegion(DateTime.Now) + "XXX";
        }

        public static string CheckDateRegion(DateTime dDate)
        {
            string strYear = string.Empty;
            if (dDate.Year > 2500)
                strYear = (dDate.Year - 543).ToString();
            else
                strYear = dDate.Year.ToString();

            return strYear + dDate.Month.ToString("0#") + dDate.Day.ToString("0#");
        }


    }
}