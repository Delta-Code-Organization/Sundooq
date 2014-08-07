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

        public int CustomRank(int _val)
        {
            Users Current = (Users)HttpContext.Current.Session["User"];
            if (Current != null)
            {
                int mat = MatchTags(Tags, Current.Tags);
                if (mat == 0)
                    return 0;
                int TotalHours = (int)((TimeSpan)(DateTime.Now - PubDate)).TotalHours + 1;
                if (TotalHours < 8)
                {
                    int bost = 8 - TotalHours;
                    return (int)((mat * _val) + (Rank / TotalHours) - (_val * bost));
                }
                else
                {
                    int days = (int)((TimeSpan)(DateTime.Now - PubDate)).TotalDays;
                    return (int)((mat * _val) + (Rank / TotalHours) - (_val * TotalHours));
                }
            }
            else
                return (int)Rank;
        }

        public int MatchTags(string Tags1, string Tags2)
        {
            int Matches = 0;
            foreach (string Tag1 in Tags1.Split('#'))
            {
                if (Tag1.Trim().Length > 0 && (Tags2.ToLower().Contains("#" + Tag1.ToLower().Trim())))
                    Matches++;
            }
            return Matches;
        }

        public IEnumerable<Topics> GetUserTopics(int days = -1)
        {
            Users Current = HttpContext.Current.Session["User"] as Users;
            if (Current == null)
                return null;
            DateTime limit = DateTime.Now.AddDays(days);
            List<Topics> Topics = new List<Models.Topics>();
            var dbtopics = db.Topics.Where(p => p.PubDate >= limit);
            int tcount = dbtopics.Count();
            var tsum = dbtopics.Sum(t => t.Rank);
            int val = 0;
            if (tsum == null || tcount == 0)
            {
                tsum = 0;
            }
            else
            val = (int)tsum / tcount;
            if (tcount > 2500)
            {
                return dbtopics.ToList().Where(p => p.CustomRank(val) != 0 && !Current.History.Any(t => t.TopicId == p.Id)).Distinct(new DistinctItemComparer()).OrderByDescending(p => p.CustomRank(val));
            }
            else
            {
                limit = DateTime.Now.AddDays(days - 1);
                dbtopics = db.Topics.Where(p => p.PubDate >= limit);
                return dbtopics.ToList().Where(p => p.CustomRank(val) != 0 && !Current.History.Any(t => t.TopicId == p.Id)).Distinct(new DistinctItemComparer()).OrderByDescending(p => p.CustomRank(val));
            }
        }
    }
    class DistinctItemComparer : IEqualityComparer<Topics>
    {

        public bool Equals(Topics x, Topics y)
        {
            return x.URL == y.URL ||
                x.Title == y.Title;
        }

        public int GetHashCode(Topics obj)
        {
            return obj.Title.GetHashCode();
        }
    }
}