namespace BTNextGen.BizTalk.ERPOrders.Maps {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.BizTalk.ERPOrders.Schemas.IntermediateXMLOrder", typeof(global::BTNextGen.BizTalk.ERPOrders.Schemas.IntermediateXMLOrder))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.BizTalk.ERPOrders.Schemas.ERPBISACOrder", typeof(global::BTNextGen.BizTalk.ERPOrders.Schemas.ERPBISACOrder))]
    public sealed class IntXMLtoBISAC : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0 userCSharp"" version=""1.0"" xmlns:s0=""http://BTNextGen.BizTalk.ERPOrders.Schemas.IntermediateXMLOrder"" xmlns:ns0=""BTNextGen.BizTalk.ERPOrders.Schemas.ERPBISACOrder"" xmlns:userCSharp=""http://schemas.microsoft.com/BizTalk/2003/userCSharp"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s0:PO"" />
  </xsl:template>
  <xsl:template match=""/s0:PO"">
    <xsl:variable name=""var:v1"" select=""userCSharp:DateCurrentDateTime()"" />
    <xsl:variable name=""var:v18"" select=""count(/s0:PO/Detail/LineItem)"" />
    <ns0:BISAC>
      <BISAC_00>
        <RecordTag>
          <xsl:text>00</xsl:text>
        </RecordTag>
        <RecordSeqNumber>
          <xsl:text>00001</xsl:text>
        </RecordSeqNumber>
        <FileSourceSAN>
          <xsl:text>NEXTGEN</xsl:text>
        </FileSourceSAN>
        <SourceSuffix>
          <xsl:value-of select=""Header/Order/TargetERP/text()"" />
        </SourceSuffix>
        <FileSourceName>
          <xsl:value-of select=""Header/Order/TransmissionNumber/text()"" />
        </FileSourceName>
        <DateOfTransmission>
          <xsl:text>      </xsl:text>
        </DateOfTransmission>
        <NameOfFile>
          <xsl:value-of select=""$var:v1"" />
        </NameOfFile>
        <BISACVersionNumber>
          <xsl:text>F03</xsl:text>
        </BISACVersionNumber>
        <FileDestinationSAN>
          <xsl:text>1556150</xsl:text>
        </FileDestinationSAN>
        <FileDestinationSuffix>
          <xsl:text>     </xsl:text>
        </FileDestinationSuffix>
        <Filler>
          <xsl:text>     </xsl:text>
        </Filler>
      </BISAC_00>
      <BISAC_10>
        <RecordTag>
          <xsl:text>10</xsl:text>
        </RecordTag>
        <RecordSeqNumber>
          <xsl:text>00002</xsl:text>
        </RecordSeqNumber>
        <PONumber>
          <xsl:value-of select=""Header/Order/PONumber/text()"" />
        </PONumber>
        <CustomerSAN>
          <xsl:text>       </xsl:text>
        </CustomerSAN>
        <SourceSuffix>
          <xsl:text>     </xsl:text>
        </SourceSuffix>
        <VendorSAN>
          <xsl:text>1692100</xsl:text>
        </VendorSAN>
        <VendorSuffix>
          <xsl:text>     </xsl:text>
        </VendorSuffix>
        <OrderDate>
          <xsl:text>      </xsl:text>
        </OrderDate>
        <OrderCancelDate>
          <xsl:text>      </xsl:text>
        </OrderCancelDate>
        <xsl:variable name=""var:v2"" select=""userCSharp:BKOHead(string(Header/Customer/@BO))"" />
        <BackOrderCode>
          <xsl:value-of select=""$var:v2"" />
        </BackOrderCode>
        <DoNotExceedAction>
          <xsl:text> </xsl:text>
        </DoNotExceedAction>
        <DoNotExceedAmount>
          <xsl:text>       </xsl:text>
        </DoNotExceedAmount>
        <InvoiceCopies>
          <xsl:text>01</xsl:text>
        </InvoiceCopies>
        <SpecialInstructionCode>
          <xsl:text>N</xsl:text>
        </SpecialInstructionCode>
        <CatalogServiceCode>
          <xsl:text>0</xsl:text>
        </CatalogServiceCode>
        <CustShipToRecCount>
          <xsl:text>0</xsl:text>
        </CustShipToRecCount>
        <CustBillToRecCount>
          <xsl:text> </xsl:text>
        </CustBillToRecCount>
        <ISBNCode>
          <xsl:text>G</xsl:text>
        </ISBNCode>
        <SubstitutionCode>
          <xsl:text> </xsl:text>
        </SubstitutionCode>
        <DoNotShipBeforeDate>
          <xsl:text>110101</xsl:text>
        </DoNotShipBeforeDate>
        <ShippingCode>
          <xsl:text> </xsl:text>
        </ShippingCode>
      </BISAC_10>
      <BISAC_16>
        <RecordTag>
          <xsl:text>16</xsl:text>
        </RecordTag>
        <RecordSeqNumber>
          <xsl:text>00003</xsl:text>
        </RecordSeqNumber>
        <PONumber>
          <xsl:value-of select=""Header/Order/PONumber/text()"" />
        </PONumber>
        <CFCSAccount20Byte>
          <xsl:value-of select=""Header/Customer/AccountNumber/text()"" />
        </CFCSAccount20Byte>
        <CreditToken>
          <xsl:text>            </xsl:text>
        </CreditToken>
        <SpecialDiscountPercentage>
          <xsl:text>                            </xsl:text>
        </SpecialDiscountPercentage>
      </BISAC_16>
      <BISAC_21>
        <RecordTag>
          <xsl:text>21</xsl:text>
        </RecordTag>
        <RecordSeqNumber>
          <xsl:text>00004</xsl:text>
        </RecordSeqNumber>
        <PONumber>
          <xsl:value-of select=""Header/Order/PONumber/text()"" />
        </PONumber>
        <xsl:if test=""Header/Order/SpecialInstructions"">
          <SpecialInstructions>
            <xsl:value-of select=""Header/Order/SpecialInstructions/text()"" />
          </SpecialInstructions>
        </xsl:if>
      </BISAC_21>
      <xsl:for-each select=""Detail/LineItem"">
        <xsl:variable name=""var:v3"" select=""position()"" />
        <xsl:variable name=""var:v5"" select=""userCSharp:MathRound(string(Quantity/text()) , &quot;0&quot;)"" />
        <xsl:variable name=""var:v11"" select=""userCSharp:StringConcat(&quot;  &quot; , string(@BTKey))"" />
        <BISAC_Cons>
          <RecordTag_40>
            <xsl:text>40</xsl:text>
          </RecordTag_40>
          <xsl:variable name=""var:v4"" select=""userCSharp:SetNumber(string($var:v3))"" />
          <RecordSeqNumber_40>
            <xsl:value-of select=""$var:v4"" />
          </RecordSeqNumber_40>
          <PONumber_40>
            <xsl:value-of select=""../../Header/Order/PONumber/text()"" />
          </PONumber_40>
          <AppendRecordDate_40>
            <xsl:text>N</xsl:text>
          </AppendRecordDate_40>
          <LineItemNumber_40>
            <xsl:value-of select=""LineNumber/text()"" />
          </LineItemNumber_40>
          <ISBN_40>
            <xsl:text>          </xsl:text>
          </ISBN_40>
          <Quantity_40>
            <xsl:value-of select=""$var:v5"" />
          </Quantity_40>
          <CatalogServiceCode_40>
            <xsl:text> </xsl:text>
          </CatalogServiceCode_40>
          <xsl:variable name=""var:v6"" select=""userCSharp:priceFormat(string(ListPrice/text()))"" />
          <Price_40>
            <xsl:value-of select=""$var:v6"" />
          </Price_40>
          <VolumesInSet_40>
            <xsl:text>  </xsl:text>
          </VolumesInSet_40>
          <LCPrefix_40>
            <xsl:text>   </xsl:text>
          </LCPrefix_40>
          <LC_40>
            <xsl:text>        </xsl:text>
          </LC_40>
          <DiscountPercent_40>
            <xsl:text>00000</xsl:text>
          </DiscountPercent_40>
          <SubstitutionCode_40>
            <xsl:text> </xsl:text>
          </SubstitutionCode_40>
          <CatalogCode_40>
            <xsl:text>000</xsl:text>
          </CatalogCode_40>
          <xsl:variable name=""var:v7"" select=""userCSharp:BKOLine(string(@BO))"" />
          <BackOrderCode_40>
            <xsl:value-of select=""$var:v7"" />
          </BackOrderCode_40>
          <Filler_40>
            <xsl:text>  </xsl:text>
          </Filler_40>
          <xsl:variable name=""var:v8"" select=""userCSharp:MyDelim()"" />
          <Delimiter_46>
            <xsl:value-of select=""$var:v8"" />
          </Delimiter_46>
          <RecordTag_46>
            <xsl:text>46</xsl:text>
          </RecordTag_46>
          <xsl:variable name=""var:v9"" select=""userCSharp:SetNumber(string($var:v3))"" />
          <xsl:variable name=""var:v10"" select=""userCSharp:MathAdd(string($var:v9) , &quot;1&quot;)"" />
          <RecordSeqNumber_46>
            <xsl:value-of select=""$var:v10"" />
          </RecordSeqNumber_46>
          <PONumber_46>
            <xsl:value-of select=""../../Header/Order/PONumber/text()"" />
          </PONumber_46>
          <BTKey_46>
            <xsl:value-of select=""$var:v11"" />
          </BTKey_46>
          <ReplacementBTKey_46>
            <xsl:text>                    </xsl:text>
          </ReplacementBTKey_46>
          <Filler_46>
            <xsl:text>                    </xsl:text>
          </Filler_46>
          <xsl:variable name=""var:v12"" select=""userCSharp:MyDelim()"" />
          <Delimiter_48>
            <xsl:value-of select=""$var:v12"" />
          </Delimiter_48>
          <RecordTag_48>
            <xsl:text>48</xsl:text>
          </RecordTag_48>
          <xsl:variable name=""var:v13"" select=""userCSharp:SetNumber(string($var:v3))"" />
          <xsl:variable name=""var:v14"" select=""userCSharp:MathAdd(string($var:v13) , &quot;1&quot;)"" />
          <xsl:variable name=""var:v15"" select=""userCSharp:MathAdd(string($var:v14) , &quot;1&quot;)"" />
          <RecordSeqNumber_48>
            <xsl:value-of select=""$var:v15"" />
          </RecordSeqNumber_48>
          <PONumber_48>
            <xsl:value-of select=""../../Header/Order/PONumber/text()"" />
          </PONumber_48>
          <PromoCode_48>
            <xsl:value-of select=""Promocode/text()"" />
          </PromoCode_48>
          <PriceOverrideIndicator_48>
            <xsl:text>     </xsl:text>
          </PriceOverrideIndicator_48>
          <xsl:variable name=""var:v16"" select=""userCSharp:priceFormat(string(OverridePrice/text()))"" />
          <OverridePrice_48>
            <xsl:value-of select=""$var:v16"" />
          </OverridePrice_48>
          <xsl:variable name=""var:v17"" select=""userCSharp:discperFormat(string(OverrideDiscount/text()))"" />
          <OverrideDiscount_48>
            <xsl:value-of select=""$var:v17"" />
          </OverrideDiscount_48>
          <Filler_48>
            <xsl:text>                                       </xsl:text>
          </Filler_48>
        </BISAC_Cons>
      </xsl:for-each>
      <BISAC_50>
        <RecordTag>
          <xsl:text>50</xsl:text>
        </RecordTag>
        <xsl:variable name=""var:v19"" select=""userCSharp:CalcSeq(string($var:v18))"" />
        <RecordSeqNumber>
          <xsl:value-of select=""$var:v19"" />
        </RecordSeqNumber>
        <PONumber>
          <xsl:value-of select=""Header/Order/PONumber/text()"" />
        </PONumber>
        <TotalPurchaseOrderRecords>
          <xsl:text>00001</xsl:text>
        </TotalPurchaseOrderRecords>
        <TotalTitlesOrdered>
          <xsl:text>0000000001</xsl:text>
        </TotalTitlesOrdered>
        <TotalBooksOrdered>
          <xsl:text>0000000001</xsl:text>
        </TotalBooksOrdered>
        <xsl:variable name=""var:v20"" select=""userCSharp:StrToHex(string(Header/Order/TransmissionNumber/text()))"" />
        <Filler>
          <xsl:value-of select=""$var:v20"" />
        </Filler>
      </BISAC_50>
      <BISAC_90>
        <RecordTag>
          <xsl:text>90</xsl:text>
        </RecordTag>
        <xsl:variable name=""var:v21"" select=""userCSharp:CalcSeq(string($var:v18))"" />
        <xsl:variable name=""var:v22"" select=""userCSharp:MathAdd(string($var:v21) , &quot;1&quot;)"" />
        <SequenceNumber>
          <xsl:value-of select=""$var:v22"" />
        </SequenceNumber>
        <TotalLineItems>
          <xsl:text>0000000000001</xsl:text>
        </TotalLineItems>
        <TotalPurchaseOrders>
          <xsl:text>00001</xsl:text>
        </TotalPurchaseOrders>
        <TotalBooks>
          <xsl:text>0000000001</xsl:text>
        </TotalBooks>
        <Total_0_9_Records>
          <xsl:text>     </xsl:text>
        </Total_0_9_Records>
        <Total_10_19_Records>
          <xsl:text>     </xsl:text>
        </Total_10_19_Records>
        <Total_20_29_Records>
          <xsl:text>     </xsl:text>
        </Total_20_29_Records>
        <Total_30_39_Records>
          <xsl:text>     </xsl:text>
        </Total_30_39_Records>
        <Total_40_49_Records>
          <xsl:text>     </xsl:text>
        </Total_40_49_Records>
        <Total_50_59_Records>
          <xsl:text>     </xsl:text>
        </Total_50_59_Records>
        <Total_60_69_Records>
          <xsl:text>     </xsl:text>
        </Total_60_69_Records>
        <Total_70_79_Records>
          <xsl:text>     </xsl:text>
        </Total_70_79_Records>
        <Total_80_89_Records>
          <xsl:text>     </xsl:text>
        </Total_80_89_Records>
      </BISAC_90>
    </ns0:BISAC>
  </xsl:template>
  <msxsl:script language=""C#"" implements-prefix=""userCSharp""><![CDATA[
