using System;
using System.IO;
using System.Net;
using System.Configuration;
using TS360.Common.FileLogging;



namespace TS360.FTP
{
    public class CRELFTPSend
    {
        
        public  void FTP (string LocalFile, string LocalFileNameOnly)
     

        {
            string LogFolder = ConfigurationManager.AppSettings["LogFolder"];
            string LogPrefix = ConfigurationManager.AppSettings["LogFilePrefix"];
            bool passivemode = Convert.ToBoolean(ConfigurationManager.AppSettings["OutboundFTPPassive"]);
            string FTPUri = ConfigurationManager.AppSettings["OutboundFTPURI"];
            string FTPUser = ConfigurationManager.AppSettings["OutboundFTPUser"];
            string FTPPass = FTPUser; 

            FileLogRepository fileLogLoad = new FileLogRepository(LogFolder, LogPrefix);
            try
            {

                // Get the object used to communicate with the server.
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(FTPUri + LocalFileNameOnly );
                request.UsePassive = passivemode;
                request.Method = WebRequestMethods.Ftp.UploadFile; 
                request.Credentials = new NetworkCredential(FTPUser , FTPPass );


                using (var fileStream = File.OpenRead(LocalFile ))
                {
                    using (var requestStream = request.GetRequestStream())
                    {
                        fileStream.CopyTo(requestStream);
                        requestStream.Close();
                    }
                }

                var response = (FtpWebResponse)request.GetResponse();
                Console.WriteLine("Upload done: {0}", response.StatusDescription);
                response.Close();


            }
            catch (Exception ex)
            {
                fileLogLoad.Write("Exception ftping file: " + ex.ToString(), FileLogRepository.Level.ERROR);
                throw new Exception(ex.Message); 

            }
        }
    }
}