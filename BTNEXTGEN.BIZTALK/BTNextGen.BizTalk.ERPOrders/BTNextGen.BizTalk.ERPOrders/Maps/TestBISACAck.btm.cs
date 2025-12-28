namespace BTNextGen.BizTalk.ERP.Maps {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.BizTalk.ERP.Schemas.POAckBTB", typeof(global::BTNextGen.BizTalk.ERP.Schemas.POAckBTB))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.BizTalk.ERP.Schemas.IntermediateXMLOrder", typeof(global::BTNextGen.BizTalk.ERP.Schemas.IntermediateXMLOrder))]
    public sealed class TestBISACAck : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0"" version=""1.0"" xmlns:s0=""BTNextGen.BizTalk.ERP.Schemas.POAckBTB"" xmlns:ns0=""BTNextGen.BizTalk.ERP.Schemas.IntermediateXMLOrder"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s0:POAckBTB"" />
  </xsl:template>
  <xsl:template match=""/s0:POAckBTB"" />
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"BTNextGen.BizTalk.ERP.Schemas.POAckBTB";
        
        private const global::BTNextGen.BizTalk.ERP.Schemas.POAckBTB _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"BTNextGen.BizTalk.ERP.Schemas.IntermediateXMLOrder";
        
        private const global::BTNextGen.BizTalk.ERP.Schemas.IntermediateXMLOrder _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"BTNextGen.BizTalk.ERP.Schemas.POAckBTB";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"BTNextGen.BizTalk.ERP.Schemas.IntermediateXMLOrder";
                return _TrgSchemas;
            }
        }
    }
}
