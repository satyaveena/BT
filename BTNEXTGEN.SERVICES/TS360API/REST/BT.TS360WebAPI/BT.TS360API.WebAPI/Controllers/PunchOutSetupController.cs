using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BT.TS360API.WebAPI.Models;
//using BT.TS360API.WebAPI.Classes.Punchout;
using System.IO;
using System.Web;
using System.Web.Services;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Linq;
using BT.TS360API.WebAPI.Common;
using BT.TS360API.WebAPI.Common.Configuration;
using BT.TS360API.WebAPI.Common.DataAccess;
using BT.TS360API.WebAPI.Services;
using BT.TS360API.Security;
using BT.TS360API.WebAPI.ServiceReferencePunchOut;
using System.Text.RegularExpressions;


namespace BT.TS360API.WebAPI.Controllers
{
    public class PunchOutSetupController : ApiController
    {
        /// <summary>
        ///Setup a Punchout Session
        /// </summary>
        public HttpResponseMessage PostRawXMLMessage(HttpRequestMessage request)
        {     

// initialize and assign majority of variables from web.config 
                string logRequestLog = AppSetting.PunchOutEnableTraceLogFile;
                string logRequestFile = AppSetting.PunchOutEnableTraceRequestFile;
                string logRequestFileDTD = AppSetting.PunchOutEnableTraceRequestFileDTD;
                string logResponseFile = AppSetting.PunchOutEnableTraceResponseFile;
                string logResponseFileDTD = AppSetting.PunchOutEnableTraceResponseFileDTD;
                string guid = Guid.NewGuid().ToString();
                string innerXML = null;
                string InvalidField = null; 
                string PUNCHOUT_REQFILEName = string.Format("{0}_{1}_{2}.xml", AppSetting.PunchOutREQFilePrefix, DateTime.Now.ToString("yyyy-MM-dd"), guid);
                string PUNCHOUT_REQFile = string.Format("{0}\\{1}", AppSetting.PunchOutREQFolder, PUNCHOUT_REQFILEName);
                string PUNCHOUT_REQDTDFILEName = string.Format("{0}_{1}_{2}.xml", AppSetting.PunchOutREQDTDFilePrefix, DateTime.Now.ToString("yyyy-MM-dd"), guid);
                string PUNCHOUT_REQNODTDFile = string.Format("{0}\\{1}", AppSetting.PunchOutREQDTDFolder, PUNCHOUT_REQDTDFILEName);
                string PUNCHOUT_RESPFILEName = string.Format("{0}_{1}_{2}.xml", AppSetting.PunchOutRESPFilePrefix, DateTime.Now.ToString("yyyy-MM-dd"), guid);
                string PUNCHOUT_RESPFile = string.Format("{0}\\{1}", AppSetting.PunchOutRESPFolder, PUNCHOUT_RESPFILEName);
                string PUNCHOUT_RESPDTDFILEName = string.Format("{0}_{1}_{2}.xml", AppSetting.PunchOutRESPDTDFilePrefix, DateTime.Now.ToString("yyyy-MM-dd"), guid);
                string PUNCHOUT_RESPNODTDFile = string.Format("{0}\\{1}", AppSetting.PunchOutRESPDTDFolder, PUNCHOUT_RESPDTDFILEName);
                string PUNCHOUT_DUNS = AppSetting.PunchOutDUNS;
                string PUNCHOUT_SENDER = AppSetting.PunchOutSender; 
                string PUNCHOUT_SHAREDKEY = AppSetting.PunchOutSharedSecret;
                string PUNCHOUT_PASSWORD = AppSetting.PunchOutPassword;
                string PUNCHOUT_ORGID = AppSetting.PunchOutOrgID; 

                
                FileLogRepository PUNCHOUT_LOGFILE = new FileLogRepository(AppSetting.PunchOutLogFolder, AppSetting.PunchOutLogFilePrefix);
                try
                {

                // Log entry into controller... 
                #region FileLogging
                if (logRequestLog == "ON")
                {
                    PUNCHOUT_LOGFILE.Write("Initial: " + guid, FileLogRepository.Level.INFO);
                }
                #endregion


// Create an instance of cxml class, load request to xml doc, save request to file. 
//HttpExceptstatus = "406"; 

                    cXML cxml = new cXML();
                    try
                    {
                      var xmlDoc = new XmlDocument();
                      xmlDoc.Load(request.Content.ReadAsStreamAsync().Result);

                      //Save original request 
                      #region FileLogging
                    if (logRequestLog == "ON")
                    {
                        PUNCHOUT_LOGFILE.Write("Saving request to file for GUID: " + guid, FileLogRepository.Level.INFO);
                    }
                    #endregion
                      xmlDoc.Save(PUNCHOUT_REQFile);



                      //Remove dtd from request
                      XmlDocumentType XDType = xmlDoc.DocumentType;
                      xmlDoc.RemoveChild(XDType);



                    //Saving request without dtd
                    #region FileLogging
                    if (logRequestLog == "ON")
                    {
                        PUNCHOUT_LOGFILE.Write("Saving request dtd to file for GUID: " + guid, FileLogRepository.Level.INFO);
                    }
                    #endregion
                    xmlDoc.Save(PUNCHOUT_REQNODTDFile);
                    }
                    catch (Exception ex)
                    {

                        PunchOutSetupRepository poBAD = new PunchOutSetupRepository();
                        bool isBADMessageCreate = poBAD.BadMessageCreate("406", "Not Acceptable: " + ex.Message, PUNCHOUT_RESPFile, PUNCHOUT_RESPNODTDFile, out innerXML);
                        return new HttpResponseMessage(HttpStatusCode.NotAcceptable)
                        {
                            Content = new StringContent(innerXML, System.Text.Encoding.UTF8, "application/xml")
                        };

                    }





  //Deserialize the dtd file into cxml class
                FileStream myFileStream = new FileStream(PUNCHOUT_REQNODTDFile, FileMode.Open);
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(cXML));
                cxml = (cXML)xmlSerializer.Deserialize(myFileStream);


  //Assign Extrinsic values
                string extrinsicString = "";
                string extrinsicUser = "";
                string extrinsicUserEmail = "";
                string extrinsicUserID = "";
                string extrinsicUserFullName = "";
                string extrinsicFirstName = "";
                string extrinsicLastName = "";

                if (cxml.Request.PunchOutSetupRequest.Extrinsic != null)
                {
                    foreach (cXMLRequestPunchOutSetupRequestExtrinsic myExtrinsic in cxml.Request.PunchOutSetupRequest.Extrinsic)
                    {
                        if (myExtrinsic.Value == null) { myExtrinsic.Value = ""; }
                        extrinsicString += string.Format("{0}{1}{2}{3}", myExtrinsic.name.ToString(), "=", myExtrinsic.Value.ToString(), "|");
                        
                        if (myExtrinsic.name == "User") { extrinsicUser = myExtrinsic.Value.ToString(); }
                        if (myExtrinsic.name == "UserEmail") { extrinsicUserEmail = myExtrinsic.Value.ToString(); }
                        if (myExtrinsic.name == "UserId") { extrinsicUserID = myExtrinsic.Value.ToString(); }
                        if (myExtrinsic.name == "UserFullName") { extrinsicUserFullName = myExtrinsic.Value.ToString(); }
                        if (myExtrinsic.name == "FirstName") { extrinsicFirstName = myExtrinsic.Value.ToString(); }
                        if (myExtrinsic.name == "LastName") { extrinsicLastName = myExtrinsic.Value.ToString(); }


                    }
                }

  //Validate that the duns id, shared network key are valid, useremail are valid
  //PunchOutSetupOut results = null;
  //results = new PunchOutSetupOut();
                if (PUNCHOUT_DUNS != cxml.Header.To.Credential.Identity.ToString())
                {
                    PunchOutSetupRepository poBAD = new PunchOutSetupRepository();
                    bool isBADMessageCreate = poBAD.BadMessageCreate("401", "Unauthorized: Invalid To Identity", PUNCHOUT_RESPFile, PUNCHOUT_RESPNODTDFile, out innerXML);
                    return new HttpResponseMessage(HttpStatusCode.Unauthorized)
                    {
                        Content = new StringContent(innerXML, System.Text.Encoding.UTF8, "application/xml")
                    };
                }


                if (PUNCHOUT_SENDER != cxml.Header.Sender.Credential.Identity.ToString())
                {
                    PunchOutSetupRepository poBAD = new PunchOutSetupRepository();
                    bool isBADMessageCreate = poBAD.BadMessageCreate("401", "Unauthorized: Invalid Sender Identity", PUNCHOUT_RESPFile, PUNCHOUT_RESPNODTDFile, out innerXML);
                    return new HttpResponseMessage(HttpStatusCode.Unauthorized)
                    {
                        Content = new StringContent(innerXML, System.Text.Encoding.UTF8, "application/xml")
                    };
                }

                if (PUNCHOUT_SHAREDKEY != cxml.Header.Sender.Credential.SharedSecret.ToString())
                {
                    PunchOutSetupRepository poBAD = new PunchOutSetupRepository();
                    bool isBADMessageCreate = poBAD.BadMessageCreate("401", "Unauthorized: Invalid Shared Secret", PUNCHOUT_RESPFile, PUNCHOUT_RESPNODTDFile, out innerXML);
                    return new HttpResponseMessage(HttpStatusCode.Unauthorized)
                    {
                        Content = new StringContent(innerXML, System.Text.Encoding.UTF8, "application/xml")
                    };
                }

                if (extrinsicUserEmail == null || extrinsicUserEmail == "")
                {
                    PunchOutSetupRepository poBAD = new PunchOutSetupRepository();
                    bool isBADMessageCreate = poBAD.BadMessageCreate("401", "Unauthorized: Missing UserEmail", PUNCHOUT_RESPFile, PUNCHOUT_RESPNODTDFile, out innerXML);
                    return new HttpResponseMessage(HttpStatusCode.Unauthorized)
                    {
                        Content = new StringContent(innerXML, System.Text.Encoding.UTF8, "application/xml")
                    };
                }

                //bool isValidEmailDomain = extrinsicUserEmail.EndsWith(".com");
                //bool isvalidEmail =  Regex.IsMatch(extrinsicUserEmail,
                //              @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                //              @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$",
                //              RegexOptions.IgnoreCase);
                
                //if (isvalidEmail == false || isValidEmailDomain == false)
               // {
               //     PunchOutSetupRepository poBAD = new PunchOutSetupRepository();
               //     bool isBADMessageCreate = poBAD.BadMessageCreate("401", "Unauthorized: Invalid UserEmail Format", PUNCHOUT_RESPFile, PUNCHOUT_RESPNODTDFile, out innerXML);
                //    return new HttpResponseMessage(HttpStatusCode.Unauthorized)
               //     {
               //         Content = new StringContent(innerXML, System.Text.Encoding.UTF8, "application/xml")
               //     };
               // }


  //Call user service to add user

                PunchOutServiceClient poSVC = new PunchOutServiceClient();
                try
                {
                    CreateUserResult poSVCresult = new CreateUserResult();
                    poSVCresult = poSVC.CreateNewUser(extrinsicUserEmail, PUNCHOUT_PASSWORD, PUNCHOUT_ORGID);
                    if (string.IsNullOrEmpty(poSVCresult.UserId))
                    {

                        if (poSVCresult.ErrorCode == "401")
                        {
                            PunchOutSetupRepository poBAD = new PunchOutSetupRepository();
                            bool isBADMessageCreate = poBAD.BadMessageCreate("401", "Unauthorized: User Disabled", PUNCHOUT_RESPFile, PUNCHOUT_RESPNODTDFile, out innerXML);
                            return new HttpResponseMessage(HttpStatusCode.Unauthorized)
                            {
                                Content = new StringContent(innerXML, System.Text.Encoding.UTF8, "application/xml")
                            };
                        }
                        else if (poSVCresult.ErrorCode == "500")
                        {

                            PunchOutSetupRepository poBAD = new PunchOutSetupRepository();
                            bool isBADMessageCreate = poBAD.BadMessageCreate("500", "InternalError: " + "Error Authenticating User1", PUNCHOUT_RESPFile, PUNCHOUT_RESPNODTDFile, out innerXML);
                            return new HttpResponseMessage(HttpStatusCode.InternalServerError)

                            {
                                Content = new StringContent(innerXML, System.Text.Encoding.UTF8, "application/xml")
                            };
                        }
                        else                         
                        {
                            PunchOutSetupRepository poBAD = new PunchOutSetupRepository();
                            bool isBADMessageCreate = poBAD.BadMessageCreate("500", "InternalError: " + "Error Authenticating User2", PUNCHOUT_RESPFile, PUNCHOUT_RESPNODTDFile, out innerXML);
                            return new HttpResponseMessage(HttpStatusCode.InternalServerError)

                            {
                                Content = new StringContent(innerXML, System.Text.Encoding.UTF8, "application/xml")
                            };
                        }

                    }
                }
                catch (Exception ex)
                {
                    PunchOutSetupRepository poBAD = new PunchOutSetupRepository();
                    bool isBADMessageCreate = poBAD.BadMessageCreate("500", "InternalError: " + ex.Message, PUNCHOUT_RESPFile, PUNCHOUT_RESPNODTDFile, out innerXML);
                    return new HttpResponseMessage(HttpStatusCode.InternalServerError)

                    {
                        Content = new StringContent(innerXML, System.Text.Encoding.UTF8, "application/xml")
                    };
                }
                finally
                {
                    poSVC.Close();
                }

                    
  //Call encrypt method to encrypt email and date time into token

                string token = extrinsicUserEmail + " | " + DateTime.Now.ToString();
                string tokenEncrypted = string.Empty;       // encrypted text
                string passPhrase = AppSetting.passPhrase;     // this is a shared secret/public key that will be provided to vendors
                string initVector = AppSetting.initVector; // must be 16 bytes


                RijndaelEnhanced rijndaelKey =
                    new RijndaelEnhanced(passPhrase, initVector);

                tokenEncrypted = rijndaelKey.Encrypt(token);

  //Assign token value to punchoutsql object
                PunchOutSetupSQL punchoutsql = new PunchOutSetupSQL();
                punchoutsql.RequestPayloadID = cxml.payloadID;
                punchoutsql.FromDomain = cxml.Header.From.Credential.domain;
                punchoutsql.FromIdentity = cxml.Header.From.Credential.Identity;
                punchoutsql.ToDomain = cxml.Header.To.Credential.domain;
                punchoutsql.ToIdentity = cxml.Header.To.Credential.Identity;
                punchoutsql.SenderDomain = cxml.Header.Sender.Credential.domain;
                punchoutsql.SenderIdentity = cxml.Header.Sender.Credential.Identity;
                punchoutsql.SenderUserAgent = cxml.Header.Sender.UserAgent;
                punchoutsql.LoginEmail = extrinsicUserEmail;
                punchoutsql.BuyerCookie = cxml.Request.PunchOutSetupRequest.BuyerCookie;
                punchoutsql.Token = tokenEncrypted;
                punchoutsql.Extrinsic = extrinsicString;
                punchoutsql.BrowserFormPost = cxml.Request.PunchOutSetupRequest.BrowserFormPost.URL; 


  //Validate all values passed to ensure no nulls
                PunchOutSetupRepository poValidate = new PunchOutSetupRepository();
                bool isValid = poValidate.ValidateFields(punchoutsql, out InvalidField); 
                 
                if (isValid == false)
                {

                    PunchOutSetupRepository poBAD = new PunchOutSetupRepository();
                    bool isBADMessageCreate = poBAD.BadMessageCreate("400", "BadRequest: Element " + InvalidField + " is empty", PUNCHOUT_RESPFile, PUNCHOUT_RESPNODTDFile, out innerXML);
                    return new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new StringContent(innerXML, System.Text.Encoding.UTF8, "application/xml")
                    };

                }

                 

