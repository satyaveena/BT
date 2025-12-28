namespace BTNextGen.BizTalk.Profiles.Maps {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.BizTalk.Profiles.Schemas.ProcedureResultSet_dbo_procGetOrganization+StoredProcedureResultSet0", typeof(global::BTNextGen.BizTalk.Profiles.Schemas.ProcedureResultSet_dbo_procGetOrganization.StoredProcedureResultSet0))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.BizTalk.CS.Profiles.Schemas.BTProfile", typeof(global::BTNextGen.BizTalk.CS.Profiles.Schemas.BTProfile))]
    public sealed class OrgProfilesLegacy_To_OrgProfilesCommerce : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0 userCSharp"" version=""1.0"" xmlns:s0=""http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procGetOrganization"" xmlns:userCSharp=""http://schemas.microsoft.com/BizTalk/2003/userCSharp"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s0:StoredProcedureResultSet0"" />
  </xsl:template>
  <xsl:template match=""/s0:StoredProcedureResultSet0"">
    <xsl:variable name=""var:v1"" select=""userCSharp:StringConcat(&quot;;&quot;)"" />
    <ProfileDocument>
      <Organization>
        <GeneralInfo>
          <xsl:if test=""s0:CSOrgGUID"">
            <org_id>
              <xsl:value-of select=""s0:CSOrgGUID/text()"" />
            </org_id>
          </xsl:if>
          <xsl:if test=""s0:alias"">
            <name>
              <xsl:value-of select=""s0:alias/text()"" />
            </name>
          </xsl:if>
        </GeneralInfo>
        <BTNextGen>
          <xsl:if test=""s0:Legacy_org_id"">
            <bt_org_id>
              <xsl:value-of select=""s0:Legacy_org_id/text()"" />
            </bt_org_id>
          </xsl:if>
          <web_market_type>
            <xsl:value-of select=""s0:u_web_market_type/text()"" />
          </web_market_type>
          <xsl:if test=""s0:allowed_users_count"">
            <allowed_users_count>
              <xsl:value-of select=""s0:allowed_users_count/text()"" />
            </allowed_users_count>
          </xsl:if>
          <users_use_count>
            <xsl:value-of select=""s0:users_use_count/text()"" />
          </users_use_count>
          <xsl:if test=""s0:alias"">
            <alias>
              <xsl:value-of select=""s0:alias/text()"" />
            </alias>
          </xsl:if>
          <contact_fax>
            <xsl:value-of select=""s0:u_contact_fax/text()"" />
          </contact_fax>
          <contact_email>
            <xsl:value-of select=""s0:u_contact_email/text()"" />
          </contact_email>
          <xsl:if test=""s0:all_wearhouse"">
            <all_warehouse>
              <xsl:value-of select=""s0:all_wearhouse/text()"" />
            </all_warehouse>
          </xsl:if>
          <xsl:if test=""s0:is_active"">
            <is_active>
              <xsl:value-of select=""s0:is_active/text()"" />
            </is_active>
          </xsl:if>
          <xsl:if test=""s0:web_order"">
            <web_order>
              <xsl:value-of select=""s0:web_order/text()"" />
            </web_order>
          </xsl:if>
          <default_book_account_id>
            <xsl:value-of select=""s0:u_default_book_account/text()"" />
          </default_book_account_id>
          <default_entertainment_account_id>
            <xsl:value-of select=""s0:u_default_entertainment_account/text()"" />
          </default_entertainment_account_id>
          <xsl:if test=""s0:CSOrgGUID"">
            <address_id>
              <xsl:value-of select=""s0:CSOrgGUID/text()"" />
            </address_id>
          </xsl:if>
          <xsl:call-template name=""output-AccountsList"">
            <xsl:with-param name=""list"" select=""string(s0:u_accounts/text())"" />
            <xsl:with-param name=""delimiter"" select=""string($var:v1)"" />
          </xsl:call-template>
          <xsl:if test=""s0:entertainment_product"">
            <entertainment_product>
              <xsl:value-of select=""s0:entertainment_product/text()"" />
            </entertainment_product>
          </xsl:if>
          <xsl:if test=""s0:table_of_contents"">
            <table_of_contents>
              <xsl:value-of select=""s0:table_of_contents/text()"" />
            </table_of_contents>
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
          <xsl:if test=""s0:I_fulltext_reviews"">
            <fulltext_reviews>
              <xsl:value-of select=""s0:I_fulltext_reviews/text()"" />
            </fulltext_reviews>
          </xsl:if>
          <xsl:call-template name=""output-ReviewsList"">
            <xsl:with-param name=""list"" select=""string(s0:ReviewTypes/text())"" />
            <xsl:with-param name=""delimiter"" select=""string($var:v1)"" />
          </xsl:call-template>
          <xsl:if test=""s0:home_delivery_allowed"">
            <home_delivery_allowed>
              <xsl:value-of select=""s0:home_delivery_allowed/text()"" />
            </home_delivery_allowed>
          </xsl:if>
          <contact_phone>
            <xsl:value-of select=""s0:u_contact_phone/text()"" />
          </contact_phone>
          <xsl:call-template name=""output-UsersList"">
            <xsl:with-param name=""list"" select=""string(s0:u_users/text())"" />
            <xsl:with-param name=""delimiter"" select=""string($var:v1)"" />
          </xsl:call-template>
          <xsl:if test=""s0:i_account_count"">
            <account_count>
              <xsl:value-of select=""s0:i_account_count/text()"" />
            </account_count>
          </xsl:if>
          <xsl:if test=""s0:Personal_prod_url"">
            <personal_prod_url>
              <xsl:value-of select=""s0:Personal_prod_url/text()"" />
            </personal_prod_url>
          </xsl:if>
          <xsl:if test=""s0:prod_lookup_index"">
            <prod_lookup_index>
              <xsl:value-of select=""s0:prod_lookup_index/text()"" />
            </prod_lookup_index>
          </xsl:if>
          <xsl:if test=""s0:isbn_lookup_code"">
            <isbn_lookup_code>
              <xsl:value-of select=""s0:isbn_lookup_code/text()"" />
            </isbn_lookup_code>
          </xsl:if>
          <xsl:if test=""s0:isbn_link_displayed"">
            <isbn_link_displayed>
              <xsl:value-of select=""s0:isbn_link_displayed/text()"" />
            </isbn_link_displayed>
          </xsl:if>
          <xsl:if test=""s0:prod_suffix_lookup"">
            <prod_suffix_lookup>
              <xsl:value-of select=""s0:prod_suffix_lookup/text()"" />
            </prod_suffix_lookup>
          </xsl:if>
          <xsl:if test=""s0:is_internal"">
            <is_bt_employee>
              <xsl:value-of select=""s0:is_internal/text()"" />
            </is_bt_employee>
          </xsl:if>
          <u_contact_name>
            <xsl:value-of select=""s0:u_contact_name/text()"" />
          </u_contact_name>
          <xsl:if test=""s0:Legacy_org_id"">
            <legacy_org_id>
              <xsl:value-of select=""s0:Legacy_org_id/text()"" />
            </legacy_org_id>
          </xsl:if>
          <org_type>
            <xsl:value-of select=""s0:u_org_type/text()"" />
          </org_type>
          <xsl:if test=""s0:Legacy_source_system"">
            <legacy_source_system>
              <xsl:value-of select=""s0:Legacy_source_system/text()"" />
            </legacy_source_system>
          </xsl:if>
          <xsl:variable name=""var:v2"" select=""userCSharp:MyConcatOE(string(s0:b_original_entry/text()))"" />
          <original_entry>
            <xsl:value-of select=""$var:v2"" />
          </original_entry>
          <xsl:variable name=""var:v3"" select=""userCSharp:MyConcatGE(string(s0:b_grid_enabled/text()))"" />
          <grid_enabled>
            <xsl:value-of select=""$var:v3"" />
          </grid_enabled>
          <xsl:variable name=""var:v4"" select=""userCSharp:MyConcatscwe(string(s0:b_shared_cart_workflow_enabled/text()))"" />
          <shared_cart_workflow_enabled>
            <xsl:value-of select=""$var:v4"" />
          </shared_cart_workflow_enabled>
          <xsl:variable name=""var:v5"" select=""userCSharp:MyConcatMPE(string(s0:b_marc_profiler_enabled/text()))"" />
          <marc_profiler_enabled>
            <xsl:value-of select=""$var:v5"" />
          </marc_profiler_enabled>
          <xsl:variable name=""var:v6"" select=""userCSharp:MyConcatoclc(string(s0:b_oclc_cataloging_plus_enabled/text()))"" />
          <oclc_cataloging_plus_enabled>
            <xsl:value-of select=""$var:v6"" />
          </oclc_cataloging_plus_enabled>
          <xsl:variable name=""var:v7"" select=""userCSharp:MyConcatsbn(string(s0:b_show_bib_number/text()))"" />
          <show_bib_number>
            <xsl:value-of select=""$var:v7"" />
          </show_bib_number>
          <xsl:variable name=""var:v8"" select=""userCSharp:MyConcatfmp(string(s0:b_is_full_marc_profile/text()))"" />
          <is_full_marc_profile>
            <xsl:value-of select=""$var:v8"" />
          </is_full_marc_profile>
          <xsl:call-template name=""output-esuppliers"">
            <xsl:with-param name=""list"" select=""string(s0:u_e_suppliers/text())"" />
            <xsl:with-param name=""delimiter"" select=""string($var:v1)"" />
          </xsl:call-template>
          <xsl:variable name=""var:v9"" select=""userCSharp:MyConcatsr(string(s0:b_slip_report/text()))"" />
          <slip_report>
            <xsl:value-of select=""$var:v9"" />
          </slip_report>
          <xsl:variable name=""var:v10"" select=""userCSharp:MyConcatlsh(string(s0:b_library_system_handling/text()))"" />
          <library_system_handling>
            <xsl:value-of select=""$var:v10"" />
          </library_system_handling>
        </BTNextGen>
      </Organization>
    </ProfileDocument>
  </xsl:template>
  <msxsl:script language=""C#"" implements-prefix=""userCSharp""><![CDATA[
