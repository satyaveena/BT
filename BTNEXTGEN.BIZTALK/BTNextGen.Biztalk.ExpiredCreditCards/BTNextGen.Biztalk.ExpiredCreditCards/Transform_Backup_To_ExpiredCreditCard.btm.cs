namespace BTNextGen.Biztalk.ExpiredCreditCards {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.BizTalk.CS.Profiles.Schemas.ProfilesQueryResponseMessage+ProfileDocumentList", typeof(global::BTNextGen.BizTalk.CS.Profiles.Schemas.ProfilesQueryResponseMessage.ProfileDocumentList))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.Biztalk.ExpiredCreditCards.ExpiredCreditCardSchema", typeof(global::BTNextGen.Biztalk.ExpiredCreditCards.ExpiredCreditCardSchema))]
    public sealed class Transform_Backup_To_ExpiredCreditCard : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var userCSharp"" version=""1.0"" xmlns:ns0=""http://BTNextGen.Biztalk.ExpiredCreditCards.ExpiredCreditCardSchema"" xmlns:userCSharp=""http://schemas.microsoft.com/BizTalk/2003/userCSharp"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/ProfileDocumentList"" />
  </xsl:template>
  <xsl:template match=""/ProfileDocumentList"">
    <ns0:ExpiredCreditCardsList>
      <CreditCardsDoc>
        <CreditCardsX>
          <xsl:for-each select=""ProfileDocument"">
            <xsl:for-each select=""CreditCard"">
              <xsl:variable name=""var:v1"" select=""userCSharp:MyConcat(string(GeneralInfo/expiration_month/text()))"" />
              <xsl:variable name=""var:v2"" select=""userCSharp:LogicalIsString(string($var:v1))"" />
              <xsl:if test=""$var:v2"">
                <CreditCardData>
                  <id>
                    <xsl:value-of select=""GeneralInfo/id/text()"" />
                  </id>
                  <xsl:if test=""GeneralInfo/payment_group_id"">
                    <paymentgroupid>
                      <xsl:value-of select=""GeneralInfo/payment_group_id/text()"" />
                    </paymentgroupid>
                  </xsl:if>
                  <ccnumber>
                    <xsl:value-of select=""GeneralInfo/cc_number/text()"" />
                  </ccnumber>
                  <last4digits>
                    <xsl:value-of select=""GeneralInfo/last_4_digits/text()"" />
                  </last4digits>
                  <billingaddress>
                    <xsl:value-of select=""GeneralInfo/billing_address/text()"" />
                  </billingaddress>
                  <expirationmonth>
                    <xsl:value-of select=""GeneralInfo/expiration_month/text()"" />
                  </expirationmonth>
                  <expirationyear>
                    <xsl:value-of select=""GeneralInfo/expiration_year/text()"" />
                  </expirationyear>
                  <xsl:if test=""BTNextGen/card_contact_user"">
                    <cardcontactuser>
                      <xsl:value-of select=""BTNextGen/card_contact_user/text()"" />
                    </cardcontactuser>
                  </xsl:if>
                  <xsl:if test=""BTNextGen/credit_card_token"">
                    <cardtoken>
                      <xsl:value-of select=""BTNextGen/credit_card_token/text()"" />
                    </cardtoken>
                  </xsl:if>
                  <xsl:if test=""BTNextGen/payment_status"">
                    <paymentstatus>
                      <xsl:value-of select=""BTNextGen/payment_status/text()"" />
                    </paymentstatus>
                  </xsl:if>
                  <xsl:if test=""BTNextGen/notification_status"">
                    <notificationstatus>
                      <xsl:value-of select=""BTNextGen/notification_status/text()"" />
                    </notificationstatus>
                  </xsl:if>
                  <xsl:if test=""BTNextGen/date_last_failed_notification"">
                    <datelastfailednotification>
                      <xsl:value-of select=""BTNextGen/date_last_failed_notification/text()"" />
                    </datelastfailednotification>
                  </xsl:if>
                  <xsl:if test=""BTNextGen/date_last_preemptive_notification"">
                    <datelastpreemptivenotification>
                      <xsl:value-of select=""BTNextGen/date_last_preemptive_notification/text()"" />
                    </datelastpreemptivenotification>
                  </xsl:if>
                  <xsl:if test=""BTNextGen/alias"">
                    <alias>
                      <xsl:value-of select=""BTNextGen/alias/text()"" />
                    </alias>
                  </xsl:if>
                  <xsl:if test=""BTNextGen/primary_indicator"">
                    <primaryindicator>
                      <xsl:value-of select=""BTNextGen/primary_indicator/text()"" />
                    </primaryindicator>
                  </xsl:if>
                </CreditCardData>
              </xsl:if>
            </xsl:for-each>
          </xsl:for-each>
        </CreditCardsX>
      </CreditCardsDoc>
    </ns0:ExpiredCreditCardsList>
  </xsl:template>
  <msxsl:script language=""C#"" implements-prefix=""userCSharp""><![CDATA[
///*Uncomment the following code for a sample Inline C# function
//that concatenates two inputs. Change the number of parameters of
//this function to be equal to the number of inputs connected to this //functoid.*/

public string MyConcat(string param1)
{
int intCurrentMonth = System.DateTime.Today.Month; 
string strCurrentMonth = System.Convert.ToString(intCurrentMonth);

if (param1== strCurrentMonth)

	return param1;
else 
                return """"; 
}


public bool LogicalIsString(string val)
{
	return (val != null && val !="""");
}



]]></msxsl:script>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"BTNextGen.BizTalk.CS.Profiles.Schemas.ProfilesQueryResponseMessage+ProfileDocumentList";
        
        private const global::BTNextGen.BizTalk.CS.Profiles.Schemas.ProfilesQueryResponseMessage.ProfileDocumentList _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"BTNextGen.Biztalk.ExpiredCreditCards.ExpiredCreditCardSchema";
        
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
                _SrcSchemas[0] = @"BTNextGen.BizTalk.CS.Profiles.Schemas.ProfilesQueryResponseMessage+ProfileDocumentList";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"BTNextGen.Biztalk.ExpiredCreditCards.ExpiredCreditCardSchema";
                return _TrgSchemas;
            }
        }
    }
}
