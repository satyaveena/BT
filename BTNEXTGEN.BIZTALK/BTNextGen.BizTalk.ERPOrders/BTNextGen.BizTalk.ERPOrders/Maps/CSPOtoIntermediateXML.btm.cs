namespace BTNextGen.BizTalk.ERPOrders.Maps {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.BizTalk.CS.Orders.Schemas.BTOrderGroupSchema", typeof(global::BTNextGen.BizTalk.CS.Orders.Schemas.BTOrderGroupSchema))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.BizTalk.ERPOrders.Schemas.IntermediateXMLOrder", typeof(global::BTNextGen.BizTalk.ERPOrders.Schemas.IntermediateXMLOrder))]
    public sealed class CSPOtoIntermediateXML : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var userCSharp"" version=""1.0"" xmlns:ns0=""http://BTNextGen.BizTalk.ERPOrders.Schemas.IntermediateXMLOrder"" xmlns:userCSharp=""http://schemas.microsoft.com/BizTalk/2003/userCSharp"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/OrderGroups"" />
  </xsl:template>
  <xsl:template match=""/OrderGroups"">
    <ns0:PO>
      <Header>
        <xsl:for-each select=""PurchaseOrderEx"">
          <xsl:for-each select=""OrderForms/OrderFormEx"">
            <xsl:variable name=""var:v2"" select=""userCSharp:LogicalEq(string(@BookAccountId) , &quot;&quot;)"" />
            <xsl:variable name=""var:v4"" select=""userCSharp:LogicalNot(string($var:v2))"" />
            <Customer>
              <xsl:variable name=""var:v1"" select=""userCSharp:BKOHeader(string(@IsBackOrder))"" />
              <xsl:attribute name=""BO"">
                <xsl:value-of select=""$var:v1"" />
              </xsl:attribute>
              <xsl:if test=""string($var:v2)='true'"">
                <xsl:variable name=""var:v3"" select=""@EntertainmentAccountId"" />
                <AccountNumber>
                  <xsl:value-of select=""$var:v3"" />
                </AccountNumber>
              </xsl:if>
              <xsl:if test=""string($var:v4)='true'"">
                <xsl:variable name=""var:v5"" select=""@BookAccountId"" />
                <AccountNumber>
                  <xsl:value-of select=""$var:v5"" />
                </AccountNumber>
              </xsl:if>
              <xsl:if test=""@ShippingMethodExId"">
                <ShipMethod>
                  <xsl:value-of select=""@ShippingMethodExId"" />
                </ShipMethod>
              </xsl:if>
            </Customer>
          </xsl:for-each>
        </xsl:for-each>
        <xsl:for-each select=""PurchaseOrderEx"">
          <xsl:for-each select=""OrderForms/OrderFormEx"">
            <xsl:variable name=""var:v6"" select=""userCSharp:StringConcat(string(@TransmissionNumber) , string(@BTTargetERP))"" />
            <Order>
              <xsl:if test=""@TransmissionNumber"">
                <TransmissionNumber>
                  <xsl:value-of select=""@TransmissionNumber"" />
                </TransmissionNumber>
              </xsl:if>
              <xsl:if test=""@OrderGroupId"">
                <OrderGroupID>
                  <xsl:value-of select=""@OrderGroupId"" />
                </OrderGroupID>
              </xsl:if>
              <xsl:if test=""@BTTargetERP"">
                <TargetERP>
                  <xsl:value-of select=""@BTTargetERP"" />
                </TargetERP>
              </xsl:if>
              <xsl:if test=""@PONumber"">
                <PONumber>
                  <xsl:value-of select=""@PONumber"" />
                </PONumber>
              </xsl:if>
              <xsl:if test=""@IsHomeDeliveryIndicator"">
                <ISHomeDelivery>
                  <xsl:value-of select=""@IsHomeDeliveryIndicator"" />
                </ISHomeDelivery>
              </xsl:if>
              <TransNoAndERP>
                <xsl:value-of select=""$var:v6"" />
              </TransNoAndERP>
              <xsl:if test=""@SpecialInstructions"">
                <SpecialInstructions>
                  <xsl:value-of select=""@SpecialInstructions"" />
                </SpecialInstructions>
              </xsl:if>
            </Order>
          </xsl:for-each>
        </xsl:for-each>
        <xsl:for-each select=""PurchaseOrderEx"">
          <xsl:for-each select=""Addresses/OrderAddressEx"">
            <xsl:variable name=""var:v7"" select=""userCSharp:LogicalExistence(boolean(.))"" />
            <Address>
              <xsl:if test=""string($var:v7)='true'"">
                <xsl:variable name=""var:v8"" select=""&quot;Y&quot;"" />
                <xsl:attribute name=""AddressChangeFlag"">
                  <xsl:value-of select=""$var:v8"" />
                </xsl:attribute>
              </xsl:if>
              <xsl:if test=""@Line2"">
                <xsl:attribute name=""Address2"">
                  <xsl:value-of select=""@Line2"" />
                </xsl:attribute>
              </xsl:if>
              <xsl:if test=""@Line1"">
                <Address1>
                  <xsl:value-of select=""@Line1"" />
                </Address1>
              </xsl:if>
              <xsl:if test=""@Line3"">
                <Address3>
                  <xsl:value-of select=""@Line3"" />
                </Address3>
              </xsl:if>
              <xsl:if test=""@Line4"">
                <Address4>
                  <xsl:value-of select=""@Line4"" />
                </Address4>
              </xsl:if>
              <xsl:if test=""@Name"">
                <Name>
                  <xsl:value-of select=""@Name"" />
                </Name>
              </xsl:if>
              <xsl:if test=""@City"">
                <City>
                  <xsl:value-of select=""@City"" />
                </City>
              </xsl:if>
              <xsl:if test=""@RegionCode"">
                <State>
                  <xsl:value-of select=""@RegionCode"" />
                </State>
              </xsl:if>
              <xsl:if test=""@CountryCode"">
                <Country>
                  <xsl:value-of select=""@CountryCode"" />
                </Country>
              </xsl:if>
              <xsl:if test=""@PostalCode"">
                <Zip>
                  <xsl:value-of select=""@PostalCode"" />
                </Zip>
              </xsl:if>
              <xsl:if test=""@DaytimePhoneNumber"">
                <Phone>
                  <xsl:value-of select=""@DaytimePhoneNumber"" />
                </Phone>
              </xsl:if>
            </Address>
          </xsl:for-each>
        </xsl:for-each>
      </Header>
      <Detail>
        <xsl:for-each select=""PurchaseOrderEx"">
          <xsl:for-each select=""OrderForms/OrderFormEx"">
            <xsl:for-each select=""LineItems/LineItemEx"">
              <xsl:variable name=""var:v10"" select=""userCSharp:LogicalEq(string(ItemLevelDiscountsApplied/DiscountApplicationRecord/@LineItemId) , string(@LineItemId))"" />
              <LineItem>
                <xsl:if test=""@BTKey"">
                  <xsl:attribute name=""BTKey"">
                    <xsl:value-of select=""@BTKey"" />
                  </xsl:attribute>
                </xsl:if>
                <xsl:if test=""@BTGTIN"">
                  <xsl:attribute name=""GTIN"">
                    <xsl:value-of select=""@BTGTIN"" />
                  </xsl:attribute>
                </xsl:if>
                <xsl:variable name=""var:v9"" select=""userCSharp:BKOLine(string(../../@IsBackOrder))"" />
                <xsl:attribute name=""BO"">
                  <xsl:value-of select=""$var:v9"" />
                </xsl:attribute>
                <xsl:if test=""@Quantity"">
                  <Quantity>
                    <xsl:value-of select=""@Quantity"" />
                  </Quantity>
                </xsl:if>
                <xsl:if test=""string($var:v10)='true'"">
                  <xsl:variable name=""var:v11"" select=""ItemLevelDiscountsApplied/DiscountApplicationRecord/@PromoCode"" />
                  <Promocode>
                    <xsl:value-of select=""$var:v11"" />
                  </Promocode>
                </xsl:if>
                <xsl:if test=""@ExtendedPrice"">
                  <OverridePrice>
                    <xsl:value-of select=""@ExtendedPrice"" />
                  </OverridePrice>
                </xsl:if>
                <xsl:if test=""@ListPrice"">
                  <ListPrice>
                    <xsl:value-of select=""@ListPrice"" />
                  </ListPrice>
                </xsl:if>
                <xsl:variable name=""var:v12"" select=""userCSharp:DiscPer(string(@ListPrice) , string(@ExtendedPrice))"" />
                <OverrideDiscount>
                  <xsl:value-of select=""$var:v12"" />
                </OverrideDiscount>
                <xsl:if test=""@LineItemNumberId"">
                  <LineNumber>
                    <xsl:value-of select=""@LineItemNumberId"" />
                  </LineNumber>
                </xsl:if>
                <xsl:if test=""@BTISBN"">
                  <ISBN>
                    <xsl:value-of select=""@BTISBN"" />
                  </ISBN>
                </xsl:if>
                <xsl:if test=""@BTUPC"">
                  <UPC>
                    <xsl:value-of select=""@BTUPC"" />
                  </UPC>
                </xsl:if>
              </LineItem>
            </xsl:for-each>
          </xsl:for-each>
        </xsl:for-each>
      </Detail>
      <xsl:for-each select=""PurchaseOrderEx"">
        <xsl:for-each select=""OrderForms/OrderFormEx"">
          <Footer>
            <xsl:if test=""@BTGiftWrapCode"">
              <GiftWrapCode>
                <xsl:value-of select=""@BTGiftWrapCode"" />
              </GiftWrapCode>
            </xsl:if>
            <xsl:if test=""@BTGiftWrapMessage"">
              <GiftWrapMsg>
                <xsl:value-of select=""@BTGiftWrapMessage"" />
              </GiftWrapMsg>
            </xsl:if>
            <xsl:if test=""@BTGiftWrapMessage"">
              <GiftWrapMsg2>
                <xsl:value-of select=""@BTGiftWrapMessage"" />
              </GiftWrapMsg2>
            </xsl:if>
          </Footer>
        </xsl:for-each>
      </xsl:for-each>
    </ns0:PO>
  </xsl:template>
  <msxsl:script language=""C#"" implements-prefix=""userCSharp""><![CDATA[
