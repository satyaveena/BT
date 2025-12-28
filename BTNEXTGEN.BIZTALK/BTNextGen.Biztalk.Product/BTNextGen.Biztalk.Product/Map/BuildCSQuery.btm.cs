namespace BTNextGen.Biztalk.Product.Map {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.Biztalk.Product.Schema.BTInitiator", typeof(global::BTNextGen.Biztalk.Product.Schema.BTInitiator))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.Biztalk.Product.RepoCSGetProductsTypedProcedure_dbo+procCommerceServerGetNextGenProducts", typeof(global::BTNextGen.Biztalk.Product.RepoCSGetProductsTypedProcedure_dbo.procCommerceServerGetNextGenProducts))]
    public sealed class BuildCSQuery : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0 userCSharp"" version=""1.0"" xmlns:ns3=""http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procCommerceServerGetNextGenProducts"" xmlns:s0=""http://BTNextGen.BizTalk.Product.Schemas.BTInitiator"" xmlns:ns0=""http://schemas.microsoft.com/Sql/2008/05/TypedProcedures/dbo"" xmlns:userCSharp=""http://schemas.microsoft.com/BizTalk/2003/userCSharp"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s0:Action"" />
  </xsl:template>
  <xsl:template match=""/s0:Action"">
    <xsl:variable name=""var:v1"" select=""userCSharp:StringConcat(&quot;50&quot;)"" />
    <ns0:procCommerceServerGetNextGenProducts>
      <ns0:CatalogName>
        <xsl:value-of select=""CatalogName/text()"" />
      </ns0:CatalogName>
      <ns0:RowCount>
        <xsl:value-of select=""$var:v1"" />
      </ns0:RowCount>
    </ns0:procCommerceServerGetNextGenProducts>
  </xsl:template>
  <msxsl:script language=""C#"" implements-prefix=""userCSharp""><![CDATA[
public string StringConcat(string param0)
{
   return param0;
}



]]></msxsl:script>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"BTNextGen.Biztalk.Product.Schema.BTInitiator";
        
        private const global::BTNextGen.Biztalk.Product.Schema.BTInitiator _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"BTNextGen.Biztalk.Product.RepoCSGetProductsTypedProcedure_dbo+procCommerceServerGetNextGenProducts";
        
        private const global::BTNextGen.Biztalk.Product.RepoCSGetProductsTypedProcedure_dbo.procCommerceServerGetNextGenProducts _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"BTNextGen.Biztalk.Product.Schema.BTInitiator";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"BTNextGen.Biztalk.Product.RepoCSGetProductsTypedProcedure_dbo+procCommerceServerGetNextGenProducts";
                return _TrgSchemas;
            }
        }
    }
}
