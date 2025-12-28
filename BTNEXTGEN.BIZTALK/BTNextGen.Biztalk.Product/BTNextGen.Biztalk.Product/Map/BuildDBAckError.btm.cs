namespace BTNextGen.Biztalk.Product {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.BizTalk.CS.Catalogue.Schemas.CatalogResponseMessage", typeof(global::BTNextGen.BizTalk.CS.Catalogue.Schemas.CatalogResponseMessage))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.Biztalk.Product.RepoCSGetProductsTypedProcedure_dbo+procCommerceServerGetNextGenProductsResponse", typeof(global::BTNextGen.Biztalk.Product.RepoCSGetProductsTypedProcedure_dbo.procCommerceServerGetNextGenProductsResponse))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.Biztalk.Product.RepoCSGetProductsRowFailureTypedProcedure_dbo+procCommerceServerSetNextGenProductsRowFailure", typeof(global::BTNextGen.Biztalk.Product.RepoCSGetProductsRowFailureTypedProcedure_dbo.procCommerceServerSetNextGenProductsRowFailure))]
    public sealed class BuildDBAckError : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s1 s0 userCSharp"" version=""1.0"" xmlns:ns0=""http://schemas.microsoft.com/Sql/2008/05/TypedProcedures/dbo"" xmlns:s0=""http://schemas.microsoft.com/BizTalk/2003/aggschema"" xmlns:s1=""http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procCommerceServerGetNextGenProducts"" xmlns:ns3=""http://schemas.microsoft.com/Sql/2008/05/Types/TableTypes/dbo"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:userCSharp=""http://schemas.microsoft.com/BizTalk/2003/userCSharp"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s0:Root"" />
  </xsl:template>
  <xsl:template match=""/s0:Root"">
    <ns0:procCommerceServerSetNextGenProductsRowFailure>
      <ns0:CSAckProducts>
        <xsl:for-each select=""InputMessagePart_0/CommerceServerCatalogImportResponse/CatalogImportError"">
          <xsl:variable name=""var:v2"" select=""userCSharp:StringConcat(string(@Message))"" />
          <ns3:udtCSAckProducts>
            <xsl:if test=""../../../InputMessagePart_1/ns0:procCommerceServerGetNextGenProductsResponse/ns0:StoredProcedureResultSet0/s1:StoredProcedureResultSet0/s1:BTKEY"">
              <xsl:variable name=""var:v1"" select=""string(../../../InputMessagePart_1/ns0:procCommerceServerGetNextGenProductsResponse/ns0:StoredProcedureResultSet0/s1:StoredProcedureResultSet0/s1:BTKEY/@xsi:nil) = 'true'"" />
              <xsl:if test=""string($var:v1)='true'"">
                <ns3:BTKey>
                  <xsl:attribute name=""xsi:nil"">
                    <xsl:value-of select=""'true'"" />
                  </xsl:attribute>
                </ns3:BTKey>
              </xsl:if>
              <xsl:if test=""string($var:v1)='false'"">
                <ns3:BTKey>
                  <xsl:value-of select=""../../../InputMessagePart_1/ns0:procCommerceServerGetNextGenProductsResponse/ns0:StoredProcedureResultSet0/s1:StoredProcedureResultSet0/s1:BTKEY/text()"" />
                </ns3:BTKey>
              </xsl:if>
            </xsl:if>
            <ns3:CSLoadError>
              <xsl:value-of select=""$var:v2"" />
            </ns3:CSLoadError>
            <xsl:if test=""../../../InputMessagePart_1/ns0:procCommerceServerGetNextGenProductsResponse/ns0:StoredProcedureResultSet0/s1:StoredProcedureResultSet0"">
              <xsl:value-of select=""../../../InputMessagePart_1/ns0:procCommerceServerGetNextGenProductsResponse/ns0:StoredProcedureResultSet0/s1:StoredProcedureResultSet0/text()"" />
            </xsl:if>
          </ns3:udtCSAckProducts>
        </xsl:for-each>
      </ns0:CSAckProducts>
      <xsl:variable name=""var:v3"" select=""string(InputMessagePart_1/ns0:procCommerceServerGetNextGenProductsResponse/ns0:StoredProcedureResultSet3/s1:StoredProcedureResultSet3/s1:BatchID/@xsi:nil) = 'true'"" />
      <xsl:if test=""string($var:v3)='true'"">
        <ns0:BatchID>
          <xsl:attribute name=""xsi:nil"">
            <xsl:value-of select=""'true'"" />
          </xsl:attribute>
        </ns0:BatchID>
      </xsl:if>
      <xsl:if test=""string($var:v3)='false'"">
        <ns0:BatchID>
          <xsl:value-of select=""InputMessagePart_1/ns0:procCommerceServerGetNextGenProductsResponse/ns0:StoredProcedureResultSet3/s1:StoredProcedureResultSet3/s1:BatchID/text()"" />
        </ns0:BatchID>
      </xsl:if>
    </ns0:procCommerceServerSetNextGenProductsRowFailure>
  </xsl:template>
  <msxsl:script language=""C#"" implements-prefix=""userCSharp""><![CDATA[
public string StringConcat(string param0)
{
   return param0;
}



]]></msxsl:script>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"BTNextGen.BizTalk.CS.Catalogue.Schemas.CatalogResponseMessage";
        
        private const global::BTNextGen.BizTalk.CS.Catalogue.Schemas.CatalogResponseMessage _srcSchemaTypeReference0 = null;
        
        private const string _strSrcSchemasList1 = @"BTNextGen.Biztalk.Product.RepoCSGetProductsTypedProcedure_dbo+procCommerceServerGetNextGenProductsResponse";
        
        private const global::BTNextGen.Biztalk.Product.RepoCSGetProductsTypedProcedure_dbo.procCommerceServerGetNextGenProductsResponse _srcSchemaTypeReference1 = null;
        
        private const string _strTrgSchemasList0 = @"BTNextGen.Biztalk.Product.RepoCSGetProductsRowFailureTypedProcedure_dbo+procCommerceServerSetNextGenProductsRowFailure";
        
        private const global::BTNextGen.Biztalk.Product.RepoCSGetProductsRowFailureTypedProcedure_dbo.procCommerceServerSetNextGenProductsRowFailure _trgSchemaTypeReference0 = null;
        
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
                string[] _SrcSchemas = new string [2];
                _SrcSchemas[0] = @"BTNextGen.BizTalk.CS.Catalogue.Schemas.CatalogResponseMessage";
                _SrcSchemas[1] = @"BTNextGen.Biztalk.Product.RepoCSGetProductsTypedProcedure_dbo+procCommerceServerGetNextGenProductsResponse";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"BTNextGen.Biztalk.Product.RepoCSGetProductsRowFailureTypedProcedure_dbo+procCommerceServerSetNextGenProductsRowFailure";
                return _TrgSchemas;
            }
        }
    }
}
