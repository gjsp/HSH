using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HSH.Data.Models;

namespace HSH.Backend.Helper
{
    public class SessionHelper
    {
        public const string SESSION_MANAGER = "SESSION_BACKEND";
        public static AspNetUsers CurrentUserInfo
        {
            get
            {
                if (null != HttpContext.Current.Session[SESSION_MANAGER])
                    return (AspNetUsers)HttpContext.Current.Session[SESSION_MANAGER];
                else
                    return null;
            }
            set
            {
                if (HttpContext.Current.Session != null)
                    HttpContext.Current.Session[SESSION_MANAGER] = value;
            }
        }
    }
}