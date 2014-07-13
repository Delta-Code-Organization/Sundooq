using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;

namespace SundooqLanding.Models
{
    public static class Helpers
    {
        public static string baseUrl = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority +
                        HttpContext.Current.Request.ApplicationPath.TrimEnd('/') + "/";
        public static void sendEmail(string _to, string _subject, string _body)
        {
            try
            {
                // set body-message and encoding
                string body = "";
                body += "<div style='font-size:24;text-align:left;width:100%'>" + _body + "</div>";
                body += "<div style='font-size:24;text-align:left;width:100%'><br/> Sincerely, <br/> Sundoq Team</div>";
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
                }
                catch (Exception ex)
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