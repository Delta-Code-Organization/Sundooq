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
                body = "<div style='background-color:#f00;width:100%;height:50px'></div>";
                body += "<div style='text-align:center;width:100%;font-size:30px'>Sundooq</div>";
                body += "<div style='text-align:left;width:100%'>" + _body + "</div>";
                body += "<div style='text-align:left;width:100%'><br/> Sincerly, <br/> Sundooq Team</div>";
                try
                {
                    MailMessage mail = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                    mail.From = new MailAddress("aali.ibtekar@gmail.com");
                    mail.To.Add(_to);
                    mail.Subject = _subject;
                    mail.Body = body;
                    mail.IsBodyHtml = true;
                    SmtpServer.Port = 587;
                    SmtpServer.Credentials = new System.Net.NetworkCredential("aali.ibtekar@gmail.com", "2662006AmirAmira");
                    SmtpServer.EnableSsl = true;

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