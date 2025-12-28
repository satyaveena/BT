namespace BTNextGen.BizTalk.Profiles.Maps {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.BizTalk.Profiles.Schemas.ProcedureResultSet_dbo_procGetBTAccount+StoredProcedureResultSet0", typeof(global::BTNextGen.BizTalk.Profiles.Schemas.ProcedureResultSet_dbo_procGetBTAccount.StoredProcedureResultSet0))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.BizTalk.CS.Profiles.Schemas.BTProfile", typeof(global::BTNextGen.BizTalk.CS.Profiles.Schemas.BTProfile))]
    public sealed class AccountsLegacy_To_AccountsCommerce : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0 userCSharp"" version=""1.0"" xmlns:s0=""http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procGetBTAccount"" xmlns:userCSharp=""http://schemas.microsoft.com/BizTalk/2003/userCSharp"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s0:StoredProcedureResultSet0"" />
  </xsl:template>
  <xsl:template match=""/s0:StoredProcedureResultSet0"">
    <xsl:variable name=""var:v1"" select=""userCSharp:StringConcat(&quot;;&quot;)"" />
    <ProfileDocument>
      <BTAccount>
        <BTNextGen>
          <xsl:if test=""s0:ShipToAccountGUID"">
            <account_id>
              <xsl:value-of select=""s0:ShipToAccountGUID/text()"" />
            </account_id>
          </xsl:if>
          <xsl:if test=""s0:u_disable_reason_code"">
            <disable_reason_code>
              <xsl:value-of select=""s0:u_disable_reason_code/text()"" />
            </disable_reason_code>
          </xsl:if>
          <xsl:if test=""s0:dt_disable_date"">
            <disable_date>
              <xsl:value-of select=""s0:dt_disable_date/text()"" />
            </disable_date>
          </xsl:if>
          <xsl:if test=""s0:max_copy_per_line"">
            <max_copy_per_line>
              <xsl:value-of select=""s0:max_copy_per_line/text()"" />
            </max_copy_per_line>
          </xsl:if>
          <xsl:if test=""s0:b_is_disabled"">
            <is_disabled>
              <xsl:value-of select=""s0:b_is_disabled/text()"" />
            </is_disabled>
          </xsl:if>
          <xsl:if test=""s0:account_alias"">
            <account_alias>
              <xsl:value-of select=""s0:account_alias/text()"" />
            </account_alias>
          </xsl:if>
          <xsl:if test=""s0:u_disable_user_id"">
            <disable_user_id>
              <xsl:value-of select=""s0:u_disable_user_id/text()"" />
            </disable_user_id>
          </xsl:if>
          <xsl:call-template name=""output-UserCreateCartsList"">
            <xsl:with-param name=""list"" select=""string(s0:u_users_create_cart/text())"" />
            <xsl:with-param name=""delimiter"" select=""string($var:v1)"" />
          </xsl:call-template>
          <xsl:call-template name=""output-UsersViewOrderList"">
            <xsl:with-param name=""list"" select=""string(s0:u_users_view_order/text())"" />
            <xsl:with-param name=""delimiter"" select=""string($var:v1)"" />
          </xsl:call-template>
          <xsl:if test=""s0:CSOrgGUID"">
            <org_id>
              <xsl:value-of select=""s0:CSOrgGUID/text()"" />
            </org_id>
          </xsl:if>
          <xsl:if test=""s0:legacy_source_system"">
            <legacy_source_system>
              <xsl:value-of select=""s0:legacy_source_system/text()"" />
            </legacy_source_system>
          </xsl:if>
          <xsl:if test=""s0:legacy_account_id"">
            <legacy_account_id>
              <xsl:value-of select=""s0:legacy_account_id/text()"" />
            </legacy_account_id>
          </xsl:if>
          <xsl:if test=""s0:legacy_org_id"">
            <legacy_org_id>
              <xsl:value-of select=""s0:legacy_org_id/text()"" />
            </legacy_org_id>
          </xsl:if>
        </BTNextGen>
      </BTAccount>
    </ProfileDocument>
  </xsl:template>
  <msxsl:script language=""C#"" implements-prefix=""userCSharp""><![CDATA[
public string StringConcat(string param0)
{
   return param0;
}



]]></msxsl:script>
  <xsl:template name=""output-UserCreateCartsList"">
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
    <users_create_cart>
      <xsl:value-of select=""$first"" />
    </users_create_cart>
  </xsl:if>
  <xsl:if test=""$remaining"">
    <xsl:call-template name=""output-UserCreateCartsList"">
      <xsl:with-param name=""list"" select=""$remaining"" />
      <xsl:with-param name=""delimiter"">
        <xsl:value-of select=""$delimiter"" />
      </xsl:with-param>
    </xsl:call-template>
  </xsl:if>
</xsl:template>
  <xsl:template name=""output-UsersViewOrderList"">
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
    <users_view_order>
      <xsl:value-of select=""$first"" />
    </users_view_order>
  </xsl:if>
  <xsl:if test=""$remaining"">
    <xsl:call-template name=""output-UsersViewOrderList"">
      <xsl:with-param name=""list"" select=""$remaining"" />
      <xsl:with-param name=""delimiter"">
        <xsl:value-of select=""$delimiter"" />
      </xsl:with-param>
    </xsl:call-template>
  </xsl:if>
</xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"BTNextGen.BizTalk.Profiles.Schemas.ProcedureResultSet_dbo_procGetBTAccount+StoredProcedureResultSet0";
        
        private const global::BTNextGen.BizTalk.Profiles.Schemas.ProcedureResultSet_dbo_procGetBTAccount.StoredProcedureResultSet0 _srcSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"BTNextGen.BizTalk.Profiles.Schemas.ProcedureResultSet_dbo_procGetBTAccount+StoredProcedureResultSet0";
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
