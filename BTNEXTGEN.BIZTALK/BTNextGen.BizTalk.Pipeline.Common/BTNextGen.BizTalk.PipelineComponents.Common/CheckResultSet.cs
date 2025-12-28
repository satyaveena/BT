using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.BizTalk.Message.Interop;
using Microsoft.BizTalk.Component.Interop;
using System.Xml;
using System.Text.RegularExpressions;

namespace BTNextGen.BizTalk.PipelineComponents.Common
{
    [ComponentCategory(CategoryTypes.CATID_PipelineComponent)]
    [ComponentCategory(CategoryTypes.CATID_Decoder)]
    [System.Runtime.InteropServices.Guid("513E5521-CD77-4ADD-9936-35564F1E1073")]
    public class CheckResultSet : IBaseComponent,
                                      IComponentUI,
                                      IComponent,
                                      IPersistPropertyBag
    {
        string systemPropertiesNamespace = @"http://schemas.microsoft.com/BizTalk/2003/system-properties";
        #region Constructors

        public CheckResultSet()
        {

        }

        #endregion

        #region IBaseComponent Members

        public string Description
        {
            get { return "Check Sql Resultset"; }
        }

        public string Name
        {
            get { return "Check Resultset"; }
        }

        public string Version
        {
            get { return "1.0.0.0"; }
        }

        #endregion

        #region IPersistPropertyBag Members
        //private string _NameSpace;
        //public string NameSpace
        //{
        //    get { return _NameSpace; }
        //    set { _NameSpace = value; }
        //}

        //private string _RootNode;
        //public string RootNode
        //{
        //    get { return _RootNode; }
        //    set { _RootNode = value; }
        //}


        public void GetClassID(out Guid classID)
        {
            classID = new Guid("513E5521-CD77-4ADD-9936-35564F1E1073");
        }

        public void InitNew()
        {
        }

        public void Load(IPropertyBag propertyBag, int errorLog)
        {
            //object valNamespace = null;
            //object valRoot = null;
            //try
            //{
            //    propertyBag.Read("NameSpace", out valNamespace, 0);
            //    propertyBag.Read("RootNode", out valRoot, 0);

            //}
            //catch (Exception ex)
            //{
            //    throw new ApplicationException("Error reading propertybag: " + ex.Message);
            //}

            //if (valNamespace != null)
            //{
            //    _NameSpace = (string)valNamespace;

            //}
            //if (valRoot != null)
            //{
            //    _RootNode = (string)valRoot;

            //}


        }


        public void Save(IPropertyBag propertyBag, bool clearDirty, bool saveAllProperties)
        {
            //object valNamespace = (object)_NameSpace;
            //propertyBag.Write("NameSpace", ref valNamespace);
            //object valRoot = (object)_RootNode;
            //propertyBag.Write("RootNode", ref valRoot);
        }

        #endregion

        #region IComponentUI Members

        public IntPtr Icon
        {
            get { return System.IntPtr.Zero; }
        }

        public System.Collections.IEnumerator Validate(object projectSystem)
        {
            return null;
        }

        #endregion

        #region IComponent

        public IBaseMessage Execute(IPipelineContext pContext, IBaseMessage pInMsg)
        {
            string RcvPort = string.Empty;
            string MsgType = String.Empty;
            try
            {

                IBaseMessagePart msgPart = pInMsg.BodyPart;
                RcvPort = pInMsg.Context.Read("ReceivePortName", systemPropertiesNamespace).ToString();
                //fetch original message
                Stream originalMessageStream = pInMsg.BodyPart.GetOriginalDataStream();
                XmlTextReader xtrdr = new XmlTextReader(originalMessageStream);

                while (xtrdr.Read())
                {
                    if (xtrdr.LocalName.Equals("StoredProcedureResultSet0"))
                    {

                        if (RcvPort.Equals("GetUsersProfilesFromLegacy"))
                        {
                            MsgType = "http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procGetUser#StoredProcedureResultSet0";
                           //pInMsg.Context.Promote("MessageType", systemPropertiesNamespace, "http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procGetUser#StoredProcedureResultSet0");

                        }


                        if (RcvPort.Equals("GetAccountProfilesFromLegacy"))
                        {

                            //pInMsg.Context.Promote("MessageType", systemPropertiesNamespace, "http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procGetBTAccount#StoredProcedureResultSet0");
                            MsgType = "http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procGetBTAccount#StoredProcedureResultSet0";

                        }


                        originalMessageStream.Seek(0, SeekOrigin.Begin);
                        msgPart.Data = originalMessageStream;
                        IBaseMessage outMsg = pInMsg;
                        outMsg.BodyPart.Data = originalMessageStream;
                        outMsg.Context.Promote("MessageType", systemPropertiesNamespace, MsgType);
                        xtrdr.Close();
                        return outMsg;
                                               
                    }


                }
                System.Diagnostics.EventLog.WriteEntry("No Message in RcvPort " + RcvPort, "No/Incorrect Message in RcvPort " + RcvPort, System.Diagnostics.EventLogEntryType.Information);
                return null;


            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        #endregion


    }
}