public string StringConcat(string param0)
{
   return param0;
}



public bool MyConcatGE(string param1)
{
string one = ""1"";
 if (param1.Equals(one.ToString()))
{ return true; }
return false;
}


public bool MyConcatMPE(string param1)
{
string one = ""1"";
 if (param1.Equals(one.ToString()))
{ return true; }
return false;
}


public bool MyConcatOE(string param1)
{
string one = ""1"";
 if (param1.Equals(one.ToString()))
{ return true; }
return false;
}


public bool MyConcatscwe(string param1)
{
string one = ""1"";
 if (param1.Equals(one.ToString()))
{ return true; }
return false;
}


public bool MyConcatfmp(string param1)
{
string one = ""1"";
 if (param1.Equals(one.ToString()))
{ return true; }
return false;
}


public bool MyConcatsbn(string param1)
{
string one = ""1"";
 if (param1.Equals(one.ToString()))
{ return true; }
return false;
}


public bool MyConcatsr(string param1)
{
string one = ""1"";
 if (param1.Equals(one.ToString()))
{ return true; }
return false;
}


public bool MyConcatlsh(string param1)
{
string one = ""1"";
 if (param1.Equals(one.ToString()))
{ return true; }
return false;
}

public bool MyConcatoclc(string param1)
{
string one = ""1"";
 if (param1.Equals(one.ToString()))
{ return true; }
return false;
}


]]></msxsl:script>
  <xsl:template name=""output-AccountsList"">
  <xsl:param name=""list"" />
  <xsl:param name=""delimiter"" />
  <xsl:variable name=""newlist"">
    <xsl:choose>
      <xsl:when test=""contains($list, $delimiter)"">
        <xsl:value-of select=""normalize-space($list)"" />
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select=""concat(normalize-space($list), $delimiter)"" />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:variable>
  <xsl:variable name=""first"" select=""substring-before($newlist, $delimiter)"" />
  <xsl:variable name=""remaining"" select=""substring-after($newlist, $delimiter)"" />
  <xsl:if test=""string-length($first) != 0"">
    <account_list>
      <xsl:value-of select=""$first"" />
    </account_list>
  </xsl:if>
  <xsl:if test=""$remaining"">
    <xsl:call-template name=""output-AccountsList"">
      <xsl:with-param name=""list"" select=""$remaining"" />
      <xsl:with-param name=""delimiter"">
        <xsl:value-of select=""$delimiter"" />
      </xsl:with-param>
    </xsl:call-template>
  </xsl:if>
