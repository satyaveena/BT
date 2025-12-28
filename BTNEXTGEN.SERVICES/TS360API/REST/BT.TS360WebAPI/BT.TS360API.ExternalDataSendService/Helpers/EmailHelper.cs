using BT.TS360API.ExternalDataSendService.Configration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace BT.TS360API.ExternalDataSendService.Helpers
{
    public class EmailHelper
    {
        public static bool SendEmail(string[] toEmails, string subject, string body)
        {
            if (toEmails == null || toEmails.Length == 0)
                return false;

            try
            {
                using (var smtpClient = new SmtpClient())
                {
                    var message = new MailMessage();
                    message.From = new MailAddress(message.From.Address, "TS360 Services Support Team");

                    foreach (string toEmail in toEmails)
                    {
                        if (!string.IsNullOrWhiteSpace(toEmail))
                            message.To.Add(new MailAddress(toEmail.Trim()));
                    }

                    message.Subject = subject;
                    message.Body = body;
                    message.IsBodyHtml = true;

                    smtpClient.Send(message);

                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public static bool NotifyException(string error)
        {
            string[] toEmails = AppSettings.EmailTo.Split(';');
            string currentEnv = AppSettings.CurrentEnvironment;

            string subject = AppSettings.CurrentEnvironment + " - TS360 External Data Send API Error";

            return SendEmail(toEmails, subject, error);
        }
    }
}