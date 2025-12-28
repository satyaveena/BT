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
    [System.Runtime.InteropServices.Guid("1050D88A-565E-432B-B8BC-E5548BB97616")]
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
        private string _NameSpace;
        public string NameSpace
        {
            get { return _NameSpace; }
            set { _NameSpace = value; }
        }

        private string _RootNode;
        public string RootNode
        {
            get { return _RootNode; }
            set { _RootNode = value; }
        }


        public void GetClassID(out Guid classID)
        {
            classID = new Guid("1050D88A-565E-432B-B8BC-E5548BB97616");
        }

        public void InitNew()
        {
        }

        public void Load(IPropertyBag propertyBag, int errorLog)
        {
            object valNamespace = null;
            object valRoot = null;
            try
            {
                propertyBag.Read("NameSpace", out valNamespace, 0);
                propertyBag.Read("RootNode", out valRoot, 0);
                               
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error reading propertybag: " + ex.Message);
            }

            if (valNamespace != null)
            {
                _NameSpace = (string)valNamespace;

            }
            if (valRoot != null)
            {
                _RootNode = (string)valRoot;

            }


        }


        public void Save(IPropertyBag propertyBag, bool clearDirty, bool saveAllProperties)
        {
            object valNamespace = (object)_NameSpace;
            propertyBag.Write("NameSpace", ref valNamespace);
            object valRoot = (object)_RootNode;
            propertyBag.Write("RootNode", ref valRoot);  
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
                msgtype = this.NameSpace+ "#" + this.RootNode;
              //  msgtype = this._NameSpace + "#" + this._RootNode;
                pInMsg.Context.Promote("MessageType", systemPropertiesNamespace, msgtype);
                return pInMsg;
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
           

        }

        #endregion


    }
}

