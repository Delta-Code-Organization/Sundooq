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
            int tagValue = 0;
            if (ticks * 5 > topics.Count)
            {
                topics.AddRange(new Topics().GetUserTopics());
                Session["topics"] = topics;
            }
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
            int tagValue;
            List<Topics> lst = new List<Topics>();
            lst = new Topics().GetUserTopics().ToList();
            Session["topics"] = lst;
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
            if (user != null && !user.History.Any(t => t.TopicId == Topic.Id))
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
            SundooqDBEntities2 db = new SundooqDBEntities2();
            List<Topics> LOT = new List<Topics>();
            LOT = db.Topics.Where(t => t.Tags.ToLower().Contains(keyword.ToLower())).ToList();
            ViewBag.topics = LOT.Skip(0).Take(9);
            TempData["FilteredTopics"] = LOT;
            TempData["SkipNo"] = 0;
            TempData["LoadedToUser"] = LOT.Skip(0).Take(9).ToList();
            TempData.Keep();
            ViewBag.sourcename = System.Web.HttpUtility.UrlDecode(id);
            return View();
        }
        public JsonResult GetNext(string _url)
        {
            List<Topics> topics = (List<Topics>)Session["topics"];
            if (topics == null)
                return null;
            int indx = topics.FindIndex(p => p.URL == _url);
            if (topics.Count > indx + 2)
                return Json(topics[indx + 1].Id);
            else
                return null;
        }
        public JsonResult GetPrev(string _url)
        {
            List<Topics> topics = (List<Topics>)Session["topics"];
            if (topics == null)
                return null;
            int indx = topics.FindIndex(p => p.URL == _url);
            if (indx >1)
                return Json(topics[indx - 1].Id);
            else
                return null;
        }
        [HttpPost]
        public JsonResult GetMoreFilteredTopics()
        {
            int Skip = (int)TempData["SkipNo"] + 9;
            List<Topics> LOT = TempData["FilteredTopics"] as List<Topics>;
            List<Topics> LOTToSend = LOT.Skip(Skip).Take(9).ToList();
            TempData["SkipNo"] = Skip;
            TempData["FilteredTopics"] = LOT;
            List<Topics> Loaded = TempData["LoadedToUser"] as List<Topics>;
            Loaded.AddRange(LOTToSend);
            TempData["LoadedToUser"] = Loaded;
            var LOTInJson = (from T in LOTToSend
                             select new
                             {
                                 T.Id,
                                 T.Img,
                                 T.Title,
                                 T.Descr
                             }).ToList();
            TempData.Keep();
            return Json(LOTInJson);
        }

        [HttpPost]
        public JsonResult FilterFilteredTopics(string Keyword)
        {
            if (Keyword == "sk44@ass-449*as-6490as-x?zc.86!6Q")
            {
                List<Topics> Loaded = TempData["LoadedToUser"] as List<Topics>;
                List<Topics> LOT = TempData["FilteredTopics"] as List<Topics>;
                var LOTInJson = (from T in Loaded
                                 select new
                                 {
                                     T.Id,
                                     T.Img,
                                     T.Title,
                                     T.Descr
                                 }).ToList();
                TempData["FilteredTopics"] = LOT;
                TempData.Keep();
                return Json(LOTInJson);
            }
            else
            {
                List<Topics> LOT = TempData["FilteredTopics"] as List<Topics>;
                List<Topics> LOTFiltered = LOT.Where(p => p.Title.ToLower().Contains(Keyword.ToLower()) || p.Descr.ToLower().Contains(Keyword.ToLower())).ToList();
                var LOTInJson = (from T in LOTFiltered
                                 select new
                                 {
                                     T.Id,
                                     T.Img,
                                     T.Title,
                                     T.Descr
                                 }).ToList();
                TempData["FilteredTopics"] = LOT;
                TempData.Keep();
                return Json(LOTInJson);
            }
        }
    }
}
