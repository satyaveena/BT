namespace PromotionsExtract {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.Biztalk.PromotionsExtract.IPromotion_tempuri_org+GetPromotionsResponse", typeof(global::BTNextGen.Biztalk.PromotionsExtract.IPromotion_tempuri_org.GetPromotionsResponse))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"PromotionsExtract.PromotionsExtractFFSchema", typeof(global::PromotionsExtract.PromotionsExtractFFSchema))]
    public sealed class TransformSvcResponsetoERP : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0 s1"" version=""1.0"" xmlns:s1=""http://tempuri.org/"" xmlns:s0=""http://schemas.datacontract.org/2004/07/BTNextGen.Services.IPromotions"" xmlns:ns0=""http://PromotionsExtract.PromotionsExtractFFSchema"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s1:GetPromotionsResponse"" />
  </xsl:template>
  <xsl:template match=""/s1:GetPromotionsResponse"">
    <ns0:PromotionsExtractRecord>
      <xsl:for-each select=""s1:GetPromotionsResult"">
        <xsl:for-each select=""s0:Promotion"">
          <PromotionsExtractRecord_Child1>
            <xsl:if test=""s0:PromoStartDate"">
              <Startdate>
                <xsl:value-of select=""s0:PromoStartDate/text()"" />
              </Startdate>
            </xsl:if>
            <xsl:if test=""s0:PromoEndDate"">
              <Enddate>
                <xsl:value-of select=""s0:PromoEndDate/text()"" />
              </Enddate>
            </xsl:if>
            <xsl:if test=""s0:PromoName"">
              <Promocode>
                <xsl:value-of select=""s0:PromoName/text()"" />
              </Promocode>
            </xsl:if>
            <xsl:if test=""s0:PromoType"">
              <Promotype>
                <xsl:value-of select=""s0:PromoType/text()"" />
              </Promotype>
            </xsl:if>
            <xsl:if test=""s0:PromoDescription"">
              <Promodescription>
                <xsl:value-of select=""s0:PromoDescription/text()"" />
              </Promodescription>
            </xsl:if>
          </PromotionsExtractRecord_Child1>
        </xsl:for-each>
      </xsl:for-each>
    </ns0:PromotionsExtractRecord>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"BTNextGen.Biztalk.PromotionsExtract.IPromotion_tempuri_org+GetPromotionsResponse";
        
        private const global::BTNextGen.Biztalk.PromotionsExtract.IPromotion_tempuri_org.GetPromotionsResponse _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"PromotionsExtract.PromotionsExtractFFSchema";
        
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
                _SrcSchemas[0] = @"BTNextGen.Biztalk.PromotionsExtract.IPromotion_tempuri_org+GetPromotionsResponse";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"PromotionsExtract.PromotionsExtractFFSchema";
                return _TrgSchemas;
            }
        }
    }
}
