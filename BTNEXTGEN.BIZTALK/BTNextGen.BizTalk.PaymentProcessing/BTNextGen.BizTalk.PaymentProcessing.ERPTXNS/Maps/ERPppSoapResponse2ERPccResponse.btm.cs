namespace BTNextGen.BizTalk.PaymentProcessing.Maps {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.BizTalk.PaymentProcessing.PaymentProcessing_edi_btol_com+CallCyberSourceResponse", typeof(global::BTNextGen.BizTalk.PaymentProcessing.PaymentProcessing_edi_btol_com.CallCyberSourceResponse))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.BizTalk.PaymentProcessing.Schemas.ERPccResponse", typeof(global::BTNextGen.BizTalk.PaymentProcessing.Schemas.ERPccResponse))]
    public sealed class ERPppSoapResponse2ERPccResponse : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s1 s0"" version=""1.0"" xmlns:ns0=""BTNextGen.BizTalk.PaymentProcessing.ERPTXNs.ERPccResponse"" xmlns:s1=""http://edi.btol.com/"" xmlns:s0=""BTNextGen.BizTalk.PaymentProcessing.ERPTXNs.ERPccRequest"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s1:CallCyberSourceResponse"" />
  </xsl:template>
  <xsl:template match=""/s1:CallCyberSourceResponse"">
    <ns0:ERPccResponseRoot>
      <xsl:copy-of select=""ns0:CallCyberSourceResult/@*"" />
      <xsl:copy-of select=""ns0:CallCyberSourceResult/*"" />
    </ns0:ERPccResponseRoot>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"BTNextGen.BizTalk.PaymentProcessing.PaymentProcessing_edi_btol_com+CallCyberSourceResponse";
        
        private const global::BTNextGen.BizTalk.PaymentProcessing.PaymentProcessing_edi_btol_com.CallCyberSourceResponse _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"BTNextGen.BizTalk.PaymentProcessing.Schemas.ERPccResponse";
        
        private const global::BTNextGen.BizTalk.PaymentProcessing.Schemas.ERPccResponse _trgSchemaTypeReference0 = null;
        
        public override string XmlContent {
            get {
                return _strMap;
            }
        }
        
        public override string XsltArgumentListContent {
            get {
                return _strArgList;
            }
        }
        
        public override string[] SourceSchemas {
            get {
                string[] _SrcSchemas = new string [1];
                _SrcSchemas[0] = @"BTNextGen.BizTalk.PaymentProcessing.PaymentProcessing_edi_btol_com+CallCyberSourceResponse";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"BTNextGen.BizTalk.PaymentProcessing.Schemas.ERPccResponse";
                return _TrgSchemas;
            }
        }
    }
}
