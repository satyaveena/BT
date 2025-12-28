namespace BTNextGen.BizTalk.Profiles.Maps {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.BizTalk.Profiles.Schemas.ProcedureResultSet_dbo_procGetUser+StoredProcedureResultSet0", typeof(global::BTNextGen.BizTalk.Profiles.Schemas.ProcedureResultSet_dbo_procGetUser.StoredProcedureResultSet0))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.BizTalk.CS.Profiles.Schemas.BTProfile", typeof(global::BTNextGen.BizTalk.CS.Profiles.Schemas.BTProfile))]
    public sealed class UserProfileLegacy_To_UserProfileCommerceServer : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0 userCSharp"" version=""1.0"" xmlns:s0=""http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procGetUser"" xmlns:userCSharp=""http://schemas.microsoft.com/BizTalk/2003/userCSharp"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s0:StoredProcedureResultSet0"" />
  </xsl:template>
  <xsl:template match=""/s0:StoredProcedureResultSet0"">
    <xsl:variable name=""var:v1"" select=""userCSharp:StringConcat(&quot;;&quot;)"" />
    <ProfileDocument>
      <UserObject>
        <AccountInfo>
          <xsl:if test=""s0:org_id"">
            <org_id>
              <xsl:value-of select=""s0:org_id/text()"" />
            </org_id>
          </xsl:if>
          <xsl:call-template name=""output-AccountCreateCartsList"">
            <xsl:with-param name=""list"" select=""string(s0:u_accounts_create_carts/text())"" />
            <xsl:with-param name=""delimiter"" select=""string($var:v1)"" />
          </xsl:call-template>
          <xsl:call-template name=""output-AccountViewOrderList"">
            <xsl:with-param name=""list"" select=""string(s0:u_accounts_view_orders/text())"" />
            <xsl:with-param name=""delimiter"" select=""string($var:v1)"" />
          </xsl:call-template>
          <xsl:if test=""s0:account_status"">
            <account_status>
              <xsl:value-of select=""s0:account_status/text()"" />
            </account_status>
          </xsl:if>
        </AccountInfo>
        <GeneralInfo>
          <xsl:if test=""s0:user_id"">
            <user_id>
              <xsl:value-of select=""s0:user_id/text()"" />
            </user_id>
          </xsl:if>
          <xsl:if test=""s0:user_type"">
            <user_type>
              <xsl:value-of select=""s0:user_type/text()"" />
            </user_type>
          </xsl:if>
          <xsl:if test=""s0:tel_number"">
            <tel_number>
              <xsl:value-of select=""s0:tel_number/text()"" />
            </tel_number>
          </xsl:if>
          <xsl:if test=""s0:email_address"">
            <email_address>
              <xsl:value-of select=""s0:email_address/text()"" />
            </email_address>
          </xsl:if>
          <xsl:if test=""s0:user_security_password"">
            <user_security_password>
              <xsl:value-of select=""s0:user_security_password/text()"" />
            </user_security_password>
          </xsl:if>
          <xsl:if test=""s0:last_name"">
            <last_name>
              <xsl:value-of select=""s0:last_name/text()"" />
            </last_name>
          </xsl:if>
          <xsl:if test=""s0:first_name"">
            <first_name>
              <xsl:value-of select=""s0:first_name/text()"" />
            </first_name>
          </xsl:if>
        </GeneralInfo>
        <BTNextGen>
          <xsl:if test=""s0:user_name"">
            <user_name>
              <xsl:value-of select=""s0:user_name/text()"" />
            </user_name>
          </xsl:if>
          <default_book_account>
            <xsl:value-of select=""s0:u_default_book_account/text()"" />
          </default_book_account>
          <default_entertainment_account>
            <xsl:value-of select=""s0:u_default_entertainment_account/text()"" />
          </default_entertainment_account>
          <roletypes>
            <xsl:value-of select=""s0:u_roletypes/text()"" />
          </roletypes>
          <xsl:call-template name=""output-AudienceTypesList"">
            <xsl:with-param name=""list"" select=""string(s0:u_audience_types/text())"" />
            <xsl:with-param name=""delimiter"" select=""string($var:v1)"" />
          </xsl:call-template>
          <product_type_list>
            <xsl:value-of select=""s0:u_product_types/text()"" />
          </product_type_list>
          <xsl:if test=""s0:function2"">
            <function_list>
              <xsl:value-of select=""s0:function2/text()"" />
            </function_list>
          </xsl:if>
          <xsl:if test=""s0:function3"">
            <function_list>
              <xsl:value-of select=""s0:function3/text()"" />
            </function_list>
          </xsl:if>
          <xsl:if test=""s0:function1"">
            <function_list>
              <xsl:value-of select=""s0:function1/text()"" />
            </function_list>
          </xsl:if>
          <xsl:if test=""s0:function4"">
            <function_list>
              <xsl:value-of select=""s0:function4/text()"" />
            </function_list>
          </xsl:if>
          <is_bt_employee>
            <xsl:value-of select=""s0:b_is_bt_employee/text()"" />
          </is_bt_employee>
          <bt_status>
            <xsl:value-of select=""s0:bt_status/text()"" />
          </bt_status>
          <xsl:if test=""s0:cust_user_name"">
            <cust_user_name>
              <xsl:value-of select=""s0:cust_user_name/text()"" />
            </cust_user_name>
          </xsl:if>
          <xsl:if test=""s0:default_home_delivery_account_id"">
            <default_home_delivery_account_id>
              <xsl:value-of select=""s0:default_home_delivery_account_id/text()"" />
            </default_home_delivery_account_id>
          </xsl:if>
          <xsl:if test=""s0:default_home_delivery_summary_view"">
            <default_home_delivery_summary_view>
              <xsl:value-of select=""s0:default_home_delivery_summary_view/text()"" />
            </default_home_delivery_summary_view>
          </xsl:if>
          <xsl:if test=""s0:legacy_source_system"">
            <legacy_source_system>
              <xsl:value-of select=""s0:legacy_source_system/text()"" />
            </legacy_source_system>
          </xsl:if>
          <xsl:if test=""s0:legacy_user_id"">
            <legacy_user_id>
              <xsl:value-of select=""s0:legacy_user_id/text()"" />
            </legacy_user_id>
          </xsl:if>
          <user_alias>
            <xsl:value-of select=""s0:user_alias/text()"" />
          </user_alias>
          <xsl:if test=""s0:legacy_created_date"">
            <legacy_created_date>
              <xsl:value-of select=""s0:legacy_created_date/text()"" />
            </legacy_created_date>
          </xsl:if>
          <xsl:if test=""s0:Legacy_updated_date"">
            <legacy_updated_date>
              <xsl:value-of select=""s0:Legacy_updated_date/text()"" />
            </legacy_updated_date>
          </xsl:if>
        </BTNextGen>
        <MyPreferences>
          <cart_format>
            <xsl:value-of select=""s0:u_cart_format/text()"" />
          </cart_format>
          <cart_sort_by>
            <xsl:value-of select=""s0:u_cart_sort_by/text()"" />
          </cart_sort_by>
          <cart_sort_order>
            <xsl:value-of select=""s0:u_cart_sort_order/text()"" />
          </cart_sort_order>
          <default_duplicate_carts>
            <xsl:value-of select=""s0:u_default_duplicate_carts/text()"" />
          </default_duplicate_carts>
          <default_duplicate_orders>
            <xsl:value-of select=""s0:u_default_duplicate_orders/text()"" />
          </default_duplicate_orders>
          <xsl:call-template name=""product_type_filter_list"">
            <xsl:with-param name=""list"" select=""string(s0:u_product_type_filter/text())"" />
            <xsl:with-param name=""delimiter"" select=""string($var:v1)"" />
          </xsl:call-template>
          <result_format>
            <xsl:value-of select=""s0:u_result_format/text()"" />
          </result_format>
          <result_sort_by>
            <xsl:value-of select=""s0:u_result_sort_by/text()"" />
          </result_sort_by>
          <result_sort_order>
            <xsl:value-of select=""s0:u_result_sort_order/text()"" />
          </result_sort_order>
          <xsl:if test=""s0:home_delivery_allowed"">
            <home_delivery_allowed>
              <xsl:value-of select=""s0:home_delivery_allowed/text()"" />
            </home_delivery_allowed>
          </xsl:if>
          <xsl:variable name=""var:v2"" select=""userCSharp:MyConcatsRSCFWE(string(s0:b_remove_share_carts_folder_when_empty/text()))"" />
          <remove_share_carts_folder_when_empty>
            <xsl:value-of select=""$var:v2"" />
          </remove_share_carts_folder_when_empty>
        </MyPreferences>
      </UserObject>
    </ProfileDocument>
  </xsl:template>
  <msxsl:script language=""C#"" implements-prefix=""userCSharp""><![CDATA[
