using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SundoqEmailSender
{
    public static class Operations
    {
        #region Context
        private static SundooqDBEntities db = new SundooqDBEntities();
        #endregion
        
        public static void Do()
        {
            List<Users> LOU = db.Users.Where(p => p.AccountStatus != 5).ToList();
            foreach (Users item in LOU)
            {
                if (item.AccountStatus != 2)
                {
                    if (item.Registered.Value.AddDays(3) < DateTime.Now)
                    {
                        var LastMail = (from m in db.Emails
                                        where m.Type == (int)MailTypes.Activate && m.User == item.Id
                                        orderby m.Date descending
                                        select m).ToList().FirstOrDefault();
                        if (LastMail == null || ((DateTime)LastMail.Date).AddDays(3) < DateTime.Now)
                        {
                            string Msg;
                            Msg = "Hello " + item.Email;
                            Msg += "<br/>Thank you for registering with SUNDOQ. You did the right thing!";
                            Msg += "<br/>Please verify your e-mail address by clicking on the following link: <a href='" + "http://sundoq.com/" + "User/Activate/" + item.Tags + "'>Activate My Account</a>";
                            Msg += "<br/><strong>What’s Next ?</strong>";
                            Msg += "<br/>Once you activate your account, you will be able to follow your favorite sources and/or tags. Please select at least 3 sources to build your feeds.";
                            Msg += "<br/><br/><strong>How to follow/unfollow new sources or tags?</strong>";
                            Msg += "<br/>- On your feeds page, by clicking the small tag icon over topic’s image, you will view topic’s tags and you can follow/unfollow any of them.";
                            Msg += "<br/>- On the article’s page, by clicking the small tag icon on the top bar, you will view topic’s tags and you can follow/unfollow any of them.";
                            Msg += "<br/>- You can also visit your Account page to follow/unfollow any sources or tags";
                            Msg += "<br/>Because we use SUNDOQ, we have clearer inboxes. Forget about the “no reply” dull rule and feel free to reply to any of our emails. We will get back to you very soon.";
                            Helper.sendEmail(item.Email, "Activate your SUNDOQ account ", Msg, MailTypes.Register, item.Id);
                        }
                    }
                }
                else
                {
                    if (item.LastLogin == null || item.LastLogin.Value.AddDays(3) < DateTime.Now)
                    {
                        var LastMail = (from m in db.Emails
                                        where m.Type == (int)MailTypes.HotTopic && m.User == item.Id
                                        orderby m.Date descending
                                        select m).ToList().FirstOrDefault();
                        if (LastMail == null || LastMail.Date.Value.AddDays(3) < DateTime.Now)
                        {
                            List<Topics> LOT = new Topics().GetUserTopics(item).OrderByDescending(p => p.Rank).Take(5).ToList();
                            string Msg;
                            Msg = "Hello " + item.Email;
                            foreach (Topics T in LOT)
                            {
                                Msg += "<br/><img src='" + T.Img + "' /><br/>";
                                Msg += "<br/><h1>" + T.Title + "</h1><br/>";
                                Msg += "<br/><hr/><br/>";
                            }
                            Helper.sendEmail(item.Email, "Sundoq hot topics", Msg, MailTypes.HotTopic, item.Id);
                        }
                    }
                    DateTime firstDate = DateTime.Now.AddDays(-1);
                    DateTime secondDate = DateTime.Now.AddDays(-3);
                    if (item.LastLogin == null || (item.LastLogin < firstDate && item.LastLogin > secondDate))
                    {
                        List<Topics> LOT = new Topics().GetUserTopics(item).OrderByDescending(p => p.PubDate).ToList();
                        int Counter = 0;
                        string Msg;
                        Msg = "Hello " + item.Email;
                        foreach (Topics T in LOT)
                        {
                            if (T.PubDate > DateTime.Now.AddDays(-1))
                            {
                                Msg += "<br/><img src='" + T.Img + "' /><br/>";
                                Msg += "<br/><h1>" + T.Title + "</h1><br/>";
                                Msg += "<br/><hr/><br/>";
                                Counter++;
                                if (Counter == 5)
                                {
                                    break;
                                }
                            }
                        }
                        Helper.sendEmail(item.Email, "Sundoq morning coffee", Msg, MailTypes.MorningCoffee, item.Id);
                    }
                }
            }
        }
    }
}
