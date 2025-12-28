namespace BTNextGen.Biztalk.ActiveCreditCards.Maps {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.Biztalk.ActiveCreditCards.Schemas.InitCreditCard", typeof(global::BTNextGen.Biztalk.ActiveCreditCards.Schemas.InitCreditCard))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.Biztalk.ActiveCreditCards.Schemas.IProfile_tempuri_org_ActiveCreditCards+GetCreditCardDetails", typeof(global::BTNextGen.Biztalk.ActiveCreditCards.Schemas.IProfile_tempuri_org_ActiveCreditCards.GetCreditCardDetails))]
    public sealed class Init_To_CardsReq : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0"" version=""1.0"" xmlns:ns0=""http://tempuri.org/"" xmlns:s0=""http://BTNextGen.Biztalk.ActiveCreditCards.InitCreditCard"" xmlns:ns1=""http://schemas.datacontract.org/2004/07/IProfiles"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s0:InitCreditCards"" />
  </xsl:template>
  <xsl:template match=""/s0:InitCreditCards"">
    <ns0:GetCreditCardDetails>
      <ns0:NoOfCards>
        <xsl:value-of select=""TopRow/text()"" />
      </ns0:NoOfCards>
    </ns0:GetCreditCardDetails>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"BTNextGen.Biztalk.ActiveCreditCards.Schemas.InitCreditCard";
        
        private const global::BTNextGen.Biztalk.ActiveCreditCards.Schemas.InitCreditCard _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"BTNextGen.Biztalk.ActiveCreditCards.Schemas.IProfile_tempuri_org_ActiveCreditCards+GetCreditCardDetails";
        
        private const global::BTNextGen.Biztalk.ActiveCreditCards.Schemas.IProfile_tempuri_org_ActiveCreditCards.GetCreditCardDetails _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"BTNextGen.Biztalk.ActiveCreditCards.Schemas.InitCreditCard";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"BTNextGen.Biztalk.ActiveCreditCards.Schemas.IProfile_tempuri_org_ActiveCreditCards+GetCreditCardDetails";
                return _TrgSchemas;
            }
        }
    }
}