public string StringConcat(string param0)
{
   return param0;
}


public bool MyConcatsRSCFWE(string param1)
{
string one = ""1"";
 if (param1.Equals(one.ToString()))
{ return true; }
return false;
}



]]></msxsl:script>
  <xsl:template name=""output-AudienceTypesList"">
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
    <audience_types>
      <xsl:value-of select=""$first"" />
    </audience_types>
  </xsl:if>
  <xsl:if test=""$remaining"">
    <xsl:call-template name=""output-AudienceTypesList"">
      <xsl:with-param name=""list"" select=""$remaining"" />
      <xsl:with-param name=""delimiter"">
        <xsl:value-of select=""$delimiter"" />
      </xsl:with-param>
    </xsl:call-template>
  </xsl:if>
</xsl:template>
  <xsl:template name=""output-AccountCreateCartsList"">
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
    <account_create_carts>
      <xsl:value-of select=""$first"" />
    </account_create_carts>
  </xsl:if>
  <xsl:if test=""$remaining"">
    <xsl:call-template name=""output-AccountCreateCartsList"">
      <xsl:with-param name=""list"" select=""$remaining"" />
      <xsl:with-param name=""delimiter"">
        <xsl:value-of select=""$delimiter"" />
      </xsl:with-param>
    </xsl:call-template>
  </xsl:if>
