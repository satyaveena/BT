namespace BTNextGen.Biztalk.BAMAlerts {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.Biztalk.BAMAlerts.TypedProcedure_dbo+procGetNoAckERPOrdersBAMResponse", typeof(global::BTNextGen.Biztalk.BAMAlerts.TypedProcedure_dbo.procGetNoAckERPOrdersBAMResponse))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.Biztalk.BAMAlerts.TableOperation_dbo_BT_BAMAlertHistory+Insert", typeof(global::BTNextGen.Biztalk.BAMAlerts.TableOperation_dbo_BT_BAMAlertHistory.Insert))]
    public sealed class InsertAlertHistoryInsert : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0 s1 userCSharp"" version=""1.0"" xmlns:s1=""http://schemas.microsoft.com/Sql/2008/05/TypedProcedures/dbo"" xmlns:s0=""http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procGetNoAckERPOrdersBAM"" xmlns:array=""http://schemas.microsoft.com/2003/10/Serialization/Arrays"" xmlns:ns3=""http://schemas.microsoft.com/Sql/2008/05/Types/Tables/dbo"" xmlns:ns0=""http://schemas.microsoft.com/Sql/2008/05/TableOp/dbo/BT_BAMAlertHistory"" xmlns:userCSharp=""http://schemas.microsoft.com/BizTalk/2003/userCSharp"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s1:procGetNoAckERPOrdersBAMResponse"" />
  </xsl:template>
  <xsl:template match=""/s1:procGetNoAckERPOrdersBAMResponse"">
    <ns0:Insert>
      <ns0:Rows>
        <xsl:for-each select=""s1:StoredProcedureResultSet0"">
          <xsl:for-each select=""s0:StoredProcedureResultSet0"">
            <xsl:variable name=""var:v1"" select=""userCSharp:StringConcat(&quot;ERP Orders Alerts&quot;)"" />
            <xsl:variable name=""var:v3"" select=""userCSharp:StringConcat(&quot;1&quot;)"" />
            <xsl:variable name=""var:v4"" select=""userCSharp:StringConcat(&quot;true&quot;)"" />
            <ns3:BT_BAMAlertHistory>
              <ns3:Interface_x0020_Name>
                <xsl:value-of select=""$var:v1"" />
              </ns3:Interface_x0020_Name>
              <xsl:if test=""s0:ActivityID"">
                <xsl:variable name=""var:v2"" select=""string(s0:ActivityID/@xsi:nil) = 'true'"" />
                <xsl:if test=""string($var:v2)='true'"">
                  <ns3:ActivityID>
                    <xsl:attribute name=""xsi:nil"">
                      <xsl:value-of select=""'true'"" />
                    </xsl:attribute>
                  </ns3:ActivityID>
                </xsl:if>
                <xsl:if test=""string($var:v2)='false'"">
                  <ns3:ActivityID>
                    <xsl:value-of select=""s0:ActivityID/text()"" />
                  </ns3:ActivityID>
                </xsl:if>
              </xsl:if>
              <ns3:RetryCount>
                <xsl:value-of select=""$var:v3"" />
              </ns3:RetryCount>
              <ns3:AlertExpired>
                <xsl:value-of select=""$var:v4"" />
              </ns3:AlertExpired>
            </ns3:BT_BAMAlertHistory>
          </xsl:for-each>
        </xsl:for-each>
      </ns0:Rows>
    </ns0:Insert>
  </xsl:template>
  <msxsl:script language=""C#"" implements-prefix=""userCSharp""><![CDATA[
public string StringConcat(string param0)
{
   return param0;
}



]]></msxsl:script>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"BTNextGen.Biztalk.BAMAlerts.TypedProcedure_dbo+procGetNoAckERPOrdersBAMResponse";
        
        private const string _strTrgSchemasList0 = @"BTNextGen.Biztalk.BAMAlerts.TableOperation_dbo_BT_BAMAlertHistory+Insert";
        
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
                _TrgSchemas[0] = @"BTNextGen.Biztalk.BAMAlerts.TableOperation_dbo_BT_BAMAlertHistory+Insert";
                return _TrgSchemas;
            }
        }
    }
}
