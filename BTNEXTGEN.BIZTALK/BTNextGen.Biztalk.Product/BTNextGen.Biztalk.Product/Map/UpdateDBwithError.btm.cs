namespace BTNextGen.Biztalk.Product {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.BizTalk.CS.Catalogue.Schemas.CatalogResponseMessage", typeof(global::BTNextGen.BizTalk.CS.Catalogue.Schemas.CatalogResponseMessage))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.Biztalk.Product.Schema.procCSAckProductsTypedProcedure_dbo+procCSAckProducts", typeof(global::BTNextGen.Biztalk.Product.Schema.procCSAckProductsTypedProcedure_dbo.procCSAckProducts))]
    public sealed class UpdateDBwithError : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var"" version=""1.0"" xmlns:ns0=""http://schemas.microsoft.com/Sql/2008/05/TypedProcedures/dbo"" xmlns:ns3=""http://schemas.microsoft.com/Sql/2008/05/Types/TableTypes/dbo"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/CommerceServerCatalogImportResponse"" />
  </xsl:template>
  <xsl:template match=""/CommerceServerCatalogImportResponse"" />
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"BTNextGen.BizTalk.CS.Catalogue.Schemas.CatalogResponseMessage";
        
        private const global::BTNextGen.BizTalk.CS.Catalogue.Schemas.CatalogResponseMessage _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"BTNextGen.Biztalk.Product.Schema.procCSAckProductsTypedProcedure_dbo+procCSAckProducts";
        
        private const global::BTNextGen.Biztalk.Product.Schema.procCSAckProductsTypedProcedure_dbo.procCSAckProducts _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"BTNextGen.BizTalk.CS.Catalogue.Schemas.CatalogResponseMessage";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"BTNextGen.Biztalk.Product.Schema.procCSAckProductsTypedProcedure_dbo+procCSAckProducts";
                return _TrgSchemas;
            }
        }
    }
}