  //Call sp to insert request and token

                PunchOutSetupRepository.InsertProcurementRequests(punchoutsql);

                PunchOutSetupRepository poOK = new PunchOutSetupRepository();
                bool isGoodMessageCreate = poOK.GoodMessageCreate(tokenEncrypted, PUNCHOUT_RESPFile, PUNCHOUT_RESPNODTDFile, out innerXML);


  //Cleanup files used
                if (logRequestFile != "ON" && File.Exists(PUNCHOUT_REQFile))
                {
                    File.Delete(PUNCHOUT_REQFile);
                }

                if (logRequestFileDTD != "ON" && File.Exists(PUNCHOUT_REQNODTDFile))
                {
                    File.Delete(PUNCHOUT_REQNODTDFile);
                }
                if (logResponseFile != "ON" && File.Exists(PUNCHOUT_RESPFile))
                {
                    File.Delete(PUNCHOUT_RESPFile);
                }

                if (logResponseFileDTD != "ON" && File.Exists(PUNCHOUT_RESPNODTDFile))
                {
                    File.Delete(PUNCHOUT_RESPNODTDFile);
                }



                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(innerXML, System.Text.Encoding.UTF8, "application/xml")
                };


            }
            catch (Exception ex)
            {
                //Log 2 DB, File and Email Exception... 

                System.Diagnostics.EventLog appLog = new System.Diagnostics.EventLog();
                appLog.Source = "TS360WebAPI";
                appLog.WriteEntry("Error in Punchout Method: " + ex.Message);

                EmailExceptions.SendEmailExceptionGeneric("Exception occurred: " + ex.Message, "Punchout v2", AppSetting.PunchOutEmailExceptionFlag.ToLower(),AppSetting.PunchOutEmailToExceptions, AppSetting.PunchOutEmailServer);

                PunchOutSetupRepository poBAD = new PunchOutSetupRepository();
                bool isBADMessageCreate = poBAD.BadMessageCreate("500", "InternalError: " + ex.Message, PUNCHOUT_RESPFile, PUNCHOUT_RESPNODTDFile, out innerXML);
                return new HttpResponseMessage(HttpStatusCode.InternalServerError)

                {
                    Content = new StringContent(innerXML, System.Text.Encoding.UTF8, "application/xml")
                };


            }

        }
    }
}
