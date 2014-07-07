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
                return "User already registered with this email";
            }
            else
            {
                string Msg = "";
                if (Registered != null)
                {
                    Registered.Tags = Guid.NewGuid().ToString();
                    Msg = "Welcome " + Registered.Email;
                    Msg += " <br/> We are very exicted to have you in Sundooq <br/> ";
                    Msg += "Please click the link below to activate your account and start with Sundooq <br/> ";
                    Msg += "<a href='" + baseUrl + "User/Activate/" + Registered.Tags + "'>Activate My Account</a>";
                    Registered.AccountStatus = 0;
                    db.SaveChanges();
                }
                else
                {
                    this.Tags = Guid.NewGuid().ToString();
                    Msg = "Welcome " + this.Email;
                    Msg += " <br/> We are very exicted to have you in Sundooq <br/> ";
                    Msg += "Please click the link below to activate your account and start with Sundooq <br/> ";
                    Msg += "<a href='" + baseUrl + "User/Activate/" + this.Tags + "'>Activate My Account</a>";
                    db.Users.Add(this);
                    db.SaveChanges();
                }
                Helpers.sendEmail(this.Email, "Welcome to Sundooq, Activate your account now", Msg);
                _success = true;
                return "Registered succefully, please check your email to activate your account";
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
            return "User data updated succefully";
        }
        public string Login(out bool _success)
        {
            Users User = db.Users.Where(p => p.Email == this.Email && p.Password == this.Password).SingleOrDefault();
            if (User != null && User.Id > 0 && User.AccountStatus >= 1)
            {
                HttpContext.Current.Session.Add("User", User);
                _success = true;
                return "Login succefully, you should be redirected to your home but it will take a minute to prepare your list, Please wait";
            }
            else if (User != null && User.AccountStatus == null)
            {
                _success = false;
                return "You should activate your account first, please check your email or <a href='/user/sendactivation/" + User.Id + "'> click here</a>to resend activation email";
            }
            else
            {
                _success = false;
                return "Login failed, please check your email and password";
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
                string Msg = "Welcome " + this.Email;
                Msg += " <br/> Please follow the link below to reset your password <br/> ";
                Msg += "<a href='" + baseUrl + "User/reset/" + user.Password + "'>Reset my password</a>";
                db.Users.Add(this);
                db.SaveChanges();
                Helpers.sendEmail(user.Email, "Sundooq.com, Reset your password", Msg);
                return "Please check your email in minutes to reset your password";
            }
            else
            {
                _success = false;
                return "This email is not registered";
            }
        }
    }
}