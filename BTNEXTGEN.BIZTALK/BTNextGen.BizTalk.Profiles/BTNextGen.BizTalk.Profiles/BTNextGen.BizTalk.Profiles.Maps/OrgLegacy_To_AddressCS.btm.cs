namespace BTNextGen.BizTalk.Profiles.Maps {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.BizTalk.Profiles.Schemas.ProcedureResultSet_dbo_procGetOrganization+StoredProcedureResultSet0", typeof(global::BTNextGen.BizTalk.Profiles.Schemas.ProcedureResultSet_dbo_procGetOrganization.StoredProcedureResultSet0))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.BizTalk.CS.Profiles.Schemas.BTProfile", typeof(global::BTNextGen.BizTalk.CS.Profiles.Schemas.BTProfile))]
    public sealed class OrgLegacy_To_AddressCS : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0"" version=""1.0"" xmlns:s0=""http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procGetOrganization"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s0:StoredProcedureResultSet0"" />
  </xsl:template>
  <xsl:template match=""/s0:StoredProcedureResultSet0"">
    <ProfileDocument>
      <Address>
        <GeneralInfo>
          <xsl:if test=""s0:CSOrgGUID"">
            <address_id>
              <xsl:value-of select=""s0:CSOrgGUID/text()"" />
            </address_id>
          </xsl:if>
          <address_line1>
            <xsl:value-of select=""s0:Address_Line1/text()"" />
          </address_line1>
          <address_line2>
            <xsl:value-of select=""s0:Address_Line2/text()"" />
          </address_line2>
          <city>
            <xsl:value-of select=""s0:u_city/text()"" />
          </city>
          <region_code>
            <xsl:value-of select=""s0:u_region_code/text()"" />
          </region_code>
          <postal_code>
            <xsl:value-of select=""s0:u_postal_code/text()"" />
          </postal_code>
          <country_code>
            <xsl:value-of select=""s0:u_country_code/text()"" />
          </country_code>
        </GeneralInfo>
        <BTNextGen>
          <address_line3>
            <xsl:value-of select=""s0:Address_Line3/text()"" />
          </address_line3>
          <address_line4>
            <xsl:value-of select=""s0:Address_Line4/text()"" />
          </address_line4>
        </BTNextGen>
      </Address>
    </ProfileDocument>
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
