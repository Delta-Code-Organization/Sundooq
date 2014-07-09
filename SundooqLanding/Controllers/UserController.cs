using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SundooqLanding.Models;
using System.Web.Configuration;
namespace SundooqLanding.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /User/
        public ActionResult logout()
        {
            Session.Clear();
            if (Request.Cookies["Sundooq"] != null)
            {
                HttpCookie myCookie = new HttpCookie("Sundooq");
                myCookie.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(myCookie);
            }
            return RedirectToAction("index", "Home");
        }

        public ActionResult Index()
        {
            SundooqDBEntities2 db = new SundooqDBEntities2();
            Users currentUser = (Users)Session["User"];
            if (currentUser == null || currentUser.AccountStatus < 1)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.User = currentUser;
            string[] Tech = WebConfigurationManager.AppSettings["Technology"].ToString().Split('#');
            string[] Business = WebConfigurationManager.AppSettings["Business"].ToString().Split('#');
            string[] Health = WebConfigurationManager.AppSettings["Health"].ToString().Split('#');
            string[] News = WebConfigurationManager.AppSettings["News"].ToString().Split('#');
            string[] Sources = WebConfigurationManager.AppSettings["Sources"].ToString().Split('#');
            List<Sources> LOSources = db.Sources.ToList();
            List<string> LOSToBeConverted = new List<string>();
            string[] AutoComSources;
            foreach (Sources item in LOSources)
            {
                LOSToBeConverted.Add(item.SourceName);
            }
            AutoComSources = LOSToBeConverted.ToArray();
            TempData["AllSources"] = AutoComSources;
            List<Topics> LOTopics = db.Topics.ToList();
            string LOTagsToBeConverted = "";
            string[] AutoComTags;
            foreach (Topics item in LOTopics)
            {
                LOTagsToBeConverted += item.Tags;
            }
            AutoComTags = LOTagsToBeConverted.Split('#');
            TempData["AllTags"] = AutoComTags;
            ViewBag.Tech = Tech;
            ViewBag.Business = Business;
            ViewBag.Health = Health;
            ViewBag.News = News;
            ViewBag.Sources = Sources;
            TempData.Keep();
            return View();
        }

        [HttpPost]
        public JsonResult GetAllTags()
        {
            string[] LOT = TempData["AllTags"] as string[];
            LOT = LOT.Distinct().ToArray();
            Users currentUser = (Users)Session["User"];
            string[] AlreadyTag = currentUser.Tags.Split('#');
            LOT = LOT.Except(AlreadyTag).ToArray();
            TempData.Keep();
            return Json(LOT);
        }

        [HttpPost]
        public JsonResult GetAllSources()
        {
            string[] LOT = TempData["AllSources"] as string[];
            LOT = LOT.Distinct().ToArray();
            Users currentUser = (Users)Session["User"];
            string[] AlreadyTag = currentUser.Tags.Split('#');
            LOT = LOT.Except(AlreadyTag).ToArray();
            TempData.Keep();
            return Json(LOT);
        }

        public ActionResult Home(string id = "1")
        {
            Session["Sorting"] = id;
            Users currentUser = (Users)Session["User"];
            if (Session["Tags"] == null)
                Session["Tags"] = currentUser.Tags;
            if (currentUser == null || currentUser.AccountStatus < 1)
            {
                return RedirectToAction("Index", "User");
            }
            if (currentUser.AccountStatus == 1)
            {
                return RedirectToAction("Activate", "User");
            }
            if (id == "1")
            {
                List<Topics> topics ;
                if (Session["Topics"] == null || Session["Tags"].ToString() != currentUser.Tags)
                {
                    topics = new Topics().GetUserTopics().ToList();
                    Session["Tags"] = currentUser.Tags;
                }
                else
                {
                    topics = (List<Topics>)Session["topics"];
                }
                ViewBag.Sorting = 1;
                ViewBag.Topics = topics.OrderByDescending(p => p.CustomRank).ToList();
                Session["topics"] = ViewBag.Topics;
            }
            else
            {
                List<Topics> topics;
                if (Session["Topics"] == null || Session["Tags"].ToString() != currentUser.Tags)
                {
                    topics = new Topics().GetUserTopics().ToList();
                    Session["Tags"] = currentUser.Tags;
                }
                else
                {
                    topics = (List<Topics>)Session["topics"];

                }
                ViewBag.Sorting = 0;
                ViewBag.Topics = topics.OrderByDescending(p => p.PubDate).ToList();
                Session["topics"] = ViewBag.Topics;
            }
            ViewBag.currenttags = currentUser.Tags;
            return View();
        }
        public ActionResult History()
        {
            return View();
        }
        [HttpPost]
        public JsonResult Update(string _mail, string _password, string _gender, string _dob, string _tags)
        {
            Users CurrentUser = (Users)Session["User"];
            JSONReply result = new JSONReply();
            if (CurrentUser == null)
            {

                result.Msg = "Update failed !";
                result.result = false;
                return Json(result);
            }
            CurrentUser.Email = _mail;
            CurrentUser.Password = _password;
            CurrentUser.DateOfBirth = DateTime.Parse(_dob);
            CurrentUser.Gender = int.Parse(_gender);
            CurrentUser.Tags = _tags;
            CurrentUser.AccountStatus = 2;
            bool success;
            result = new JSONReply();
            result.Msg = CurrentUser.Update(out success);
            result.result = success;
            Session["User"] = CurrentUser;
            return Json(result);

        }
        public JsonResult login(string _mail, string _password)
        {
            JSONReply result = new JSONReply();
            Users CurrentUser = new Users();
            CurrentUser.Email = _mail;
            CurrentUser.Password = _password;
            bool success;
            result = new JSONReply();
            result.Msg = CurrentUser.Login(out success);
            result.result = success;
            HttpCookie myCookie = new HttpCookie("Sundooq");
            DateTime now = DateTime.Now;
            // Set the cookie value.
            myCookie.Value = _mail + "$" + _password;
            // Set the cookie expiration date.
            myCookie.Expires = now.AddYears(50); // For a cookie to effectively never expire
            // Add the cookie.
            Response.Cookies.Add(myCookie);
            Session.Add("Tags", CurrentUser.Tags);
            return Json(result);
        }
        public ActionResult Activate(string id)
        {
            Users NewUser = new Users();
            NewUser.Tags = id;
            JSONReply result = new JSONReply();
            NewUser = NewUser.Activate();
            if (NewUser == null && Session["User"] != null)
                NewUser = (Users)Session["User"];
            if (NewUser != null)
            {
                string[] Tech = WebConfigurationManager.AppSettings["Technology"].ToString().Split('#');
                string[] Business = WebConfigurationManager.AppSettings["Business"].ToString().Split('#');
                string[] Health = WebConfigurationManager.AppSettings["Health"].ToString().Split('#');
                string[] News = WebConfigurationManager.AppSettings["News"].ToString().Split('#');
                string[] Sources = WebConfigurationManager.AppSettings["Sources"].ToString().Split('#');
                ViewBag.Tech = Tech;
                ViewBag.Business = Business;
                ViewBag.Health = Health;
                ViewBag.News = News;
                ViewBag.Sources = Sources;
                ViewBag.User = NewUser;
                return View();
            }
            else
                return RedirectToAction("index", "Home");
        }
        [HttpPost]
        public void Manage(string tag)
        {
            Users Current = (Users)Session["User"];
            Users NewUser = new Users();
            NewUser.Id = Current.Id;
            NewUser.Email = Current.Email;
            NewUser.Password = Current.Password;
            NewUser.Tags = Current.Tags;
            NewUser.RegisteredWith = Current.RegisteredWith;
            NewUser.DateOfBirth = Current.DateOfBirth;
            NewUser.AccountStatus = Current.AccountStatus;
            if (Current != null)
            {
                if (NewUser.Tags.ToLower().Contains("#" + tag.ToLower()))
                    NewUser.Tags =  NewUser.Tags.Replace("#" + tag, "");
                else
                    NewUser.Tags += "#" + tag;
                bool success;
                NewUser.Update(out success);
            }
        }
        [HttpPost]
        public void FacebookLogin(string _code)
        {
            SundooqDBEntities2 db = new SundooqDBEntities2();
            Users user = db.Users.Where(u => u.Email == _code).SingleOrDefault();
            HttpCookie myCookie = new HttpCookie("Sundooq");
            DateTime now = DateTime.Now;
            // Set the cookie value.
            myCookie.Value = "facebook" + "$" + _code;
            // Set the cookie expiration date.
            myCookie.Expires = now.AddYears(50); // For a cookie to effectively never expire
            // Add the cookie.
            Response.Cookies.Add(myCookie);
            if (user == null)
            {
                user = new Users();
                user.Email = _code;
                user.Tags = "";
                user.DateOfBirth = DateTime.Now.AddYears(-16);
                user.AccountStatus = 1;
                db.Users.Add(user);
                db.SaveChanges();
                Session.Add("Tags", user.Tags);
            }
            else
            {
                if (user.DateOfBirth == null)
                {
                    user.DateOfBirth = DateTime.Now.AddYears(-16);
                    user.Tags = "";
                }
                Session["User"] = user;
            }
            Session["Tags"] = user.Tags;
        }
        public ActionResult sendactivation(string id)
        {
            SundooqDBEntities2 db = new SundooqDBEntities2();
            int _id = 0;
            if (int.TryParse(id, out _id))
            {
                Users current = db.Users.Where(u => u.Id == _id).FirstOrDefault();
                if (current != null)
                {
                    string Msg = "Welcome " + current.Email;
                    Msg += " <br/> We are very exicted to have you in Sundooq <br/> ";
                    Msg += "Please click the link below to activate your account and start with Sundooq <br/> ";
                    Msg += "<a href='" + Helpers.baseUrl + "/User/Activate/" + current.Tags + "'>Activate My Account</a>";
                    Helpers.sendEmail(current.Email, "Welcome to Sundooq, Activate your account now", Msg);
                }
            }
            return RedirectToAction("index", "Home");
        }
        public JsonResult passchange(string _password)
        {
            Users CurrentUser = (Users)Session["User"];
            JSONReply result = new JSONReply();
            if (CurrentUser == null)
            {

                result.Msg = "Update failed !";
                result.result = false;
                return Json(result);
            }
            CurrentUser.Password = _password;
            bool success;
            result = new JSONReply();
            result.Msg = CurrentUser.Update(out success);
            result.result = success;
            Session["User"] = CurrentUser;
            return Json(result);

        }
        [HttpPost]
        public JsonResult sendreset(string _email)
        {
            JSONReply result = new JSONReply();
            bool success;
            Users user = new Users();
            user.Email = _email;
            result.Msg = user.sendreset(out success);
            result.result = success;
            return Json(result);
        }
        public ActionResult sendreset()
        {
            return View();
        }
        public ActionResult reset(string id)
        {
            SundooqDBEntities2 db = new SundooqDBEntities2();
            Users user = db.Users.Where(u => u.Password == id).FirstOrDefault();
            if (user != null && user.Id > 0)
            {
                Session["User"] = user;
                ViewBag.User = user;
                return View();
            }
            else
                return RedirectToAction("index", "home");
        }

    }
}
