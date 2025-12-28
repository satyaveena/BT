namespace BTNextGen.BizTalk.ERPOrders.Maps {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.BizTalk.ERPOrders.Schemas.ERPOrdentBulk", typeof(global::BTNextGen.BizTalk.ERPOrders.Schemas.ERPOrdentBulk))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.BizTalk.ERPOrders.Schemas.NGExceptionMessage", typeof(global::BTNextGen.BizTalk.ERPOrders.Schemas.NGExceptionMessage))]
    public sealed class ORDENTBulkToException : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0 userCSharp"" version=""1.0"" xmlns:s0=""BTNextGen.BizTalk.ERPOrders.Schemas.ERPOrdentBulk"" xmlns:ns0=""BTNextGen.BizTalk.ERPOrders.Schemas.NGExceptionMessage"" xmlns:userCSharp=""http://schemas.microsoft.com/BizTalk/2003/userCSharp"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s0:ORDENT"" />
  </xsl:template>
  <xsl:template match=""/s0:ORDENT"">
    <xsl:variable name=""var:v1"" select=""userCSharp:StringUpperCase(&quot;Successful ORDENT Bulk Translation&quot;)"" />
    <ns0:Exception>
      <ErrorDetail>
        <ErrDetail>
          <xsl:value-of select=""$var:v1"" />
        </ErrDetail>
        <InnerException>
          <xsl:value-of select=""$var:v1"" />
        </InnerException>
        <Message>
          <xsl:value-of select=""$var:v1"" />
        </Message>
      </ErrorDetail>
    </ns0:Exception>
  </xsl:template>
  <msxsl:script language=""C#"" implements-prefix=""userCSharp""><![CDATA[
public string StringUpperCase(string str)
{
	if (str == null)
	{
		return """";
	}
	return str.ToUpper(System.Globalization.CultureInfo.InvariantCulture);
}



]]></msxsl:script>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"BTNextGen.BizTalk.ERPOrders.Schemas.ERPOrdentBulk";
        
        private const global::BTNextGen.BizTalk.ERPOrders.Schemas.ERPOrdentBulk _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"BTNextGen.BizTalk.ERPOrders.Schemas.NGExceptionMessage";
        
        private const global::BTNextGen.BizTalk.ERPOrders.Schemas.NGExceptionMessage _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"BTNextGen.BizTalk.ERPOrders.Schemas.ERPOrdentBulk";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"BTNextGen.BizTalk.ERPOrders.Schemas.NGExceptionMessage";
                return _TrgSchemas;
            }
        }
    }
}