</xsl:template>
  <xsl:template name=""output-UsersList"">
  <xsl:param name=""list"" />
  <xsl:param name=""delimiter"" />
  <xsl:variable name=""newlist"">
    <xsl:choose>
      <xsl:when test=""contains($list, $delimiter)"">
        <xsl:value-of select=""normalize-space($list)"" />
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select=""concat(normalize-space($list), $delimiter)"" />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:variable>
  <xsl:variable name=""first"" select=""substring-before($newlist, $delimiter)"" />
  <xsl:variable name=""remaining"" select=""substring-after($newlist, $delimiter)"" />
  <xsl:if test=""string-length($first) != 0"">
    <user_list>
      <xsl:value-of select=""$first"" />
    </user_list>
  </xsl:if>
  <xsl:if test=""$remaining"">
    <xsl:call-template name=""output-UsersList"">
      <xsl:with-param name=""list"" select=""$remaining"" />
      <xsl:with-param name=""delimiter"">
        <xsl:value-of select=""$delimiter"" />
      </xsl:with-param>
    </xsl:call-template>
  </xsl:if>
</xsl:template>
  <xsl:template name=""output-ReviewsList"">
  <xsl:param name=""list"" />
  <xsl:param name=""delimiter"" />
  <xsl:variable name=""newlist"">
    <xsl:choose>
      <xsl:when test=""contains($list, $delimiter)"">
        <xsl:value-of select=""normalize-space($list)"" />
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select=""concat(normalize-space($list), $delimiter)"" />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:variable>
  <xsl:variable name=""first"" select=""substring-before($newlist, $delimiter)"" />
  <xsl:variable name=""remaining"" select=""substring-after($newlist, $delimiter)"" />
  <xsl:if test=""string-length($first) != 0"">
    <review_type_list>
      <xsl:value-of select=""$first"" />
    </review_type_list>
  </xsl:if>
  <xsl:if test=""$remaining"">
    <xsl:call-template name=""output-ReviewsList"">
      <xsl:with-param name=""list"" select=""$remaining"" />
      <xsl:with-param name=""delimiter"">
        <xsl:value-of select=""$delimiter"" />
      </xsl:with-param>
    </xsl:call-template>
  </xsl:if>
</xsl:template>
  <xsl:template name=""output-esuppliers"">
  <xsl:param name=""list"" />
  <xsl:param name=""delimiter"" />
  <xsl:variable name=""newlist"">
    <xsl:choose>
      <xsl:when test=""contains($list, $delimiter)"">
        <xsl:value-of select=""normalize-space($list)"" />
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select=""concat(normalize-space($list), $delimiter)"" />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:variable>
  <xsl:variable name=""first"" select=""substring-before($newlist, $delimiter)"" />
  <xsl:variable name=""remaining"" select=""substring-after($newlist, $delimiter)"" />
  <xsl:if test=""string-length($first) != 0"">
    <e_suppliers>
      <xsl:value-of select=""$first"" />
    </e_suppliers>
  </xsl:if>
  <xsl:if test=""$remaining"">
    <xsl:call-template name=""output-esuppliers"">
      <xsl:with-param name=""list"" select=""$remaining"" />
      <xsl:with-param name=""delimiter"">
        <xsl:value-of select=""$delimiter"" />
      </xsl:with-param>
    </xsl:call-template>
  </xsl:if>
</xsl:template>
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
