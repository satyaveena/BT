namespace BTNextGen.BizTalk.Profiles.Maps {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.BizTalk.Profiles.Schemas.ProcedureResultSet_dbo_procGetOrganization+StoredProcedureResultSet0", typeof(global::BTNextGen.BizTalk.Profiles.Schemas.ProcedureResultSet_dbo_procGetOrganization.StoredProcedureResultSet0))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.BizTalk.CS.Profiles.Schemas.BTProfile", typeof(global::BTNextGen.BizTalk.CS.Profiles.Schemas.BTProfile))]
    public sealed class SubscriptionLegacy_To_SubscriptionCommerceServer : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0 userCSharp"" version=""1.0"" xmlns:s0=""http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procGetOrganization"" xmlns:userCSharp=""http://schemas.microsoft.com/BizTalk/2003/userCSharp"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s0:StoredProcedureResultSet0"" />
  </xsl:template>
  <xsl:template match=""/s0:StoredProcedureResultSet0"">
    <ProfileDocument>
      <BTSubscription>
        <BTNextGen>
          <xsl:variable name=""var:v1"" select=""userCSharp:GetGuid()"" />
          <subscription_id>
            <xsl:value-of select=""$var:v1"" />
          </subscription_id>
          <xsl:if test=""s0:CSOrgGUID"">
            <org_id>
              <xsl:value-of select=""s0:CSOrgGUID/text()"" />
            </org_id>
          </xsl:if>
          <xsl:if test=""s0:subscription_start_date"">
            <subscription_start_date>
              <xsl:value-of select=""s0:subscription_start_date/text()"" />
            </subscription_start_date>
          </xsl:if>
          <xsl:if test=""s0:subscription_end_date"">
            <subscription_end_date>
              <xsl:value-of select=""s0:subscription_end_date/text()"" />
            </subscription_end_date>
          </xsl:if>
          <xsl:if test=""s0:entertainment_product"">
            <entertainment_product>
              <xsl:value-of select=""s0:entertainment_product/text()"" />
            </entertainment_product>
          </xsl:if>
          <xsl:if test=""s0:full_txt_review_enabled"">
            <full_txt_review_enabled>
              <xsl:value-of select=""s0:full_txt_review_enabled/text()"" />
            </full_txt_review_enabled>
          </xsl:if>
          <xsl:if test=""s0:table_of_contents"">
            <table_of_contents>
              <xsl:value-of select=""s0:table_of_contents/text()"" />
            </table_of_contents>
          </xsl:if>
          <xsl:if test=""s0:I_fulltext_reviews"">
            <fulltext_reviews>
              <xsl:value-of select=""s0:I_fulltext_reviews/text()"" />
            </fulltext_reviews>
          </xsl:if>
        </BTNextGen>
      </BTSubscription>
    </ProfileDocument>
  </xsl:template>
  <msxsl:script language=""C#"" implements-prefix=""userCSharp""><![CDATA[
///*Uncomment the following code for a sample Inline C# function
//that concatenates two inputs. Change the number of parameters of
//this function to be equal to the number of inputs connected to this functoid.*/

public string GetGuid()
{
 return ""{""+System.Guid.NewGuid().ToString()+""}"";

}


]]></msxsl:script>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"BTNextGen.BizTalk.Profiles.Schemas.ProcedureResultSet_dbo_procGetOrganization+StoredProcedureResultSet0";
        
        private const global::BTNextGen.BizTalk.Profiles.Schemas.ProcedureResultSet_dbo_procGetOrganization.StoredProcedureResultSet0 _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"BTNextGen.BizTalk.CS.Profiles.Schemas.BTProfile";
        
        private const global::BTNextGen.BizTalk.CS.Profiles.Schemas.BTProfile _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"BTNextGen.BizTalk.Profiles.Schemas.ProcedureResultSet_dbo_procGetOrganization+StoredProcedureResultSet0";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"BTNextGen.BizTalk.CS.Profiles.Schemas.BTProfile";
                return _TrgSchemas;
            }
        }
    }
}
