namespace BTNextGen.Biztalk.Product.Map {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.Biztalk.Product.procGetRelationsTypedProcedure_dbo+procCSGetProductRelationshipsResponse", typeof(global::BTNextGen.Biztalk.Product.procGetRelationsTypedProcedure_dbo.procCSGetProductRelationshipsResponse))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.BizTalk.CS.Catalogue.Schemas.BTCatalog", typeof(global::BTNextGen.BizTalk.CS.Catalogue.Schemas.BTCatalog))]
    public sealed class DBResponse_To_CSRelationships : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s1 s0 userCSharp"" version=""1.0"" xmlns:s1=""http://schemas.microsoft.com/Sql/2008/05/TypedProcedures/dbo"" xmlns:s0=""http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procCSGetProductRelationships"" xmlns:userCSharp=""http://schemas.microsoft.com/BizTalk/2003/userCSharp"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s1:procCSGetProductRelationshipsResponse"" />
  </xsl:template>
  <xsl:template match=""/s1:procCSGetProductRelationshipsResponse"">
    <xsl:variable name=""var:v1"" select=""userCSharp:StringConcat(&quot;3.0&quot;)"" />
    <MSCommerceCatalogCollection2>
      <xsl:attribute name=""version"">
        <xsl:value-of select=""$var:v1"" />
      </xsl:attribute>
      <xsl:for-each select=""s1:StoredProcedureResultSet1"">
        <xsl:for-each select=""s0:StoredProcedureResultSet1"">
          <xsl:variable name=""var:v2"" select=""userCSharp:StringConcat(&quot;USD&quot;)"" />
          <xsl:variable name=""var:v3"" select=""userCSharp:DateCurrentDateTime()"" />
          <xsl:variable name=""var:v4"" select=""userCSharp:StringConcat(&quot;en-US&quot;)"" />
          <xsl:variable name=""var:v5"" select=""userCSharp:StringConcat(&quot;variantId&quot;)"" />
          <xsl:variable name=""var:v6"" select=""userCSharp:StringConcat(&quot;BTKey&quot;)"" />
          <Catalog>
            <xsl:attribute name=""currency"">
              <xsl:value-of select=""$var:v2"" />
            </xsl:attribute>
            <xsl:attribute name=""lastmodified"">
              <xsl:value-of select=""$var:v3"" />
            </xsl:attribute>
            <xsl:attribute name=""languages"">
              <xsl:value-of select=""$var:v4"" />
            </xsl:attribute>
            <xsl:if test=""s0:CatalogName"">
              <xsl:attribute name=""name"">
                <xsl:value-of select=""s0:CatalogName/text()"" />
              </xsl:attribute>
            </xsl:if>
            <xsl:attribute name=""startDate"">
              <xsl:value-of select=""$var:v3"" />
            </xsl:attribute>
            <xsl:attribute name=""endDate"">
              <xsl:value-of select=""$var:v3"" />
            </xsl:attribute>
            <xsl:attribute name=""variantUID"">
              <xsl:value-of select=""$var:v5"" />
            </xsl:attribute>
            <xsl:attribute name=""productUID"">
              <xsl:value-of select=""$var:v6"" />
            </xsl:attribute>
            <xsl:attribute name=""DefaultLanguage"">
              <xsl:value-of select=""$var:v4"" />
            </xsl:attribute>
            <xsl:attribute name=""ReportingLanguage"">
              <xsl:value-of select=""$var:v4"" />
            </xsl:attribute>
            <DisplayName>
              <xsl:if test=""s0:CatalogName"">
                <xsl:attribute name=""Value"">
                  <xsl:value-of select=""s0:CatalogName/text()"" />
                </xsl:attribute>
              </xsl:if>
              <xsl:attribute name=""language"">
                <xsl:value-of select=""$var:v4"" />
              </xsl:attribute>
            </DisplayName>
            <xsl:for-each select=""../../s1:StoredProcedureResultSet0"">
              <xsl:for-each select=""s0:StoredProcedureResultSet0"">
                <xsl:variable name=""var:v7"" select=""userCSharp:DateCurrentDateTime()"" />
                <xsl:variable name=""var:v8"" select=""userCSharp:StringLeft(string(s0:BTTitle/text()) , &quot;100&quot;)"" />
                <xsl:variable name=""var:v9"" select=""userCSharp:StringConcat(&quot;BTProduct&quot;)"" />
                <xsl:variable name=""var:v10"" select=""string(s0:BTTitle/text())"" />
                <xsl:variable name=""var:v11"" select=""userCSharp:StringLeft($var:v10 , &quot;128&quot;)"" />
                <xsl:variable name=""var:v12"" select=""userCSharp:StringConcat(&quot;en-US&quot;)"" />
                <xsl:variable name=""var:v13"" select=""userCSharp:StringConcat($var:v10 , &quot;:&quot; , string(s0:BTSubTitle/text()))"" />
                <xsl:variable name=""var:v14"" select=""userCSharp:LogicalEq(string(s0:BTProductRelations/text()) , &quot;&quot;)"" />
                <xsl:variable name=""var:v15"" select=""userCSharp:LogicalNot(string($var:v14))"" />
                <Product>
                  <xsl:if test=""s0:BTKEY"">
                    <xsl:attribute name=""id"">
                      <xsl:value-of select=""s0:BTKEY/text()"" />
                    </xsl:attribute>
                  </xsl:if>
                  <xsl:attribute name=""lastmodified"">
                    <xsl:value-of select=""$var:v7"" />
                  </xsl:attribute>
                  <xsl:attribute name=""name"">
                    <xsl:value-of select=""$var:v8"" />
                  </xsl:attribute>
                  <xsl:if test=""s0:BTKEY"">
                    <xsl:attribute name=""BTKey"">
                      <xsl:value-of select=""s0:BTKEY/text()"" />
                    </xsl:attribute>
                  </xsl:if>
                  <xsl:if test=""s0:BTSubTitle"">
                    <xsl:attribute name=""BTSubTitle"">
                      <xsl:value-of select=""s0:BTSubTitle/text()"" />
                    </xsl:attribute>
                  </xsl:if>
                  <xsl:attribute name=""Definition"">
                    <xsl:value-of select=""$var:v9"" />
                  </xsl:attribute>
                  <DisplayName>
                    <xsl:attribute name=""Value"">
                      <xsl:value-of select=""$var:v11"" />
                    </xsl:attribute>
                    <xsl:attribute name=""language"">
                      <xsl:value-of select=""$var:v12"" />
                    </xsl:attribute>
                  </DisplayName>
                  <Description>
                    <xsl:attribute name=""Value"">
                      <xsl:value-of select=""$var:v13"" />
                    </xsl:attribute>
                    <xsl:attribute name=""language"">
                      <xsl:value-of select=""$var:v12"" />
                    </xsl:attribute>
                  </Description>
                  <xsl:if test=""string($var:v15)='true'"">
                    <xsl:variable name=""var:v16"" select=""s0:BTProductRelations/text()"" />
                    <xsl:call-template name=""output-tokens"">
                      <xsl:with-param name=""list"" select=""string($var:v16)"" />
                      <xsl:with-param name=""delimiter"" select=""&quot;;&quot;"" />
                    </xsl:call-template>
                  </xsl:if>
                </Product>
              </xsl:for-each>
            </xsl:for-each>
          </Catalog>
        </xsl:for-each>
      </xsl:for-each>
    </MSCommerceCatalogCollection2>
  </xsl:template>
  <msxsl:script language=""C#"" implements-prefix=""userCSharp""><![CDATA[
