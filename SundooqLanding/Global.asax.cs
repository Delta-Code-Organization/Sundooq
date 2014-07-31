using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace SundooqLanding
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
        //protected void Application_Error(object sender, EventArgs e)
        //{
        //    string Message = Request.UserAgent + "<br/>";
        //    Message += Request.RawUrl + "<br/>";
        //    Message += Request.UserHostAddress + "<br/>";
        //    Message += Request.UserHostName + "<br/>";
        //    if (Session["User"] != null)
        //        Message += (Session["User"] as SundooqLanding.Models.Users).Email + "<br/><br/>";
        //    Message += Server.GetLastError().Message + "<br/>";
        //    SundooqLanding.Models.Helpers.sendEmail("amir-aly-eesa@hotmail.com", "Sundooq Error", Server.GetLastError().Message, Models.MailTypes.Error, null);
        //    Response.Redirect("/Home/error");
        //}
    }
}