namespace CartMigration {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"CartMigration.IOrder_tempuri_org+CreateLegacyBasketsResponse", typeof(global::CartMigration.IOrder_tempuri_org.CreateLegacyBasketsResponse))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"CartMigration.CartMigrationErrorProcedure_dbo+procSetBasketError", typeof(global::CartMigration.CartMigrationErrorProcedure_dbo.procSetBasketError))]
    public sealed class TransformCommerceReplyToSQL : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0 s1"" version=""1.0"" xmlns:ns0=""http://schemas.microsoft.com/Sql/2008/05/Procedures/dbo"" xmlns:s1=""http://tempuri.org/"" xmlns:ns3=""http://schemas.datacontract.org/2004/07/System.Data"" xmlns:s0=""http://schemas.datacontract.org/2004/07/BTNextGen.Services.IOrders"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s1:CreateLegacyBasketsResponse"" />
  </xsl:template>
  <xsl:template match=""/s1:CreateLegacyBasketsResponse"">
    <ns0:procSetBasketError>
      <xsl:if test=""s1:CreateLegacyBasketsResult/s0:BasketDetailsResponse/s0:LegacyBasketId"">
        <xsl:variable name=""var:v1"" select=""string(s1:CreateLegacyBasketsResult/s0:BasketDetailsResponse/s0:LegacyBasketId/@xsi:nil) = 'true'"" />
        <xsl:if test=""string($var:v1)='true'"">
          <ns0:pstrHdrIDS>
            <xsl:attribute name=""xsi:nil"">
              <xsl:value-of select=""'true'"" />
            </xsl:attribute>
          </ns0:pstrHdrIDS>
        </xsl:if>
        <xsl:if test=""string($var:v1)='false'"">
          <ns0:pstrHdrIDS>
            <xsl:value-of select=""s1:CreateLegacyBasketsResult/s0:BasketDetailsResponse/s0:LegacyBasketId/text()"" />
          </ns0:pstrHdrIDS>
        </xsl:if>
      </xsl:if>
      <xsl:if test=""s1:CreateLegacyBasketsResult/s0:BasketDetailsResponse/s0:LoadStatus"">
        <xsl:variable name=""var:v2"" select=""string(s1:CreateLegacyBasketsResult/s0:BasketDetailsResponse/s0:LoadStatus/@xsi:nil) = 'true'"" />
        <xsl:if test=""string($var:v2)='true'"">
          <ns0:pstrLoadStatus>
            <xsl:attribute name=""xsi:nil"">
              <xsl:value-of select=""'true'"" />
            </xsl:attribute>
          </ns0:pstrLoadStatus>
        </xsl:if>
        <xsl:if test=""string($var:v2)='false'"">
          <ns0:pstrLoadStatus>
            <xsl:value-of select=""s1:CreateLegacyBasketsResult/s0:BasketDetailsResponse/s0:LoadStatus/text()"" />
          </ns0:pstrLoadStatus>
        </xsl:if>
      </xsl:if>
      <xsl:if test=""s1:CreateLegacyBasketsResult/s0:BasketDetailsResponse/s0:LegacySourceSystem"">
        <ns0:pstrSourceSystem>
          <xsl:value-of select=""s1:CreateLegacyBasketsResult/s0:BasketDetailsResponse/s0:LegacySourceSystem/text()"" />
        </ns0:pstrSourceSystem>
      </xsl:if>
      <xsl:if test=""s1:CreateLegacyBasketsResult/s0:BasketDetailsResponse/s0:ErrorMessage"">
        <ns0:pstrErrorMessage>
          <xsl:value-of select=""s1:CreateLegacyBasketsResult/s0:BasketDetailsResponse/s0:ErrorMessage/text()"" />
        </ns0:pstrErrorMessage>
      </xsl:if>
    </ns0:procSetBasketError>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"CartMigration.IOrder_tempuri_org+CreateLegacyBasketsResponse";
        
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
                _SrcSchemas[0] = @"CartMigration.IOrder_tempuri_org+CreateLegacyBasketsResponse";
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
