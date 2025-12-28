using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Configuration; 
using System.IO;

namespace TS360.Common.Email
{
    public class EmailSomebody
    {


        public static void SendEmail(string EmailMessage)
        {



            try
            {

                if (ConfigurationManager.AppSettings["EmailFlag"].ToUpper() == "TRUE")
                {
                    string to = ConfigurationManager.AppSettings["EmailToRecipients"];
                    string from = ConfigurationManager.AppSettings["EmailFrom"];
                    MailMessage message = new MailMessage(from, to);
                    message.Subject = "CRELExtract"  ;
                    message.Body = EmailMessage;
                    

                    SmtpClient client = new SmtpClient(ConfigurationManager.AppSettings["EmailServer"]);
                    // Credentials are necessary if the server requires the client  
                    // to authenticate before it will send e-mail on the client's behalf.
                    client.UseDefaultCredentials = false;
                    client.Send(message);



                }
            }
            // empty catch block here 
            catch
            {
                //CREATE_LOGFILE.Write("EmailException " + Guid + ex.Message, FileLogRepository.Level.ERROR);

            }


        }




        public static void SendEmailException(string EmailMessage)
        {

            //FileLogRepository CREATE_LOGFILE = new FileLogRepository(AppSetting.CreateLogFolder, AppSetting.CreateLogFilePrefix);

            try
            {

                if (ConfigurationManager.AppSettings["EmailExceptionFlag"].ToUpper() == "TRUE")
                {
                    string to = ConfigurationManager.AppSettings["EmailExceptionToRecipients"];
                    string from = ConfigurationManager.AppSettings["EmailFrom"];
                    MailMessage message = new MailMessage(from, to);
                    message.Subject = "CRELExtract Exception" ;
                    message.Body = EmailMessage;

                    SmtpClient client = new SmtpClient(ConfigurationManager.AppSettings["EmailServer"]);
                    // Credentials are necessary if the server requires the client  
                    // to authenticate before it will send e-mail on the client's behalf.
                    client.UseDefaultCredentials = false;
                    client.Send(message);



                }
            }
            // empty catch block here 
            catch
            {
                //CREATE_LOGFILE.Write("EmailException " + Guid + ex.Message, FileLogRepository.Level.ERROR);

            }


        }






    }
}
