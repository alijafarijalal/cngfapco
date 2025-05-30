using cngfapco.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using cngfapco.Models;
using System.Globalization;
using System.Web.Management;

namespace cngfapco
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //BundleConfig.RegisterBundles(BundleTable.Bundles);
            ModelBinders.Binders.Add(typeof(DateTime), new DateTimeBinder());
            ModelBinders.Binders.Add(typeof(DateTime?), new NullableDateTimeBinder());
            //GlobalFilters.Filters.Add(new TrackLoginsFilter());
            //SecurityHelper.GetLoggedInUsers().Remove(WebSecurity.CurrentUserName);

        }
        protected void Application_BeginRequest(object sender, EventArgs e) //PersianCulture برای استفاده از کلاس 
        {
            var persianCulture = new PersianCulture();
            Thread.CurrentThread.CurrentCulture = persianCulture;
            Thread.CurrentThread.CurrentUICulture = persianCulture;

            //CultureInfo newCulture = (CultureInfo)
            //System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            //newCulture.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            //newCulture.DateTimeFormat.DateSeparator = "-";
            //Thread.CurrentThread.CurrentCulture = newCulture;

        }

        //protected void Application_Error(object sender, EventArgs e)
        //{
        //    Exception exception = Server.GetLastError();
        //    Response.Clear();

        //    //HttpException httpException = exception as HttpException;
        //    var ex = Server.GetLastError();
        //    var httpException = ex as HttpException ?? ex.InnerException as HttpException;
        //    if (httpException == null) return;

        //    if (httpException.WebEventCode == WebEventCodes.RuntimeErrorPostTooLarge)
        //    {
        //        //handle the error
        //        Response.Write("Sorry, file is too big"); //show this message for instance
        //    }

        //    if (httpException != null)
        //    {
        //        string action;

        //        switch (httpException.GetHttpCode())
        //        {
        //            case 400:
        //                // bad request
        //                action = "Page400";
        //                break;
        //            case 404:
        //                // page not found
        //                action = "Page404";
        //                break;
        //            case 500:
        //                // server error
        //                action = "Page500";
        //                break;
        //            case 504:
        //                // server error
        //                action = "Page504";
        //                break;
        //            default:
        //                action = "General";
        //                break;
        //        }

        //        // clear error on server
        //        Server.ClearError();
        //        Response.Redirect(String.Format("~/Home/{0}/?message={1}", action, exception.Message));
        //    }

        //}

        ////
        ///
        //public class TrackLoginsFilter : ActionFilterAttribute
        //{
        //    public override void OnActionExecuting(ActionExecutingContext filterContext)
        //    {
        //        Dictionary<string, DateTime> loggedInUsers = SecurityHelper.GetLoggedInUsers();

        //        if (HttpContext.Current.User.Identity.IsAuthenticated)
        //        {
        //            if (loggedInUsers.ContainsKey(HttpContext.Current.User.Identity.Name))
        //            {
        //                loggedInUsers[HttpContext.Current.User.Identity.Name] = System.DateTime.Now;
        //            }
        //            else
        //            {
        //                loggedInUsers.Add(HttpContext.Current.User.Identity.Name, System.DateTime.Now);
        //            }

        //        }

        //        // remove users where time exceeds session timeout
        //        var keys = loggedInUsers.Where(u => DateTime.Now.Subtract(u.Value).Minutes >
        //                   HttpContext.Current.Session.Timeout).Select(u => u.Key);
        //        foreach (var key in keys)
        //        {
        //            loggedInUsers.Remove(key);
        //        }

        //    }
        //}

        //public static class SecurityHelper
        //{
        //    public static Dictionary<string, DateTime> GetLoggedInUsers()
        //    {
        //        Dictionary<string, DateTime> loggedInUsers = new Dictionary<string, DateTime>();

        //        if (HttpContext.Current != null)
        //        {
        //            loggedInUsers = (Dictionary<string, DateTime>)HttpContext.Current.Application["loggedinusers"];
        //            if (loggedInUsers == null)
        //            {
        //                loggedInUsers = new Dictionary<string, DateTime>();
        //                HttpContext.Current.Application["loggedinusers"] = loggedInUsers;
        //            }
        //        }
        //        return loggedInUsers;

        //    }
        //}
    }
}
