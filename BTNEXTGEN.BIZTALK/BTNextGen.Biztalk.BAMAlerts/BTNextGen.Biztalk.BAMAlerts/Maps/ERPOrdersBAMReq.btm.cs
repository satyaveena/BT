namespace BTNextGen.Biztalk.BAMAlerts {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.Biztalk.BAMAlerts.BamAlertTrigger", typeof(global::BTNextGen.Biztalk.BAMAlerts.BamAlertTrigger))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.Biztalk.BAMAlerts.TypedProcedure_dbo+procGetNoAckERPOrdersBAM", typeof(global::BTNextGen.Biztalk.BAMAlerts.TypedProcedure_dbo.procGetNoAckERPOrdersBAM))]
    public sealed class ERPOrdersBAMReq : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0"" version=""1.0"" xmlns:ns3=""http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procGetNoAckERPOrdersBAM"" xmlns:s0=""http://BTNextGen.Biztalk.BAMAlerts.BamAlertTrigger"" xmlns:ns0=""http://schemas.microsoft.com/Sql/2008/05/TypedProcedures/dbo"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s0:BAMAlertTrigger"" />
  </xsl:template>
  <xsl:template match=""/s0:BAMAlertTrigger"">
    <ns0:procGetNoAckERPOrdersBAM>
      <ns0:DURATION>
        <xsl:value-of select=""Duration/text()"" />
      </ns0:DURATION>
    </ns0:procGetNoAckERPOrdersBAM>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"BTNextGen.Biztalk.BAMAlerts.BamAlertTrigger";
        
        private const string _strTrgSchemasList0 = @"BTNextGen.Biztalk.BAMAlerts.TypedProcedure_dbo+procGetNoAckERPOrdersBAM";
        
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
                _SrcSchemas[0] = @"BTNextGen.Biztalk.BAMAlerts.BamAlertTrigger";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"BTNextGen.Biztalk.BAMAlerts.TypedProcedure_dbo+procGetNoAckERPOrdersBAM";
                return _TrgSchemas;
            }
        }
    }
}
