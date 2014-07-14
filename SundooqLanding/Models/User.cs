using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SundooqLanding.Models;

namespace SundooqLanding.Models
{
    enum RegistredWith
    {
        facebook,
        twitter
    }
    public partial class Users
    {
        SundooqDBEntities2 db = new SundooqDBEntities2();
        public string baseUrl = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority +
                                HttpContext.Current.Request.ApplicationPath.TrimEnd('/') + "/";
        public string regiter(out bool _success)
        {
            Users Registered = db.Users.Where(p => p.Email == this.Email).FirstOrDefault();
            if (Registered != null && Registered.AccountStatus >= 0)
            {
                _success = false;
                return "This email is already registered with us";
            }
            else
            {
                string Msg = "";
                if (Registered != null)
                {
                    Registered.Tags = Guid.NewGuid().ToString();
                    Msg = "Hello " + Registered.Email;
                    Msg += "<br/>Thank you for registering with SUNDOQ. You did the right thing!";
                    Msg += "<br/>Please verify your e-mail address by clicking on the following link: <a href='" + baseUrl + "User/Activate/" + Registered.Tags + "'>Activate My Account</a>";
                    Msg += "<br/><strong>What’s Next ?</strong>";
                    Msg += "<br/>Once you activate your account, you will be able to follow your favorite sources and/or tags. Please select at least 3 sources to build your feeds.";
                    Msg += "<br/><br/><strong>How to follow/unfollow new sources or tags?</strong>";
                    Msg += "<br/>- On your feeds page, by clicking the small tag icon over topic’s image, you will view topic’s tags and you can follow/unfollow any of them.";
                    Msg += "<br/>- On the article’s page, by clicking the small tag icon on the top bar, you will view topic’s tags and you can follow/unfollow any of them.";
                    Msg += "<br/>- You can also visit your Account page to follow/unfollow any sources or tags";
                    Msg += "<br/>Because we use SUNDOQ, we have clearer inboxes. Forget about the “no reply” dull rule and feel free to reply to any of our emails. We will get back to you very soon.";
                    Registered.AccountStatus = 0;
                    db.SaveChanges();
                }
                else
                {
                    this.Tags = Guid.NewGuid().ToString();
                    Msg = "Hello " + Registered.Email;
                    Msg += "<br/>Thank you for registering with SUNDOQ. You did the right thing!";
                    Msg += "<br/>Please verify your e-mail address by clicking on the following link: <a href='" + baseUrl + "User/Activate/" + Registered.Tags + "'>Activate My Account</a>";
                    Msg += "<br/><strong>What’s Next ?</strong>";
                    Msg += "<br/>Once you activate your account, you will be able to follow your favorite sources and/or tags. Please select at least 3 sources to build your feeds.";
                    Msg += "<br/><br/><strong>How to follow/unfollow new sources or tags?</strong>";
                    Msg += "<br/>- On your feeds page, by clicking the small tag icon over topic’s image, you will view topic’s tags and you can follow/unfollow any of them.";
                    Msg += "<br/>- On the article’s page, by clicking the small tag icon on the top bar, you will view topic’s tags and you can follow/unfollow any of them.";
                    Msg += "<br/>- You can also visit your Account page to follow/unfollow any sources or tags";
                    Msg += "<br/>Because we use SUNDOQ, we have clearer inboxes. Forget about the “no reply” dull rule and feel free to reply to any of our emails. We will get back to you very soon.";
                    db.Users.Add(this);
                    db.SaveChanges();
                }
                Helpers.sendEmail(this.Email, "Activate your SUNDOQ account ", Msg);
                _success = true;
                return "You're In! Now you need click on the link we sent to your email to activate your account.";
            }
        }
        public Users Activate()
        {
            if (db.Users.Any(p => p.Tags == this.Tags))
            {
                Users User = db.Users.Where(p => p.Tags == this.Tags).SingleOrDefault();
                db.Users.Attach(User);
                User.AccountStatus = 1;
                //User.Tags = "";
                db.SaveChanges();
                HttpContext.Current.Session.Add("User", User);
                return User;
            }
            else
            {
                return null;
            }
        }
        public string Update(out bool _success)
        {
            Users User = db.Users.Where(p => p.Id == this.Id).SingleOrDefault();
            User.Gender = this.Gender;
            User.Password = this.Password;
            User.DateOfBirth = this.DateOfBirth;
            User.Tags = this.Tags;
            User.AccountStatus = 2;
            db.SaveChanges();
            HttpContext.Current.Session["User"] = User;
            _success = true;
            return "Hoooray! All changes have been updated succesfully.";
        }
        public string Login(out bool _success)
        {
            Users User = db.Users.Where(p => p.Email == this.Email && p.Password == this.Password).SingleOrDefault();
            if (User != null && User.Id > 0 && User.AccountStatus >= 1)
            {
                HttpContext.Current.Session.Add("User", User);
                _success = true;
                return "Welcome Back! Please hold on while loading Your Feeds.";
            }
            else if (User != null && User.AccountStatus == null)
            {
                _success = false;
                return "You should activate your account first, please check your email or <a href='/user/sendactivation/" + User.Id + "'> click here</a>to resend activation email";
            }
            else
            {
                _success = false;
                return "Sorry Dude! You entered a wrong Email or Password. Try again";
            }
        }
        public string regiterWithFacebook(out bool _success)
        {
            if (db.Users.Any(p => p.Email == this.Email))
            {
                _success = false;
                return "User already regitered with this email";
            }
            else
            {
                db.Users.Add(this);
                db.SaveChanges();
                _success = true;
                return "Regitered succefully, please check your email to activate your account";
            }
        }
        public string sendreset(out bool _success)
        {
            if (db.Users.Any(p => p.Email == this.Email))
            {
                _success = true;
                Users user = db.Users.Where(p => p.Email == this.Email).FirstOrDefault();
                user.Password = Guid.NewGuid().ToString();
                string Msg = "Dear " + this.Email;
                Msg += "Hello [Name],";
                Msg += "<br/><br/>You asked us to reset your password in SUNDOQ – Don’t worry, it happens with many of us!";
                Msg += "<br/>All you have to do is to click on the following link to choose a new password.";
                Msg += "<br/><br/><a href='" + baseUrl + "User/reset/" + user.Password + "'>Reset my password</a>";
                Msg += "<br/><br/>If you have not requested a password reset please contact us immediately at help-me@sundoq.com";
                db.SaveChanges();
                Helpers.sendEmail(user.Email, "Forgot it? Reset your SUNDOQ password here", Msg);
                return "Please check your email in minutes to reset your password";
            }
            else
            {
                _success = false;
                return "This email is not registered";
            }
        }
        public string getSuggestedTags()
        {
            string Tags = "";
            if (HttpContext.Current.Session["User"] != null)
            {
                Users Current = (Users)HttpContext.Current.Session["User"];
                Random r = new Random();
                string LastFollowedTag = Current.Tags.Split('#')[r.Next(0, Current.Tags.Split('#').Length)];
                DateTime limit = DateTime.Now.AddDays(-1);
                var lstOftTags = db.Topics.Where(t => t.PubDate >= limit && t.Tags.ToLower().Contains(LastFollowedTag.ToLower())).ToList();
                List<string> CollectdTags = new List<string>();
                foreach (Topics t in lstOftTags)
                {
                    CollectdTags.AddRange(t.Tags.Split('#').ToList());
                }
                if (Current.IgnoredTags == null)
                    Current.IgnoredTags = "";
                CollectdTags = CollectdTags.Except(Current.Tags.Split('#').ToList()).Except(Current.IgnoredTags.Split('#').ToList()).ToList();
                List<KeyValuePair<string, int>> lst = new List<KeyValuePair<string, int>>();
                foreach (string tag in CollectdTags)
                {
                    if (tag.Trim().Length < 1)
                        continue;
                    KeyValuePair<string, int> newpair = new KeyValuePair<string, int>(tag, db.Topics.Where(t => t.Tags.Contains(tag)).Count());
                    lst.Add(newpair);
                }
                lst.OrderByDescending(x => x.Value);
                int count = 0;
                foreach (KeyValuePair<string, int> pair in lst)
                {
                    if (count == 5)
                        break;
                    Tags += "#" + pair.Key;
                    count++;
                }
            }
            return Tags;
        }
    }
}