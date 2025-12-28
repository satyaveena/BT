namespace CartMigration {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"CartMigration.MessageCartsFromSQLType", typeof(global::CartMigration.MessageCartsFromSQLType))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"CartMigration.CartMigrationErrorProcedure_dbo+procSetBasketError", typeof(global::CartMigration.CartMigrationErrorProcedure_dbo.procSetBasketError))]
    public sealed class Transform_2 : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var"" version=""1.0"" xmlns:ns3=""http://schemas.datacontract.org/2004/07/System.Data"" xmlns:ns0=""http://schemas.microsoft.com/Sql/2008/05/Procedures/dbo"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/CartMigrationErrorListing"" />
  </xsl:template>
  <xsl:template match=""/CartMigrationErrorListing"">
    <ns0:procSetBasketError>
      <ns0:pstrHdrIDS>
        <xsl:value-of select=""BasketID/text()"" />
      </ns0:pstrHdrIDS>
      <ns0:pstrLoadStatus>
        <xsl:value-of select=""Status/text()"" />
      </ns0:pstrLoadStatus>
      <ns0:pstrSourceSystem>
        <xsl:value-of select=""SourceSytem/text()"" />
      </ns0:pstrSourceSystem>
      <ns0:pstrErrorMessage>
        <xsl:value-of select=""ErrorMessage/text()"" />
      </ns0:pstrErrorMessage>
    </ns0:procSetBasketError>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"CartMigration.MessageCartsFromSQLType";
        
        private const string _strTrgSchemasList0 = @"CartMigration.CartMigrationErrorProcedure_dbo+procSetBasketError";
        
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
                _SrcSchemas[0] = @"CartMigration.MessageCartsFromSQLType";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"CartMigration.CartMigrationErrorProcedure_dbo+procSetBasketError";
                return _TrgSchemas;
            }
        }
    }
}
