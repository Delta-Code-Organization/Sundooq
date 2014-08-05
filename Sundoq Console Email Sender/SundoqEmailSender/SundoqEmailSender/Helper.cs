using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;

namespace SundoqEmailSender
{
    public class Helper
    {
        public static void sendEmail(string _to, string _subject, string _body, MailTypes Type, int? UserID)
        {
            try
            {
                // set body-message and encoding
                string body = "";
                body += "<div style='font-size:24;text-align:left;width:100%'>" + _body + "</div>";
                body += "<div style='font-size:24;text-align:left;width:100%'><br/><br/>Thanks,<br/>Your Friends at SUNDOQ</div>";
                try
                {
                    MailMessage mail = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient("mail.tebasel.com");

                    mail.From = new MailAddress("please-reply@sundoq.com");
                    mail.To.Add(_to);
                    mail.Bcc.Add("please-reply@sundoq.com");
                    mail.Subject = _subject;
                    mail.Body = body;
                    mail.IsBodyHtml = true;
                    SmtpServer.Port = 26;
                    SmtpServer.Credentials = new System.Net.NetworkCredential("please-reply@sundoq.com", "@%g36Nbk5oN#");
                    SmtpServer.EnableSsl = false;
                    SmtpServer.Send(mail);
                    Emails E = new Emails();
                    E.Date = DateTime.Now;
                    E.Type = (int)Type;
                    E.User = UserID;
                    using (SundooqDBEntities db = new SundooqDBEntities())
                    {
                        db.Emails.Add(E);
                        db.SaveChanges();
                    }
                }
                catch (Exception)
                {

                }
            }

            catch (SmtpException ex)
            {
                throw new ApplicationException
                  ("SmtpException has occured: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
