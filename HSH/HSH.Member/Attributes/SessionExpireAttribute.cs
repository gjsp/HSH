using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HSH.Member.Helper;

namespace HSH.Member.Attributes
{
    public class SessionExpireAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if ( SessionHelper.CurrentUserInfo == null)
            {
                filterContext.Result = new RedirectResult("~/Home/UserLogin");
            }

            ////get controlname and view
            //string actionName = filterContext.ActionDescriptor.ActionName;
            //string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;

            //if (actionName == "Index" && controllerName == "TradeLots")
            //{
            //    string xx = "xxx";
            //}
            //var types =
            //from a in AppDomain.CurrentDomain.GetAssemblies()
            //from t in a.GetTypes()
            //where typeof(IController).IsAssignableFrom(t) &&
            //   string.Equals(controllerName + "Controller", t.Name, StringComparison.OrdinalIgnoreCase)
            //select t;
            //List<string> lt;
            //var controllerType = types.FirstOrDefault();

            //if (controllerType == null)
            //{
            //    lt = Enumerable.Empty<string>().ToList();
            //}
            //lt = new ReflectedControllerDescriptor(controllerType)
            //    .GetCanonicalActions().Select(x => x.ActionName)
            //    .ToList();


            base.OnActionExecuting(filterContext);
        }
    }
}