namespace BTNextGen.Biztalk.ExpiredCreditCards {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.BizTalk.CS.Profiles.Schemas.ProfilesQuery", typeof(global::BTNextGen.BizTalk.CS.Profiles.Schemas.ProfilesQuery))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.BizTalk.CS.Profiles.Schemas.ProfilesQueryResponseMessage+ProfileDocumentList", typeof(global::BTNextGen.BizTalk.CS.Profiles.Schemas.ProfilesQueryResponseMessage.ProfileDocumentList))]
    public sealed class Transform_3 : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0"" version=""1.0"" xmlns:s0=""http://schemas.microsoft.com/CommerceServer/2004/02/Expressions"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/CommerceServerProfilesQuery"" />
  </xsl:template>
  <xsl:template match=""/CommerceServerProfilesQuery"" />
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"BTNextGen.BizTalk.CS.Profiles.Schemas.ProfilesQuery";
        
        private const global::BTNextGen.BizTalk.CS.Profiles.Schemas.ProfilesQuery _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"BTNextGen.BizTalk.CS.Profiles.Schemas.ProfilesQueryResponseMessage+ProfileDocumentList";
        
        private const global::BTNextGen.BizTalk.CS.Profiles.Schemas.ProfilesQueryResponseMessage.ProfileDocumentList _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"BTNextGen.BizTalk.CS.Profiles.Schemas.ProfilesQuery";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"BTNextGen.BizTalk.CS.Profiles.Schemas.ProfilesQueryResponseMessage+ProfileDocumentList";
                return _TrgSchemas;
            }
        }
    }
}
