namespace BTNextGen.BizTalk.ERPOrders.Maps {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.BizTalk.ERPOrders.Schemas.IntermediateXMLOrder", typeof(global::BTNextGen.BizTalk.ERPOrders.Schemas.IntermediateXMLOrder))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.BizTalk.ERPOrders.Schemas.ERPOrdentBulk", typeof(global::BTNextGen.BizTalk.ERPOrders.Schemas.ERPOrdentBulk))]
    public sealed class IntXMLtoORDENTBulk : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0 userCSharp"" version=""1.0"" xmlns:s0=""http://BTNextGen.BizTalk.ERPOrders.Schemas.IntermediateXMLOrder"" xmlns:ns0=""BTNextGen.BizTalk.ERPOrders.Schemas.ERPOrdentBulk"" xmlns:userCSharp=""http://schemas.microsoft.com/BizTalk/2003/userCSharp"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s0:PO"" />
  </xsl:template>
  <xsl:template match=""/s0:PO"">
    <xsl:variable name=""var:v3"" select=""userCSharp:StringUpperCase(&quot;Y&quot;)"" />
    <xsl:variable name=""var:v4"" select=""userCSharp:DateCurrentDateTime()"" />
    <ns0:ORDENT>
      <Begin>
        <BEGIN>
          <xsl:text>BEGIN</xsl:text>
        </BEGIN>
        <TxnType>
          <xsl:text>ORDENT</xsl:text>
        </TxnType>
        <TransID>
          <xsl:value-of select=""Header/Order/TransmissionNumber/text()"" />
        </TransID>
      </Begin>
      <FileType>
        <FileType>
          <xsl:text>ORDENT</xsl:text>
        </FileType>
      </FileType>
      <FileHeader>
        <xsl:variable name=""var:v1"" select=""userCSharp:IncrementLineNum(string(Header/Customer/AccountNumber/text()))"" />
        <LineNumber>
          <xsl:value-of select=""$var:v1"" />
        </LineNumber>
        <HeaderInfo>
          <EDIID>
            <Name>
              <xsl:text>~EDIID</xsl:text>
            </Name>
            <Value>
              <xsl:value-of select=""Header/Customer/AccountNumber/text()"" />
            </Value>
          </EDIID>
          <BO>
            <Name>
              <xsl:text>BO</xsl:text>
            </Name>
            <xsl:variable name=""var:v2"" select=""userCSharp:BKOHead(string(Header/Customer/@BO))"" />
            <Value>
              <xsl:value-of select=""$var:v2"" />
            </Value>
          </BO>
          <SHPVIA>
            <Name>
              <xsl:text>SHPVIA</xsl:text>
            </Name>
          </SHPVIA>
          <PONUM>
            <Name>
              <xsl:text>PONUM</xsl:text>
            </Name>
            <Value>
              <xsl:value-of select=""Header/Order/PONumber/text()"" />
            </Value>
          </PONUM>
          <POCODE>
            <Name>
              <xsl:text>POCODE</xsl:text>
            </Name>
            <Value>
              <xsl:text>NE</xsl:text>
            </Value>
          </POCODE>
          <BLK>
            <Name>
              <xsl:text>BLK</xsl:text>
            </Name>
            <Value>
              <xsl:value-of select=""$var:v3"" />
            </Value>
          </BLK>
          <TRANSID>
            <Name>
              <xsl:text>TRANSID</xsl:text>
            </Name>
            <Value>
              <xsl:value-of select=""Header/Order/TransmissionNumber/text()"" />
            </Value>
          </TRANSID>
        </HeaderInfo>
      </FileHeader>
      <Date1>
        <xsl:variable name=""var:v5"" select=""userCSharp:IncrementLineNum(string($var:v4))"" />
        <LineNumber>
          <xsl:value-of select=""$var:v5"" />
        </LineNumber>
        <DateInfo>
          <DATECD1>
            <Name>
              <xsl:text>DATECD1</xsl:text>
            </Name>
          </DATECD1>
          <DATE1>
            <Name>
              <xsl:text>DATE1</xsl:text>
            </Name>
            <xsl:variable name=""var:v6"" select=""userCSharp:YYYYMMDD(string($var:v4))"" />
            <Value>
              <xsl:value-of select=""$var:v6"" />
            </Value>
          </DATE1>
        </DateInfo>
      </Date1>
      <HeadRefInfo>
        <xsl:variable name=""var:v7"" select=""userCSharp:IncrementLineNum(string(Header/Order/OrderGroupID/text()))"" />
        <LineNumber>
          <xsl:value-of select=""$var:v7"" />
        </LineNumber>
        <RefInfo>
          <REFCODE>
            <Name>
              <xsl:text>REFCODE</xsl:text>
            </Name>
            <Value>
              <xsl:text>CO</xsl:text>
            </Value>
          </REFCODE>
          <REFNUM>
            <Name>
              <xsl:text>REFNUM</xsl:text>
            </Name>
            <xsl:variable name=""var:v8"" select=""userCSharp:StrToHex(string(Header/Order/TransmissionNumber/text()))"" />
            <Value>
              <xsl:value-of select=""$var:v8"" />
            </Value>
          </REFNUM>
        </RefInfo>
      </HeadRefInfo>
      <Address0>
        <xsl:variable name=""var:v9"" select=""userCSharp:IncrementLineNum(string(Header/Address/Address1/text()))"" />
        <LineNumber>
          <xsl:value-of select=""$var:v9"" />
        </LineNumber>
        <EDIENT>
          <Name>
            <xsl:text>EDIENT</xsl:text>
          </Name>
          <Value>
            <xsl:text>ST</xsl:text>
          </Value>
        </EDIENT>
      </Address0>
      <xsl:for-each select=""Detail/LineItem"">
        <xsl:variable name=""var:v11"" select=""string(Quantity/text())"" />
        <xsl:variable name=""var:v12"" select=""userCSharp:MathRound($var:v11 , &quot;0&quot;)"" />
        <Details>
          <LineItemUPC>
            <xsl:variable name=""var:v10"" select=""userCSharp:IncrementLineNum(string(Quantity/text()))"" />
            <LineNumber>
              <xsl:value-of select=""$var:v10"" />
            </LineNumber>
            <LineItem>
              <CUSLIN>
                <Name>
                  <xsl:text>CUSLIN</xsl:text>
                </Name>
                <Value>
                  <xsl:value-of select=""LineNumber/text()"" />
                </Value>
              </CUSLIN>
              <QTY>
                <Name>
                  <xsl:text>QTY</xsl:text>
                </Name>
                <Value>
                  <xsl:value-of select=""$var:v12"" />
                </Value>
              </QTY>
              <UPC>
                <Name>
                  <xsl:text>EDIITM</xsl:text>
                </Name>
                <xsl:variable name=""var:v13"" select=""userCSharp:MySKU(string(ISBN/text()) , string(@GTIN) , string(UPC/text()) , string(@BTKey))"" />
                <Value>
                  <xsl:value-of select=""$var:v13"" />
                </Value>
              </UPC>
              <PO>
                <Name>
                  <xsl:text>PO</xsl:text>
                </Name>
                <xsl:if test=""POPerLine"">
                  <Value>
                    <xsl:value-of select=""POPerLine/text()"" />
                  </Value>
                </xsl:if>
              </PO>
              <DETPRICE>
                <Name>
                  <xsl:text>DETPRICE</xsl:text>
                </Name>
                <Value>
                  <xsl:value-of select=""ListPrice/text()"" />
                </Value>
              </DETPRICE>
              <BO>
                <Name>
                  <xsl:text>BO</xsl:text>
                </Name>
                <xsl:variable name=""var:v14"" select=""userCSharp:BKOLine(string(@BO))"" />
                <Value>
                  <xsl:value-of select=""$var:v14"" />
                </Value>
              </BO>
              <PROMOCD>
                <Name>
                  <xsl:text>PROMOCD</xsl:text>
                </Name>
                <Value>
                  <xsl:value-of select=""Promocode/text()"" />
                </Value>
              </PROMOCD>
              <PROMOPR>
                <Name>
                  <xsl:text>PROMOPR</xsl:text>
                </Name>
                <Value>
                  <xsl:value-of select=""OverridePrice/text()"" />
                </Value>
              </PROMOPR>
            </LineItem>
          </LineItemUPC>
          <xsl:value-of select=""./text()"" />
        </Details>
      </xsl:for-each>
      <FileFooter>
        <LineNumber>
          <xsl:text>999999</xsl:text>
        </LineNumber>
        <xsl:variable name=""var:v15"" select=""userCSharp:IncrementLineNum(string(Footer/text()))"" />
        <xsl:variable name=""var:v16"" select=""userCSharp:MathSubtract(string($var:v15) , &quot;1&quot;)"" />
        <TotalLines>
          <xsl:value-of select=""$var:v16"" />
        </TotalLines>
      </FileFooter>
      <End>
        <END>
          <xsl:text>END</xsl:text>
        </END>
        <TxnType>
          <xsl:text>ORDENT</xsl:text>
        </TxnType>
        <TransID>
          <xsl:value-of select=""Header/Order/TransmissionNumber/text()"" />
        </TransID>
      </End>
    </ns0:ORDENT>
  </xsl:template>
  <msxsl:script language=""C#"" implements-prefix=""userCSharp""><![CDATA[
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

