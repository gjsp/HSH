using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HSH.Data.Models;

namespace HSH.Member.Helper
{
    public class SessionHelper
    {
        public const string SESSION_MANAGER = "SessionMember";
        public static UserOnline CurrentUserInfo
        {
            get
            {
                if (null != HttpContext.Current.Session[SESSION_MANAGER])
                    return (UserOnline)HttpContext.Current.Session[SESSION_MANAGER];
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