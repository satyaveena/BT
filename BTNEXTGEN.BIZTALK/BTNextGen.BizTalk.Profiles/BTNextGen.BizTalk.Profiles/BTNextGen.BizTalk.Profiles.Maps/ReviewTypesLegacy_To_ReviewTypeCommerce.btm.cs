namespace BTNextGen.BizTalk.Profiles.Maps {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.BizTalk.Profiles.Schemas.ProcedureResultSet_dbo_procGetBTUserReviewType+StoredProcedureResultSet0", typeof(global::BTNextGen.BizTalk.Profiles.Schemas.ProcedureResultSet_dbo_procGetBTUserReviewType.StoredProcedureResultSet0))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.BizTalk.CS.Profiles.Schemas.BTProfile", typeof(global::BTNextGen.BizTalk.CS.Profiles.Schemas.BTProfile))]
    public sealed class ReviewTypesLegacy_To_ReviewTypeCommerce : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0 userCSharp"" version=""1.0"" xmlns:s0=""http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procGetBTUserReviewType"" xmlns:userCSharp=""http://schemas.microsoft.com/BizTalk/2003/userCSharp"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s0:StoredProcedureResultSet0"" />
  </xsl:template>
  <xsl:template match=""/s0:StoredProcedureResultSet0"">
    <xsl:variable name=""var:v2"" select=""userCSharp:StringLowerCase(&quot;ToBeReplaced&quot;)"" />
    <ProfileDocument>
      <BTUserReviewType>
        <GeneralInfo>
          <xsl:variable name=""var:v1"" select=""userCSharp:GetGuid()"" />
          <userReview_id>
            <xsl:value-of select=""$var:v1"" />
          </userReview_id>
        </GeneralInfo>
        <BTNextGen>
          <user_id>
            <xsl:value-of select=""$var:v2"" />
          </user_id>
          <xsl:if test=""s0:review_type_id"">
            <review_type_id>
              <xsl:value-of select=""s0:review_type_id/text()"" />
            </review_type_id>
          </xsl:if>
          <xsl:if test=""s0:ordinal"">
            <ordinal>
              <xsl:value-of select=""s0:ordinal/text()"" />
            </ordinal>
          </xsl:if>
        </BTNextGen>
      </BTUserReviewType>
    </ProfileDocument>
  </xsl:template>
  <msxsl:script language=""C#"" implements-prefix=""userCSharp""><![CDATA[
public string StringLowerCase(string str)
{
	if (str == null)
	{
		return """";
	}
	return str.ToLower(System.Globalization.CultureInfo.InvariantCulture);
}


///*Uncomment the following code for a sample Inline C# function
//that concatenates two inputs. Change the number of parameters of
//this function to be equal to the number of inputs connected to this functoid.*/

public string GetGuid()
{
 return ""{""+System.Guid.NewGuid().ToString()+""}"";

}


]]></msxsl:script>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"BTNextGen.BizTalk.Profiles.Schemas.ProcedureResultSet_dbo_procGetBTUserReviewType+StoredProcedureResultSet0";
        
        private const global::BTNextGen.BizTalk.Profiles.Schemas.ProcedureResultSet_dbo_procGetBTUserReviewType.StoredProcedureResultSet0 _srcSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"BTNextGen.BizTalk.Profiles.Schemas.ProcedureResultSet_dbo_procGetBTUserReviewType+StoredProcedureResultSet0";
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
