
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Microsoft.SharePoint.Utilities;

namespace BT.TS360API.Common.Helpers
{
    public class Emailer
    {
        private string _smtpSever;

        public Emailer(string smtpServer)
        {
            _smtpSever = smtpServer;
        }

        public bool Send(string emailLists, string messageSubject, string messageBody)
        {
            bool rs = true;
            try
            {
                SmtpClient client = new SmtpClient(_smtpSever);
                MailAddress from = new MailAddress("no-reply@btol.com", "Support Team");

                MailMessage message = new MailMessage();

                message.From = from;
                message.IsBodyHtml = true;

                message.To.Clear();

                string[] emailToAddressList = emailLists.Split(';');
                foreach (string emailToAddress in emailToAddressList)
                {
                    MailAddress to = new MailAddress(emailToAddress);
                    message.To.Add(to);
                }

                messageBody = messageBody.Replace(Environment.NewLine, "<br>");

                message.Body = messageBody;
                message.Subject = messageSubject;

                client.Send(message);
            }
            catch (Exception ex)
            {
                rs = false;
            }
            return rs;
        }

        public static string GetDataFixEmailContentTemplate()
        {
            var createEmailContnetTemplate = System.Web.Hosting.HostingEnvironment.MapPath("~/Template/DataFixTemplate.xml");
            return createEmailContnetTemplate;
        }
    }
    public static class StringHelpers
    {
        public static string StripHTML(this string HTMLText)
        {
            var reg = new Regex("<[^>]+>", RegexOptions.IgnoreCase);
            return reg.Replace(HTMLText, "");
        }

        public static string Left(this string @this, int count)
        {
            if (@this.Length <= count)
            {
                return @this;
            }
            else
            {
                return @this.Substring(0, count);
            }
        }
    }
    
}

