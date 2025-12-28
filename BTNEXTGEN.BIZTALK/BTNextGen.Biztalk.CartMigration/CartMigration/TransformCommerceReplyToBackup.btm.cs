namespace CartMigration {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"CartMigration.IOrder_tempuri_org+CreateLegacyBasketsResponse", typeof(global::CartMigration.IOrder_tempuri_org.CreateLegacyBasketsResponse))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"CartMigration.CartMigrationErrorBackupSchema", typeof(global::CartMigration.CartMigrationErrorBackupSchema))]
    public sealed class TransformCommerceReplyToBackup : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0 s1"" version=""1.0"" xmlns:s0=""http://schemas.datacontract.org/2004/07/BTNextGen.Services.IOrders"" xmlns:s1=""http://tempuri.org/"" xmlns:ns0=""http://CartMigration.CartMigrationErrorBackupSchema"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s1:CreateLegacyBasketsResponse"" />
  </xsl:template>
  <xsl:template match=""/s1:CreateLegacyBasketsResponse"">
    <ns0:CartMigrationErrorResponses>
      <CartMigrationErrorResponse>
        <xsl:for-each select=""s1:CreateLegacyBasketsResult"">
          <xsl:for-each select=""s0:BasketDetailsResponse"">
            <CartMigrationErrorListing>
              <xsl:if test=""s0:ErrorMessage"">
                <ErrorMessage>
                  <xsl:value-of select=""s0:ErrorMessage/text()"" />
                </ErrorMessage>
              </xsl:if>
              <xsl:if test=""s0:LegacyBasketId"">
                <BasketID>
                  <xsl:value-of select=""s0:LegacyBasketId/text()"" />
                </BasketID>
              </xsl:if>
              <xsl:if test=""s0:LegacySourceSystem"">
                <SourceSytem>
                  <xsl:value-of select=""s0:LegacySourceSystem/text()"" />
                </SourceSytem>
              </xsl:if>
              <xsl:if test=""s0:LoadStatus"">
                <Status>
                  <xsl:value-of select=""s0:LoadStatus/text()"" />
                </Status>
              </xsl:if>
            </CartMigrationErrorListing>
          </xsl:for-each>
        </xsl:for-each>
      </CartMigrationErrorResponse>
    </ns0:CartMigrationErrorResponses>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"CartMigration.IOrder_tempuri_org+CreateLegacyBasketsResponse";
        
        private const string _strTrgSchemasList0 = @"CartMigration.CartMigrationErrorBackupSchema";
        
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
                _SrcSchemas[0] = @"CartMigration.IOrder_tempuri_org+CreateLegacyBasketsResponse";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"CartMigration.CartMigrationErrorBackupSchema";
                return _TrgSchemas;
            }
        }
    }
}
