namespace BTNextGen.BizTalk.ERPOrders.Maps {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.BizTalk.ERPOrders.Schemas.POAckBISAC", typeof(global::BTNextGen.BizTalk.ERPOrders.Schemas.POAckBISAC))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.BizTalk.ERPOrders.Schemas.IntermediateXMLAck", typeof(global::BTNextGen.BizTalk.ERPOrders.Schemas.IntermediateXMLAck))]
    public sealed class BISACAckToIXMLAck : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0 userCSharp"" version=""1.0"" xmlns:s0=""BTNextGen.BizTalk.ERPOrders.Schemas.POAckBISAC"" xmlns:ns0=""BTNextGen.BizTalk.ERPOrders.Schemas.IntermediateXMLAck"" xmlns:userCSharp=""http://schemas.microsoft.com/BizTalk/2003/userCSharp"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s0:POAck"" />
  </xsl:template>
  <xsl:template match=""/s0:POAck"">
    <xsl:variable name=""var:v1"" select=""userCSharp:StringUpperCase(&quot;BTB&quot;)"" />
    <xsl:variable name=""var:v2"" select=""userCSharp:StringConcat(string(Header/FileHeader/SourceSan02/text()) , string(Header/FileHeader/SourceSuffix02/text()))"" />
    <xsl:variable name=""var:v6"" select=""userCSharp:DateCurrentDate()"" />
    <xsl:variable name=""var:v7"" select=""userCSharp:StringConcat(string(Header/FileHeader/TransmissionID/text()) , &quot;BTB&quot;)"" />
    <ns0:POAck>
      <POAckHeader>
        <TransactionID>
          <xsl:value-of select=""Header/FileHeader/TransmissionID/text()"" />
        </TransactionID>
        <TargetERP>
          <xsl:value-of select=""$var:v1"" />
        </TargetERP>
        <SourceSan>
          <xsl:value-of select=""$var:v2"" />
        </SourceSan>
        <AccountID>
          <xsl:value-of select=""Header/POHeader12/AccountNumber/text()"" />
        </AccountID>
        <PONumber>
          <xsl:value-of select=""Header/POHeader11/PONumber/text()"" />
        </PONumber>
        <xsl:variable name=""var:v3"" select=""userCSharp:InitCumulativeConcat(0)"" />
        <xsl:for-each select=""/s0:POAck/Header/Lines21/POHeader21/Message"">
          <xsl:variable name=""var:v4"" select=""userCSharp:AddToCumulativeConcat(0,string(./text()),&quot;1000&quot;)"" />
        </xsl:for-each>
        <xsl:variable name=""var:v5"" select=""userCSharp:GetCumulativeConcat(0)"" />
        <ERPStatusMessage>
          <xsl:value-of select=""$var:v5"" />
        </ERPStatusMessage>
        <AckDate>
          <xsl:value-of select=""$var:v6"" />
        </AckDate>
        <ERPOrderNumber>
          <xsl:value-of select=""Header/POHeader12/BTBOrderNumber/text()"" />
        </ERPOrderNumber>
        <TransNoAndERP>
          <xsl:value-of select=""$var:v7"" />
        </TransNoAndERP>
        <xsl:for-each select=""Trailer"">
          <xsl:for-each select=""Lines58"">
            <xsl:variable name=""var:v10"" select=""userCSharp:LogicalExistence(boolean(.))"" />
            <BasketVAS>
              <xsl:for-each select=""POTrailer58"">
                <VASRecord>
                  <xsl:variable name=""var:v8"" select=""userCSharp:StrToIntVQ(string(VASQuantity/text()))"" />
                  <VASQuantity>
                    <xsl:value-of select=""$var:v8"" />
                  </VASQuantity>
                  <xsl:variable name=""var:v9"" select=""userCSharp:StrToIntDiv100VUP(string(VASUnitPrice/text()))"" />
                  <VASUnitPrice>
                    <xsl:value-of select=""$var:v9"" />
                  </VASUnitPrice>
                  <VASDescription>
                    <xsl:value-of select=""VASDescription/text()"" />
                  </VASDescription>
                  <VASBTKey>
                    <xsl:value-of select=""VASBTKey/text()"" />
                  </VASBTKey>
                </VASRecord>
              </xsl:for-each>
              <xsl:if test=""string($var:v10)='true'"">
                <xsl:variable name=""var:v11"" select=""./text()"" />
                <xsl:value-of select=""$var:v11"" />
              </xsl:if>
            </BasketVAS>
          </xsl:for-each>
        </xsl:for-each>
        <xsl:variable name=""var:v12"" select=""userCSharp:InitCumulativeConcat(0)"" />
        <xsl:for-each select=""/s0:POAck/Header/Lines21/POHeader21/Message"">
          <xsl:variable name=""var:v13"" select=""userCSharp:AddToCumulativeConcat(0,string(./text()),&quot;1000&quot;)"" />
        </xsl:for-each>
        <xsl:variable name=""var:v14"" select=""userCSharp:GetCumulativeConcat(0)"" />
        <xsl:variable name=""var:v15"" select=""userCSharp:StringTrimRight(string($var:v14))"" />
        <xsl:variable name=""var:v16"" select=""userCSharp:StringTrimLeft(string($var:v15))"" />
        <xsl:variable name=""var:v17"" select=""userCSharp:IsCPQAk(string($var:v16))"" />
        <IsCPQAck>
          <xsl:value-of select=""$var:v17"" />
        </IsCPQAck>
      </POAckHeader>
      <xsl:for-each select=""LineItem"">
        <xsl:for-each select=""POLine46"">
          <xsl:variable name=""var:v19"" select=""string(BTKeyOrdered/text())"" />
          <xsl:variable name=""var:v20"" select=""userCSharp:StringTrimLeft($var:v19)"" />
          <xsl:variable name=""var:v22"" select=""userCSharp:StringTrimLeft(string(BTKeyReplaced/text()))"" />
          <xsl:variable name=""var:v25"" select=""string(../POLine40/OrderQTY/text())"" />
          <xsl:variable name=""var:v26"" select=""string(../POLine40/ShippableQTY/text())"" />
          <xsl:variable name=""var:v28"" select=""string(../POLine40/StatusCode/text())"" />
          <POAckDetails>
            <xsl:variable name=""var:v18"" select=""userCSharp:ReturnNaught(string(BTKeyOrdered/text()))"" />
            <ISBN>
              <xsl:value-of select=""$var:v18"" />
            </ISBN>
            <BTKey>
              <xsl:value-of select=""$var:v20"" />
            </BTKey>
            <xsl:variable name=""var:v21"" select=""userCSharp:ReturnNaught($var:v19)"" />
            <GTIN>
              <xsl:value-of select=""$var:v21"" />
            </GTIN>
            <ReplacementBTKey>
              <xsl:value-of select=""$var:v22"" />
            </ReplacementBTKey>
            <Description>
              <xsl:value-of select=""../POLine42/Title/text()"" />
            </Description>
            <xsl:variable name=""var:v23"" select=""userCSharp:StrToInt(string(../POLine40/OrderQTY/text()))"" />
            <OrderQTY>
              <xsl:value-of select=""$var:v23"" />
            </OrderQTY>
            <xsl:variable name=""var:v24"" select=""userCSharp:StrToInt(string(../POLine40/ShippableQTY/text()))"" />
            <ShippableQTY>
              <xsl:value-of select=""$var:v24"" />
            </ShippableQTY>
            <xsl:variable name=""var:v27"" select=""userCSharp:CalcCancel(string(../POLine40/StatusCode/text()) , $var:v25 , $var:v26)"" />
            <CancelQTY>
              <xsl:value-of select=""$var:v27"" />
            </CancelQTY>
            <xsl:variable name=""var:v29"" select=""userCSharp:CalcBackorderl($var:v28 , $var:v25 , $var:v26)"" />
            <BackOrderQTY>
              <xsl:value-of select=""$var:v29"" />
            </BackOrderQTY>
            <StatusCode>
              <xsl:value-of select=""../POLine40/StatusCode/text()"" />
            </StatusCode>
            <xsl:variable name=""var:v30"" select=""userCSharp:XLateStatus($var:v28)"" />
            <StatusDescription>
              <xsl:value-of select=""$var:v30"" />
            </StatusDescription>
            <xsl:variable name=""var:v31"" select=""userCSharp:XlateWHS(string(../POLine40/StatusWHS/text()))"" />
            <StatusWHS>
              <xsl:value-of select=""$var:v31"" />
            </StatusWHS>
            <LineNumber>
              <xsl:value-of select=""../POLine40/CusLineNumber/text()"" />
            </LineNumber>
            <xsl:variable name=""var:v32"" select=""userCSharp:YNtoBoolVA(string(../LineItem49/POLine49/VASAvailable/text()))"" />
            <VASAvailable>
              <xsl:value-of select=""$var:v32"" />
            </VASAvailable>
            <xsl:variable name=""var:v33"" select=""userCSharp:YNtoBoolSI(string(../LineItem49/POLine49/SurchargeItem/text()))"" />
            <SurchargeItem>
              <xsl:value-of select=""$var:v33"" />
            </SurchargeItem>
            <xsl:variable name=""var:v34"" select=""userCSharp:VexStrToIntDiv100(string(../LineItem49/POLine49/ExtendedWithVAS/text()))"" />
            <ExtendedWithVAS>
              <xsl:value-of select=""$var:v34"" />
            </ExtendedWithVAS>
            <xsl:variable name=""var:v35"" select=""userCSharp:StrToIntDiv100QP(string(../POLine40/NetPrice/text()))"" />
            <NetPrice>
              <xsl:value-of select=""$var:v35"" />
            </NetPrice>
            <xsl:for-each select=""../POLine41"">
              <SplitShipWHS>
                <xsl:variable name=""var:v36"" select=""userCSharp:AnalyzeShipQTY(string(ShippableQTY[1]/text()))"" />
                <QTY>
                  <xsl:value-of select=""$var:v36"" />
                </QTY>
                <xsl:variable name=""var:v37"" select=""userCSharp:GetWHS(string(WHSCode/text()))"" />
                <WHSCOD>
                  <xsl:value-of select=""$var:v37"" />
                </WHSCOD>
              </SplitShipWHS>
            </xsl:for-each>
          </POAckDetails>
        </xsl:for-each>
      </xsl:for-each>
    </ns0:POAck>
  </xsl:template>
  <msxsl:script language=""C#"" implements-prefix=""userCSharp""><![CDATA[
