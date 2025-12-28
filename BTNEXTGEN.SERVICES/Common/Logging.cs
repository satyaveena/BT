using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace BTNextGen.Services.Common
{
    class Logging
    {
        public void LogServiceFiles()
        {
            try
            {
                // TODO: configure this
                if (Config.IsLoggingOn)
                {
                    //////Load the XML file that represents the order.
                    ////XmlDocument doc = new XmlDocument();
                    //////For Testing save a sample file to an XML file format
                    ////doc.Load(@"C:\NextGenBasket.xml");  //doc.Load(@"Basket.xml"); 

                    
                    ////XmlTextWriter writer = new XmlTextWriter("C:\\AcceptBasketResult.xml", System.Text.Encoding.UTF8);
                    ////writer.Formatting = Formatting.Indented;
                    ////writer.WriteStartDocument();
                    ////poXml.WriteTo(writer);
                    ////writer.WriteEndDocument();
                    ////writer.Flush();
                }
            }
            catch
            {
                // call exception handling
            }

        }
    }
}
