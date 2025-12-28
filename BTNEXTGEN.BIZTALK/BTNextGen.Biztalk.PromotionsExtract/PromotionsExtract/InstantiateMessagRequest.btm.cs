namespace PromotionsExtract {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"PromotionsExtract.GetPromotions", typeof(global::PromotionsExtract.GetPromotions))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.Biztalk.PromotionsExtract.IPromotion_tempuri_org+GetPromotions", typeof(global::BTNextGen.Biztalk.PromotionsExtract.IPromotion_tempuri_org.GetPromotions))]
    public sealed class InstantiateMessagRequest : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0"" version=""1.0"" xmlns:ns0=""http://tempuri.org/"" xmlns:ns1=""http://schemas.datacontract.org/2004/07/BTNextGen.Services.IPromotions"" xmlns:s0=""http://PromotionsExtract.GetPromotions"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s0:GetPromotions"" />
  </xsl:template>
  <xsl:template match=""/s0:GetPromotions"">
    <ns0:GetPromotions>
      <xsl:value-of select=""GetPromotionsField/text()"" />
    </ns0:GetPromotions>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"PromotionsExtract.GetPromotions";
        
        private const string _strTrgSchemasList0 = @"BTNextGen.Biztalk.PromotionsExtract.IPromotion_tempuri_org+GetPromotions";
        
        private const global::BTNextGen.Biztalk.PromotionsExtract.IPromotion_tempuri_org.GetPromotions _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"PromotionsExtract.GetPromotions";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"BTNextGen.Biztalk.PromotionsExtract.IPromotion_tempuri_org+GetPromotions";
                return _TrgSchemas;
            }
        }
    }
}