public string StringConcat(string param0, string param1)
{
   return param0 + param1;
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


public bool LogicalExistence(bool val)
{
	return val;
}


public string StringConcat(string param0)
{
   return param0;
}


public bool LogicalNot(string val)
{
	return !ValToBool(val);
}


///*Uncomment the following code for a sample Inline C# function
//that concatenates two inputs. Change the number of parameters of
//this function to be equal to the number of inputs connected to this functoid.*/

public double DiscPer(double list, double extended)
{
   double perdisc =  100 - (extended / list) * 100;
   if(perdisc < .01) 
     perdisc = 0;  
   return perdisc;
}


///*Uncomment the following code for a sample Inline C# function
//that concatenates two inputs. Change the number of parameters of
//this function to be equal to the number of inputs connected to this functoid.*/

public string BKOHeader(string IsBackOrder)
{
   if(IsBackOrder == ""true"")
      return ""Y"";
   else  if(IsBackOrder == ""false"")
      return ""N"";
   else
      return "" "";
}


///*Uncomment the following code for a sample Inline C# function
//that concatenates two inputs. Change the number of parameters of
//this function to be equal to the number of inputs connected to this functoid.*/

//The souce is the header because that is the only thing the UI touches.

public string BKOLine(string AllowBackordersAndPreorders)
{
   if(AllowBackordersAndPreorders == ""true"")
      return ""Y"";
   else if(AllowBackordersAndPreorders == ""false"")
      return ""N"";
   else
      return "" "";
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
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"BTNextGen.BizTalk.CS.Orders.Schemas.BTOrderGroupSchema";
        
        private const global::BTNextGen.BizTalk.CS.Orders.Schemas.BTOrderGroupSchema _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"BTNextGen.BizTalk.ERPOrders.Schemas.IntermediateXMLOrder";
        
        private const global::BTNextGen.BizTalk.ERPOrders.Schemas.IntermediateXMLOrder _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"BTNextGen.BizTalk.CS.Orders.Schemas.BTOrderGroupSchema";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"BTNextGen.BizTalk.ERPOrders.Schemas.IntermediateXMLOrder";
                return _TrgSchemas;
            }
        }
    }
}
