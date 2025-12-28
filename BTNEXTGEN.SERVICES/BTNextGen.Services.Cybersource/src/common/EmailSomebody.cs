using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using BT.TS360.Services.Cybersource;
using BT.TS360.Services.Cybersource.Common.Configuration;
using System.IO;

namespace BT.TS360.Services.Cybersource.Common
{
    public class EmailSomebody
    {


        public static void SendEmail(string EmailMessage, string MethodName, string EmailFlag, string Guid)
        {



            try
            {

                if (EmailFlag == "true")
                {
                    string to = AppSetting.EmailToRecipients;
                    string from = "no-reply@baker-taylor.com";
                    MailMessage message = new MailMessage(from, to);
                    message.Subject = AppSetting.Environment + "Cybersource report downloads - " + MethodName;
                    message.Body = EmailMessage;
             

                    SmtpClient client = new SmtpClient(AppSetting.EmailServer);
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


        public static void SendEmailAttachments(string EmailMessage, string MethodName, string EmailFlag, string Guid, string Path, string EmailTo, string archivePath)
        {



            try
            {

                if (EmailFlag == "true")
                {
                    string to = EmailTo;
                    string from = "no-reply@baker-taylor.com";
                    MailMessage message = new MailMessage(from, to);
                    message.Subject = AppSetting.Environment + ":TS360 Mongo CompassLoad" ;
                    message.Body = EmailMessage;

                    string[] fileEntries = Directory.GetFiles(Path);

                     foreach (string fileName in fileEntries)
                    {
                        // Create  the file attachment for this e-mail message.
                        Attachment data = new Attachment(fileName, MediaTypeNames.Application.Octet);
                        message.Attachments.Add(data);
                       
                    }
                   fileEntries = null; 
                    SmtpClient client = new SmtpClient(AppSetting.EmailServer);
                    // Credentials are necessary if the server requires the client  
                    // to authenticate before it will send e-mail on the client's behalf.
                    client.UseDefaultCredentials = true;
                    client.Send(message);
                    message.Dispose(); 

                    //If we get this far the email send worked.  archive any files sent in an email. 
                    string[] files = System.IO.Directory.GetFiles(Path);
                    foreach (string s in files)
                    {
                        // Use static Path methods to extract only the file name from the path.
                        string fileName; 
                        string destFile;
                        string fileNameNoExt = System.IO.Path.GetFileNameWithoutExtension(s) ;

                        string fileTimeStamp = DateTime.Now.ToString("yyyy-MM-dd-HHmm");
                        fileName = fileNameNoExt + fileTimeStamp + ".txt"; 
                        
                       
                        destFile = System.IO.Path.Combine(archivePath, fileName);
                        System.IO.File.Move(s, destFile);

                    }
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
                    string to = AppSetting.EmailToRecipients;
                    string from = "no-reply@baker-taylor.com";
                    MailMessage message = new MailMessage(from, to);
                    message.Subject = "TS360 WEB API - " + MethodName;
                    message.Body = EmailMessage;

                    SmtpClient client = new SmtpClient(AppSetting.EmailServer);
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
