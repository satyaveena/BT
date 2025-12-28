namespace BTNextGen.BizTalk.Profiles.Maps {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.BizTalk.Profiles.Schemas.ProcedureResultSet_dbo_procGetOrganization+StoredProcedureResultSet0", typeof(global::BTNextGen.BizTalk.Profiles.Schemas.ProcedureResultSet_dbo_procGetOrganization.StoredProcedureResultSet0))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.BizTalk.CS.Profiles.Schemas.BTProfile", typeof(global::BTNextGen.BizTalk.CS.Profiles.Schemas.BTProfile))]
    public sealed class BTProdLookUpLegacy_To_BTProdLookUpCommerceServer : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0 userCSharp"" version=""1.0"" xmlns:s0=""http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procGetOrganization"" xmlns:userCSharp=""http://schemas.microsoft.com/BizTalk/2003/userCSharp"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s0:StoredProcedureResultSet0"" />
  </xsl:template>
  <xsl:template match=""/s0:StoredProcedureResultSet0"">
    <ProfileDocument>
      <BTProductLookup>
        <BTNextGen>
          <xsl:variable name=""var:v1"" select=""userCSharp:GetGuid()"" />
          <product_lookup_id>
            <xsl:value-of select=""$var:v1"" />
          </product_lookup_id>
          <xsl:if test=""s0:isbn_link_displayed"">
            <isbn_link_displayed>
              <xsl:value-of select=""s0:isbn_link_displayed/text()"" />
            </isbn_link_displayed>
          </xsl:if>
          <xsl:if test=""s0:isbn_lookup_code"">
            <isbn_lookup_code>
              <xsl:value-of select=""s0:isbn_lookup_code/text()"" />
            </isbn_lookup_code>
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
          <xsl:if test=""s0:prod_suffix_lookup"">
            <prod_suffix_lookup>
              <xsl:value-of select=""s0:prod_suffix_lookup/text()"" />
            </prod_suffix_lookup>
          </xsl:if>
        </BTNextGen>
      </BTProductLookup>
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
