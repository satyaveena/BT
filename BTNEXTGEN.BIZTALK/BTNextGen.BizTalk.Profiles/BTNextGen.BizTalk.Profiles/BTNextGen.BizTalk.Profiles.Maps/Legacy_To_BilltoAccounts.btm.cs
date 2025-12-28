namespace BTNextGen.BizTalk.Profiles.Maps {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.BizTalk.Profiles.Schemas.ProcedureResultSet_dbo_procGetBTAccount+StoredProcedureResultSet0", typeof(global::BTNextGen.BizTalk.Profiles.Schemas.ProcedureResultSet_dbo_procGetBTAccount.StoredProcedureResultSet0))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.BizTalk.CS.Profiles.Schemas.BTProfile", typeof(global::BTNextGen.BizTalk.CS.Profiles.Schemas.BTProfile))]
    public sealed class Legacy_To_BilltoAccounts : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0"" version=""1.0"" xmlns:s0=""http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procGetBTAccount"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s0:StoredProcedureResultSet0"" />
  </xsl:template>
  <xsl:template match=""/s0:StoredProcedureResultSet0"">
    <ProfileDocument>
      <BTAccount>
        <BTNextGen>
          <account_id>
            <xsl:value-of select=""s0:BILLTOAccountGUID/text()"" />
          </account_id>
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
          <xsl:if test=""s0:legacy_org_id"">
            <legacy_org_id>
              <xsl:value-of select=""s0:legacy_org_id/text()"" />
            </legacy_org_id>
          </xsl:if>
        </BTNextGen>
      </BTAccount>
    </ProfileDocument>
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
