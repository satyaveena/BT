namespace BTNextGen.Biztalk.BasketMigration {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.Biztalk.BasketMigration.newdataset", typeof(global::BTNextGen.Biztalk.BasketMigration.newdataset))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.Biztalk.BasketMigration.CompositeWhteOpsMaster+Request", typeof(global::BTNextGen.Biztalk.BasketMigration.CompositeWhteOpsMaster.Request))]
    public sealed class Transform_RequestForSQL : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var userCSharp"" version=""1.0"" xmlns:ns0=""http://schemas.microsoft.com/Sql/2008/05/TypedProcedures/dbo"" xmlns:ns1=""http://BTNextGen.Biztalk.BasketMigration.CompositeWhteOpsMaster"" xmlns:userCSharp=""http://schemas.microsoft.com/BizTalk/2003/userCSharp"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/NewDataSet"" />
  </xsl:template>
  <xsl:template match=""/NewDataSet"">
    <ns1:Request>
      <ns0:procInsertBasketSummary_R1>
        <ns0:BasketName>
          <xsl:value-of select=""Basket/Name/text()"" />
        </ns0:BasketName>
        <ns0:SoldToId>
          <xsl:value-of select=""Basket/SoldToId/text()"" />
        </ns0:SoldToId>
        <ns0:PriorStatus>
          <xsl:text>Open</xsl:text>
        </ns0:PriorStatus>
        <ns0:BasketID>
          <xsl:value-of select=""Basket/BasketID/text()"" />
        </ns0:BasketID>
        <ns0:BasketFolderId>
          <xsl:value-of select=""OrderForms/FolderId/text()"" />
        </ns0:BasketFolderId>
        <ns0:BasketNote>
          <xsl:value-of select=""OrderForms/BTNote/text()"" />
        </ns0:BasketNote>
        <ns0:PrimaryIndicator>
          <xsl:value-of select=""OrderForms/IsPrimary/text()"" />
        </ns0:PrimaryIndicator>
        <ns0:ArchivedIndicator>
          <xsl:value-of select=""OrderForms/IsArchived/text()"" />
        </ns0:ArchivedIndicator>
        <ns0:SpecialInstructions>
          <xsl:value-of select=""OrderForms/SpecialInstructions/text()"" />
        </ns0:SpecialInstructions>
        <ns0:LineItemCount>
          <xsl:value-of select=""Basket/LineItemCount/text()"" />
        </ns0:LineItemCount>
        <ns0:OrderGroupId>
          <xsl:value-of select=""Basket/OrderGroupId/text()"" />
        </ns0:OrderGroupId>
        <ns0:Created>
          <xsl:value-of select=""Basket/Created/text()"" />
        </ns0:Created>
        <ns0:Modified>
          <xsl:value-of select=""Basket/LastModified/text()"" />
        </ns0:Modified>
      </ns0:procInsertBasketSummary_R1>
      <ns0:procInsertBasketOrderForm_R1>
        <ns0:BasketOrderFormID>
          <xsl:value-of select=""OrderForms/OrderFormId/text()"" />
        </ns0:BasketOrderFormID>
        <ns0:BasketSummaryID>
          <xsl:value-of select=""Basket/BasketID/text()"" />
        </ns0:BasketSummaryID>
        <ns0:BasketName>
          <xsl:value-of select=""OrderForms/Name/text()"" />
        </ns0:BasketName>
        <ns0:IsHomeDeliveryIndicator>
          <xsl:value-of select=""OrderForms/IsHomeDeliveryIndicator/text()"" />
        </ns0:IsHomeDeliveryIndicator>
        <ns0:OrderGroupId>
          <xsl:value-of select=""Basket/OrderGroupId/text()"" />
        </ns0:OrderGroupId>
        <ns0:Created>
          <xsl:value-of select=""OrderForms/Created/text()"" />
        </ns0:Created>
        <ns0:Modified>
          <xsl:value-of select=""OrderForms/LastModified/text()"" />
        </ns0:Modified>
      </ns0:procInsertBasketOrderForm_R1>
      <xsl:for-each select=""LineItems"">
        <xsl:variable name=""var:v1"" select=""userCSharp:MathRound(string(InStockQuantity/text()) , &quot;4&quot;)"" />
        <ns0:procInsertBasketLineItems_R1>
          <ns0:OrderGroupId>
            <xsl:value-of select=""../Basket/OrderGroupId/text()"" />
          </ns0:OrderGroupId>
          <ns0:BasketLineItemid>
            <xsl:value-of select=""LineItemId/text()"" />
          </ns0:BasketLineItemid>
          <ns0:BasketLineItemid2>
            <xsl:value-of select=""LineItemId/text()"" />
          </ns0:BasketLineItemid2>
          <ns0:BasketOrderFormId>
            <xsl:value-of select=""OrderFormId/text()"" />
          </ns0:BasketOrderFormId>
          <ns0:ProductCatalog>
            <xsl:value-of select=""ProductCatalog/text()"" />
          </ns0:ProductCatalog>
          <ns0:ProductCategory>
            <xsl:value-of select=""ProductCatalog/text()"" />
          </ns0:ProductCategory>
          <ns0:ProductID>
            <xsl:value-of select=""ProductId/text()"" />
          </ns0:ProductID>
          <ns0:PlacedPrice>
            <xsl:value-of select=""PlacedPrice/text()"" />
          </ns0:PlacedPrice>
          <ns0:ListPrice>
            <xsl:value-of select=""ListPrice/text()"" />
          </ns0:ListPrice>
          <ns0:LineItemsDiscountAmount>
            <xsl:value-of select=""LineItemDiscountAmount/text()"" />
          </ns0:LineItemsDiscountAmount>
          <ns0:DisplayName>
            <xsl:value-of select=""DisplayName/text()"" />
          </ns0:DisplayName>
          <ns0:BTKey>
            <xsl:value-of select=""BTKey/text()"" />
          </ns0:BTKey>
          <ns0:ISBN>
            <xsl:value-of select=""BTISBN/text()"" />
          </ns0:ISBN>
          <ns0:InStockQuantity>
            <xsl:value-of select=""$var:v1"" />
          </ns0:InStockQuantity>
          <ns0:ContractPrice>
            <xsl:value-of select=""ContractPrice/text()"" />
          </ns0:ContractPrice>
          <ns0:Quantity>
            <xsl:value-of select=""Quantity/text()"" />
          </ns0:Quantity>
          <ns0:Created>
            <xsl:value-of select=""Created/text()"" />
          </ns0:Created>
          <ns0:Modified>
            <xsl:value-of select=""LastModified/text()"" />
          </ns0:Modified>
        </ns0:procInsertBasketLineItems_R1>
      </xsl:for-each>
      <ns0:procSetBasketSummaryComplete_R1>
        <ns0:OrderGroupId>
          <xsl:value-of select=""Basket/OrderGroupId/text()"" />
        </ns0:OrderGroupId>
        <ns0:BasketSummaryID>
          <xsl:value-of select=""Basket/BasketID/text()"" />
        </ns0:BasketSummaryID>
      </ns0:procSetBasketSummaryComplete_R1>
    </ns1:Request>
  </xsl:template>
  <msxsl:script language=""C#"" implements-prefix=""userCSharp""><![CDATA[
public string MathRound(string val)
{
	return MathRound(val, ""0"");
}

public string MathRound(string val, string decimals)
{
	string retval = """";
	double v = 0;
	double db = 0;
	if (IsNumeric(val, ref v) && IsNumeric(decimals, ref db))
	{
		try
		{
			int d = (int) db;
			double ret = Math.Round(v, d);
			retval = ret.ToString(System.Globalization.CultureInfo.InvariantCulture);
		}
		catch (Exception)
		{
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
        
        private const string _strSrcSchemasList0 = @"BTNextGen.Biztalk.BasketMigration.newdataset";
        
        private const string _strTrgSchemasList0 = @"BTNextGen.Biztalk.BasketMigration.CompositeWhteOpsMaster+Request";
        
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
                _SrcSchemas[0] = @"BTNextGen.Biztalk.BasketMigration.newdataset";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"BTNextGen.Biztalk.BasketMigration.CompositeWhteOpsMaster+Request";
                return _TrgSchemas;
            }
        }
    }
}
