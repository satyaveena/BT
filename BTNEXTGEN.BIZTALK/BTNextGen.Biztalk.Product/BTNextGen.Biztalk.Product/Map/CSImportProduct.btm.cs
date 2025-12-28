namespace BTNextGen.Biztalk.Product.Map {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.Biztalk.Product.RepoCSGetProductsTypedProcedure_dbo+procCommerceServerGetNextGenProductsResponse", typeof(global::BTNextGen.Biztalk.Product.RepoCSGetProductsTypedProcedure_dbo.procCommerceServerGetNextGenProductsResponse))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.BizTalk.CS.Catalogue.Schemas.BTCatalog", typeof(global::BTNextGen.BizTalk.CS.Catalogue.Schemas.BTCatalog))]
    public sealed class CSImportProduct : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s1 s0 userCSharp"" version=""1.0"" xmlns:s1=""http://schemas.microsoft.com/Sql/2008/05/TypedProcedures/dbo"" xmlns:s0=""http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procCommerceServerGetNextGenProducts"" xmlns:userCSharp=""http://schemas.microsoft.com/BizTalk/2003/userCSharp"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s1:procCommerceServerGetNextGenProductsResponse"" />
  </xsl:template>
  <xsl:template match=""/s1:procCommerceServerGetNextGenProductsResponse"">
    <xsl:variable name=""var:v1"" select=""userCSharp:StringConcat(&quot;3.0&quot;)"" />
    <MSCommerceCatalogCollection2>
      <xsl:attribute name=""version"">
        <xsl:value-of select=""$var:v1"" />
      </xsl:attribute>
      <xsl:for-each select=""s1:StoredProcedureResultSet3/s0:StoredProcedureResultSet3"">
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
          <xsl:attribute name=""name"">
            <xsl:value-of select=""s0:CatalogName/text()"" />
          </xsl:attribute>
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
            <xsl:attribute name=""Value"">
              <xsl:value-of select=""s0:CatalogName/text()"" />
            </xsl:attribute>
            <xsl:attribute name=""language"">
              <xsl:value-of select=""$var:v4"" />
            </xsl:attribute>
          </DisplayName>
          <xsl:for-each select=""../../s1:StoredProcedureResultSet0"">
            <xsl:for-each select=""s0:StoredProcedureResultSet0"">
              <xsl:variable name=""var:v7"" select=""userCSharp:DateCurrentDateTime()"" />
              <xsl:variable name=""var:v8"" select=""userCSharp:StringConcat(&quot;0&quot;)"" />
              <xsl:variable name=""var:v9"" select=""userCSharp:StringLeft(string(s0:BTTitle/text()) , &quot;100&quot;)"" />
              <xsl:variable name=""var:v12"" select=""userCSharp:StringConcat(&quot;BTProduct&quot;)"" />
              <xsl:variable name=""var:v13"" select=""string(s0:BTTitle/text())"" />
              <xsl:variable name=""var:v14"" select=""userCSharp:StringLeft($var:v13 , &quot;128&quot;)"" />
              <xsl:variable name=""var:v15"" select=""userCSharp:StringConcat(&quot;en-US&quot;)"" />
              <xsl:variable name=""var:v16"" select=""userCSharp:StringConcat($var:v13 , &quot;:&quot; , string(s0:BTSubTitle/text()))"" />
              <Product>
                <xsl:if test=""s0:BTKEY"">
                  <xsl:attribute name=""id"">
                    <xsl:value-of select=""s0:BTKEY/text()"" />
                  </xsl:attribute>
                </xsl:if>
                <xsl:attribute name=""lastmodified"">
                  <xsl:value-of select=""$var:v7"" />
                </xsl:attribute>
                <xsl:attribute name=""UseCategoryPricing"">
                  <xsl:value-of select=""$var:v8"" />
                </xsl:attribute>
                <xsl:attribute name=""name"">
                  <xsl:value-of select=""$var:v9"" />
                </xsl:attribute>
                <xsl:if test=""s0:BTAccumulatorType"">
                  <xsl:attribute name=""BTAccumulatorType"">
                    <xsl:value-of select=""s0:BTAccumulatorType/text()"" />
                  </xsl:attribute>
                </xsl:if>
                <xsl:if test=""s0:BTAffidavitOnSaleDate"">
                  <xsl:attribute name=""BTAffidavitOnSaleDate"">
                    <xsl:value-of select=""s0:BTAffidavitOnSaleDate/text()"" />
                  </xsl:attribute>
                </xsl:if>
                <xsl:if test=""s0:BTAudience"">
                  <xsl:attribute name=""BTAudience"">
                    <xsl:value-of select=""s0:BTAudience/text()"" />
                  </xsl:attribute>
                </xsl:if>
                <xsl:variable name=""var:v10"" select=""userCSharp:MyConcat(string(s0:BTBlowOutInactiveFlag/text()))"" />
                <xsl:attribute name=""BTBlowOutInactiveFlag"">
                  <xsl:value-of select=""$var:v10"" />
                </xsl:attribute>
                <xsl:if test=""s0:BTBookClassificationCode"">
                  <xsl:attribute name=""BTBookClassificationCode"">
                    <xsl:value-of select=""s0:BTBookClassificationCode/text()"" />
                  </xsl:attribute>
                </xsl:if>
                <xsl:if test=""s0:BTBookClassificationDesc"">
                  <xsl:attribute name=""BTBookClassificationDesc"">
                    <xsl:value-of select=""s0:BTBookClassificationDesc/text()"" />
                  </xsl:attribute>
                </xsl:if>
                <xsl:if test=""s0:BTBoxCost"">
                  <xsl:attribute name=""BTBoxCost"">
                    <xsl:value-of select=""s0:BTBoxCost/text()"" />
                  </xsl:attribute>
                </xsl:if>
                <xsl:if test=""s0:BTDealerCost"">
                  <xsl:attribute name=""BTDealerCost"">
                    <xsl:value-of select=""s0:BTDealerCost/text()"" />
                  </xsl:attribute>
                </xsl:if>
                <xsl:if test=""s0:BTDiscountAvailable"">
                  <xsl:attribute name=""BTDiscountFlag"">
                    <xsl:value-of select=""s0:BTDiscountAvailable/text()"" />
                  </xsl:attribute>
                </xsl:if>
                <xsl:if test=""s0:BTInternationalListPrice"">
                  <xsl:attribute name=""BTInternationalListPrice"">
                    <xsl:value-of select=""s0:BTInternationalListPrice/text()"" />
                  </xsl:attribute>
                </xsl:if>
                <xsl:if test=""s0:BTInternationalListPriceCurrencyCode"">
                  <xsl:attribute name=""BTInternationalListPriceCurrencyCode"">
                    <xsl:value-of select=""s0:BTInternationalListPriceCurrencyCode/text()"" />
                  </xsl:attribute>
                </xsl:if>
                <xsl:if test=""s0:BTInternationalNetPrice"">
                  <xsl:attribute name=""BTInternationalNetPrice"">
                    <xsl:value-of select=""s0:BTInternationalNetPrice/text()"" />
                  </xsl:attribute>
                </xsl:if>
                <xsl:if test=""s0:BTInternationalNetPriceCurrencyCode"">
                  <xsl:attribute name=""BTInternationalNetPriceCurrencyCode"">
                    <xsl:value-of select=""s0:BTInternationalNetPriceCurrencyCode/text()"" />
                  </xsl:attribute>
                </xsl:if>
                <xsl:if test=""s0:BTISActive"">
                  <xsl:attribute name=""BTIsActive"">
                    <xsl:value-of select=""s0:BTISActive/text()"" />
                  </xsl:attribute>
                </xsl:if>
                <xsl:if test=""s0:ISBN"">
                  <xsl:attribute name=""BTISBN"">
                    <xsl:value-of select=""s0:ISBN/text()"" />
                  </xsl:attribute>
                </xsl:if>
                <xsl:if test=""s0:ISBN10"">
                  <xsl:attribute name=""BTISBN10"">
                    <xsl:value-of select=""s0:ISBN10/text()"" />
                  </xsl:attribute>
                </xsl:if>
                <xsl:if test=""s0:BTItemGroup"">
                  <xsl:attribute name=""BTItemGroup"">
                    <xsl:value-of select=""s0:BTItemGroup/text()"" />
                  </xsl:attribute>
                </xsl:if>
                <xsl:if test=""s0:BTKEY"">
                  <xsl:attribute name=""BTKey"">
                    <xsl:value-of select=""s0:BTKEY/text()"" />
                  </xsl:attribute>
                </xsl:if>
                <xsl:if test=""s0:BTLanguageCode"">
                  <xsl:attribute name=""BTLanguageCode"">
                    <xsl:value-of select=""s0:BTLanguageCode/text()"" />
                  </xsl:attribute>
                </xsl:if>
                <xsl:if test=""s0:BTManufacturerCode"">
                  <xsl:attribute name=""BTManufacturerCode"">
                    <xsl:value-of select=""s0:BTManufacturerCode/text()"" />
                  </xsl:attribute>
                </xsl:if>
                <xsl:if test=""s0:BTMarketExclusions"">
                  <xsl:attribute name=""BTMarketExclusions"">
                    <xsl:value-of select=""s0:BTMarketExclusions/text()"" />
                  </xsl:attribute>
                </xsl:if>
                <xsl:if test=""s0:BTMarketRights"">
                  <xsl:attribute name=""BTMarketRights"">
                    <xsl:value-of select=""s0:BTMarketRights/text()"" />
                  </xsl:attribute>
                </xsl:if>
                <xsl:if test=""s0:BTMerchandiseCategory"">
                  <xsl:attribute name=""BTMerchandiseCategory"">
                    <xsl:value-of select=""s0:BTMerchandiseCategory/text()"" />
                  </xsl:attribute>
                </xsl:if>
                <xsl:if test=""s0:BTMSRPListPrice"">
                  <xsl:attribute name=""BTMSRPListPrice"">
                    <xsl:value-of select=""s0:BTMSRPListPrice/text()"" />
                  </xsl:attribute>
                </xsl:if>
                <xsl:if test=""s0:BTMSRPListPriceCurrencyCode"">
                  <xsl:attribute name=""BTMSRPListPriceCurrencyCode"">
                    <xsl:value-of select=""s0:BTMSRPListPriceCurrencyCode/text()"" />
                  </xsl:attribute>
                </xsl:if>
                <xsl:if test=""s0:BTNumberOfVolumes"">
                  <xsl:attribute name=""BTNumberofVolumes"">
                    <xsl:value-of select=""s0:BTNumberOfVolumes/text()"" />
                  </xsl:attribute>
                </xsl:if>
                <xsl:if test=""s0:BTOnSaleDate"">
                  <xsl:attribute name=""BTOnSaleDate"">
                    <xsl:value-of select=""s0:BTOnSaleDate/text()"" />
                  </xsl:attribute>
                </xsl:if>
                <xsl:if test=""s0:BTPartNumber"">
                  <xsl:attribute name=""BTPartNumber"">
                    <xsl:value-of select=""s0:BTPartNumber/text()"" />
                  </xsl:attribute>
                </xsl:if>
                <xsl:if test=""s0:PreOrderDate"">
                  <xsl:attribute name=""BTPreOrderDate"">
                    <xsl:value-of select=""s0:PreOrderDate/text()"" />
                  </xsl:attribute>
                </xsl:if>
                <xsl:if test=""s0:BTPriceCode"">
                  <xsl:attribute name=""BTPriceCode"">
                    <xsl:value-of select=""s0:BTPriceCode/text()"" />
                  </xsl:attribute>
                </xsl:if>
                <xsl:if test=""s0:BTPriceKey"">
                  <xsl:attribute name=""BTPriceKey"">
                    <xsl:value-of select=""s0:BTPriceKey/text()"" />
                  </xsl:attribute>
                </xsl:if>
                <xsl:if test=""s0:BTPricePack"">
                  <xsl:attribute name=""BTPricePack"">
                    <xsl:value-of select=""s0:BTPricePack/text()"" />
                  </xsl:attribute>
                </xsl:if>
                <xsl:if test=""s0:BTProductCode"">
                  <xsl:attribute name=""BTProductCode"">
                    <xsl:value-of select=""s0:BTProductCode/text()"" />
                  </xsl:attribute>
                </xsl:if>
                <xsl:if test=""s0:BTProductLine"">
                  <xsl:attribute name=""BTProductLine"">
                    <xsl:value-of select=""s0:BTProductLine/text()"" />
                  </xsl:attribute>
                </xsl:if>
                <xsl:if test=""s0:BTProductType"">
                  <xsl:attribute name=""BTProductType"">
                    <xsl:value-of select=""s0:BTProductType/text()"" />
                  </xsl:attribute>
                </xsl:if>
                <xsl:if test=""s0:BTPubCodeD"">
                  <xsl:attribute name=""BTPubCodeD"">
                    <xsl:value-of select=""s0:BTPubCodeD/text()"" />
                  </xsl:attribute>
                </xsl:if>
                <xsl:if test=""s0:BTPublisher"">
                  <xsl:attribute name=""BTPublisher"">
                    <xsl:value-of select=""s0:BTPublisher/text()"" />
                  </xsl:attribute>
                </xsl:if>
                <xsl:if test=""s0:BTPubStatusCode"">
                  <xsl:attribute name=""BTPubStatus"">
                    <xsl:value-of select=""s0:BTPubStatusCode/text()"" />
                  </xsl:attribute>
                </xsl:if>
                <xsl:if test=""s0:BTQuantityPack"">
                  <xsl:attribute name=""BTQuantityPack"">
                    <xsl:value-of select=""s0:BTQuantityPack/text()"" />
                  </xsl:attribute>
                </xsl:if>
                <xsl:if test=""s0:BTReleaseDate"">
                  <xsl:attribute name=""BTReleaseDate"">
                    <xsl:value-of select=""s0:BTReleaseDate/text()"" />
                  </xsl:attribute>
                </xsl:if>
                <xsl:variable name=""var:v11"" select=""userCSharp:MyConcat(string(s0:BTReservedInventoryFlag/text()))"" />
                <xsl:attribute name=""BTReservedInventoryFlag"">
                  <xsl:value-of select=""$var:v11"" />
                </xsl:attribute>
                <xsl:if test=""s0:BTResponsibleParties"">
                  <xsl:attribute name=""BTResponsibleParty"">
                    <xsl:value-of select=""s0:BTResponsibleParties/text()"" />
                  </xsl:attribute>
                </xsl:if>
                <xsl:if test=""s0:BTRetailPrice"">
                  <xsl:attribute name=""BTRetailPrice"">
                    <xsl:value-of select=""s0:BTRetailPrice/text()"" />
                  </xsl:attribute>
                </xsl:if>
                <xsl:if test=""s0:BTRetailPriceCheck"">
                  <xsl:attribute name=""BTRetailPriceCheck"">
                    <xsl:value-of select=""s0:BTRetailPriceCheck/text()"" />
                  </xsl:attribute>
                </xsl:if>
                <xsl:if test=""s0:BTSpecialBoxCost"">
                  <xsl:attribute name=""BTSpecialBoxCost"">
                    <xsl:value-of select=""s0:BTSpecialBoxCost/text()"" />
                  </xsl:attribute>
                </xsl:if>
                <xsl:if test=""s0:BTSpecialCost"">
                  <xsl:attribute name=""BTSpecialCost"">
                    <xsl:value-of select=""s0:BTSpecialCost/text()"" />
                  </xsl:attribute>
                </xsl:if>
                <xsl:if test=""s0:BTSubTitle"">
                  <xsl:attribute name=""BTSubTitle"">
                    <xsl:value-of select=""s0:BTSubTitle/text()"" />
                  </xsl:attribute>
                </xsl:if>
                <xsl:if test=""s0:BTVendorNumber"">
                  <xsl:attribute name=""BTTOLASVendorNumber"">
                    <xsl:value-of select=""s0:BTVendorNumber/text()"" />
                  </xsl:attribute>
                </xsl:if>
                <xsl:if test=""s0:BTUnitCost"">
                  <xsl:attribute name=""BTUnitCost"">
                    <xsl:value-of select=""s0:BTUnitCost/text()"" />
                  </xsl:attribute>
                </xsl:if>
                <xsl:if test=""s0:BTUPC"">
                  <xsl:attribute name=""BTUPC"">
                    <xsl:value-of select=""s0:BTUPC/text()"" />
                  </xsl:attribute>
                </xsl:if>
                <xsl:if test=""s0:BTVendorNumber"">
                  <xsl:attribute name=""BTVendorNumber"">
                    <xsl:value-of select=""s0:BTVendorNumber/text()"" />
                  </xsl:attribute>
                </xsl:if>
                <xsl:if test=""s0:ESupplier"">
                  <xsl:attribute name=""ESupplier"">
                    <xsl:value-of select=""s0:ESupplier/text()"" />
                  </xsl:attribute>
                </xsl:if>
                <xsl:if test=""s0:FormDetails"">
                  <xsl:attribute name=""FormDetails"">
                    <xsl:value-of select=""s0:FormDetails/text()"" />
                  </xsl:attribute>
                </xsl:if>
                <xsl:if test=""s0:BTProductType"">
                  <xsl:attribute name=""ProductType"">
                    <xsl:value-of select=""s0:BTProductType/text()"" />
                  </xsl:attribute>
                </xsl:if>
                <xsl:if test=""s0:PurchaseOption"">
                  <xsl:attribute name=""PurchaseOption"">
                    <xsl:value-of select=""s0:PurchaseOption/text()"" />
                  </xsl:attribute>
                </xsl:if>
                <xsl:attribute name=""Definition"">
                  <xsl:value-of select=""$var:v12"" />
                </xsl:attribute>
                <xsl:if test=""s0:BTMSRPListPrice"">
                  <xsl:attribute name=""listprice"">
                    <xsl:value-of select=""s0:BTMSRPListPrice/text()"" />
                  </xsl:attribute>
                </xsl:if>
                <DisplayName>
                  <xsl:attribute name=""Value"">
                    <xsl:value-of select=""$var:v14"" />
                  </xsl:attribute>
                  <xsl:attribute name=""language"">
                    <xsl:value-of select=""$var:v15"" />
                  </xsl:attribute>
                </DisplayName>
                <Description>
                  <xsl:attribute name=""Value"">
                    <xsl:value-of select=""$var:v16"" />
                  </xsl:attribute>
                  <xsl:attribute name=""language"">
                    <xsl:value-of select=""$var:v15"" />
                  </xsl:attribute>
                </Description>
              </Product>
            </xsl:for-each>
          </xsl:for-each>
        </Catalog>
      </xsl:for-each>
    </MSCommerceCatalogCollection2>
  </xsl:template>
  <msxsl:script language=""C#"" implements-prefix=""userCSharp""><![CDATA[
public string StringConcat(string param0, string param1, string param2)
{
   return param0 + param1 + param2;
}


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


public string MyConcat(string param1)
{
    if (param1 == ""true"") 
        {
              return  ""1"";
        }
    else
      {
        return ""0"";	
       }

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


]]></msxsl:script>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"BTNextGen.Biztalk.Product.RepoCSGetProductsTypedProcedure_dbo+procCommerceServerGetNextGenProductsResponse";
        
        private const global::BTNextGen.Biztalk.Product.RepoCSGetProductsTypedProcedure_dbo.procCommerceServerGetNextGenProductsResponse _srcSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"BTNextGen.Biztalk.Product.RepoCSGetProductsTypedProcedure_dbo+procCommerceServerGetNextGenProductsResponse";
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
