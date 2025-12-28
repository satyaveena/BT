namespace BTNextGen.Biztalk.BAMAlerts {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.Biztalk.BAMAlerts.TypedProcedure_dbo+procGetNoAckERPOrdersBAMResponse", typeof(global::BTNextGen.Biztalk.BAMAlerts.TypedProcedure_dbo.procGetNoAckERPOrdersBAMResponse))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.Biztalk.BAMAlerts.Schema.MissingACKAlert", typeof(global::BTNextGen.Biztalk.BAMAlerts.Schema.MissingACKAlert))]
    public sealed class EmailMessage : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0 s1 userCSharp"" version=""1.0"" xmlns:s1=""http://schemas.microsoft.com/Sql/2008/05/TypedProcedures/dbo"" xmlns:s0=""http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procGetNoAckERPOrdersBAM"" xmlns:ns0=""http://BTNextGen.Biztalk.BAMAlerts.Schema.MissingACKAlert"" xmlns:userCSharp=""http://schemas.microsoft.com/BizTalk/2003/userCSharp"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s1:procGetNoAckERPOrdersBAMResponse"" />
  </xsl:template>
  <xsl:template match=""/s1:procGetNoAckERPOrdersBAMResponse"">
    <xsl:variable name=""var:v1"" select=""userCSharp:StringConcat(&quot;PONum&quot;)"" />
    <xsl:variable name=""var:v2"" select=""userCSharp:StringConcat(&quot;TransmissionNo&quot;)"" />
    <xsl:variable name=""var:v3"" select=""userCSharp:StringConcat(&quot;PORcvd&quot;)"" />
    <xsl:variable name=""var:v4"" select=""userCSharp:StringConcat(&quot;ERPSent&quot;)"" />
    <xsl:variable name=""var:v5"" select=""userCSharp:StringConcat(&quot;ERPAccountNo&quot;)"" />
    <xsl:variable name=""var:v6"" select=""userCSharp:StringConcat(&quot;ERPSystem&quot;)"" />
    <xsl:variable name=""var:v7"" select=""userCSharp:StringConcat(&quot;                                         &quot;)"" />
    <ns0:Group>
      <Header>
        <PONum>
          <xsl:value-of select=""$var:v1"" />
        </PONum>
        <TransmissionNo>
          <xsl:value-of select=""$var:v2"" />
        </TransmissionNo>
        <PORcvd>
          <xsl:value-of select=""$var:v3"" />
        </PORcvd>
        <ERPSent>
          <xsl:value-of select=""$var:v4"" />
        </ERPSent>
        <ERPAccountNum>
          <xsl:value-of select=""$var:v5"" />
        </ERPAccountNum>
        <ERPSystem>
          <xsl:value-of select=""$var:v6"" />
        </ERPSystem>
      </Header>
      <EmptyLine>
        <Blank>
          <xsl:value-of select=""$var:v7"" />
        </Blank>
      </EmptyLine>
      <xsl:for-each select=""s1:StoredProcedureResultSet0"">
        <xsl:for-each select=""s0:StoredProcedureResultSet0"">
          <xsl:variable name=""var:v8"" select=""userCSharp:LogicalIsString(string(s0:PORcvd/text()))"" />
          <xsl:variable name=""var:v11"" select=""userCSharp:LogicalIsString(string(s0:POSentERP/text()))"" />
          <Detail>
            <xsl:if test=""s0:PONum"">
              <PONo>
                <xsl:value-of select=""s0:PONum/text()"" />
              </PONo>
            </xsl:if>
            <xsl:if test=""s0:TransNum"">
              <TransNo>
                <xsl:value-of select=""s0:TransNum/text()"" />
              </TransNo>
            </xsl:if>
            <xsl:if test=""string($var:v8)='true'"">
              <xsl:variable name=""var:v9"" select=""s0:PORcvd/text()"" />
              <xsl:variable name=""var:v10"" select=""userCSharp:FormatDate(string($var:v9))"" />
              <PORcvd>
                <xsl:value-of select=""$var:v10"" />
              </PORcvd>
            </xsl:if>
            <xsl:if test=""string($var:v11)='true'"">
              <xsl:variable name=""var:v12"" select=""s0:POSentERP/text()"" />
              <xsl:variable name=""var:v13"" select=""userCSharp:FormatDate(string($var:v12))"" />
              <ERPSent>
                <xsl:value-of select=""$var:v13"" />
              </ERPSent>
            </xsl:if>
            <xsl:if test=""s0:AccountNum"">
              <ERPAccountNum>
                <xsl:value-of select=""s0:AccountNum/text()"" />
              </ERPAccountNum>
            </xsl:if>
            <xsl:if test=""s0:TargetERP"">
              <ERPSystem>
                <xsl:value-of select=""s0:TargetERP/text()"" />
              </ERPSystem>
            </xsl:if>
          </Detail>
        </xsl:for-each>
      </xsl:for-each>
    </ns0:Group>
  </xsl:template>
  <msxsl:script language=""C#"" implements-prefix=""userCSharp""><![CDATA[
public string StringConcat()
{
   return """";
}


public string StringConcat(string param0)
{
   return param0;
}


public string FormatDate( string strdatetime)
{
if (strdatetime.Length > 20)
{
strdatetime=strdatetime.Substring(0,10) +"" ""+ strdatetime.Substring(11,8);

}
return strdatetime;	
 }


public bool LogicalIsString(string val)
{
	return (val != null && val !="""");
}



]]></msxsl:script>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"BTNextGen.Biztalk.BAMAlerts.TypedProcedure_dbo+procGetNoAckERPOrdersBAMResponse";
        
        private const global::BTNextGen.Biztalk.BAMAlerts.TypedProcedure_dbo.procGetNoAckERPOrdersBAMResponse _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"BTNextGen.Biztalk.BAMAlerts.Schema.MissingACKAlert";
        
        private const global::BTNextGen.Biztalk.BAMAlerts.Schema.MissingACKAlert _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"BTNextGen.Biztalk.BAMAlerts.TypedProcedure_dbo+procGetNoAckERPOrdersBAMResponse";
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
