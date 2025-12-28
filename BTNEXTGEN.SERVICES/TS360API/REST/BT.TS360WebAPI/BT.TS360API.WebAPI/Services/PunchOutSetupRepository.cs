using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Configuration;
using System.ServiceModel;
using BT.TS360API.WebAPI.Common.Configuration;
using System.Xml.Serialization;
using System.IO;
using System.Xml; 
using BT.TS360API.WebAPI.Models; 


namespace BT.TS360API.WebAPI.Services
{
    public class PunchOutSetupRepository
    {
        //private string dbConnectionString;
        //private SqlConnection dbConnection;

        public enum ExpectedType
        { 
            StringType = 0,
            NumberType=1,
            DateType = 2,
            BooleanType = 3,
            ImageType = 4
        }

        public PunchOutSetupRepository()
        {
        }



        public static void LogAPIMessage(
            string webMethod, string requestMessage,
            string responseMessage, string vendorAPIKey,
            string exceptionMessage 
            )
        {
            var dataConnect = ConfigurationManager.ConnectionStrings["ExceptionLogging"].ConnectionString;

            SqlConnection DBConnection = new SqlConnection(dataConnect);

            try
            {
                SqlCommand storedProc = new SqlCommand("procTS360APILogRequests", DBConnection);

                storedProc.CommandType = CommandType.StoredProcedure;


                SqlParameter webMethodIn = storedProc.Parameters.Add("@webMethod", SqlDbType.NVarChar, 50);
                webMethodIn.Direction = ParameterDirection.Input;
                webMethodIn.Value = webMethod;

                SqlParameter requestMessageIn = storedProc.Parameters.Add("@requestMessage", SqlDbType.NVarChar, 8000);
                requestMessageIn.Direction = ParameterDirection.Input;
                requestMessageIn.Value = requestMessage;

                SqlParameter responseMessageIn = storedProc.Parameters.Add("@responseMessage", SqlDbType.NVarChar, 8000);
                responseMessageIn.Direction = ParameterDirection.Input;
                responseMessageIn.Value = responseMessage;

                SqlParameter vendorAPIKeyIn = storedProc.Parameters.Add("@vendorAPIKey", SqlDbType.NVarChar, 255);
                vendorAPIKeyIn.Direction = ParameterDirection.Input;
                vendorAPIKeyIn.Value = vendorAPIKey;

                SqlParameter exceptionMessageIn = storedProc.Parameters.Add("@exceptionMessage", SqlDbType.NVarChar, 8000);
                exceptionMessageIn.Direction = ParameterDirection.Input;
                exceptionMessageIn.Value = exceptionMessage;

                SqlParameter createdOnIn = storedProc.Parameters.Add("@createdOn", SqlDbType.DateTime2, 7);
                createdOnIn.Direction = ParameterDirection.Input;
                createdOnIn.Value = DateTime.Now;

                SqlParameter createdByIn = storedProc.Parameters.Add("@createdBy", SqlDbType.NVarChar, 50);
                createdByIn.Direction = ParameterDirection.Input;
                createdByIn.Value = "TS360 API Service";

                DBConnection.Open();

                var records = storedProc.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
            finally
            {
               
                DBConnection.Close();
            }

        }


        public bool GoodMessageCreate (string tokenEncrypted, string RESPFileName, string RESPNODTDFileName,  out string innerXMLvalue)
        {
            bool noIssues = false; 

            //////////////////////////////////////////////////////
            cXML goodroot = new cXML();

            cXMLResponse goodresponse = null;
            goodresponse = new cXMLResponse();

            cXMLResponseStatus goodresponsestat = null;
            goodresponsestat = new cXMLResponseStatus();
            goodresponsestat.code = "200";
            goodresponsestat.text = "success";

            cXMLResponsePunchOutSetupResponse goodPunchOutSetupResponse = null;
            goodPunchOutSetupResponse = new cXMLResponsePunchOutSetupResponse();

            cXMLResponsePunchOutSetupResponseStartPage goodPOStartPage = null;
            goodPOStartPage = new cXMLResponsePunchOutSetupResponseStartPage();
            goodPOStartPage.URL = AppSetting.PunchOutURL + tokenEncrypted;


            goodroot.Response = goodresponse;
            goodroot.Response.Status = goodresponsestat;
            goodroot.Response.PunchOutSetupResponse = goodPunchOutSetupResponse;
            goodroot.Response.PunchOutSetupResponse.StartPage = goodPOStartPage;

            //Serialize the cXML, this result will be without DTD
            XmlSerializer myserializer = new XmlSerializer(typeof(cXML));
            StreamWriter myWriter = new StreamWriter(RESPNODTDFileName);
            myserializer.Serialize(myWriter, goodroot);
            myWriter.Close();

            //Create a document type node and  
            //add it to the document.
            XmlDocument doc = new XmlDocument();
            XmlDocumentType doctype;
            doc.XmlResolver = null;
            doctype = doc.CreateDocumentType("cXML", null, "http://xml.cxml.org/schemas/cXML/1.2.021/cXML.dtd", null);
            doc.AppendChild(doctype);
            doc.AppendChild(doc.CreateElement("cXML"));
            XmlAttribute attr = doc.CreateAttribute("payloadID");
            attr.Value = string.Format("{0}@btol.com", DateTime.Now.ToString("yyyyMMddHHmmssfffffff"));
            doc.DocumentElement.SetAttributeNode(attr);

            XmlAttribute attr1 = doc.CreateAttribute("timestamp");
            attr1.Value = DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:sszzz");
            doc.DocumentElement.SetAttributeNode(attr1);

            XmlAttribute attr2 = doc.CreateAttribute("version");
            attr2.Value = "1.0";
            doc.DocumentElement.SetAttributeNode(attr2);

            XmlAttribute attr3 = doc.CreateAttribute("xml:lang");
            attr3.Value = "en";
            doc.DocumentElement.SetAttributeNode(attr3);

            XmlDocument docold = new XmlDocument();
            docold.Load(RESPNODTDFileName);

            //Import the node from doc2 into the original document.
            XmlNode newResponse = doc.ImportNode(docold.DocumentElement.LastChild, true);
            doc.DocumentElement.AppendChild(newResponse);

            doc.Save(RESPFileName);
            innerXMLvalue = doc.InnerXml.ToString();

            noIssues = true;  
            /////////////////////////////////////////////////////////////////////////////////////////

            return noIssues; 

        }


        public bool BadMessageCreate(string StatusCode, string MessageText, string RESPFileName, string RESPNODTDFileName, out string innerXMLvalue)
        {
            bool noIssues = false;

            //////////////////////////////////////////////////////
            cXML badroot = new cXML();

            cXMLResponse badresponse = null;
            badresponse = new cXMLResponse();

            cXMLResponseStatus badresponsestat = null;
            badresponsestat = new cXMLResponseStatus();
            badresponsestat.code = StatusCode;
            badresponsestat.text = MessageText;

            badroot.Response = badresponse;
            badroot.Response.Status = badresponsestat;

            //Serialize the cXML, this result will be without DTD
            XmlSerializer myserializer = new XmlSerializer(typeof(cXML));
            StreamWriter myWriter = new StreamWriter(RESPNODTDFileName);
            myserializer.Serialize(myWriter, badroot);
            myWriter.Close();

            //Create a document type node and  
            //add it to the document.
            XmlDocument doc = new XmlDocument();
            XmlDocumentType doctype;
            doc.XmlResolver = null;
            doctype = doc.CreateDocumentType("cXML", null, "http://xml.cxml.org/schemas/cXML/1.2.021/cXML.dtd", null);
            doc.AppendChild(doctype);
            doc.AppendChild(doc.CreateElement("cXML"));
            XmlAttribute attr = doc.CreateAttribute("payloadID");
            attr.Value = string.Format("{0}@btol.com", DateTime.Now.ToString("yyyyMMddHHmmssfffffff"));
            doc.DocumentElement.SetAttributeNode(attr);

            XmlAttribute attr1 = doc.CreateAttribute("timestamp");
            attr1.Value = DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:sszzz");
            doc.DocumentElement.SetAttributeNode(attr1);

            XmlAttribute attr2 = doc.CreateAttribute("version");
            attr2.Value = "1.0";
            doc.DocumentElement.SetAttributeNode(attr2);

            XmlAttribute attr3 = doc.CreateAttribute("xml:lang");
            attr3.Value = "en";
            doc.DocumentElement.SetAttributeNode(attr3);

            XmlDocument docold = new XmlDocument();
            docold.Load(RESPNODTDFileName);

            //Import the node from doc2 into the original document.
            XmlNode newResponse = doc.ImportNode(docold.DocumentElement.LastChild, true);
            doc.DocumentElement.AppendChild(newResponse);

            doc.Save(RESPFileName);

            innerXMLvalue = doc.InnerXml.ToString();

            noIssues = true;
            /////////////////////////////////////////////////////////////////////////////////////////

            return noIssues;

        }




        public bool ValidateFields(PunchOutSetupSQL punchoutsql, out string FieldNameValue)
        {
            bool noIssues = true;
            FieldNameValue = ""; 

            if (punchoutsql.Extrinsic == null)  
            { 
                FieldNameValue = "Extrinsic";
                noIssues = false; 
            }
            else if (punchoutsql.BuyerCookie == null || punchoutsql.BuyerCookie =="") 
            {
                FieldNameValue = "BuyerCookie";
                noIssues = false; 
            }
            else if (punchoutsql.FromDomain == null || punchoutsql.FromDomain == "") 
            {
                FieldNameValue = "FromDomain";
                noIssues = false; 
            }
            else if (punchoutsql.FromIdentity == null || punchoutsql.FromIdentity == "") 
            {
                FieldNameValue = "FromIdentity";
                noIssues = false; 
            }
            else if (punchoutsql.ToDomain == null || punchoutsql.ToDomain == "") 
            {
                FieldNameValue = "ToDomain";
                noIssues = false; 
            }
            else if (punchoutsql.ToIdentity == null || punchoutsql.ToIdentity =="") 
            {
                FieldNameValue = "ToIdentity";
                noIssues = false; 
            }
            else if (punchoutsql.LoginEmail == null || punchoutsql.LoginEmail =="") 
            {
                FieldNameValue = "LoginEmail";
                noIssues = false; 
            }
            else if (punchoutsql.RequestPayloadID == null || punchoutsql.RequestPayloadID =="") 
            {
                FieldNameValue = "PayloadID";
                noIssues = false; 
            }
            else if (punchoutsql.SenderDomain == null || punchoutsql.SenderDomain =="") 
            {
                FieldNameValue = "SenderDomain";
                noIssues = false; 
            }

            else if (punchoutsql.SenderIdentity == null || punchoutsql.SenderIdentity =="") 
            {
                FieldNameValue = "SenderIdentity";
                noIssues = false; 
            }
            else if (punchoutsql.BrowserFormPost == null || punchoutsql.BrowserFormPost =="")
            {
                FieldNameValue = "BrowserFormPost";
                noIssues = false;
            }
            else 
            {
                noIssues = true; 
            }

       
            return noIssues;

        }     


      public static void InsertProcurementRequests(PunchOutSetupSQL punchoutsql) 

        {
            var dataConnect = ConfigurationManager.ConnectionStrings["Profiles"].ConnectionString;

            SqlConnection DBConnection = new SqlConnection(dataConnect);

            try
            {
                SqlCommand storedProc = new SqlCommand("procInsertProcurementSetupRequests", DBConnection);

                storedProc.CommandType = CommandType.StoredProcedure;


                SqlParameter loginEmail = storedProc.Parameters.Add("@loginEmail", SqlDbType.NVarChar, 100);
                loginEmail.Direction = ParameterDirection.Input;
                loginEmail.Value = punchoutsql.LoginEmail;

                SqlParameter requestPayloadID = storedProc.Parameters.Add("@requestPayloadID", SqlDbType.NVarChar, 100);
                requestPayloadID.Direction = ParameterDirection.Input;
                requestPayloadID.Value = punchoutsql.RequestPayloadID;

                SqlParameter fromDomain = storedProc.Parameters.Add("@fromDomain", SqlDbType.NVarChar, 100);
                fromDomain.Direction = ParameterDirection.Input;
                fromDomain.Value = punchoutsql.FromDomain;
                
                SqlParameter fromIdentity = storedProc.Parameters.Add("@fromIdentity", SqlDbType.NVarChar, 100);
                fromIdentity.Direction = ParameterDirection.Input;
                fromIdentity.Value = punchoutsql.FromIdentity;

                SqlParameter toDomain = storedProc.Parameters.Add("@toDomain", SqlDbType.NVarChar, 100);
                toDomain.Direction = ParameterDirection.Input;
                toDomain.Value = punchoutsql.ToDomain;

                SqlParameter toIdentity = storedProc.Parameters.Add("@toIdentity", SqlDbType.NVarChar, 100);
                toIdentity.Direction = ParameterDirection.Input;
                toIdentity.Value = punchoutsql.ToIdentity;


                SqlParameter senderDomain = storedProc.Parameters.Add("@senderDomain", SqlDbType.NVarChar, 100);
                senderDomain.Direction = ParameterDirection.Input;
                senderDomain.Value = punchoutsql.SenderDomain;

                SqlParameter senderIdentity = storedProc.Parameters.Add("@senderIdentity", SqlDbType.NVarChar, 100);
                senderIdentity.Direction = ParameterDirection.Input;
                senderIdentity.Value = punchoutsql.SenderIdentity;

                SqlParameter senderUserAgent = storedProc.Parameters.Add("@senderUserAgent", SqlDbType.NVarChar, 300);
                senderUserAgent.Direction = ParameterDirection.Input;
                senderUserAgent.Value = punchoutsql.SenderUserAgent;

                SqlParameter buyerCookie = storedProc.Parameters.Add("@buyerCookie", SqlDbType.NVarChar, 100);
                buyerCookie.Direction = ParameterDirection.Input;
                buyerCookie.Value = punchoutsql.BuyerCookie;

                SqlParameter extrinsic = storedProc.Parameters.Add("@extrinsic", SqlDbType.NVarChar, 8000);
                extrinsic.Direction = ParameterDirection.Input;
                extrinsic.Value = punchoutsql.Extrinsic;

                SqlParameter token = storedProc.Parameters.Add("@token", SqlDbType.NVarChar, 100);
                token.Direction = ParameterDirection.Input;
                token.Value = punchoutsql.Token;

                SqlParameter browserFormPost = storedProc.Parameters.Add("@browserFormPost", SqlDbType.NVarChar, 300);
                browserFormPost.Direction = ParameterDirection.Input;
                browserFormPost.Value = punchoutsql.BrowserFormPost;

                DBConnection.Open();

                var records = storedProc.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
            finally
            {
               
                DBConnection.Close();
            }

        }


    }
}