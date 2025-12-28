using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BT.CDMS.Business.Helpers
{
    public class CommonHelper
    {
        public static void SendEmail(Exception ex)
        {
            MailMessage mail = new MailMessage();
            
            var emailFrom = ConfigurationManager.AppSettings["EmailFrom"];
            var emailTo = ConfigurationManager.AppSettings["EmailTo"];
            var environment = ConfigurationManager.AppSettings["Environment"];
            var emailSubject = "CDMS API Exception from " + Environment.MachineName + " " + environment;
            var emailBody = ex.Message + " Please see details in MongoDB log.";
            

            mail.From = new MailAddress(emailFrom);
            mail.To.Add(emailTo);

            mail.Subject = emailSubject;
            mail.Body = emailBody;

            SmtpClient client = new SmtpClient();            
            client.Send(mail);
        }
    }
}
