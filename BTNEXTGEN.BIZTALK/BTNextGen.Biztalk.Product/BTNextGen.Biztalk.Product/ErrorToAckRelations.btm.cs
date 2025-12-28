namespace BTNextGen.Biztalk.Product {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.BizTalk.CS.Catalogue.Schemas.CatalogResponseMessage", typeof(global::BTNextGen.BizTalk.CS.Catalogue.Schemas.CatalogResponseMessage))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.Biztalk.Product.procGetRelationsTypedProcedure_dbo+procCSGetProductRelationshipsResponse", typeof(global::BTNextGen.Biztalk.Product.procGetRelationsTypedProcedure_dbo.procCSGetProductRelationshipsResponse))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.Biztalk.Product.TypedProcedure_dbo+procCSAckProductRelationships", typeof(global::BTNextGen.Biztalk.Product.TypedProcedure_dbo.procCSAckProductRelationships))]
    public sealed class ErrorToAckRelations : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s1 s0"" version=""1.0"" xmlns:ns0=""http://schemas.microsoft.com/Sql/2008/05/TypedProcedures/dbo"" xmlns:s0=""http://schemas.microsoft.com/BizTalk/2003/aggschema"" xmlns:s1=""http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procCSGetProductRelationships"" xmlns:ns3=""http://schemas.microsoft.com/Sql/2008/05/Types/TableTypes/dbo"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s0:Root"" />
  </xsl:template>
  <xsl:template match=""/s0:Root"">
    <ns0:procCSAckProductRelationships>
      <ns0:CSAckFamilyKeys>
        <xsl:for-each select=""InputMessagePart_1/ns0:procCSGetProductRelationshipsResponse/ns0:StoredProcedureResultSet0"">
          <xsl:for-each select=""s1:StoredProcedureResultSet0"">
            <ns3:udtCSAckProductRelationships>
              <xsl:if test=""s1:BTKEY"">
                <xsl:variable name=""var:v1"" select=""string(s1:BTKEY/@xsi:nil) = 'true'"" />
                <xsl:if test=""string($var:v1)='true'"">
                  <ns3:BTKey>
                    <xsl:attribute name=""xsi:nil"">
                      <xsl:value-of select=""'true'"" />
                    </xsl:attribute>
                  </ns3:BTKey>
                </xsl:if>
                <xsl:if test=""string($var:v1)='false'"">
                  <ns3:BTKey>
                    <xsl:value-of select=""s1:BTKEY/text()"" />
                  </ns3:BTKey>
                </xsl:if>
              </xsl:if>
              <xsl:if test=""../../../../InputMessagePart_0/CommerceServerCatalogImportResponse/CatalogImportError/@Message"">
                <ns3:CSLoadError>
                  <xsl:value-of select=""../../../../InputMessagePart_0/CommerceServerCatalogImportResponse/CatalogImportError/@Message"" />
                </ns3:CSLoadError>
              </xsl:if>
            </ns3:udtCSAckProductRelationships>
          </xsl:for-each>
        </xsl:for-each>
      </ns0:CSAckFamilyKeys>
    </ns0:procCSAckProductRelationships>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"BTNextGen.BizTalk.CS.Catalogue.Schemas.CatalogResponseMessage";
        
        private const global::BTNextGen.BizTalk.CS.Catalogue.Schemas.CatalogResponseMessage _srcSchemaTypeReference0 = null;
        
        private const string _strSrcSchemasList1 = @"BTNextGen.Biztalk.Product.procGetRelationsTypedProcedure_dbo+procCSGetProductRelationshipsResponse";
        
        private const global::BTNextGen.Biztalk.Product.procGetRelationsTypedProcedure_dbo.procCSGetProductRelationshipsResponse _srcSchemaTypeReference1 = null;
        
        private const string _strTrgSchemasList0 = @"BTNextGen.Biztalk.Product.TypedProcedure_dbo+procCSAckProductRelationships";
        
        private const global::BTNextGen.Biztalk.Product.TypedProcedure_dbo.procCSAckProductRelationships _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[1] = @"BTNextGen.Biztalk.Product.procGetRelationsTypedProcedure_dbo+procCSGetProductRelationshipsResponse";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"BTNextGen.Biztalk.Product.TypedProcedure_dbo+procCSAckProductRelationships";
                return _TrgSchemas;
            }
        }
    }
}
