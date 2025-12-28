namespace BTNextGen.Biztalk.Inventory.Map {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.Biztalk.Inventory.Schema.procCSGetInventoryTypedProcedure_dbo+procCSGetInventoryResponse", typeof(global::BTNextGen.Biztalk.Inventory.Schema.procCSGetInventoryTypedProcedure_dbo.procCSGetInventoryResponse))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.BizTalk.CS.Inventory.Schemas.BTInventory", typeof(global::BTNextGen.BizTalk.CS.Inventory.Schemas.BTInventory))]
    public sealed class CSImportInventory : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s1 s0 userCSharp"" version=""1.0"" xmlns:s1=""http://schemas.microsoft.com/Sql/2008/05/TypedProcedures/dbo"" xmlns:s0=""http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procCSGetInventory"" xmlns:userCSharp=""http://schemas.microsoft.com/BizTalk/2003/userCSharp"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s1:procCSGetInventoryResponse"" />
  </xsl:template>
  <xsl:template match=""/s1:procCSGetInventoryResponse"">
    <xsl:variable name=""var:v1"" select=""userCSharp:StringConcat(&quot;1.0&quot;)"" />
    <MSInventoryCollection>
      <xsl:attribute name=""version"">
        <xsl:value-of select=""$var:v1"" />
      </xsl:attribute>
      <xsl:for-each select=""s1:StoredProcedureResultSet1"">
        <xsl:for-each select=""s0:StoredProcedureResultSet1"">
          <InventoryCatalog>
            <xsl:if test=""s0:InventoryCatalogName"">
              <xsl:attribute name=""name"">
                <xsl:value-of select=""s0:InventoryCatalogName/text()"" />
              </xsl:attribute>
            </xsl:if>
            <ProductCatalog>
              <xsl:if test=""s0:ProductCatalogName"">
                <xsl:attribute name=""name"">
                  <xsl:value-of select=""s0:ProductCatalogName/text()"" />
                </xsl:attribute>
              </xsl:if>
            </ProductCatalog>
            <xsl:for-each select=""../../s1:StoredProcedureResultSet0"">
              <xsl:for-each select=""s0:StoredProcedureResultSet0"">
                <xsl:variable name=""var:v2"" select=""userCSharp:DateCurrentDate()"" />
                <xsl:variable name=""var:v3"" select=""userCSharp:StringConcat(&quot;true&quot;)"" />
                <xsl:variable name=""var:v4"" select=""userCSharp:StringConcat(&quot;1&quot;)"" />
                <xsl:variable name=""var:v8"" select=""userCSharp:LogicalEq(string(s0:WarehouseCode/text()) , &quot;Warehouse1&quot;)"" />
                <xsl:variable name=""var:v13"" select=""string(s0:WarehouseCode/text())"" />
                <xsl:variable name=""var:v14"" select=""userCSharp:LogicalEq($var:v13 , &quot;Warehouse2&quot;)"" />
                <xsl:variable name=""var:v19"" select=""userCSharp:LogicalEq($var:v13 , &quot;Warehouse3&quot;)"" />
                <xsl:variable name=""var:v24"" select=""userCSharp:LogicalEq($var:v13 , &quot;Warehouse4&quot;)"" />
                <xsl:variable name=""var:v29"" select=""userCSharp:LogicalEq($var:v13 , &quot;Warehouse5&quot;)"" />
                <xsl:variable name=""var:v34"" select=""userCSharp:LogicalEq($var:v13 , &quot;Warehouse6&quot;)"" />
                <InventorySku>
                  <xsl:if test=""../../s1:StoredProcedureResultSet1/s0:StoredProcedureResultSet1/s0:InventoryCatalogName"">
                    <xsl:attribute name=""InventoryCatalogName"">
                      <xsl:value-of select=""../../s1:StoredProcedureResultSet1/s0:StoredProcedureResultSet1/s0:InventoryCatalogName/text()"" />
                    </xsl:attribute>
                  </xsl:if>
                  <xsl:attribute name=""LastModified"">
                    <xsl:value-of select=""$var:v2"" />
                  </xsl:attribute>
                  <xsl:if test=""../../s1:StoredProcedureResultSet1/s0:StoredProcedureResultSet1/s0:ProductCatalogName"">
                    <xsl:attribute name=""ProductCatalogName"">
                      <xsl:value-of select=""../../s1:StoredProcedureResultSet1/s0:StoredProcedureResultSet1/s0:ProductCatalogName/text()"" />
                    </xsl:attribute>
                  </xsl:if>
                  <xsl:if test=""s0:BTKEY"">
                    <xsl:attribute name=""ProductId"">
                      <xsl:value-of select=""s0:BTKEY/text()"" />
                    </xsl:attribute>
                  </xsl:if>
                  <xsl:attribute name=""Backorderable"">
                    <xsl:value-of select=""$var:v3"" />
                  </xsl:attribute>
                  <xsl:attribute name=""Preorderable"">
                    <xsl:value-of select=""$var:v3"" />
                  </xsl:attribute>
                  <xsl:attribute name=""Status"">
                    <xsl:value-of select=""$var:v4"" />
                  </xsl:attribute>
                  <xsl:variable name=""var:v5"" select=""userCSharp:InitCumulativeSum(0)"" />
                  <xsl:for-each select=""/s1:procCSGetInventoryResponse/s1:StoredProcedureResultSet0/s0:StoredProcedureResultSet0"">
                    <xsl:variable name=""var:v6"" select=""userCSharp:AddToCumulativeSum(0,string(s0:AvailableQuantity/text()),string(s0:BTKEY/text()))"" />
                  </xsl:for-each>
                  <xsl:variable name=""var:v7"" select=""userCSharp:GetCumulativeSum(0)"" />
                  <xsl:attribute name=""OnHandQuantity"">
                    <xsl:value-of select=""$var:v7"" />
                  </xsl:attribute>
                  <xsl:attribute name=""StockOutThreshold"">
                    <xsl:value-of select=""$var:v4"" />
                  </xsl:attribute>
                  <xsl:attribute name=""LastRestocked"">
                    <xsl:value-of select=""$var:v2"" />
                  </xsl:attribute>
                  <xsl:if test=""string($var:v8)='true'"">
                    <xsl:variable name=""var:v9"" select=""s0:VirtualInventory/text()"" />
                    <xsl:attribute name=""Warehouse1_InventoryType"">
                      <xsl:value-of select=""$var:v9"" />
                    </xsl:attribute>
                  </xsl:if>
                  <xsl:if test=""string($var:v8)='true'"">
                    <xsl:variable name=""var:v10"" select=""s0:LEAvailableQuantity/text()"" />
                    <xsl:attribute name=""Warehouse1_LEQuantity"">
                      <xsl:value-of select=""$var:v10"" />
                    </xsl:attribute>
                  </xsl:if>
                  <xsl:if test=""string($var:v8)='true'"">
                    <xsl:variable name=""var:v11"" select=""s0:AvailableQuantity/text()"" />
                    <xsl:attribute name=""Warehouse1_OnHandQuantity"">
                      <xsl:value-of select=""$var:v11"" />
                    </xsl:attribute>
                  </xsl:if>
                  <xsl:if test=""string($var:v8)='true'"">
                    <xsl:variable name=""var:v12"" select=""s0:OnOrderQuantity/text()"" />
                    <xsl:attribute name=""Warehouse1_OnOrderQuantity"">
                      <xsl:value-of select=""$var:v12"" />
                    </xsl:attribute>
                  </xsl:if>
                  <xsl:if test=""string($var:v14)='true'"">
                    <xsl:variable name=""var:v15"" select=""s0:VirtualInventory/text()"" />
                    <xsl:attribute name=""Warehouse2_InventoryType"">
                      <xsl:value-of select=""$var:v15"" />
                    </xsl:attribute>
                  </xsl:if>
                  <xsl:if test=""string($var:v14)='true'"">
                    <xsl:variable name=""var:v16"" select=""s0:LEAvailableQuantity/text()"" />
                    <xsl:attribute name=""Warehouse2_LEQuantity"">
                      <xsl:value-of select=""$var:v16"" />
                    </xsl:attribute>
                  </xsl:if>
                  <xsl:if test=""string($var:v14)='true'"">
                    <xsl:variable name=""var:v17"" select=""s0:AvailableQuantity/text()"" />
                    <xsl:attribute name=""Warehouse2_OnHandQuantity"">
                      <xsl:value-of select=""$var:v17"" />
                    </xsl:attribute>
                  </xsl:if>
                  <xsl:if test=""string($var:v14)='true'"">
                    <xsl:variable name=""var:v18"" select=""s0:OnOrderQuantity/text()"" />
                    <xsl:attribute name=""Warehouse2_OnOrderQuantity"">
                      <xsl:value-of select=""$var:v18"" />
                    </xsl:attribute>
                  </xsl:if>
                  <xsl:if test=""string($var:v19)='true'"">
                    <xsl:variable name=""var:v20"" select=""s0:VirtualInventory/text()"" />
                    <xsl:attribute name=""Warehouse3_InventoryType"">
                      <xsl:value-of select=""$var:v20"" />
                    </xsl:attribute>
                  </xsl:if>
                  <xsl:if test=""string($var:v19)='true'"">
                    <xsl:variable name=""var:v21"" select=""s0:LEAvailableQuantity/text()"" />
                    <xsl:attribute name=""Warehouse3_LEQuantity"">
                      <xsl:value-of select=""$var:v21"" />
                    </xsl:attribute>
                  </xsl:if>
                  <xsl:if test=""string($var:v19)='true'"">
                    <xsl:variable name=""var:v22"" select=""s0:AvailableQuantity/text()"" />
                    <xsl:attribute name=""Warehouse3_OnhandQuantity"">
                      <xsl:value-of select=""$var:v22"" />
                    </xsl:attribute>
                  </xsl:if>
                  <xsl:if test=""string($var:v19)='true'"">
                    <xsl:variable name=""var:v23"" select=""s0:OnOrderQuantity/text()"" />
                    <xsl:attribute name=""Warehouse3_OnOrderQuantity"">
                      <xsl:value-of select=""$var:v23"" />
                    </xsl:attribute>
                  </xsl:if>
                  <xsl:if test=""string($var:v24)='true'"">
                    <xsl:variable name=""var:v25"" select=""s0:VirtualInventory/text()"" />
                    <xsl:attribute name=""Warehouse4_InventoryType"">
                      <xsl:value-of select=""$var:v25"" />
                    </xsl:attribute>
                  </xsl:if>
                  <xsl:if test=""string($var:v24)='true'"">
                    <xsl:variable name=""var:v26"" select=""s0:LEAvailableQuantity/text()"" />
                    <xsl:attribute name=""Warehouse4_LEQuantity"">
                      <xsl:value-of select=""$var:v26"" />
                    </xsl:attribute>
                  </xsl:if>
                  <xsl:if test=""string($var:v24)='true'"">
                    <xsl:variable name=""var:v27"" select=""s0:AvailableQuantity/text()"" />
                    <xsl:attribute name=""Warehouse4_OnhandQuantity"">
                      <xsl:value-of select=""$var:v27"" />
                    </xsl:attribute>
                  </xsl:if>
                  <xsl:if test=""string($var:v24)='true'"">
                    <xsl:variable name=""var:v28"" select=""s0:OnOrderQuantity/text()"" />
                    <xsl:attribute name=""Warehouse4_OnOrderQuantity"">
                      <xsl:value-of select=""$var:v28"" />
                    </xsl:attribute>
                  </xsl:if>
                  <xsl:if test=""string($var:v29)='true'"">
                    <xsl:variable name=""var:v30"" select=""s0:InventoryType/text()"" />
                    <xsl:attribute name=""Warehouse5_InventoryType"">
                      <xsl:value-of select=""$var:v30"" />
                    </xsl:attribute>
                  </xsl:if>
                  <xsl:if test=""string($var:v29)='true'"">
                    <xsl:variable name=""var:v31"" select=""s0:LEAvailableQuantity/text()"" />
                    <xsl:attribute name=""Warehouse5_LEQuantity"">
                      <xsl:value-of select=""$var:v31"" />
                    </xsl:attribute>
                  </xsl:if>
                  <xsl:if test=""string($var:v29)='true'"">
                    <xsl:variable name=""var:v32"" select=""s0:AvailableQuantity/text()"" />
                    <xsl:attribute name=""Warehouse5_OnHandQuantity"">
                      <xsl:value-of select=""$var:v32"" />
                    </xsl:attribute>
                  </xsl:if>
                  <xsl:if test=""string($var:v29)='true'"">
                    <xsl:variable name=""var:v33"" select=""s0:OnOrderQuantity/text()"" />
                    <xsl:attribute name=""Warehouse5_OnOrderQuantity"">
                      <xsl:value-of select=""$var:v33"" />
                    </xsl:attribute>
                  </xsl:if>
                  <xsl:if test=""string($var:v34)='true'"">
                    <xsl:variable name=""var:v35"" select=""s0:InventoryType/text()"" />
                    <xsl:attribute name=""Warehouse6_InventoryType"">
                      <xsl:value-of select=""$var:v35"" />
                    </xsl:attribute>
                  </xsl:if>
                  <xsl:if test=""string($var:v34)='true'"">
                    <xsl:variable name=""var:v36"" select=""s0:LEAvailableQuantity/text()"" />
                    <xsl:attribute name=""Warehouse6_LEQuantity"">
                      <xsl:value-of select=""$var:v36"" />
                    </xsl:attribute>
                  </xsl:if>
                  <xsl:if test=""string($var:v34)='true'"">
                    <xsl:variable name=""var:v37"" select=""s0:AvailableQuantity/text()"" />
                    <xsl:attribute name=""Warehouse6_OnHandQuantity"">
                      <xsl:value-of select=""$var:v37"" />
                    </xsl:attribute>
                  </xsl:if>
                  <xsl:if test=""string($var:v34)='true'"">
                    <xsl:variable name=""var:v38"" select=""s0:OnOrderQuantity/text()"" />
                    <xsl:attribute name=""Warehouse6_OnOrderQuantity"">
                      <xsl:value-of select=""$var:v38"" />
                    </xsl:attribute>
                  </xsl:if>
                </InventorySku>
              </xsl:for-each>
            </xsl:for-each>
          </InventoryCatalog>
        </xsl:for-each>
      </xsl:for-each>
    </MSInventoryCollection>
  </xsl:template>
  <msxsl:script language=""C#"" implements-prefix=""userCSharp""><![CDATA[
