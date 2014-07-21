using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SundooqLanding.Models;

namespace SundooqLanding.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            HttpCookie myCookie = new HttpCookie("Sundooq");
            myCookie = Request.Cookies["Sundooq"];

            // Read the cookie information and display it.
            if (myCookie != null)
            {
                    string mail = myCookie.Value.Split('$')[0];
                    string pass = myCookie.Value.Split('$')[1];
                    if (mail == "facebook")
                    {
                        SundooqDBEntities2 db = new SundooqDBEntities2();
                        Users user = db.Users.Where(u => u.Email == pass).SingleOrDefault();
                        
                            if (user.DateOfBirth == null)
                            {
                                user.DateOfBirth = DateTime.Now.AddYears(-16);
                                user.Tags = "";
                            }
                            Session["User"] = user;
                            Session.Add("Tags", user.Tags);
                            if (Session["User"] != null)
                                return RedirectToAction("Home", "User");
                    }
                    Users CurrentUser = new Users();
                    CurrentUser.Email = mail;
                    CurrentUser.Password = pass;
                    bool success;
                    var result = new JSONReply();
                    result.Msg = CurrentUser.Login(out success);
                    result.result = success;
                    Session.Add("Tags", CurrentUser.Tags);
            }
            if (Session["User"] != null)
                return RedirectToAction("Home", "User");
            return View();
        }

        [HttpPost]
        public JsonResult Index(string _mail, string _password)
        {
            Users NewUser = new Users();
            NewUser.Email = _mail;
            NewUser.Password = _password;
            bool success;
            JSONReply result = new JSONReply();
            result.Msg = NewUser.regiter(out success);
            result.result = success;
            return Json(result);
        }

        public ActionResult login()
        {
            return View();
        }
        public ActionResult logout()
        {
            Session.Clear();
            return View("index");
        }
        public ActionResult article()
        {
            return View();
        }
        public ActionResult error()
        {
            return View();
        }
    }
}
