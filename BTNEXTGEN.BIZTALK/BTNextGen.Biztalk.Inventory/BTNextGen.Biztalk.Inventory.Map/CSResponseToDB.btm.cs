namespace BTNextGen.Biztalk.Inventory.Map {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.Biztalk.Inventory.Schema.procCSGetInventoryTypedProcedure_dbo+procCSGetInventoryResponse", typeof(global::BTNextGen.Biztalk.Inventory.Schema.procCSGetInventoryTypedProcedure_dbo.procCSGetInventoryResponse))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.Biztalk.Inventory.Schema.procCSAckInventoryTypedProcedure_dbo+procCSAckInventory", typeof(global::BTNextGen.Biztalk.Inventory.Schema.procCSAckInventoryTypedProcedure_dbo.procCSAckInventory))]
    public sealed class CSResponseToDB : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0 userCSharp"" version=""1.0"" xmlns:ns0=""http://schemas.microsoft.com/Sql/2008/05/TypedProcedures/dbo"" xmlns:s0=""http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procCSGetInventory"" xmlns:ns3=""http://schemas.microsoft.com/Sql/2008/05/Types/TableTypes/dbo"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:userCSharp=""http://schemas.microsoft.com/BizTalk/2003/userCSharp"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/ns0:procCSGetInventoryResponse"" />
  </xsl:template>
  <xsl:template match=""/ns0:procCSGetInventoryResponse"">
    <ns0:procCSAckInventory>
      <ns0:CSAckInventory>
        <xsl:for-each select=""ns0:StoredProcedureResultSet0"">
          <xsl:for-each select=""s0:StoredProcedureResultSet0"">
            <xsl:variable name=""var:v2"" select=""userCSharp:StringConcat(&quot;Not All Inventory Data Feed Had Been Populated . Please Refer the ESB for CS Errors&quot;)"" />
            <ns3:udtCSAckInventory>
              <xsl:if test=""s0:BTKEY"">
                <xsl:variable name=""var:v1"" select=""string(s0:BTKEY/@xsi:nil) = 'true'"" />
                <xsl:if test=""string($var:v1)='true'"">
                  <ns3:BTKey>
                    <xsl:attribute name=""xsi:nil"">
                      <xsl:value-of select=""'true'"" />
                    </xsl:attribute>
                  </ns3:BTKey>
                </xsl:if>
                <xsl:if test=""string($var:v1)='false'"">
                  <ns3:BTKey>
                    <xsl:value-of select=""s0:BTKEY/text()"" />
                  </ns3:BTKey>
                </xsl:if>
              </xsl:if>
              <ns3:CSLoadError>
                <xsl:value-of select=""$var:v2"" />
              </ns3:CSLoadError>
            </ns3:udtCSAckInventory>
          </xsl:for-each>
        </xsl:for-each>
      </ns0:CSAckInventory>
    </ns0:procCSAckInventory>
  </xsl:template>
  <msxsl:script language=""C#"" implements-prefix=""userCSharp""><![CDATA[
public string StringConcat(string param0)
{
   return param0;
}



]]></msxsl:script>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"BTNextGen.Biztalk.Inventory.Schema.procCSGetInventoryTypedProcedure_dbo+procCSGetInventoryResponse";
        
        private const global::BTNextGen.Biztalk.Inventory.Schema.procCSGetInventoryTypedProcedure_dbo.procCSGetInventoryResponse _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"BTNextGen.Biztalk.Inventory.Schema.procCSAckInventoryTypedProcedure_dbo+procCSAckInventory";
        
        private const global::BTNextGen.Biztalk.Inventory.Schema.procCSAckInventoryTypedProcedure_dbo.procCSAckInventory _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"BTNextGen.Biztalk.Inventory.Schema.procCSGetInventoryTypedProcedure_dbo+procCSGetInventoryResponse";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"BTNextGen.Biztalk.Inventory.Schema.procCSAckInventoryTypedProcedure_dbo+procCSAckInventory";
                return _TrgSchemas;
            }
        }
    }
}
