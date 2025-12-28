namespace BTNextGen.Biztalk.ExpiredCreditCards {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.Biztalk.ExpiredCreditCards.IProfile_tempuri_org+GetExpiredCardsResponse", typeof(global::BTNextGen.Biztalk.ExpiredCreditCards.IProfile_tempuri_org.GetExpiredCardsResponse))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.Biztalk.ExpiredCreditCards.ExpiredCreditCardSchemaX", typeof(global::BTNextGen.Biztalk.ExpiredCreditCards.ExpiredCreditCardSchemaX))]
    public sealed class Transform_Adjust : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0 s1"" version=""1.0"" xmlns:s0=""http://schemas.datacontract.org/2004/07/IProfiles"" xmlns:ns0=""http://BTNextGen.Biztalk.ExpiredCreditCards.ExpiredCreditCardSchema"" xmlns:s1=""http://tempuri.org/"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s1:GetExpiredCardsResponse"" />
  </xsl:template>
  <xsl:template match=""/s1:GetExpiredCardsResponse"">
    <ns0:ExpiredCreditCardsList>
      <CreditCardsDoc>
        <xsl:for-each select=""s1:GetExpiredCardsResult"">
          <xsl:for-each select=""s0:ExpiredCreditCards"">
            <CreditCardData>
              <xsl:if test=""s0:card_id"">
                <id>
                  <xsl:value-of select=""s0:card_id/text()"" />
                </id>
              </xsl:if>
              <xsl:if test=""s0:last_4_digits"">
                <last4digits>
                  <xsl:value-of select=""s0:last_4_digits/text()"" />
                </last4digits>
              </xsl:if>
              <xsl:if test=""s0:expiration_month"">
                <expirationmonth>
                  <xsl:value-of select=""s0:expiration_month/text()"" />
                </expirationmonth>
              </xsl:if>
              <xsl:if test=""s0:expiration_year"">
                <expirationyear>
                  <xsl:value-of select=""s0:expiration_year/text()"" />
                </expirationyear>
              </xsl:if>
              <xsl:if test=""s0:contact_user"">
                <cardcontactuser>
                  <xsl:value-of select=""s0:contact_user/text()"" />
                </cardcontactuser>
              </xsl:if>
              <xsl:if test=""s0:alias_name"">
                <alias>
                  <xsl:value-of select=""s0:alias_name/text()"" />
                </alias>
              </xsl:if>
              <xsl:if test=""s0:first_name"">
                <firstname>
                  <xsl:value-of select=""s0:first_name/text()"" />
                </firstname>
              </xsl:if>
              <xsl:if test=""s0:last_name"">
                <lastname>
                  <xsl:value-of select=""s0:last_name/text()"" />
                </lastname>
              </xsl:if>
              <xsl:if test=""s0:email_address"">
                <emailaddress>
                  <xsl:value-of select=""s0:email_address/text()"" />
                </emailaddress>
              </xsl:if>
            </CreditCardData>
          </xsl:for-each>
        </xsl:for-each>
      </CreditCardsDoc>
    </ns0:ExpiredCreditCardsList>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"BTNextGen.Biztalk.ExpiredCreditCards.IProfile_tempuri_org+GetExpiredCardsResponse";
        
        private const string _strTrgSchemasList0 = @"BTNextGen.Biztalk.ExpiredCreditCards.ExpiredCreditCardSchemaX";
        
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
                _SrcSchemas[0] = @"BTNextGen.Biztalk.ExpiredCreditCards.IProfile_tempuri_org+GetExpiredCardsResponse";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"BTNextGen.Biztalk.ExpiredCreditCards.ExpiredCreditCardSchemaX";
                return _TrgSchemas;
            }
        }
    }
}