///*Uncomment the following code for a sample Inline C# function
//that concatenates two inputs. Change the number of parameters of
//this function to be equal to the number of inputs connected to this functoid.*/

public string MyDelim()
{
	return ""\r\n"";
}


///*Uncomment the following code for a sample Inline C# function
//that concatenates two inputs. Change the number of parameters of
//this function to be equal to the number of inputs connected to this functoid.*/

public int SetNumber(int param1)
{
                int result =  param1 + 4 + ((param1 - 1) * 2);
	return result;
}


public string MathAdd(string param0, string param1)
{
	System.Collections.ArrayList listValues = new System.Collections.ArrayList();
	listValues.Add(param0);
	listValues.Add(param1);
	double ret = 0;
	foreach (string obj in listValues)
	{
	double d = 0;
		if (IsNumeric(obj, ref d))
		{
			ret += d;
		}
		else
		{
			return """";
		}
	}
	return ret.ToString(System.Globalization.CultureInfo.InvariantCulture);
}


public string DateCurrentDateTime()
{
	DateTime dt = DateTime.Now;
	string curdate = dt.ToString(""yyyy-MM-dd"", System.Globalization.CultureInfo.InvariantCulture);
	string curtime = dt.ToString(""T"", System.Globalization.CultureInfo.InvariantCulture);
	string retval = curdate + ""T"" + curtime;
	return retval;
}


///*Uncomment the following code for a sample Inline C# function
//that concatenates two inputs. Change the number of parameters of
//this function to be equal to the number of inputs connected to this functoid.*/

public int CalcSeq(int param1)
{
	return (param1 * 3) + 4;
}


public string StringConcat(string param0, string param1)
{
   return param0 + param1;
}


//This is used to pre-format the transno into what it needs to be
//for passing to the MQ Envelope as a string. Promoted for Orch use.
//It doesn't translate correctly if passed as is.


public string buildit = """";
public string digit = """";

public string StrToHex(string Data)
{

            foreach (char c in Data.ToCharArray())
            {
                   digit = String.Format(""{0:X}"", Convert.ToUInt32(c));
                   buildit = buildit + digit;
            }

           return buildit;
}


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


///*Uncomment the following code for a sample Inline C# function
//that concatenates two inputs. Change the number of parameters of
//this function to be equal to the number of inputs connected to this functoid.*/

public string priceFormat(double price)
{
   price = price * 100;
   int iprice = Convert.ToInt32(price);
   string pricestr = iprice.ToString();
   return pricestr;
}


///*Uncomment the following code for a sample Inline C# function
//that concatenates two inputs. Change the number of parameters of
//this function to be equal to the number of inputs connected to this functoid.*/

public string discperFormat(double disc)
{
   disc = disc * 100;
   int idisc = Convert.ToInt32(disc);
   string discstr = idisc.ToString();
   return discstr;
}


///*No BKO function for bulk orders, so override to account default blank*/

public string BKOHead(string bko)
   {
      return "" "";
   }


///*No BKO function for bulk orders, so override to account default blank*/

public string BKOLine(string bko)
   {
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


]]></msxsl:script>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"BTNextGen.BizTalk.ERPOrders.Schemas.IntermediateXMLOrder";
        
        private const global::BTNextGen.BizTalk.ERPOrders.Schemas.IntermediateXMLOrder _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"BTNextGen.BizTalk.ERPOrders.Schemas.ERPBISACOrder";
        
        private const global::BTNextGen.BizTalk.ERPOrders.Schemas.ERPBISACOrder _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"BTNextGen.BizTalk.ERPOrders.Schemas.IntermediateXMLOrder";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"BTNextGen.BizTalk.ERPOrders.Schemas.ERPBISACOrder";
                return _TrgSchemas;
            }
        }
    }
}
