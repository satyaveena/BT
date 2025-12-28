using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BT.ILSQueue.Business.Helpers
{
    public class CommonHelper
    {
        public static void SendEmail(Exception ex)
        {
            MailMessage mail = new MailMessage();

            var emailFrom = ConfigurationManager.AppSettings["EmailFrom"];
            var emailTo = ConfigurationManager.AppSettings["EmailTo"];
            var environment = ConfigurationManager.AppSettings["Environment"];
            var emailSubject = "ETS API Exception from " + Environment.MachineName + " " + environment;
            var emailBody = ex.Message + " Please see details in MongoDB log.";


            mail.From = new MailAddress(emailFrom);
            mail.To.Add(emailTo);

            mail.Subject = emailSubject;
            mail.Body = emailBody;

            SmtpClient client = new SmtpClient();
            client.Send(mail);
        }

     

        /// <summary>
        /// ConvertToStringArray and remove the first element if it's numeric
        /// </summary>
        /// <param name="inputValue"> "3;0;2;1" or "2;Axis360;Gale" or "All;audiotype1;audiotype2"</param>
        /// <returns></returns>
        public static string[] ConvertToStringArrayWithoutFirstNumberElement(string inputValue)
        {
            if (string.IsNullOrEmpty(inputValue))
                return null;

            var arr = inputValue.Split(';');
            if (arr.Length <= 0)
                return null;

            if (!arr[0].All(char.IsDigit))
                return arr;

            var temp = string.Join(";", arr, 1, arr.Count() - 1);
            return temp.Split(';');
        }

      

        public static string ExtractUrlQueryString(string url, string paramName)
        {
            if (!string.IsNullOrEmpty(url) && !string.IsNullOrEmpty(paramName))
            {
                var idx = url.IndexOf('?');
                var queryString = url.Substring(idx);
                if (!string.IsNullOrEmpty(queryString))
                {
                    var qsCollection = HttpUtility.ParseQueryString(queryString);
                    if (qsCollection[paramName] != null)
                    {
                        return qsCollection[paramName];
                    }
                }
            }

            return string.Empty;
        }

        public static void CopyProperties(object self, object parent)
        {
            var fromProperties = parent.GetType().GetProperties();
            var toProperties = self.GetType().GetProperties();

            try
            {

                foreach (var fromProperty in fromProperties)
                {
                    foreach (var toProperty in toProperties)
                    {
                        if (fromProperty.Name == toProperty.Name && fromProperty.PropertyType == toProperty.PropertyType)
                        {
                            toProperty.SetValue(self, fromProperty.GetValue(parent));
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //string pause = "";
            }
        }

        public static void CreateFile(string folderName, string fileName, string content)
        {
            var fs = new FileStream(folderName + fileName, FileMode.Create);
            try
            {
                using (var writer = new BinaryWriter(fs, Encoding.Default))
                {
                    writer.Write(content.ToCharArray());
                }
            }
            finally
            {
                if (fs != null)
                    fs.Dispose();
            }
        }

        public static string GetCartManagerURL()
        {
            return AppConfigHelper.RetriveAppSettings<string>(AppConfigHelper.InternetSiteURL) + "/_layouts/CommerceServer/CartDetailsPage.aspx?CartId={0}";
        }

        public static string GetPolarisAppRelativePath(string papiUrl)
        {
            string papiAppRelativePath = papiUrl;

            if (papiAppRelativePath.EndsWith("/") || papiAppRelativePath.EndsWith("\\"))
            {
                papiAppRelativePath = papiAppRelativePath.Substring(0, papiAppRelativePath.Length - 1);
            }

            papiAppRelativePath += "/protected/";

            papiAppRelativePath += (
            AppConfigHelper.RetriveAppSettings<string>(AppConfigHelper.PolarisVersion) + "/" +
            AppConfigHelper.RetriveAppSettings<string>(AppConfigHelper.PolarisLangID) + "/" +
            AppConfigHelper.RetriveAppSettings<string>(AppConfigHelper.PolarisAppID) + "/" +
            AppConfigHelper.RetriveAppSettings<string>(AppConfigHelper.PolarisOrgID));

            return papiAppRelativePath;

        }
    }
}
