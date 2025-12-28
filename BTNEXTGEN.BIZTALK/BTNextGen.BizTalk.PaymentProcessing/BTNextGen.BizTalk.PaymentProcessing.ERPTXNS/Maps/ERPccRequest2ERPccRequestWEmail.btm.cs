namespace BTNextGen.BizTalk.PaymentProcessing.Maps {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.BizTalk.PaymentProcessing.Schemas.ERPccRequest", typeof(global::BTNextGen.BizTalk.PaymentProcessing.Schemas.ERPccRequest))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.BizTalk.PaymentProcessing.Schemas.ERPccRequest", typeof(global::BTNextGen.BizTalk.PaymentProcessing.Schemas.ERPccRequest))]
    public sealed class ERPccRequest2ERPccRequestWEmail : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var userCSharp"" version=""1.0"" xmlns:ns0=""BTNextGen.BizTalk.PaymentProcessing.ERPTXNs.ERPccRequest"" xmlns:userCSharp=""http://schemas.microsoft.com/BizTalk/2003/userCSharp"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/ns0:ERPccRequestRoot"" />
  </xsl:template>
  <xsl:template match=""/ns0:ERPccRequestRoot"">
    <xsl:variable name=""var:v1"" select=""userCSharp:StringUpperCase(&quot;dummy@dummymail.com&quot;)"" />
    <ns0:ERPccRequestRoot>
      <xsl:for-each select=""ccAuthService"">
        <ccAuthService>
          <xsl:if test=""ccAuthService_Value"">
            <ccAuthService_Value>
              <xsl:value-of select=""ccAuthService_Value/text()"" />
            </ccAuthService_Value>
          </xsl:if>
        </ccAuthService>
      </xsl:for-each>
      <xsl:for-each select=""ccCaptureService"">
        <ccCaptureService>
          <xsl:if test=""ccCaptureService_Value"">
            <ccCaptureService_Value>
              <xsl:value-of select=""ccCaptureService_Value/text()"" />
            </ccCaptureService_Value>
          </xsl:if>
        </ccCaptureService>
      </xsl:for-each>
      <xsl:for-each select=""ccAuthReversalService"">
        <ccAuthReversalService>
          <xsl:if test=""ccAuthReversalService_Value"">
            <ccAuthReversalService_Value>
              <xsl:value-of select=""ccAuthReversalService_Value/text()"" />
            </ccAuthReversalService_Value>
          </xsl:if>
        </ccAuthReversalService>
      </xsl:for-each>
      <xsl:for-each select=""ccCreditService"">
        <ccCreditService>
          <xsl:if test=""ccCreditService_Value"">
            <ccCreditService_Value>
              <xsl:value-of select=""ccCreditService_Value/text()"" />
            </ccCreditService_Value>
          </xsl:if>
        </ccCreditService>
      </xsl:for-each>
      <xsl:for-each select=""purchasingLevel"">
        <purchasingLevel>
          <xsl:if test=""purchasingLevel_Value"">
            <purchasingLevel_Value>
              <xsl:value-of select=""purchasingLevel_Value/text()"" />
            </purchasingLevel_Value>
          </xsl:if>
        </purchasingLevel>
      </xsl:for-each>
      <merchantID>
        <merchantID_Value>
          <xsl:value-of select=""merchantID/merchantID_Value/text()"" />
        </merchantID_Value>
      </merchantID>
      <merchantReferenceCode>
        <merchantReferenceCode_Value>
          <xsl:value-of select=""merchantReferenceCode/merchantReferenceCode_Value/text()"" />
        </merchantReferenceCode_Value>
      </merchantReferenceCode>
      <xsl:for-each select=""subscriptionID"">
        <subscriptionID>
          <xsl:if test=""subscriptionID_Value"">
            <subscriptionID_Value>
              <xsl:value-of select=""subscriptionID_Value/text()"" />
            </subscriptionID_Value>
          </xsl:if>
        </subscriptionID>
      </xsl:for-each>
      <xsl:for-each select=""requestID"">
        <requestID>
          <xsl:if test=""requestID_Value"">
            <requestID_Value>
              <xsl:value-of select=""requestID_Value/text()"" />
            </requestID_Value>
          </xsl:if>
        </requestID>
      </xsl:for-each>
      <xsl:for-each select=""requestToken"">
        <requestToken>
          <xsl:if test=""requestToken_Value"">
            <requestToken_Value>
              <xsl:value-of select=""requestToken_Value/text()"" />
            </requestToken_Value>
          </xsl:if>
        </requestToken>
      </xsl:for-each>
      <xsl:for-each select=""shipFromPostal"">
        <shipFromPostal>
          <xsl:if test=""shipFromPostal_Value"">
            <shipFromPostal_Value>
              <xsl:value-of select=""shipFromPostal_Value/text()"" />
            </shipFromPostal_Value>
          </xsl:if>
        </shipFromPostal>
      </xsl:for-each>
      <xsl:for-each select=""shipToCountry"">
        <shipToCountry>
          <xsl:if test=""shipToCountry_Value"">
            <shipToCountry_Value>
              <xsl:value-of select=""shipToCountry_Value/text()"" />
            </shipToCountry_Value>
          </xsl:if>
        </shipToCountry>
      </xsl:for-each>
      <xsl:for-each select=""shipToPostal"">
        <shipToPostal>
          <xsl:if test=""shipToPostal_Value"">
            <shipToPostal_Value>
              <xsl:value-of select=""shipToPostal_Value/text()"" />
            </shipToPostal_Value>
          </xsl:if>
        </shipToPostal>
      </xsl:for-each>
      <xsl:for-each select=""shipToState"">
        <shipToState>
          <xsl:if test=""shipToState_Value"">
            <shipToState_Value>
              <xsl:value-of select=""shipToState_Value/text()"" />
            </shipToState_Value>
          </xsl:if>
        </shipToState>
      </xsl:for-each>
      <item_0_unitprice>
        <item_0_unitprice_Value>
          <xsl:value-of select=""item_0_unitprice/item_0_unitprice_Value/text()"" />
        </item_0_unitprice_Value>
      </item_0_unitprice>
      <xsl:for-each select=""item_0_quantity"">
        <item_0_quantity>
          <xsl:if test=""item_0_quantity_Value"">
            <item_0_quantity_Value>
              <xsl:value-of select=""item_0_quantity_Value/text()"" />
            </item_0_quantity_Value>
          </xsl:if>
        </item_0_quantity>
      </xsl:for-each>
      <xsl:for-each select=""item_0_taxamount"">
        <item_0_taxamount>
          <xsl:if test=""item_0_taxamount_Value"">
            <item_0_taxamount_Value>
              <xsl:value-of select=""item_0_taxamount_Value/text()"" />
            </item_0_taxamount_Value>
          </xsl:if>
        </item_0_taxamount>
      </xsl:for-each>
      <xsl:for-each select=""item_0_taxrate"">
        <item_0_taxrate>
          <xsl:if test=""item_0_taxrate_Value"">
            <item_0_taxrate_Value>
              <xsl:value-of select=""item_0_taxrate_Value/text()"" />
            </item_0_taxrate_Value>
          </xsl:if>
        </item_0_taxrate>
      </xsl:for-each>
      <purchaseTotals_currency>
        <purchaseTotals_currency_Value>
          <xsl:value-of select=""purchaseTotals_currency/purchaseTotals_currency_Value/text()"" />
        </purchaseTotals_currency_Value>
      </purchaseTotals_currency>
      <xsl:for-each select=""invoiceHeader_userPO"">
        <invoiceHeader_userPO>
          <xsl:if test=""invoiceHeader_userPO_Value"">
            <invoiceHeader_userPO_Value>
              <xsl:value-of select=""invoiceHeader_userPO_Value/text()"" />
            </invoiceHeader_userPO_Value>
          </xsl:if>
        </invoiceHeader_userPO>
      </xsl:for-each>
      <xsl:for-each select=""invoiceHeader_amexDataTAA1"">
        <invoiceHeader_amexDataTAA1>
          <xsl:if test=""invoiceHeader_amexDataTAA1_Value"">
            <invoiceHeader_amexDataTAA1_Value>
              <xsl:value-of select=""invoiceHeader_amexDataTAA1_Value/text()"" />
            </invoiceHeader_amexDataTAA1_Value>
          </xsl:if>
        </invoiceHeader_amexDataTAA1>
      </xsl:for-each>
      <xsl:for-each select=""invoiceHeader_merchantDescriptor"">
        <invoiceHeader_merchantDescriptor>
          <xsl:if test=""invoiceHeader_merchantDescriptor_Value"">
            <invoiceHeader_merchantDescriptor_Value>
              <xsl:value-of select=""invoiceHeader_merchantDescriptor_Value/text()"" />
            </invoiceHeader_merchantDescriptor_Value>
          </xsl:if>
        </invoiceHeader_merchantDescriptor>
      </xsl:for-each>
      <xsl:for-each select=""invoiceHeader_merchantDescriptorContact"">
        <invoiceHeader_merchantDescriptorContact>
          <xsl:if test=""invoiceHeader_merchantDescriptorContact_Value"">
            <invoiceHeader_merchantDescriptorContact_Value>
              <xsl:value-of select=""invoiceHeader_merchantDescriptorContact_Value/text()"" />
            </invoiceHeader_merchantDescriptorContact_Value>
          </xsl:if>
        </invoiceHeader_merchantDescriptorContact>
      </xsl:for-each>
      <notificationEmail>
        <notificationEmail_Value>
          <xsl:value-of select=""$var:v1"" />
        </notificationEmail_Value>
      </notificationEmail>
    </ns0:ERPccRequestRoot>
  </xsl:template>
  <msxsl:script language=""C#"" implements-prefix=""userCSharp""><![CDATA[
public string StringUpperCase(string str)
{
	if (str == null)
	{
		return """";
	}
	return str.ToUpper(System.Globalization.CultureInfo.InvariantCulture);
}



]]></msxsl:script>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"BTNextGen.BizTalk.PaymentProcessing.Schemas.ERPccRequest";
        
        private const global::BTNextGen.BizTalk.PaymentProcessing.Schemas.ERPccRequest _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"BTNextGen.BizTalk.PaymentProcessing.Schemas.ERPccRequest";
        
        private const global::BTNextGen.BizTalk.PaymentProcessing.Schemas.ERPccRequest _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"BTNextGen.BizTalk.PaymentProcessing.Schemas.ERPccRequest";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"BTNextGen.BizTalk.PaymentProcessing.Schemas.ERPccRequest";
                return _TrgSchemas;
            }
        }
    }
}