public string StringConcat(string param0, string param1)
{
   return param0 + param1;
}


public string DateCurrentDate()
{
	DateTime dt = DateTime.Now;
	return dt.ToString(""yyyy-MM-dd"", System.Globalization.CultureInfo.InvariantCulture);
}


public string StringTrimLeft(string str)
{
	if (str == null)
	{
		return """";
	}
	return str.TrimStart(null);
}


///*Uncomment the following code for a sample Inline C# function
//that concatenates two inputs. Change the number of parameters of
//this function to be equal to the number of inputs connected to this functoid.*/

public string XLateStatus(string status)
{
   switch (status)
   {
      case ""00"": return ""Priority One/Held Order - Allocation Information Unavailable""; break;
      case ""01"": return ""Allocated for Shipping as Ordered.""; break;
      case ""02"": return ""Shipping Substituted Item.""; break;
      case ""03"": return ""Cancelled: Future Publication""; break;
      case ""04"": return ""Cancelled: Future Publication""; break;
      case ""05"": return ""Backordered: Future Publication.""; break;
      case ""06"": return ""Cancelled: Out of Stock- Being Reprinted.""; break;
      case ""07"": return ""Backordered: Out of Stock- Being Reprinted.""; break;
      case ""08"": return ""Cancelled: Out of Stock- Not Being Reprinted.""; break;
      case ""09"": return ""Partially Filled. Remainder Cancelled.""; break;
      case ""10"": return ""Partially Filled Remainder Backordered.""; break;
      case ""11"": return ""Cancelled: Out of Print in Cloth. Available in Paper.""; break;
      case ""12"": return ""Cancelled: Out of Print in Paper. Available in Cloth.""; break;
      case ""13"": return ""Cancelled: New Edition Is Being Printed.""; break;
      case ""14"": return ""Backordered: New Edition Is Being Printed and is Backordered.""; break;
      case ""15"": return ""Cancelled: Market Restricted or CPQ Cancelled Report Code.""; break;
      case ""16"": return ""Cancelled: Not Our Publication""; break;
      case ""17"": return ""Free Book or 100% Discount.""; break;
      case ""18"": return ""Cancelled: Publisher Did Not Respond by Cancellation Date.""; break;
      case ""19"": return ""Cancelled: Books Sold by Subscription Only.""; break;
      case ""20"": return ""Cancelled: We Do Not Supply This Title.""; break;
      case ""21"": return ""Cancelled: We Do Not supply This Publisher.""; break;
      case ""22"": return ""Cancelled: Importation""; break;
      case ""23"": return ""Cancelled: Apply Direct - Not Available Through Wholesale.""; break;
      case ""24"": return ""Cancelled: Kits Not Available.""; break;
      case ""25"": return ""Cancelled: Not Available as a Processed Book.""; break;
      case ""26"": return ""Cancelled: New Price From Publisher.""; break;
      case ""27"": return ""Cancelled: ISBN Not Recognized.""; break;
      case ""28"": return ""Cancelled: Out of Print.""; break;
      case ""29"": return ""Backordered: At Customer's Request.""; break;
      case ""99"": return ""Cancelled: Unknown Reason. See Order Header.  No Line Status.""; break;
      case ""  "": return ""Cancelled: Unknown Reason. See Order Header.  No Line Status.""; break;
      case "" "": return ""Cancelled: Unknown Reason. See Order Header.  No Line Status.""; break;
      case """": return ""Cancelled: Unknown Reason. See Order Header.  No Line Status.""; break;
      default: return ""Unknown Status Code: "" + status; break;
   }
}


