namespace BTNextGen.BizTalk.ERPOrders.Maps {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.BizTalk.ERPOrders.Schemas.IntermediateXMLOrder", typeof(global::BTNextGen.BizTalk.ERPOrders.Schemas.IntermediateXMLOrder))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.BizTalk.ERPOrders.Schemas.OrderUpdate", typeof(global::BTNextGen.BizTalk.ERPOrders.Schemas.OrderUpdate))]
    public sealed class IXMLOrd2CSPOUpdate : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0 userCSharp"" version=""1.0"" xmlns:s0=""http://BTNextGen.BizTalk.Schemas.Common.IntermediateXMLOrder"" xmlns:userCSharp=""http://schemas.microsoft.com/BizTalk/2003/userCSharp"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s0:PO"" />
  </xsl:template>
  <xsl:template match=""/s0:PO"">
    <xsl:variable name=""var:v1"" select=""userCSharp:StringConcat(&quot;Status&quot;)"" />
    <xsl:variable name=""var:v2"" select=""userCSharp:StringConcat(&quot;Submitted&quot;)"" />
    <PurchaseOrderUpdates>
      <PurchaseOrderEx>
        <xsl:attribute name=""OrderGroupId"">
          <xsl:value-of select=""Header/Order/OrderGroupID/text()"" />
        </xsl:attribute>
        <xsl:attribute name=""PropertyName"">
          <xsl:value-of select=""$var:v1"" />
        </xsl:attribute>
        <xsl:attribute name=""PropertyValue"">
          <xsl:value-of select=""$var:v2"" />
        </xsl:attribute>
      </PurchaseOrderEx>
    </PurchaseOrderUpdates>
  </xsl:template>
  <msxsl:script language=""C#"" implements-prefix=""userCSharp""><![CDATA[
public string StringConcat(string param0)
{
   return param0;
}



]]></msxsl:script>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"BTNextGen.BizTalk.ERPOrders.Schemas.IntermediateXMLOrder";
        
        private const global::BTNextGen.BizTalk.ERPOrders.Schemas.IntermediateXMLOrder _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"BTNextGen.BizTalk.ERPOrders.Schemas.OrderUpdate";
        
        private const global::BTNextGen.BizTalk.ERPOrders.Schemas.OrderUpdate _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"BTNextGen.BizTalk.ERPOrders.Schemas.IntermediateXMLOrder";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"BTNextGen.BizTalk.ERPOrders.Schemas.OrderUpdate";
                return _TrgSchemas;
            }
        }
    }
}
