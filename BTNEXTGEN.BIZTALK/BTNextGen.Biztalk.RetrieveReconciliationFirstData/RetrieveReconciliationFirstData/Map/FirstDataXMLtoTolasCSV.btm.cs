namespace RetrieveReconciliationFirstData.Map {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"RetrieveReconciliationFirstData.Schemas.FirstDataXML", typeof(global::RetrieveReconciliationFirstData.Schemas.FirstDataXML))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"RetrieveReconciliationFirstData.Schemas.FirstDataTolasReport", typeof(global::RetrieveReconciliationFirstData.Schemas.FirstDataTolasReport))]
    public sealed class FirstDataXMLtoTolasCSV : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0"" version=""1.0"" xmlns:ns0=""http://RetrieveReconciliationFirstData.Schemas.FirstDataTolasReport"" xmlns:s0=""BTNextGen.Biztalk.ReconBAMS"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s0:FirstDataReconRoot"" />
  </xsl:template>
  <xsl:template match=""/s0:FirstDataReconRoot"">
    <ns0:FirstData>
      <xsl:for-each select=""FirstDataReconNode"">
        <TransactionRow>
          <CurrencyCode>
            <xsl:value-of select=""F4/text()"" />
          </CurrencyCode>
          <BatchNumber>
            <xsl:value-of select=""F6/text()"" />
          </BatchNumber>
          <InvoiceNumber>
            <xsl:value-of select=""F7/text()"" />
          </InvoiceNumber>
          <CardType>
            <xsl:value-of select=""F9/text()"" />
          </CardType>
          <CardholderNumber>
            <xsl:value-of select=""F10/text()"" />
          </CardholderNumber>
          <TransAmount>
            <xsl:value-of select=""F11/text()"" />
          </TransAmount>
          <TransDate>
            <xsl:value-of select=""F14/text()"" />
          </TransDate>
          <SubmitDate>
            <xsl:value-of select=""F8/text()"" />
          </SubmitDate>
          <AuthCode>
            <xsl:value-of select=""F15/text()"" />
          </AuthCode>
          <Status>
            <xsl:value-of select=""F16/text()"" />
          </Status>
        </TransactionRow>
      </xsl:for-each>
    </ns0:FirstData>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"RetrieveReconciliationFirstData.Schemas.FirstDataXML";
        
        private const global::RetrieveReconciliationFirstData.Schemas.FirstDataXML _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"RetrieveReconciliationFirstData.Schemas.FirstDataTolasReport";
        
        private const global::RetrieveReconciliationFirstData.Schemas.FirstDataTolasReport _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"RetrieveReconciliationFirstData.Schemas.FirstDataXML";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"RetrieveReconciliationFirstData.Schemas.FirstDataTolasReport";
                return _TrgSchemas;
            }
        }
    }
}
