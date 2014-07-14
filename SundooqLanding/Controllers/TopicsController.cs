using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SundooqLanding.Models;

namespace SundooqLanding.Controllers
{
    public class TopicsController : Controller
    {
        //
        // GET: /Topics/

        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult load(string current)
        {
            List<Topics> topics = (List<Topics>)Session["topics"];
            int ticks = int.Parse(current);
            #region serialize
            var TopicsInJSON = (from TT in topics.Skip(5 * ticks)
                                select new
                                {
                                    TT.ReadyDescription,
                                    TT.EncodedTitle,
                                    TT.FB,
                                    TT.Id,
                                    TT.Img,
                                    TT.LocalShares,
                                    TT.LocalViews,
                                    TT.formatedDate,
                                    TT.Rank,
                                    TT.Tags,
                                    TT.Title,
                                    TT.TW,
                                    TT.URL,
                                    Source = new
                                    {
                                        TT.Sources.Id,
                                        TT.Sources.LogoURL,
                                        TT.Sources.Rank,
                                        TT.Sources.SourceName,
                                        TT.Sources.Tags,
                                        TT.Sources.URL
                                    }
                                }).ToList();
            #endregion
            return Json(TopicsInJSON);
        }
        public void reload()
        {
            Session["topics"] = new Topics().GetUserTopics().ToList();
        }
        public ActionResult View(string id)
        {
            SundooqDBEntities2 db = new SundooqDBEntities2();
            int ID = 0;
            bool parse = int.TryParse(id, out ID);
            if (!parse)
                return RedirectToAction("Home", "User");
            Topics Topic = db.Topics.Where(t => t.Id == ID).SingleOrDefault();
            Users user = (Users)Session["User"];
            if (Topic == null)
                return RedirectToAction("Home", "User");
            Topic.History = null;
            ViewBag.Topic = Topic;
            Topic.LocalViews += 1;
            if (Session["Sorting"] != null)
                @ViewBag.Sorting = Session["Sorting"].ToString();
            else
                @ViewBag.Sorting = 0;
            if (user != null &&  !user.History.Any(t => t.TopicId == Topic.Id))
            {
                History h = new History();
                h.TopicId = Topic.Id;
                h.UserId = user.Id;
                user.History.Add(h);
                db.History.Add(h);
                db.SaveChanges();
            }
            return View();
        }
        public ActionResult filter(string id)
        {
            string keyword = System.Web.HttpUtility.UrlDecode(id);
            SundooqDBEntities2 db = new SundooqDBEntities2 ();
            ViewBag.topics = db.Topics.Where(t => t.Tags.ToLower().Contains(keyword.ToLower()));
            ViewBag.sourcename = System.Web.HttpUtility.UrlDecode(id);
            return View();
        }
    }
}
