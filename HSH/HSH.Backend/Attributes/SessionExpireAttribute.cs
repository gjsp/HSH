using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HSH.Backend.Attributes
{
    public class SessionExpireAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Helper.SessionHelper.CurrentUserInfo == null)
            {
                //filterContext.Result = new RedirectResult("~/Account/Login");
                filterContext.Result = new RedirectResult("~/Account/Login");
                //filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new { controller = "Account", action = "Login" }));
              
            }
            base.OnActionExecuting(filterContext);
        }
    }
}