///*Uncomment the following code for a sample Inline C# function
//that concatenates two inputs. Change the number of parameters of
//this function to be equal to the number of inputs connected to this functoid.*/

public int CalcCancel(string status, int ordered, int shipped)
{
   switch (status)
   {
      case ""03"": return ordered - shipped; break;
      case ""04"": return ordered - shipped; break;
      case ""06"": return ordered - shipped; break;
      case ""08"": return ordered - shipped; break;
      case ""09"": return ordered - shipped; break;
      case ""11"": return ordered - shipped; break;
      case ""12"": return ordered - shipped; break;
      case ""13"": return ordered - shipped; break;
      case ""15"": return ordered - shipped; break;
      case ""16"": return ordered - shipped; break;
      case ""18"": return ordered - shipped; break;
      case ""19"": return ordered - shipped; break;
      case ""20"": return ordered - shipped; break;
      case ""21"": return ordered - shipped; break;
      case ""22"": return ordered - shipped; break;
      case ""23"": return ordered - shipped; break;
      case ""24"": return ordered - shipped; break;
      case ""25"": return ordered - shipped; break;
      case ""26"": return ordered - shipped; break;
      case ""27"": return ordered - shipped; break;
      case ""28"": return ordered - shipped; break;
      case ""99"": return ordered - shipped; break;
      case ""  "": return ordered - shipped; break;
      case "" "": return ordered - shipped; break;
      case """": return ordered - shipped; break;
      default: return 0; break;
   }
}


///*Uncomment the following code for a sample Inline C# function
//that concatenates two inputs. Change the number of parameters of
//this function to be equal to the number of inputs connected to this functoid.*/

public int CalcBackorderl(string status, int ordered, int shipped)
{
   switch (status)
   {
      case ""05"":return ordered - shipped; break;
      case ""07"":return ordered - shipped; break;
      case ""10"":return ordered - shipped; break;
      case ""14"":return ordered - shipped; break;
      case ""29"": return ordered - shipped; break;
      default: return 0; break;
   }
}


///*Uncomment the following code for a sample Inline C# function
//that concatenates two inputs. Change the number of parameters of
//this function to be equal to the number of inputs connected to this functoid.*/

int StrToInt(int shipped)
{
   return shipped;
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

public string ReturnNaught(string param1)
{
   return """";
}


///*Uncomment the following code for a sample Inline C# function
//that concatenates two inputs. Change the number of parameters of
//this function to be equal to the number of inputs connected to this functoid.*/

public string XlateWHS(string WHS2)
{
   switch(WHS2)
   {
      case ""CO"": return ""COM""; break;
      case ""MO"": return ""MOM""; break;
      case ""RE"": return ""REN""; break;
      case ""SO"": return ""SOM""; break;
      default: return """"; break;
   }  
}


///*Uncomment the following code for a sample Inline C# function
//that concatenates two inputs. Change the number of parameters of
//this function to be equal to the number of inputs connected to this functoid.*/

public string AnalyzeShipQTY(string Ship41)
{
   return Convert.ToString(Convert.ToInt32(Ship41));
}


///*Uncomment the following code for a sample Inline C# function
//that concatenates two inputs. Change the number of parameters of
//this function to be equal to the number of inputs connected to this functoid.*/

string GetWHS(string WHS2)
{
   return XlateWHS(WHS2);
}


///*Uncomment the following code for a sample Inline C# function
//that concatenates two inputs. Change the number of parameters of
//this function to be equal to the number of inputs connected to this functoid.*/

decimal VexStrToIntDiv100(string ExWVAS)
{ 
   decimal ExWVASDec = 0.0M;
   if(ExWVAS == """")
      return 0.0M;
   else
      ExWVASDec = Convert.ToDecimal(ExWVAS);
      return ExWVASDec / 100;
}

///*Uncomment the following code for a sample Inline C# function
//that concatenates two inputs. Change the number of parameters of
//this function to be equal to the number of inputs connected to this functoid.*/

string YNtoBoolVA(string VASAvail)
{
   if(VASAvail == ""Y"")
      return ""true"";
   else if(VASAvail == ""N"")
      return ""false"";
   else
      return ""false"";
}

///*Uncomment the following code for a sample Inline C# function
//that concatenates two inputs. Change the number of parameters of
//this function to be equal to the number of inputs connected to this functoid.*/

///*Uncomment the following code for a sample Inline C# function
//that concatenates two inputs. Change the number of parameters of
//this function to be equal to the number of inputs connected to this functoid.*/

string YNtoBoolSI(string VASAvail)
{
   if(VASAvail == ""Y"")
      return ""true"";
   else if(VASAvail == ""N"")
      return ""false"";
   else
      return ""false"";
}

///*Uncomment the following code for a sample Inline C# function
//that concatenates two inputs. Change the number of parameters of
//this function to be equal to the number of inputs connected to this functoid.*/

decimal StrToIntDiv100VUP(string VASUnitPrc)
{ 
   decimal VASUnitDec = 0.0M;
   if(VASUnitPrc == """")
      return 0.0M;
   else
      VASUnitDec = Convert.ToDecimal(VASUnitPrc);
      return VASUnitDec / 100;
}

///*Uncomment the following code for a sample Inline C# function
//that concatenates two inputs. Change the number of parameters of
//this function to be equal to the number of inputs connected to this functoid.*/

int StrToIntVQ(int VASQty)
{
   return VASQty;
}

///*Uncomment the following code for a sample Inline C# function
//that concatenates two inputs. Change the number of parameters of
//this function to be equal to the number of inputs connected to this functoid.*/

decimal StrToIntDiv100QP(string NetPrc)
{ 
   decimal NetPrcDec = 0.0M;
   if(NetPrc == """")
      return 0.0M;
   else
      NetPrcDec = Convert.ToDecimal(NetPrc);
      return NetPrcDec / 100;
}

///*Uncomment the following code for a sample Inline C# function
//that concatenates two inputs. Change the number of parameters of
//this function to be equal to the number of inputs connected to this functoid.*/

public bool IsCPQAk(string message)
{
   if(message.Contains(""CPQ QUOTE"" ))
       return true;
   else
       return false;
}


public string StringTrimRight(string str)
{
	if (str == null)
	{
		return """";
	}
	return str.TrimEnd(null);
}


public bool LogicalExistence(bool val)
{
	return val;
}


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
        
        private const string _strSrcSchemasList0 = @"BTNextGen.BizTalk.ERPOrders.Schemas.POAckBISAC";
        
        private const global::BTNextGen.BizTalk.ERPOrders.Schemas.POAckBISAC _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"BTNextGen.BizTalk.ERPOrders.Schemas.IntermediateXMLAck";
        
        private const global::BTNextGen.BizTalk.ERPOrders.Schemas.IntermediateXMLAck _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"BTNextGen.BizTalk.ERPOrders.Schemas.POAckBISAC";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"BTNextGen.BizTalk.ERPOrders.Schemas.IntermediateXMLAck";
                return _TrgSchemas;
            }
        }
    }
}
