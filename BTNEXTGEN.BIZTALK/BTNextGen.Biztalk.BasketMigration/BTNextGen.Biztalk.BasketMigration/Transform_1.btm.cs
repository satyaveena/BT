namespace BTNextGen.Biztalk.BasketMigration {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.Biztalk.BasketMigration.GetBasketIDTypedPolling_627+TypedPolling", typeof(global::BTNextGen.Biztalk.BasketMigration.GetBasketIDTypedPolling_627.TypedPolling))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.Biztalk.BasketMigration.ProfilesR2_tempuri_org+GetBasketDatasetXML", typeof(global::BTNextGen.Biztalk.BasketMigration.ProfilesR2_tempuri_org.GetBasketDatasetXML))]
    public sealed class Transform_1 : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0"" version=""1.0"" xmlns:ns0=""http://tempuri.org/"" xmlns:s0=""http://schemas.microsoft.com/Sql/2008/05/TypedPolling/_x0036_27"" xmlns:ns2=""http://schemas.datacontract.org/2004/07/IProfilesR2"" xmlns:ns1=""http://schemas.microsoft.com/2003/10/Serialization/"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s0:TypedPolling"" />
  </xsl:template>
  <xsl:template match=""/s0:TypedPolling"">
    <ns0:GetBasketDatasetXML>
      <xsl:if test=""s0:TypedPollingResultSet0/s0:TypedPollingResultSet0/s0:OrderGroupID"">
        <ns0:request>
          <xsl:value-of select=""s0:TypedPollingResultSet0/s0:TypedPollingResultSet0/s0:OrderGroupID/text()"" />
        </ns0:request>
      </xsl:if>
    </ns0:GetBasketDatasetXML>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"BTNextGen.Biztalk.BasketMigration.GetBasketIDTypedPolling_627+TypedPolling";
        
        private const string _strTrgSchemasList0 = @"BTNextGen.Biztalk.BasketMigration.ProfilesR2_tempuri_org+GetBasketDatasetXML";
        
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
                _SrcSchemas[0] = @"BTNextGen.Biztalk.BasketMigration.GetBasketIDTypedPolling_627+TypedPolling";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"BTNextGen.Biztalk.BasketMigration.ProfilesR2_tempuri_org+GetBasketDatasetXML";
                return _TrgSchemas;
            }
        }
    }
}
