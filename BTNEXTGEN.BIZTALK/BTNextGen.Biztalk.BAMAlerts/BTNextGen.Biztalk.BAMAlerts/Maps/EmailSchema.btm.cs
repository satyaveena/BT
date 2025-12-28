namespace BTNextGen.Biztalk.BAMAlerts {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.Biztalk.BAMAlerts.ProcedureResultSet_dbo_procGetNoAckERPOrdersBAM+StoredProcedureResultSet0", typeof(global::BTNextGen.Biztalk.BAMAlerts.ProcedureResultSet_dbo_procGetNoAckERPOrdersBAM.StoredProcedureResultSet0))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.Biztalk.BAMAlerts.Schema.MissingACKAlert", typeof(global::BTNextGen.Biztalk.BAMAlerts.Schema.MissingACKAlert))]
    public sealed class EmailSchema : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0"" version=""1.0"" xmlns:s0=""http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procGetNoAckERPOrdersBAM"" xmlns:ns0=""http://BTNextGen.Biztalk.BAMAlerts.Schema.MissingACKAlert"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s0:StoredProcedureResultSet0"" />
  </xsl:template>
  <xsl:template match=""/s0:StoredProcedureResultSet0"">
    <ns0:Group>
      <Detail>
        <xsl:if test=""s0:PONum"">
          <PONum>
            <xsl:value-of select=""s0:PONum/text()"" />
          </PONum>
        </xsl:if>
        <xsl:if test=""s0:TransNum"">
          <TransNo>
            <xsl:value-of select=""s0:TransNum/text()"" />
          </TransNo>
        </xsl:if>
        <xsl:if test=""s0:PORcvd"">
          <PORcvd>
            <xsl:value-of select=""s0:PORcvd/text()"" />
          </PORcvd>
        </xsl:if>
        <xsl:if test=""s0:POSentERP"">
          <ERPSent>
            <xsl:value-of select=""s0:POSentERP/text()"" />
          </ERPSent>
        </xsl:if>
        <xsl:if test=""s0:AccountNum"">
          <ERPAccountNo>
            <xsl:value-of select=""s0:AccountNum/text()"" />
          </ERPAccountNo>
        </xsl:if>
      </Detail>
    </ns0:Group>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"BTNextGen.Biztalk.BAMAlerts.ProcedureResultSet_dbo_procGetNoAckERPOrdersBAM+StoredProcedureResultSet0";
        
        private const string _strTrgSchemasList0 = @"BTNextGen.Biztalk.BAMAlerts.Schema.MissingACKAlert";
        
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
                _SrcSchemas[0] = @"BTNextGen.Biztalk.BAMAlerts.ProcedureResultSet_dbo_procGetNoAckERPOrdersBAM+StoredProcedureResultSet0";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"BTNextGen.Biztalk.BAMAlerts.Schema.MissingACKAlert";
                return _TrgSchemas;
            }
        }
    }
}
