﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SundooqLanding.Models;
using System.Web.Configuration;
using Google.GData.Apps;
using Google.Contacts;
using Google.GData.Client;
using Google.GData.Contacts;
using Google.GData.Extensions;
using System.Net;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Web.Services;
using System.Configuration;
using System.Web.Script.Serialization;

using Spring.Social.OAuth1;
using Spring.Social.Twitter.Api;
using Spring.Social.Twitter.Connect;
using HtmlAgilityPack;

namespace SundooqLanding.Controllers
{
    public class UserController : Controller
    {
        #region Twitter Consumer Key and Secret
        private const string TwitterConsumerKey = "Ov5yiAvFGEXrIPpnuJFLB3X5v";
        private const string TwitterConsumerSecret = "2Jl0eQSfsRJP5uAQfV541NaA9xkN7H9SWyJWLeLHG1GC92qsG4";

        IOAuth1ServiceProvider<ITwitter> twitterProvider =
            new TwitterServiceProvider(TwitterConsumerKey, TwitterConsumerSecret);
        #endregion

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
            string src = "";
            foreach (Sources s in db.Sources.OrderBy(p => p.SourceName))
            {
                src += "#" + s.SourceName;
            }
            ViewBag.Sources = src;
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
        public string GetSuggested()
        {
            string tags = new Users().getSuggestedTags();
            return tags;
        }
        public ActionResult Home(string id = "1")
        {
            Session["Sorting"] = id;
            int tagValue = 0;
            Users currentUser = (Users)Session["User"];
            if (currentUser != null && Session["Tags"] == null)
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
                IEnumerable<Topics> topics = null;
                if (Session["Topics"] == null || Session["Tags"].ToString() != currentUser.Tags)
                {
                    topics = new Topics().GetUserTopics();
                    Session["Tags"] = currentUser.Tags;
                }
                else
                {
                    topics = (List<Topics>)Session["topics"];
                }
                ViewBag.Sorting = 1;
                List<Topics> lst = topics.ToList();
                int count = lst.Count - 1;
                for (int i = 2; i <= lst.Count - 2; i++)
                {
                    if (lst[i].Source == lst[i - 1].Source || lst[i].Source == lst[i + 1].Source)
                    {
                        Topics temp = lst[count];
                        lst[count] = lst[i];
                        lst[i] = temp;
                        count--;
                    }
                }
                ViewBag.Topics = lst;
                Session["topics"] = ViewBag.Topics;
            }
            else
            {
                IEnumerable<Topics> topics = null;
                if (Session["Topics"] == null || Session["Tags"].ToString() != currentUser.Tags)
                {
                    topics = new Topics().GetUserTopics();
                    Session["Tags"] = currentUser.Tags;
                }
                else
                {
                    topics = (List<Topics>)Session["topics"];
                }
                ViewBag.Sorting = 0;
                ViewBag.Topics = topics.OrderByDescending(p => p.PubDate).ToList();

            }
            Session["topics"] = ViewBag.Topics;
            ViewBag.currenttags = currentUser.Tags;
            return View();
        }
        public void reload()
        {
            string id = Session["Sorting"].ToString();
            int tagValue;
            Users currentUser = (Users)Session["User"];
            if (currentUser != null && Session["Tags"] == null)
                Session["Tags"] = currentUser.Tags;
            if (id == "1")
            {
                List<Topics> topics = new List<Topics>();
                topics = new Topics().GetUserTopics().ToList();
                Session["Tags"] = currentUser.Tags;
                ViewBag.Sorting = 1;
                List<Topics> lst = topics.ToList();
                int count = lst.Count - 1;
                for (int i = 1; i <= lst.Count - 2; i++)
                {
                    if (lst[i].Source == lst[i - 1].Source || lst[i].Source == lst[i + 1].Source)
                    {
                        Topics temp = lst[count];
                        lst[count] = lst[i];
                        lst[i] = temp;
                        count--;
                    }
                }
                ViewBag.Topics = lst;
                Session["topics"] = ViewBag.Topics;
            }
            else
            {
                List<Topics> topics = new List<Topics>();
                topics = new Topics().GetUserTopics().ToList();
                Session["Tags"] = currentUser.Tags;
                ViewBag.Sorting = 0;
                ViewBag.Topics = topics.OrderByDescending(p => p.PubDate).ToList();
                Session["topics"] = ViewBag.Topics;
            }
            ViewBag.currenttags = currentUser.Tags;
        }
        public ActionResult History()
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Users U = Session["User"] as Users;
            List<History> LOH = U.History.OrderByDescending(t => t.TopicId).ToList();
            TempData["AllHistory"] = LOH;
            TempData["SkipNo"] = 0;
            ViewBag.H = LOH.Skip(0).Take(9).ToList();
            TempData["LoadedHistory"] = LOH.Skip(0).Take(9).ToList();
            TempData.Keep();
            return View();
        }

        [HttpPost]
        public JsonResult LoadMoreHistory()
        {
            List<History> LOH = TempData["AllHistory"] as List<History>;
            int Skip = (int)TempData["SkipNo"] + 9;
            List<History> LOHToSend = LOH.Skip(Skip).Take(9).ToList();
            List<History> Loaded = TempData["LoadedHistory"] as List<History>;
            Loaded.AddRange(LOHToSend);
            TempData["LoadedHistory"] = Loaded;
            var LOHInJSON = (from H in LOHToSend
                             select new
                             {
                                 H.TopicId,
                                 Topics = new
                                 {
                                     H.Topics.Img,
                                     H.Topics.Title
                                 }
                             }).ToList();
            TempData["AllHistory"] = LOH;
            TempData["SkipNo"] = Skip;
            TempData.Keep();
            return Json(LOHInJSON);
        }

        [HttpPost]
        public JsonResult FilterHistory(string Keyword)
        {
            if (Keyword == "sk44@ass-449*as-64900as-x?zc.86!6Q")
            {
                List<History> Loaded = TempData["LoadedHistory"] as List<History>;
                List<History> LOT = TempData["AllHistory"] as List<History>;
                var LOTInJson = (from H in Loaded
                                 select new
                                 {
                                     H.TopicId,
                                     Topics = new
                                     {
                                         H.Topics.Img,
                                         H.Topics.Title
                                     }
                                 }).ToList();
                TempData["AllHistory"] = LOT;
                TempData.Keep();
                return Json(LOTInJson);
            }
            else
            {
                List<History> LOT = TempData["AllHistory"] as List<History>;
                List<History> LOTFiltered = LOT.Where(p => p.Topics.Title.ToLower().Contains(Keyword.ToLower()) || p.Topics.Descr.ToLower().Contains(Keyword.ToLower())).ToList();
                var LOTInJson = (from H in LOTFiltered
                                 select new
                                 {
                                     H.TopicId,
                                     Topics = new
                                     {
                                         H.Topics.Img,
                                         H.Topics.Title
                                     }
                                 }).ToList();
                TempData["AllHistory"] = LOT;
                TempData.Keep();
                return Json(LOTInJson);
            }
        }

        [HttpPost]
        public JsonResult Update(string _mail, string _password, string _gender, string _dob, string _tags = "", string _Fullname = "")
        {
            Users CurrentUser = (Users)Session["User"];
            JSONReply result = new JSONReply();
            if (CurrentUser == null)
            {

                result.Msg = "Oops! We are sorry, something went wrong. It happens!. <br>Why don't you go back and try again.";
                result.result = false;
                return Json(result);
            }
            CurrentUser.Email = _mail;
            CurrentUser.Password = _password;
            CurrentUser.DateOfBirth = DateTime.Parse(_dob);
            CurrentUser.Gender = int.Parse(_gender);
            CurrentUser.Fullname = _Fullname;
            if (_tags.Length > 1)
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
                string[] Tech = WebConfigurationManager.AppSettings["Technology"].ToString().Split('#').OrderBy(t => t.ToString()).ToArray();
                string[] Business = WebConfigurationManager.AppSettings["Business"].ToString().Split('#').OrderBy(t => t.ToString()).ToArray();
                string[] Health = WebConfigurationManager.AppSettings["Health"].ToString().Split('#').OrderBy(t => t.ToString()).ToArray();
                string[] News = WebConfigurationManager.AppSettings["News"].ToString().Split('#').OrderBy(t => t.ToString()).ToArray();
                string[] Women = WebConfigurationManager.AppSettings["Women"].ToString().Split('#').OrderBy(t => t.ToString()).ToArray();
                string[] Sports = WebConfigurationManager.AppSettings["Sports"].ToString().Split('#').OrderBy(t => t.ToString()).ToArray();
                string[] Sources = WebConfigurationManager.AppSettings["Sources"].ToString().Split('#').OrderBy(t => t.ToString()).ToArray();
                string[] Fun = WebConfigurationManager.AppSettings["Fun"].ToString().Split('#').OrderBy(t => t.ToString()).ToArray();
                ViewBag.Tech = Tech;
                ViewBag.Business = Business;
                ViewBag.Health = Health;
                ViewBag.News = News;
                ViewBag.Women = Women;
                ViewBag.Sports = Sports;
                ViewBag.Sources = Sources;
                ViewBag.Fun = Fun;
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
            if (Current == null)
                return;
            Users NewUser = new Users();
            NewUser.Id = Current.Id;
            NewUser.Email = Current.Email;
            NewUser.Password = Current.Password;
            NewUser.Tags = Current.Tags;
            NewUser.Gender = Current.Gender;
            NewUser.RegisteredWith = Current.RegisteredWith;
            NewUser.DateOfBirth = Current.DateOfBirth;
            NewUser.AccountStatus = Current.AccountStatus;
            if (Current != null)
            {
                if (NewUser.Tags.ToLower().Contains("#" + tag.ToLower()))
                    NewUser.Tags = NewUser.Tags.Replace("#" + tag, "");
                else
                    NewUser.Tags += "#" + tag;
                bool success;
                NewUser.Update(out success);
            }
        }
        [HttpPost]
        public void ignore(string ignored)
        {
            Users Current = (Users)Session["User"];
            if (Current != null)
            {
                Users NewUser = new Users();
                NewUser.Id = Current.Id;
                NewUser.Email = Current.Email;
                NewUser.Password = Current.Password;
                NewUser.Tags = Current.Tags;
                NewUser.RegisteredWith = Current.RegisteredWith;
                NewUser.DateOfBirth = Current.DateOfBirth;
                NewUser.AccountStatus = Current.AccountStatus;
                NewUser.IgnoredTags += ignored;
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
                user.LastLogin = DateTime.Now;
                user.Registered = DateTime.Now;
                user.DateOfBirth = DateTime.Now.AddYears(-16);
                user.AccountStatus = 1;
                user.RegisteredWith = (int)RegistredWith.facebook;
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

        public ActionResult AuthorizeCallback(string oauth_verifier)
        {
            OAuthToken requestToken = Session["RequestToken"] as OAuthToken;
            AuthorizedRequestToken authorizedRequestToken = new AuthorizedRequestToken(requestToken, oauth_verifier);
            OAuthToken token = twitterProvider.OAuthOperations.ExchangeForAccessTokenAsync(authorizedRequestToken, null).Result;

            Session["AccessToken"] = token;

            ITwitter twitterClient = twitterProvider.GetApi(token.Value, token.Secret);
            TwitterProfile profile = twitterClient.UserOperations.GetUserProfileAsync().Result;
            SundooqDBEntities2 db = new SundooqDBEntities2();
            Users U = db.Users.Where(p => p.Email == profile.ScreenName).SingleOrDefault();
            HttpCookie myCookie = new HttpCookie("Sundooq");
            DateTime now = DateTime.Now;
            // Set the cookie value.
            myCookie.Value = "twitter" + "$" + profile.ScreenName;
            // Set the cookie expiration date.
            myCookie.Expires = now.AddYears(50); // For a cookie to effectively never expire
            // Add the cookie.
            Response.Cookies.Add(myCookie);
            if (U == null)
            {
                U = new Users();
                U.Email = profile.ScreenName;
                U.Tags = "";
                U.DateOfBirth = DateTime.Now.AddYears(-16);
                U.LastLogin = DateTime.Now;
                U.Registered = DateTime.Now;
                U.AccountStatus = 1;
                U.RegisteredWith = (int)RegistredWith.facebook;
                db.Users.Add(U);
                db.SaveChanges();
                Session.Add("Tags", U.Tags);
            }
            else
            {
                if (U.DateOfBirth == null)
                {
                    U.DateOfBirth = DateTime.Now.AddYears(-16);
                    U.Tags = "";
                }
                Session["User"] = U;
            }
            Session["Tags"] = U.Tags;
            return RedirectToAction("Home", "User");
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
                    Helpers.sendEmail(current.Email, "Welcome to Sundooq, Activate your account now", Msg, MailTypes.Activate, current.Id);
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

        public ActionResult InviteGmailFriends(string code)
        {
            ViewBag.Mails = GetGmailContacts(code);
            return View();
        }

        public ActionResult InviteFriends()
        {
            return View();
        }

        [HttpPost]
        public void InviteGmailFriendsNow(string Mails)
        {
            string[] SperatedMails = Mails.Split('#');
            foreach (string item in SperatedMails)
            {
                if (item.Trim() != "")
                {
                    Helpers.sendEmail(item, "Sundoq Invitation", "<h1>" + (Session["User"] as Users).Email + " invited you to join sundoq</h1>", MailTypes.Invitation, null);
                }
            }
        }

        public List<string> GetGmailContacts(string code)
        {
            List<string> emails = new List<string>();
            try
            {
                string postcontents = string.Format("code={0}&client_id={1}&client_secret={2}&redirect_uri={3}&grant_type=authorization_code"
                                   , System.Web.HttpUtility.UrlEncode(code)
                                   , System.Web.HttpUtility.UrlEncode(ConfigurationManager.AppSettings["gmailclientid"])
                                   , System.Web.HttpUtility.UrlEncode(ConfigurationManager.AppSettings["gmailsecret"])
                                   , System.Web.HttpUtility.UrlEncode(ConfigurationManager.AppSettings["gmailreturnurl"]));
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("https://accounts.google.com/o/oauth2/token");
                request.Method = "POST";
                byte[] postcontentsArray = System.Text.Encoding.UTF8.GetBytes(postcontents);
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = postcontentsArray.Length;
                GoogleOAuthToken token;
                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(postcontentsArray, 0, postcontentsArray.Length);
                    requestStream.Close();
                    WebResponse response = request.GetResponse();
                    using (Stream responseStream = response.GetResponseStream())
                    using (StreamReader reader = new StreamReader(responseStream))
                    {
                        string responseFromServer = reader.ReadToEnd();
                        reader.Close();
                        responseStream.Close();
                        response.Close();
                        // return SerializeToken(responseFromServer);
                        JavaScriptSerializer ser = new JavaScriptSerializer();
                        token = ser.Deserialize<GoogleOAuthToken>(responseFromServer);
                    }
                }
                RequestSettings contactrequest = new RequestSettings("Sundoq", token.access_token);
                contactrequest.AutoPaging = true;
                ContactsRequest req = new ContactsRequest(contactrequest);
                Feed<Contact> FCs = req.GetContacts();
                foreach (Contact contact in FCs.Entries)
                {
                    foreach (EMail email in contact.Emails)
                    {
                        emails.Add(email.Address);
                    }
                }

                return emails;
            }
            catch (Exception ex)
            {
                return emails;
            }
        }

        public static bool RSS(string URL)
        {
            var doc = XDocument.Load(URL);
            var items = doc.Root.Elements().Where(x => x.Name.ToString()
                .ToLower().Contains("channel"))
                .Elements().Where(x => x.Name.ToString().ToLower().Contains("item"));
            if (items.Count() < 1)
            {
                //RSS2(_s);
                return false;
            }
            return true;
        }

        public string AddCustomSource(string URL)
        {
            try
            {
                if (!URL.ToLower().Contains("http://") && !URL.ToLower().Contains("https://"))
                {
                    URL = "http://" + URL;
                }
                string URLSourceCode = "";
                Uri myUri;
                bool RssFound = false;
                string RssURL = "";
                if (Uri.TryCreate(URL, UriKind.RelativeOrAbsolute, out myUri))
                {
                    using (WebClient WC = new WebClient())
                    {
                        URLSourceCode = WC.DownloadString(myUri);
                    }
                    HtmlDocument Doc = new HtmlDocument();
                    Doc.LoadHtml(URLSourceCode);
                    List<HtmlNode> LOE = Doc.DocumentNode.Descendants().Where(p => p.Name == "link").ToList();
                    foreach (HtmlNode Node in LOE)
                    {
                        HtmlAttribute Attr = Node.Attributes["type"];
                        if (Attr != null && Attr.Value == "application/rss+xml")
                        {
                            RssFound = true;
                            HtmlAttribute URLAttr = Node.Attributes["href"];
                            RssURL = URLAttr.Value;
                            break;
                        }
                    }
                    if (RssFound == false)
                    {
                        return "This source cannot be added at current time ...";
                    }
                    else
                    {
                        if (!RSS(RssURL))
                        {
                            return "This source cannot be added at current time ...";
                        }
                        else
                        {
                            using (SundooqDBEntities2 db = new SundooqDBEntities2())
                            {
                                string SourceName = GetDomain.GetDomainFromUrl(RssURL);
                                SourceName = SourceName.Substring(0, SourceName.LastIndexOf('.'));
                                if (!db.Sources.Any(p => p.URL == RssURL))
                                {
                                    Sources newSource = new Sources();
                                    newSource.Description = "";
                                    newSource.LogoURL = RssURL;
                                    newSource.Rank = 1;
                                    newSource.SourceName = SourceName;
                                    newSource.Status = 1;
                                    newSource.Tags = newSource.SourceName;
                                    newSource.URL = RssURL;
                                    db.Sources.Add(newSource);
                                    db.SaveChanges();
                                }
                                Users CurrentUser = Session["User"] as Users;
                                if (!CurrentUser.Tags.ToLower().Contains(SourceName))
                                {
                                    CurrentUser.Tags += "#" + SourceName;
                                    bool successMsg;
                                    CurrentUser.Update(out successMsg);
                                    return "Source added successfully to your following sources ...";
                                }
                                else
                                {
                                    return "You are already following this source ...";
                                }
                            }
                        }
                    }
                }
                else
                {
                    return "This source cannot be added at current time ...";
                }
            }
            catch (Exception)
            {
                return "This source cannot be added at current time ...";
            }
        }

        public class GoogleOAuthToken
        {
            public string access_token;
            public string expires_in;
            public string token_type;
            public string refresh_token;
        }
    }
}
