using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BT.ETS.WinService.Helper
{
    public class EmailHelper
    {
        private string _smtpSever;

        public EmailHelper()
        {
            _smtpSever = AppSettings.SmtpServer;
        }

        public bool Send(string emailLists, string messageSubject, string messageBody)
        {
#if DEBUG
            return true;
#endif

            bool isSuccessful = true;
            SmtpClient client = new SmtpClient(_smtpSever);
            try
            {
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
            catch
            {
                // TODO: write log
                isSuccessful = false;
            }
            finally
            {
                client.Dispose();
            }

            return isSuccessful;
        }

        public void Send(string messageBody)
        {
            var emailToList = AppSettings.EmailList;
            var messageSubject = "ETSBackgroundJob " + AppSettings.Environment;

            Send(emailToList, messageSubject, messageBody);
        }
    }
}