public string DateCurrentDate()
{
	DateTime dt = DateTime.Now;
	return dt.ToString(""yyyy-MM-dd"", System.Globalization.CultureInfo.InvariantCulture);
}


public string StringConcat(string param0)
{
   return param0;
}


public string InitCumulativeSum(int index)
{
	if (index >= 0)
	{
		if (index >= myCumulativeSumArray.Count)
		{
			int i = myCumulativeSumArray.Count;
			for (; i<=index; i++)
			{
				myCumulativeSumArray.Add("""");
			}
		}
		else
		{
			myCumulativeSumArray[index] = """";
		}
	}
	return """";
}

public System.Collections.ArrayList myCumulativeSumArray = new System.Collections.ArrayList();

public string AddToCumulativeSum(int index, string val, string notused)
{
	if (index < 0 || index >= myCumulativeSumArray.Count)
	{
		return """";
    }
	double d = 0;
	if (IsNumeric(val, ref d))
	{
		if (myCumulativeSumArray[index] == """")
		{
			myCumulativeSumArray[index] = d;
		}
		else
		{
			myCumulativeSumArray[index] = (double)(myCumulativeSumArray[index]) + d;
		}
	}
	return (myCumulativeSumArray[index] is double) ? ((double)myCumulativeSumArray[index]).ToString(System.Globalization.CultureInfo.InvariantCulture) : """";
}

public string GetCumulativeSum(int index)
{
	if (index < 0 || index >= myCumulativeSumArray.Count)
	{
		return """";
	}
	return (myCumulativeSumArray[index] is double) ? ((double)myCumulativeSumArray[index]).ToString(System.Globalization.CultureInfo.InvariantCulture) : """";
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
        
        private const string _strSrcSchemasList0 = @"BTNextGen.Biztalk.Inventory.Schema.procCSGetInventoryTypedProcedure_dbo+procCSGetInventoryResponse";
        
        private const global::BTNextGen.Biztalk.Inventory.Schema.procCSGetInventoryTypedProcedure_dbo.procCSGetInventoryResponse _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"BTNextGen.BizTalk.CS.Inventory.Schemas.BTInventory";
        
        private const global::BTNextGen.BizTalk.CS.Inventory.Schemas.BTInventory _trgSchemaTypeReference0 = null;
        
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
                _TrgSchemas[0] = @"BTNextGen.BizTalk.CS.Inventory.Schemas.BTInventory";
                return _TrgSchemas;
            }
        }
    }
}
