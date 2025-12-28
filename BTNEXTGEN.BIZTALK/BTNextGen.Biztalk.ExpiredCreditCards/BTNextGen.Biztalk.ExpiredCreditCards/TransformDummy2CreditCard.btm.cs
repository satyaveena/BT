namespace BTNextGen.Biztalk.ExpiredCreditCards {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.Biztalk.ExpiredCreditCards.DummyQueryCreditCard", typeof(global::BTNextGen.Biztalk.ExpiredCreditCards.DummyQueryCreditCard))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.BizTalk.CS.Profiles.Schemas.ProfilesQuery", typeof(global::BTNextGen.BizTalk.CS.Profiles.Schemas.ProfilesQuery))]
    public sealed class TransformDummy2CreditCard : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0"" version=""1.0"" xmlns:ns0=""http://schemas.microsoft.com/CommerceServer/2004/02/Expressions"" xmlns:s0=""http://BTNextGen.Biztalk.ExpiredCreditCards.DummyQueryCreditCard"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s0:DummyQueryCreditCard"" />
  </xsl:template>
  <xsl:template match=""/s0:DummyQueryCreditCard"">
    <CommerceServerProfilesQuery>
      <ns0:CLAUSE>
        <xsl:attribute name=""OPER"">
          <xsl:text>equals</xsl:text>
        </xsl:attribute>
        <ns0:IMMED-VAL>
          <xsl:attribute name=""TYPE"">
            <xsl:text>String</xsl:text>
          </xsl:attribute>
          <ns0:VALUE>
            <xsl:value-of select=""DummyClause/DummyProperty/Value/text()"" />
          </ns0:VALUE>
        </ns0:IMMED-VAL>
      </ns0:CLAUSE>
    </CommerceServerProfilesQuery>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"BTNextGen.Biztalk.ExpiredCreditCards.DummyQueryCreditCard";
        
        private const string _strTrgSchemasList0 = @"BTNextGen.BizTalk.CS.Profiles.Schemas.ProfilesQuery";
        
        private const global::BTNextGen.BizTalk.CS.Profiles.Schemas.ProfilesQuery _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"BTNextGen.Biztalk.ExpiredCreditCards.DummyQueryCreditCard";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"BTNextGen.BizTalk.CS.Profiles.Schemas.ProfilesQuery";
                return _TrgSchemas;
            }
        }
    }
}
