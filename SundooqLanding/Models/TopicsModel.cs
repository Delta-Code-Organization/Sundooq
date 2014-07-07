using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SundooqLanding.Models
{
    public partial class Topics
    {
        #region Context
        SundooqDBEntities2 db = new SundooqDBEntities2();
        #endregion
        public string formatedDate
        {
            get
            {
                return ((DateTime)PubDate).ToShortDateString();
            }
        }
        public int CustomRank
        {
            get
            {
                Users Current = (Users)HttpContext.Current.Session["User"];
                if (Current != null)
                {
                    int mat = MatchTags(Tags, Current.Tags); 
                    return (int)(mat * Rank);
                }
                else
                    return (int)Rank;
            }
        }
        public List<Topics> GetTopics()
        {
            var Topics = db.Topics.Where(t => t.PubDate <= DateTime.Now.AddDays(-7));
            return Topics.ToList();
        }
        public Topics GetSingleTopic(string Title)
        {
            var Topic = (from T in db.Topics
                         where T.Title == Title
                         select T).ToList().FirstOrDefault();
            Topic.LocalViews += 1;
            db.SaveChanges();
            Users Current = HttpContext.Current.Session["User"] as Users;
            if (Current != null && !Current.History.Any(p => p.TopicId == Topic.Id))
            {
                History NewTopic = new History();
                NewTopic.TopicId = Topic.Id;
                NewTopic.UserId = Current.Id;
                NewTopic.Impression = 1;
                db.History.Add(NewTopic);
                db.SaveChanges();
            }
            return Topic;
        }
        public int MatchTags(string Tags1, string Tags2)
        {
            int Matches = 0;
            foreach (string Tag1 in Tags1.Split('#'))
            {
                if (Tag1.Trim().Length > 0 && Tags2.ToLower().Contains("#" + Tag1.ToLower().Trim()))
                    Matches++;
            }
            return Matches;
        }
        public IEnumerable<Topics> GetUserTopics()
        {
            Users Current = HttpContext.Current.Session["User"] as Users;
            DateTime limit = DateTime.Now.AddDays(-7);
            List<Topics> Topics = new List<Models.Topics>();
            var dbTopics = db.Topics.Where(p => p.PubDate >= limit).ToList().Where(p => p.CustomRank > 0 && !Current.History.Any(t => t.TopicId == p.Id));
            return dbTopics;
        }
        public List<Topics> GetRelated(Topics Topic)
        {
            var Topics = (from T in db.Topics
                          orderby T.Id descending
                          select T).Where(p => p.Id != Topic.Id).Take(200).ToList();
            Dictionary<Topics, int> UserTopics = new Dictionary<Topics, int>();
            foreach (Topics t in Topics)
            {
                int mat = MatchTags(t.Tags, Topic.Tags);
                t.Rank += mat * 50;
                UserTopics.Add(t, mat);
            }
            List<KeyValuePair<Topics, int>> sorted = (from kv in UserTopics orderby kv.Value select kv).ToList();
            Topics.Clear();
            foreach (KeyValuePair<Topics, int> p in sorted)
            {
                Topics.Add(p.Key);
            }
            Topics.Reverse();
            return Topics.ToList();
        }
        public List<Topics> GetHistory()
        {
            Users Current = HttpContext.Current.Session["User"] as Users;
            Current = db.Users.Where(p => p.Id == Current.Id).FirstOrDefault();
            List<Topics> UserTopics = new List<Topics>();
            if (Current != null)
            {
                foreach (History h in Current.History)
                {
                    UserTopics.Add(h.Topics);
                }
            }
            return UserTopics;
        }
    }
}