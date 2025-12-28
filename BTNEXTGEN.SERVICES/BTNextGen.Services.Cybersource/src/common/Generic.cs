using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
//using CyberSource.Api;
//using Cybersource_rest_samples_dotnet.Samples.Reporting.CoreServices;
using System.Net; 

namespace BT.TS360.Services.Cybersource.Common.Generic
{
    public class Generic
    {
        public List<Dictionary<string, string>> LoadCsvAsDictionary(string path)
        {
            var result = new List<Dictionary<string, string>>();

            var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            System.IO.StreamReader file = new System.IO.StreamReader(fs);

            string line;

            int n = 0;
            List<string> columns = null;
            while ((line = file.ReadLine()) != null)
            {


                var values = SplitCsv(line);

                if (n == 1)
                {
                    columns = values;
                }
                else
                {
                    if (n != 0)
                    {
                        var dict = new Dictionary<string, string>();
                       for (int i = 0; i < columns.Count; i++)
                            if (i < values.Count)
                                dict.Add(columns[i], values[i]);
                        result.Add(dict);
                    }
                }
                n++;
            }

            file.Close();
            return result;
        }

        public List<string> SplitCsv(string csv)
        {
            var values = new List<string>();

            int last = -1;
            bool inQuotes = false;

            int n = 0;
            while (n < csv.Length)
            {
                switch (csv[n])
                {
                    case '"':
                        inQuotes = !inQuotes;
                        break;
                    case ',':
                        if (!inQuotes)
                        {
                            values.Add(csv.Substring(last + 1, (n - last)).Trim(' ', ','));
                            last = n;
                        }
                        break;
                }
                n++;
            }

            if (last != csv.Length - 1)
                values.Add(csv.Substring(last + 1).Trim());

            return values;
        }

        public string CreateXml(object obj)
        {
            var xmlDoc = new XmlDocument();   // Represents an XML document
            var xmlSerializer = new XmlSerializer(obj.GetType()); // Initializes a new instance of the XmlDocument class.

            // Creates a stream whose backing store is memory.
            using (var xmlStream = new MemoryStream())
            {
                xmlSerializer.Serialize(xmlStream, obj);
                xmlStream.Position = 0;

                // Loads the XML document from the specified string.
                xmlDoc.Load(xmlStream);
                string xxx ;
                //xxx = xmlSerializer.Serialize(obj, Formatting.Indented); 
//string json = JsonConvert.SerializeObject(shippinglocationwaiting, Formatting.Indented);
                return xmlDoc.InnerText;
            }
        }

        public void SetNetworkSettings()
        {
            // setting servicepointmanager configs for SSL/TSL / Proxy issues
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ServicePointManager.ServerCertificateValidationCallback +=
                (sender, certificate, chain, sslPolicyErrors) => true;
        }

    }
}
