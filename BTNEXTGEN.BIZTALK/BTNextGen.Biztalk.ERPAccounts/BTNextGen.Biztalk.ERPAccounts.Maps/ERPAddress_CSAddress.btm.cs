namespace BTNextGen.Biztalk.ERPAccounts.Maps {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.Biztalk.ERPAccounts.Schemas.ProcedureResultSet_dbo_procCommerceServerGetAddresses+StoredProcedureResultSet0", typeof(global::BTNextGen.Biztalk.ERPAccounts.Schemas.ProcedureResultSet_dbo_procCommerceServerGetAddresses.StoredProcedureResultSet0))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.BizTalk.CS.Profiles.Schemas.BTProfile", typeof(global::BTNextGen.BizTalk.CS.Profiles.Schemas.BTProfile))]
    public sealed class ERPAddress_CSAddress : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0"" version=""1.0"" xmlns:s0=""http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procCommerceServerGetAddresses"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s0:StoredProcedureResultSet0"" />
  </xsl:template>
  <xsl:template match=""/s0:StoredProcedureResultSet0"">
    <ProfileDocument>
      <Address>
        <GeneralInfo>
          <xsl:if test=""s0:u_address_id"">
            <address_id>
              <xsl:value-of select=""s0:u_address_id/text()"" />
            </address_id>
          </xsl:if>
          <xsl:if test=""s0:u_address_line1"">
            <address_line1>
              <xsl:value-of select=""s0:u_address_line1/text()"" />
            </address_line1>
          </xsl:if>
          <xsl:if test=""s0:u_address_line2"">
            <address_line2>
              <xsl:value-of select=""s0:u_address_line2/text()"" />
            </address_line2>
          </xsl:if>
          <xsl:if test=""s0:u_city"">
            <city>
              <xsl:value-of select=""s0:u_city/text()"" />
            </city>
          </xsl:if>
          <xsl:if test=""s0:u_region_code"">
            <region_code>
              <xsl:value-of select=""s0:u_region_code/text()"" />
            </region_code>
          </xsl:if>
          <xsl:if test=""s0:u_postal_code"">
            <postal_code>
              <xsl:value-of select=""s0:u_postal_code/text()"" />
            </postal_code>
          </xsl:if>
          <xsl:if test=""s0:u_country_code"">
            <country_code>
              <xsl:value-of select=""s0:u_country_code/text()"" />
            </country_code>
          </xsl:if>
        </GeneralInfo>
        <BTNextGen>
          <xsl:if test=""s0:u_address_line3"">
            <address_line3>
              <xsl:value-of select=""s0:u_address_line3/text()"" />
            </address_line3>
          </xsl:if>
          <xsl:if test=""s0:u_address_line4"">
            <address_line4>
              <xsl:value-of select=""s0:u_address_line4/text()"" />
            </address_line4>
          </xsl:if>
          <xsl:if test=""s0:i_is_po_box"">
            <is_po_box>
              <xsl:value-of select=""s0:i_is_po_box/text()"" />
            </is_po_box>
          </xsl:if>
        </BTNextGen>
      </Address>
    </ProfileDocument>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"BTNextGen.Biztalk.ERPAccounts.Schemas.ProcedureResultSet_dbo_procCommerceServerGetAddresses+StoredProcedureResultSet0";
        
        private const global::BTNextGen.Biztalk.ERPAccounts.Schemas.ProcedureResultSet_dbo_procCommerceServerGetAddresses.StoredProcedureResultSet0 _srcSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"BTNextGen.Biztalk.ERPAccounts.Schemas.ProcedureResultSet_dbo_procCommerceServerGetAddresses+StoredProcedureResultSet0";
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