int linenum = 0;

public int GetLineNum()
{
               linenum++;
	return linenum;
}


///*Uncomment the following code for a sample Inline C# function
//that concatenates two inputs. Change the number of parameters of
//this function to be equal to the number of inputs connected to this functoid.*/

int IncrementLineNum(string param1)
{
	return GetLineNum();
}


public string MathSubtract(string param0, string param1)
{
	System.Collections.ArrayList listValues = new System.Collections.ArrayList();
	listValues.Add(param0);
	listValues.Add(param1);
	double ret = 0;
	bool first = true;
	foreach (string obj in listValues)
	{
		if (first)
		{
			first = false;
			double d = 0;
			if (IsNumeric(obj, ref d))
			{
				ret = d;
			}
			else
			{
				return """";
			}
		}
		else
		{
			double d = 0;
			if (IsNumeric(obj, ref d))
			{
				ret -= d;
			}
			else
			{
				return """";
			}
		}
	}
	return ret.ToString(System.Globalization.CultureInfo.InvariantCulture);
}


///*Uncomment the following code for a sample Inline C# function
//that concatenates two inputs. Change the number of parameters of
//this function to be equal to the number of inputs connected to this functoid.*/

public string YYYYMMDD(string param1)
{
	return param1.Substring(0,4) + param1.Substring(5,2) + param1.Substring(8,2);
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


public string StringUpperCase(string str)
{
	if (str == null)
	{
		return """";
	}
	return str.ToUpper(System.Globalization.CultureInfo.InvariantCulture);
}


///*Uncomment the following code for a sample Inline C# function
//that concatenates two inputs. Change the number of parameters of
//this function to be equal to the number of inputs connected to this functoid.*/

//BTKey is always passed and starts with ""6"" for BTE items.
//   Therefore if it isn't a 6 in position 1 we start by using ISBN and go from there

public string MySKU(string inISBN, string inGTIN, string inUPC, string inBTKey)
{
   if (inISBN == """")
      inISBN = ""9999999999999"";

   if (inBTKey.Substring(0, 1) != ""6"")
      return inISBN;
   else if (inGTIN.Length > 0)
      return inGTIN;
   else if (inUPC.Length > 0 )
      return inUPC;
   else
      return inISBN;
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


///*No BKO function for bulk/STH orders at line level, so override to account default blank*/

public string BKOLine(string bko)
   {
      return "" "";
   }


///*No BKO function for bulk orders, so override to account default blank*/

public string BKOHead(string bko)
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
        
        private const string _strTrgSchemasList0 = @"BTNextGen.BizTalk.ERPOrders.Schemas.ERPOrdentBulk";
        
        private const global::BTNextGen.BizTalk.ERPOrders.Schemas.ERPOrdentBulk _trgSchemaTypeReference0 = null;
        
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
                _TrgSchemas[0] = @"BTNextGen.BizTalk.ERPOrders.Schemas.ERPOrdentBulk";
                return _TrgSchemas;
            }
        }
    }
}
