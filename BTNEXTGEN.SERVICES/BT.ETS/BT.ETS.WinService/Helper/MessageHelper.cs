using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
//using BTNextGen.Elmah;
using System.IO;
using System.Net.Mail;
using BT.ETS.WinService.Constants;

namespace BT.ETS.WinService.Helper
{
    public class MessageHelper
    {
        private string _sqlConnection;
        private string _folder;
        private string _prefix;
        private string _fileName;
        private static object locker = new object();
        private string _smtpSever;
        private string _emailLists;
        private string _serviceName;
        private string _environment;
        private bool _verboseLogging;

        public MessageHelper(string sqlConnection, string folder, string prefix, string smtpServer, string emailLists, string serviceName, string environment, bool verboseLogging)
        {
            _sqlConnection = sqlConnection;
            _smtpSever = smtpServer;
            _emailLists = emailLists;
            _folder = folder;
            _prefix = prefix;
            _serviceName = serviceName;
            _environment = environment;
            _verboseLogging = verboseLogging;
            _fileName = string.Format("{0}_{1}.txt", _prefix, DateTime.Now.ToString("yyyy-MM-dd-HH0000"));
        }

        public MessageHelper(string folder, string prefix, string smtpServer, string emailLists, string serviceName, string environment, bool verboseLogging)
        {
            _smtpSever = smtpServer;
            _emailLists = emailLists;
            _folder = folder;
            _prefix = prefix;
            _serviceName = serviceName;
            _environment = environment;
            _fileName = string.Format("{0}_{1}.txt", _prefix, DateTime.Now.ToString("yyyy-MM-dd-HH0000"));
        }

        public void HandleMessage(Exception exception, string source, string message, FileLoggingLevel level)
        {
            if (level == FileLoggingLevel.ERROR)
            {
                if (exception != null)
                {
                    //LogErrorToELMAH(exception, _serviceName + " " + source);
                }
                WriteToFile(message, level);
            }
            else if (level == FileLoggingLevel.STATE)
            {
                WriteToFile(message, level);
                SendEmail(message);
            }
            else if (level == FileLoggingLevel.INFO && _verboseLogging)
            {
                WriteToFile(message, level);
            }
        }

       

        private void WriteToFile(string message, FileLoggingLevel level)
        {
            lock (locker)
            {
                string logFilePath = string.Format("{0}\\{1}", _folder, _fileName);
                using (FileStream file = new FileStream(logFilePath, FileMode.Append, FileAccess.Write, FileShare.None))
                {
                    DateTime now = DateTime.Now;
                    StreamWriter writer = new StreamWriter(file);
                    writer.Write(string.Format("{0}\t{1}\t{2}", now, level.ToString(), message));
                    writer.WriteLine();
                    writer.Flush();
                }
            }
        }

        public void SendEmail(string messageBody)
        {
            try
            {
                SmtpClient client = new SmtpClient(_smtpSever);
                MailAddress from = new MailAddress("no-reply@btol.com", "Mongo Support Team");

                MailMessage message = new MailMessage();

                message.From = from;
                message.IsBodyHtml = true;

                message.To.Clear();

                //   MailAddress to = new MailAddress(emailLists);

                var emailToList = _emailLists.Split(';');
                foreach (var emailTo in emailToList)
                {
                    if (!string.IsNullOrEmpty(emailTo))
                        message.To.Add(emailTo);
                }


                messageBody = messageBody.Replace(Environment.NewLine, "<br>");

                message.Body = messageBody;
                message.Subject = _serviceName + " " + _environment;

                client.Send(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Can not send email: " + ex.Message);
            }
        }
        
    }
}
