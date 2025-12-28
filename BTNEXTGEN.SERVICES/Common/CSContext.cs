using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Microsoft.CommerceServer.Inventory;
using Microsoft.CommerceServer;
using Microsoft.CommerceServer.Runtime;
using Microsoft.CommerceServer.Catalog;

namespace BTNextGen.Services.Common
{
    public class CSContext
    {

        public static CatalogContext CreateCatalogContextFromSiteAgent()
        {
            return CreateCatalogContextFromSiteAgent(Config.CSSiteName);
        }

        public static CatalogContext CreateCatalogContextFromServiceAgent()
        {
            return CreateCatalogContextFromServiceAgent(Config.CatalogServiceUrl);
        }

        public static CatalogContext CreateCatalogContextFromSiteAgent(string CSSiteName)
        {
            try
            {
                // Create a CatalogSiteAgent to connect to the database
                CatalogSiteAgent catalogSiteAgent = new CatalogSiteAgent();
                catalogSiteAgent.SiteName = CSSiteName;

                // Create the CatalogContext object
                CatalogContext catalogContext = CatalogContext.Create(catalogSiteAgent);
                return catalogContext;
            }
            catch 
            {
                //TODO: exception handling goes here
                return null;
            }
        }

        public static CatalogContext CreateCatalogContextFromServiceAgent(string url)
        {
            try
            {
                // Create a CatalogServiceAgent to connect to the Web service
                CatalogServiceAgent serviceAgent = null;
                CatalogContext catalogContext = null;

                //= new CatalogServiceAgent(url, new string[] { "ntlm", "kerberos", "negotiate" });
                //serviceAgent.Credentials = CredentialCache.DefaultNetworkCredentials;

                //CredentialCache credentials = new CredentialCache();
                //NetworkCredential networkCredential = new NetworkCredential();
               
                ////create new uri with web service url
                //System.Uri uri = new System.Uri(url);

                ////add the network credential to the credential cache collection
                //credentials.Add(uri, "NTLM", networkCredential);

                serviceAgent = new CatalogServiceAgent(url, ServiceAgent.DefaultAuthMethods);
                serviceAgent.Credentials = CredentialCache.DefaultNetworkCredentials;

                
                // Create the CatalogContext object
               catalogContext = CatalogContext.Create(serviceAgent);

                return catalogContext;
            }
            catch
            {
                // exception handling goes here
                return null;
            }
        }
    }
}
