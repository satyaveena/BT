namespace CartMigration {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"CartMigration.MessageCartsFromSQLType", typeof(global::CartMigration.MessageCartsFromSQLType))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"CartMigration.MessageCartsFromSQLType", typeof(global::CartMigration.MessageCartsFromSQLType))]
    public sealed class Transform_EmailFailuresToCS : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var"" version=""1.0"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/CartMigrationErrorListing"" />
  </xsl:template>
  <xsl:template match=""/CartMigrationErrorListing"">
    <CartMigrationErrorListing>
      <ErrorMessage>
        <xsl:value-of select=""ErrorMessage/text()"" />
      </ErrorMessage>
      <BasketID>
        <xsl:value-of select=""BasketID/text()"" />
      </BasketID>
      <SourceSytem>
        <xsl:value-of select=""SourceSytem/text()"" />
      </SourceSytem>
      <Status>
        <xsl:value-of select=""Status/text()"" />
      </Status>
    </CartMigrationErrorListing>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"CartMigration.MessageCartsFromSQLType";
        
        private const string _strTrgSchemasList0 = @"CartMigration.MessageCartsFromSQLType";
        
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
                _TrgSchemas[0] = @"CartMigration.MessageCartsFromSQLType";
                return _TrgSchemas;
            }
        }
    }
}
