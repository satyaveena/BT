namespace CartMigration {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"CartMigration.CartMigrationRequestBackup", typeof(global::CartMigration.CartMigrationRequestBackup))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"CartMigration.CartMigrationErrorProcedure_dbo+procSetBasketError", typeof(global::CartMigration.CartMigrationErrorProcedure_dbo.procSetBasketError))]
    public sealed class TransformCSRequestToTS3SQL : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0 userCSharp"" version=""1.0"" xmlns:ns0=""http://schemas.microsoft.com/Sql/2008/05/Procedures/dbo"" xmlns:s0=""http://CartMigration.CartMigrationRequestBackup"" xmlns:ns3=""http://schemas.datacontract.org/2004/07/System.Data"" xmlns:userCSharp=""http://schemas.microsoft.com/BizTalk/2003/userCSharp"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s0:CartMigrationRequestBackup"" />
  </xsl:template>
  <xsl:template match=""/s0:CartMigrationRequestBackup"">
    <xsl:variable name=""var:v8"" select=""userCSharp:StringTrimLeft(&quot;Http&quot;)"" />
    <ns0:procSetBasketError>
      <xsl:variable name=""var:v1"" select=""userCSharp:InitCumulativeConcat(0)"" />
      <xsl:for-each select=""/s0:CartMigrationRequestBackup/BasketList/BasketDetails"">
        <xsl:variable name=""var:v2"" select=""userCSharp:LogicalEq(string(LegacySourceSystem/text()) , &quot;TS3&quot;)"" />
        <xsl:if test=""string($var:v2)='true'"">
          <xsl:variable name=""var:v3"" select=""LegacyBasketID/text()"" />
          <xsl:variable name=""var:v4"" select=""userCSharp:StringConcat(string($var:v3) , &quot;,&quot;)"" />
          <xsl:variable name=""var:v5"" select=""userCSharp:AddToCumulativeConcat(0,string($var:v4),&quot;1000&quot;)"" />
        </xsl:if>
      </xsl:for-each>
      <xsl:variable name=""var:v6"" select=""userCSharp:GetCumulativeConcat(0)"" />
      <xsl:variable name=""var:v7"" select=""userCSharp:RemoveLastComma(string($var:v6))"" />
      <ns0:pstrHdrIDS>
        <xsl:value-of select=""$var:v7"" />
      </ns0:pstrHdrIDS>
      <ns0:pstrLoadStatus>
        <xsl:text>Failed</xsl:text>
      </ns0:pstrLoadStatus>
      <ns0:pstrSourceSystem>
        <xsl:text>TS3</xsl:text>
      </ns0:pstrSourceSystem>
      <ns0:pstrErrorMessage>
        <xsl:value-of select=""$var:v8"" />
      </ns0:pstrErrorMessage>
    </ns0:procSetBasketError>
  </xsl:template>
  <msxsl:script language=""C#"" implements-prefix=""userCSharp""><![CDATA[
public string InitCumulativeConcat(int index)
{
	if (index >= 0)
	{
		if (index >= myCumulativeConcatArray.Count)
		{
			int i = myCumulativeConcatArray.Count;
			for (; i<=index; i++)
			{
				myCumulativeConcatArray.Add("""");
			}
		}
		else
		{
			myCumulativeConcatArray[index] = """";
		}
	}
	return """";
}

public System.Collections.ArrayList myCumulativeConcatArray = new System.Collections.ArrayList();

public string AddToCumulativeConcat(int index, string val, string notused)
{
	if (index < 0 || index >= myCumulativeConcatArray.Count)
	{
		return """";
	}
	myCumulativeConcatArray[index] = (string)(myCumulativeConcatArray[index]) + val;
	return myCumulativeConcatArray[index].ToString();
}

public string GetCumulativeConcat(int index)
{
	if (index < 0 || index >= myCumulativeConcatArray.Count)
	{
		return """";
	}
	return myCumulativeConcatArray[index].ToString();
}

public string StringConcat(string param0, string param1)
{
   return param0 + param1;
}


///*Uncomment the following code for a sample Inline C# function
//that concatenates two inputs. Change the number of parameters of
//this function to be equal to the number of inputs connected to this functoid.*/

public string RemoveLastComma(string Input)
{
	return Input.Substring(0, Input.Length-1); 
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


public string StringTrimLeft(string str)
{
	if (str == null)
	{
		return """";
	}
	return str.TrimStart(null);
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


]]></msxsl:script>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"CartMigration.CartMigrationRequestBackup";
        
        private const string _strTrgSchemasList0 = @"CartMigration.CartMigrationErrorProcedure_dbo+procSetBasketError";
        
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
                _SrcSchemas[0] = @"CartMigration.CartMigrationRequestBackup";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"CartMigration.CartMigrationErrorProcedure_dbo+procSetBasketError";
                return _TrgSchemas;
            }
        }
    }
}
