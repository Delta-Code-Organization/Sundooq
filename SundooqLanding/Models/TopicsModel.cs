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
        public string ReadyDescription
        {
            get
            {
                if (Descr.Length > 300)
                    return Descr.Substring(0, 300) + "...";
                else
                    return Descr;
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
            if (Current == null)
                return null;
            DateTime limit = DateTime.Now.AddDays(-7);
            List<Topics> Topics = new List<Models.Topics>();
            var dbTopics = db.Topics.Where(p => p.PubDate >= limit).ToList().Where(p => p.CustomRank > 0 && !Current.History.Any(t => t.TopicId == p.Id));
            return dbTopics;
        }
    }
}