using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Mail; 
using System.Net.Mime;
using BT.TS360API.WebAPI.Services;
using BT.TS360API.WebAPI.Common.Configuration;


namespace BT.TS360API.WebAPI.Common
{
    public class EmailExceptions
    {


        public static void SendEmail(string emailLists, string methodName, string messageBody, string smtpServer, string EmailFlag)
        {
            try
            {

                if (EmailFlag == "true")
                {

                    SmtpClient client = new SmtpClient(smtpServer);
                    client.UseDefaultCredentials = true;

                    MailAddress from = new MailAddress("no-reply@btol.com", "TS360 Services Support Team");
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
                    message.Subject = AppSetting.Environment + "TS360 WEB API - " + methodName; ;

                    client.Send(message);
                }
            }
            catch (Exception)
            {

            }
        }

        public static void SendEmailException(string EmailMessage, string MethodName, string EmailFlag, string Guid )
        {

            FileLogRepository CREATE_LOGFILE = new FileLogRepository(AppSetting.CreateLogFolder, AppSetting.CreateLogFilePrefix);

            try
            {

                if (EmailFlag == "true")
                {
                    string to = AppSetting.CreateEmailToExceptions;
                    string from = "no-reply@baker-taylor.com";
                    MailMessage message = new MailMessage(from, to);
                    message.Subject = AppSetting.Environment + "TS360 WEB API - " + MethodName;
                    message.Body = EmailMessage;

                    SmtpClient client = new SmtpClient(AppSetting.CreateEmailServer);
                    // Credentials are necessary if the server requires the client  
                    // to authenticate before it will send e-mail on the client's behalf.
                    client.UseDefaultCredentials = true;
                    client.Send(message);



                }
            }
            // empty catch block here 
            catch 
            {
                    //CREATE_LOGFILE.Write("EmailException " + Guid + ex.Message, FileLogRepository.Level.ERROR);

            }


        }



        public static void SendEmailExceptionGeneric(string EmailMessage, string MethodName, string EmailFlag, string EmailTo, string EmailServer)
        {

            //FileLogRepository CREATE_LOGFILE = new FileLogRepository(AppSetting.CreateLogFolder, AppSetting.CreateLogFilePrefix);

            try
            {

                if (EmailFlag == "true")
                {
                    string to = AppSetting.CreateEmailToExceptions;
                    string from = "no-reply@baker-taylor.com";
                    MailMessage message = new MailMessage(from, to);
                    message.Subject = "TS360 WEB API - " + MethodName;
                    message.Body = EmailMessage;

                    SmtpClient client = new SmtpClient(AppSetting.CreateEmailServer);
                    // Credentials are necessary if the server requires the client  
                    // to authenticate before it will send e-mail on the client's behalf.
                    client.UseDefaultCredentials = true;
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