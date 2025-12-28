using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.BizTalk.Message.Interop;
using Microsoft.BizTalk.Component.Interop;
using System.Xml;
using System.Text.RegularExpressions;

namespace BTNextGen.Biztalk.ERPAccounts.PipelineComponent
{
    [ComponentCategory(CategoryTypes.CATID_PipelineComponent)]
    [ComponentCategory(CategoryTypes.CATID_Decoder)]
    [System.Runtime.InteropServices.Guid("59E1A262-D8E6-46cf-B18B-0A8E1FB42CB0")]
    public class PromoteMessageType : IBaseComponent,
                                      IComponentUI,
                                      IComponent,
                                      IPersistPropertyBag
    {
        string systemPropertiesNamespace = @"http://schemas.microsoft.com/BizTalk/2003/system-properties";
        #region Constructors

        public PromoteMessageType()
        {

        }

        #endregion

        #region IBaseComponent Members

        public string Description
        {
            get { return "Promote MessageType"; }
        }

        public string Name
        {
            get { return "Promote MessageType"; }
        }

        public string Version
        {
            get { return "1.0.0.0"; }
        }

        #endregion

        #region IPersistPropertyBag Members


        public void GetClassID(out Guid classID)
        {
            classID = new Guid("59E1A262-D8E6-46cf-B18B-0A8E1FB42CB0");
        }

        public void InitNew()
        {
        }

        public void Load(IPropertyBag propertyBag, int errorLog)
        {
        }


        public void Save(IPropertyBag propertyBag, bool clearDirty, bool saveAllProperties)
        {

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
            try
            {

                string rootNode = String.Empty;
                string tgtNamespace = String.Empty;
                string msgtype = String.Empty;
                IBaseMessagePart msgPart = pInMsg.BodyPart;
                //fetch original message
                Stream originalMessageStream = pInMsg.BodyPart.GetOriginalDataStream();
                string interchgId = pInMsg.Context.Read("InterchangeID", systemPropertiesNamespace).ToString();
                XmlTextReader xtrdr = new XmlTextReader(originalMessageStream);
                while (xtrdr.Read())
                {
                    rootNode = xtrdr.Name;
                    tgtNamespace = xtrdr.NamespaceURI;
                    break;
                }

                msgtype = tgtNamespace + "#" + rootNode;
                originalMessageStream.Seek(0, SeekOrigin.Begin);
                msgPart.Data = originalMessageStream;
                pInMsg.BodyPart.Data = originalMessageStream;
                pInMsg.Context.Promote("MessageType", systemPropertiesNamespace, msgtype);
                xtrdr.Close();
                pInMsg.Context.Promote("InterchangeID", systemPropertiesNamespace, interchgId);


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return pInMsg;

        }

        #endregion


    }
}

