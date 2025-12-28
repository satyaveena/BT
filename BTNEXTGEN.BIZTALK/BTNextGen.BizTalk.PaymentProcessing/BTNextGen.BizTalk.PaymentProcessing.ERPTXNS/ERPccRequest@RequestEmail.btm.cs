namespace BTNextGen.BizTalk.PaymentProcessing {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.BizTalk.PaymentProcessing.Schemas.ERPccRequest", typeof(global::BTNextGen.BizTalk.PaymentProcessing.Schemas.ERPccRequest))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.BizTalk.PaymentProcessing.Schemas.ERPccRequest", typeof(global::BTNextGen.BizTalk.PaymentProcessing.Schemas.ERPccRequest))]
    public sealed class ERPccRequest_RequestEmail : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var userCSharp"" version=""1.0"" xmlns:ns0=""BTNextGen.BizTalk.PaymentProcessing.ERPTXNs.ERPccRequest"" xmlns:userCSharp=""http://schemas.microsoft.com/BizTalk/2003/userCSharp"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/ns0:ERPccRequestRoot"" />
  </xsl:template>
  <xsl:template match=""/ns0:ERPccRequestRoot"">
    <xsl:variable name=""var:v1"" select=""userCSharp:StringUpperCase(&quot;DUMMY&quot;)"" />
    <ns0:ERPccRequestRoot>
      <notificationEmail>
        <xsl:value-of select=""$var:v1"" />
      </notificationEmail>
      <xsl:copy-of select=""./@*"" />
      <xsl:copy-of select=""./*"" />
    </ns0:ERPccRequestRoot>
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
        
        private const string _strSrcSchemasList0 = @"BTNextGen.BizTalk.PaymentProcessing.Schemas.ERPccRequest";
        
        private const string _strTrgSchemasList0 = @"BTNextGen.BizTalk.PaymentProcessing.Schemas.ERPccRequest";
        
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
                _SrcSchemas[0] = @"BTNextGen.BizTalk.PaymentProcessing.Schemas.ERPccRequest";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"BTNextGen.BizTalk.PaymentProcessing.Schemas.ERPccRequest";
                return _TrgSchemas;
            }
        }
    }
}