</xsl:template>
  <xsl:template name=""output-AccountViewOrderList"">
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
    <account_view_orders>
      <xsl:value-of select=""$first"" />
    </account_view_orders>
  </xsl:if>
  <xsl:if test=""$remaining"">
    <xsl:call-template name=""output-AccountViewOrderList"">
      <xsl:with-param name=""list"" select=""$remaining"" />
      <xsl:with-param name=""delimiter"">
        <xsl:value-of select=""$delimiter"" />
      </xsl:with-param>
    </xsl:call-template>
  </xsl:if>
</xsl:template>
  <xsl:template name=""product_type_filter_list"">
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
    <product_type_filter>
      <xsl:value-of select=""$first"" />
    </product_type_filter>
  </xsl:if>
  <xsl:if test=""$remaining"">
    <xsl:call-template name=""product_type_filter_list"">
      <xsl:with-param name=""list"" select=""$remaining"" />
      <xsl:with-param name=""delimiter"">
        <xsl:value-of select=""$delimiter"" />
      </xsl:with-param>
    </xsl:call-template>
  </xsl:if>
</xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"BTNextGen.BizTalk.Profiles.Schemas.ProcedureResultSet_dbo_procGetUser+StoredProcedureResultSet0";
        
        private const global::BTNextGen.BizTalk.Profiles.Schemas.ProcedureResultSet_dbo_procGetUser.StoredProcedureResultSet0 _srcSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"BTNextGen.BizTalk.Profiles.Schemas.ProcedureResultSet_dbo_procGetUser+StoredProcedureResultSet0";
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
