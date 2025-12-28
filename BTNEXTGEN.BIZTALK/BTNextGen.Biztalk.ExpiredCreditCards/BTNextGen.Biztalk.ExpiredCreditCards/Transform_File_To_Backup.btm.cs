namespace BTNextGen.Biztalk.ExpiredCreditCards {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.BizTalk.CS.Profiles.Schemas.ProfilesQueryResponseMessage+ProfileDocumentList", typeof(global::BTNextGen.BizTalk.CS.Profiles.Schemas.ProfilesQueryResponseMessage.ProfileDocumentList))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.BizTalk.CS.Profiles.Schemas.ProfilesQueryResponseMessage+ProfileDocumentList", typeof(global::BTNextGen.BizTalk.CS.Profiles.Schemas.ProfilesQueryResponseMessage.ProfileDocumentList))]
    public sealed class Transform_File_To_Backup : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var userCSharp"" version=""1.0"" xmlns:userCSharp=""http://schemas.microsoft.com/BizTalk/2003/userCSharp"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/ProfileDocumentList"" />
  </xsl:template>
  <xsl:template match=""/ProfileDocumentList"">
    <ProfileDocumentList>
      <ProfileDocument>
        <CreditCard>
          <xsl:for-each select=""ProfileDocument"">
            <xsl:for-each select=""CreditCard"">
              <xsl:variable name=""var:v1"" select=""userCSharp:MyConcat(string(GeneralInfo/expiration_month/text()))"" />
              <xsl:variable name=""var:v2"" select=""userCSharp:LogicalIsString(string($var:v1))"" />
              <xsl:if test=""$var:v2"">
                <GeneralInfo>
                  <id>
                    <xsl:value-of select=""GeneralInfo/id/text()"" />
                  </id>
                  <xsl:if test=""GeneralInfo/payment_group_id"">
                    <payment_group_id>
                      <xsl:value-of select=""GeneralInfo/payment_group_id/text()"" />
                    </payment_group_id>
                  </xsl:if>
                  <cc_number>
                    <xsl:value-of select=""GeneralInfo/cc_number/text()"" />
                  </cc_number>
                  <last_4_digits>
                    <xsl:value-of select=""GeneralInfo/last_4_digits/text()"" />
                  </last_4_digits>
                  <billing_address>
                    <xsl:value-of select=""GeneralInfo/billing_address/text()"" />
                  </billing_address>
                  <expiration_month>
                    <xsl:value-of select=""GeneralInfo/expiration_month/text()"" />
                  </expiration_month>
                  <expiration_year>
                    <xsl:value-of select=""GeneralInfo/expiration_year/text()"" />
                  </expiration_year>
                </GeneralInfo>
              </xsl:if>
            </xsl:for-each>
          </xsl:for-each>
          <xsl:for-each select=""ProfileDocument"">
            <xsl:for-each select=""CreditCard"">
              <xsl:variable name=""var:v3"" select=""string(GeneralInfo/expiration_month/text())"" />
              <xsl:variable name=""var:v4"" select=""userCSharp:MyConcat($var:v3)"" />
              <xsl:variable name=""var:v5"" select=""userCSharp:LogicalIsString(string($var:v4))"" />
              <xsl:if test=""$var:v5"">
                <BTNextGen>
                  <xsl:if test=""BTNextGen/credit_card_token"">
                    <credit_card_token>
                      <xsl:value-of select=""BTNextGen/credit_card_token/text()"" />
                    </credit_card_token>
                  </xsl:if>
                  <xsl:if test=""BTNextGen/alias"">
                    <alias>
                      <xsl:value-of select=""BTNextGen/alias/text()"" />
                    </alias>
                  </xsl:if>
                  <xsl:if test=""BTNextGen/primary_indicator"">
                    <primary_indicator>
                      <xsl:value-of select=""BTNextGen/primary_indicator/text()"" />
                    </primary_indicator>
                  </xsl:if>
                  <xsl:if test=""BTNextGen/credit_card_type"">
                    <credit_card_type>
                      <xsl:value-of select=""BTNextGen/credit_card_type/text()"" />
                    </credit_card_type>
                  </xsl:if>
                  <xsl:if test=""BTNextGen/transmitted_to_erp"">
                    <transmitted_to_erp>
                      <xsl:value-of select=""BTNextGen/transmitted_to_erp/text()"" />
                    </transmitted_to_erp>
                  </xsl:if>
                  <xsl:if test=""BTNextGen/payment_status"">
                    <payment_status>
                      <xsl:value-of select=""BTNextGen/payment_status/text()"" />
                    </payment_status>
                  </xsl:if>
                  <xsl:if test=""BTNextGen/notification_status"">
                    <notification_status>
                      <xsl:value-of select=""BTNextGen/notification_status/text()"" />
                    </notification_status>
                  </xsl:if>
                  <xsl:if test=""BTNextGen/card_contact_user"">
                    <card_contact_user>
                      <xsl:value-of select=""BTNextGen/card_contact_user/text()"" />
                    </card_contact_user>
                  </xsl:if>
                </BTNextGen>
              </xsl:if>
            </xsl:for-each>
          </xsl:for-each>
        </CreditCard>
      </ProfileDocument>
    </ProfileDocumentList>
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
        
        private const string _strTrgSchemasList0 = @"BTNextGen.BizTalk.CS.Profiles.Schemas.ProfilesQueryResponseMessage+ProfileDocumentList";
        
        private const global::BTNextGen.BizTalk.CS.Profiles.Schemas.ProfilesQueryResponseMessage.ProfileDocumentList _trgSchemaTypeReference0 = null;
        
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
                _TrgSchemas[0] = @"BTNextGen.BizTalk.CS.Profiles.Schemas.ProfilesQueryResponseMessage+ProfileDocumentList";
                return _TrgSchemas;
            }
        }
    }
}