public string StringConcat(string param0)
{
   return param0;
}


public string DateCurrentDateTime()
{
	DateTime dt = DateTime.Now;
	string curdate = dt.ToString(""yyyy-MM-dd"", System.Globalization.CultureInfo.InvariantCulture);
	string curtime = dt.ToString(""T"", System.Globalization.CultureInfo.InvariantCulture);
	string retval = curdate + ""T"" + curtime;
	return retval;
}


public string StringConcat(string param0, string param1, string param2)
{
   return param0 + param1 + param2;
}


public bool LogicalNot(string val)
{
	return !ValToBool(val);
}


public bool LogicalEq(string val1, string val2)
{
	bool ret = false;
	double d1 = 0;
	double d2 = 0;
	if (IsNumeric(val1, ref d1) && IsNumeric(val2, ref d2))
	{
		ret = d1 == d2;
	}
	else
	{
		ret = String.Compare(val1, val2, StringComparison.Ordinal) == 0;
	}
	return ret;
}


public string StringLeft(string str, string count)
{
	string retval = """";
	double d = 0;
	if (str != null && IsNumeric(count, ref d))
	{
		int i = (int) d;
		if (i > 0)
		{ 
			if (i <= str.Length)
			{
				retval = str.Substring(0, i);
			}
			else
			{
				retval = str;
			}
		}
	}
	return retval;
}


public bool IsNumeric(string val)
{
	if (val == null)
	{
		return false;
	}
	double d = 0;
	return Double.TryParse(val, System.Globalization.NumberStyles.AllowThousands | System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out d);
}

public bool IsNumeric(string val, ref double d)
{
	if (val == null)
	{
		return false;
	}
	return Double.TryParse(val, System.Globalization.NumberStyles.AllowThousands | System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out d);
}

public bool ValToBool(string val)
{
	if (val != null)
	{
		if (string.Compare(val, bool.TrueString, StringComparison.OrdinalIgnoreCase) == 0)
		{
			return true;
		}
		if (string.Compare(val, bool.FalseString, StringComparison.OrdinalIgnoreCase) == 0)
		{
			return false;
		}
		val = val.Trim();
		if (string.Compare(val, bool.TrueString, StringComparison.OrdinalIgnoreCase) == 0)
		{
			return true;
		}
		if (string.Compare(val, bool.FalseString, StringComparison.OrdinalIgnoreCase) == 0)
		{
			return false;
		}
		double d = 0;
		if (IsNumeric(val, ref d))
		{
			return (d > 0);
		}
	}
	return false;
}


]]></msxsl:script>
  <xsl:template name=""output-tokens"">
  <xsl:param name=""list"" />
  <xsl:param name=""delimiter"" />
  <xsl:variable name=""newlist"">
    <xsl:choose>
      <xsl:when test=""contains($list, $delimiter)"">
        <xsl:value-of select=""normalize-space($list)"" />
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select=""concat(normalize-space($list), $delimiter)"" />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:variable>
  <xsl:variable name=""first"" select=""substring-before($newlist, $delimiter)"" />
  <xsl:variable name=""remaining"" select=""substring-after($newlist, $delimiter)"" />

<xsl:variable name=""clog"">
<xsl:if test=""$first &gt;= 0 and $first &lt;=3000000"">
            <xsl:text>Book Catalog 1</xsl:text> 
</xsl:if>  
  <xsl:if test=""$first &gt;= 3000001 and $first &lt;=7000000"">
             <xsl:text>Book Catalog 2</xsl:text> 
</xsl:if>  
<xsl:if test=""$first &gt;= 7000001 and $first &lt;=10000000"">
            <xsl:text>Book Catalog 3</xsl:text>
</xsl:if>
<xsl:if test=""$first &gt;= 10000001 and $first &lt;=5999999999"">
            <xsl:text>Book Catalog 4</xsl:text> 
</xsl:if>  
  <xsl:if test=""$first &gt;=6000000000 and $first &lt;=7000000000"">
             <xsl:text>Entertainment Catalog 1</xsl:text> 
</xsl:if>  
<xsl:if test=""$first &gt;=7000000001 and $first &lt;=9999999999"">
	<xsl:text>Book Catalog 5</xsl:text>
</xsl:if>
</xsl:variable>


  <xsl:element name=""Relationship"">
   <xsl:attribute name=""name"">
      <xsl:text>Child</xsl:text>
    </xsl:attribute>
    <xsl:attribute name=""targetProduct"">
      <xsl:value-of select=""$first"" />
    </xsl:attribute>
    <xsl:attribute name=""targetCatalog"">
      <xsl:value-of select=""$clog"" />
     </xsl:attribute>  
   
  </xsl:element>
  <xsl:if test=""$remaining"">
    <xsl:call-template name=""output-tokens"">
      <xsl:with-param name=""list"" select=""$remaining"" />
      <xsl:with-param name=""delimiter"">
        <xsl:value-of select=""$delimiter"" />
      </xsl:with-param>
    </xsl:call-template>
  </xsl:if>
</xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"BTNextGen.Biztalk.Product.procGetRelationsTypedProcedure_dbo+procCSGetProductRelationshipsResponse";
        
        private const global::BTNextGen.Biztalk.Product.procGetRelationsTypedProcedure_dbo.procCSGetProductRelationshipsResponse _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"BTNextGen.BizTalk.CS.Catalogue.Schemas.BTCatalog";
        
        private const global::BTNextGen.BizTalk.CS.Catalogue.Schemas.BTCatalog _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"BTNextGen.Biztalk.Product.procGetRelationsTypedProcedure_dbo+procCSGetProductRelationshipsResponse";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"BTNextGen.BizTalk.CS.Catalogue.Schemas.BTCatalog";
                return _TrgSchemas;
            }
        }
    }
}
