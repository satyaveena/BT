namespace BTNextGen.BizTalk.ERPOrders.Maps {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.BizTalk.ERPOrders.Schemas.POAck855", typeof(global::BTNextGen.BizTalk.ERPOrders.Schemas.POAck855))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.BizTalk.ERPOrders.Schemas.IntermediateXMLAck", typeof(global::BTNextGen.BizTalk.ERPOrders.Schemas.IntermediateXMLAck))]
    public sealed class X12855ToIXMLAck : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0 userCSharp"" version=""1.0"" xmlns:ns0=""BTNextGen.BizTalk.ERPOrders.Schemas.IntermediateXMLAck"" xmlns:s0=""BTNextGen.BizTalk.ERPOrders.Schemas.POAck855"" xmlns:userCSharp=""http://schemas.microsoft.com/BizTalk/2003/userCSharp"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s0:POAck"" />
  </xsl:template>
  <xsl:template match=""/s0:POAck"">
    <xsl:variable name=""var:v1"" select=""userCSharp:StringUpperCase(&quot;BTE&quot;)"" />
    <xsl:variable name=""var:v2"" select=""userCSharp:StringTrimRight(string(Header/ISA/ISA08Sender/text()))"" />
    <xsl:variable name=""var:v4"" select=""userCSharp:StringConcat(string(Header/BAK/BAK08TransmissionID/text()) , &quot;BTE&quot;)"" />
    <xsl:variable name=""var:v5"" select=""userCSharp:StringConcat(&quot;0&quot;)"" />
    <xsl:variable name=""var:v6"" select=""userCSharp:StringConcat(&quot; &quot;)"" />
    <xsl:variable name=""var:v7"" select=""userCSharp:StringConcat(&quot;false&quot;)"" />
    <ns0:POAck>
      <POAckHeader>
        <TransactionID>
          <xsl:value-of select=""Header/BAK/BAK08TransmissionID/text()"" />
        </TransactionID>
        <TargetERP>
          <xsl:value-of select=""$var:v1"" />
        </TargetERP>
        <SourceSan>
          <xsl:value-of select=""$var:v2"" />
        </SourceSan>
        <AccountID>
          <xsl:value-of select=""Header/GS/GS02EDIAccount/text()"" />
        </AccountID>
        <PONumber>
          <xsl:value-of select=""Header/BAK/BAK03PONumber/text()"" />
        </PONumber>
        <xsl:variable name=""var:v3"" select=""userCSharp:StatusXlate(string(Header/BAK/BAK02StatusCode/text()))"" />
        <ERPStatusMessage>
          <xsl:value-of select=""$var:v3"" />
        </ERPStatusMessage>
        <AckDate>
          <xsl:value-of select=""Header/GS/GS04Date/text()"" />
        </AckDate>
        <ERPOrderNumber>
          <xsl:value-of select=""Header/BAK/BAK08TransmissionID/text()"" />
        </ERPOrderNumber>
        <TransNoAndERP>
          <xsl:value-of select=""$var:v4"" />
        </TransNoAndERP>
        <BasketVAS>
          <VASRecord>
            <VASQuantity>
              <xsl:value-of select=""$var:v5"" />
            </VASQuantity>
            <VASUnitPrice>
              <xsl:value-of select=""$var:v5"" />
            </VASUnitPrice>
            <VASDescription>
              <xsl:value-of select=""$var:v6"" />
            </VASDescription>
            <VASBTKey>
              <xsl:value-of select=""$var:v6"" />
            </VASBTKey>
          </VASRecord>
        </BasketVAS>
        <IsCPQAck>
          <xsl:value-of select=""$var:v7"" />
        </IsCPQAck>
      </POAckHeader>
      <xsl:for-each select=""LineItem"">
        <xsl:variable name=""var:v9"" select=""string(PO1/PO107SKUId/text())"" />
        <xsl:variable name=""var:v13"" select=""string(PO1/PO115OSN-OLN-LAN/text())"" />
        <xsl:variable name=""var:v16"" select=""userCSharp:StringConcat(&quot;0&quot;)"" />
        <POAckDetails>
          <xsl:variable name=""var:v8"" select=""userCSharp:AnalyzeSKUI(string(PO1/PO107SKUId/text()))"" />
          <ISBN>
            <xsl:value-of select=""$var:v8"" />
          </ISBN>
          <xsl:variable name=""var:v10"" select=""userCSharp:AnalyzeSKUU($var:v9)"" />
          <GTIN>
            <xsl:value-of select=""$var:v10"" />
          </GTIN>
          <xsl:variable name=""var:v11"" select=""userCSharp:StrToInt(string(PO1/PO102OrderedQTY/text()))"" />
          <OrderQTY>
            <xsl:value-of select=""$var:v11"" />
          </OrderQTY>
          <xsl:for-each select=""Status/ACK"">

<xsl:variable name=""var:setBuckets"" select=""userCSharp:SetQty(string(ACK01StatusCode/text()) , string(ACK02StatusQTY/text()))"" />

</xsl:for-each>
          <ShippableQTY>
  <xsl:value-of select=""userCSharp:GetShip()"" />
</ShippableQTY>
          <CancelQTY>
  <xsl:value-of select=""userCSharp:GetCancel()"" />
</CancelQTY>
          <BackOrderQTY>
  <xsl:value-of select=""userCSharp:GetBackorder()"" />
</BackOrderQTY>
          <xsl:variable name=""var:vzeroShip"" select=""userCSharp:ZeroQty()"" />
          <StatusDescription>
  <xsl:value-of select=""userCSharp:GetStatus()"" />
</StatusDescription>
          <LineNumber>
            <xsl:value-of select=""PO1/PO101MerchLineNumer/text()"" />
          </LineNumber>
          <xsl:variable name=""var:v12"" select=""userCSharp:ParseOSN(string(PO1/PO115OSN-OLN-LAN/text()))"" />
          <OSN>
            <xsl:value-of select=""$var:v12"" />
          </OSN>
          <xsl:variable name=""var:v14"" select=""userCSharp:ParseOLN($var:v13)"" />
          <OLN>
            <xsl:value-of select=""$var:v14"" />
          </OLN>
          <xsl:variable name=""var:v15"" select=""userCSharp:ParseLAN($var:v13)"" />
          <LAN>
            <xsl:value-of select=""$var:v15"" />
          </LAN>
          <NetPrice>
            <xsl:value-of select=""$var:v16"" />
          </NetPrice>
          <SplitShipWHS>
            <QTY>
              <xsl:text>0</xsl:text>
            </QTY>
            <WHSCOD>
              <xsl:text>   </xsl:text>
            </WHSCOD>
          </SplitShipWHS>
        </POAckDetails>
      </xsl:for-each>
    </ns0:POAck>
  </xsl:template>
  <msxsl:script language=""C#"" implements-prefix=""userCSharp""><![CDATA[

public void SetQty(string code,int qty)
{
     switch (code)
      {
         case ""IA"": shipqty = shipqty + qty; status=""Shippable As Ordered.""; break;
         case ""IQ"":shipqty = shipqty + qty; status=""Partially Shipped - ""; break;
         case ""BO"": bckorderqty = bckorderqty + qty; status=""Item Backordered.""; break;
         case ""IB"": bckorderqty = bckorderqty + qty; status=""Item Backordered.""; break;
         case ""IC"": cancelqty = cancelqty + qty; status=""Item Cancelled.""; break;
         case ""R1"": cancelqty = cancelqty + qty; status=""Item Rejected, Not Found.""; break;
         case ""R2"": cancelqty = cancelqty + qty; status=""Item Rejected, Invalid Product Number""; break;
         case ""R4"": cancelqty = cancelqty + qty; status=""Item Rejected, Item not Available""; break;
         case ""ID"": cancelqty = cancelqty + qty; status=""Item Deleted.""; break;
         case ""IR"": cancelqty = cancelqty + qty; status=""Item Rejected - ""; break;
         case ""R5"": cancelqty = cancelqty + qty; status=""Item Rejected - ""; break;
         case ""IH"": cancelqty = cancelqty + qty; status=""Item on Hold.""; break;
         default: status=""Unknown Status Code.""; break; 
      }
}


public int GetShip()
{
return shipqty;
}

public int GetBackorder()
{
return bckorderqty;
}

public int GetCancel()
{
return cancelqty;
}

public string GetStatus()
{
return status;
}

public void ZeroQty()
{
shipqty=0;
cancelqty=0;
bckorderqty=0;

}



public int shipqty=0;
public int cancelqty=0;
public int bckorderqty=0;
public string status=""OK"";


///*Uncomment the following code for a sample Inline C# function
//that concatenates two inputs. Change the number of parameters of
//this function to be equal to the number of inputs connected to this functoid.*/

public int StrToInt(int ordqty)
{
   return ordqty;
}


///*Uncomment the following code for a sample Inline C# function
//that concatenates two inputs. Change the number of parameters of
//this function to be equal to the number of inputs connected to this functoid.*/

public string StatusXlate(string statcode)
{
   switch (statcode)
      {
         case ""AD"": return ""Order Accepted With Details""; break;
         case ""AK"": return ""Order Killed All Cancelled."";break;
         case ""RA"": return ""Order Cancelled Invalid Account."";break;
         case ""PS"": return ""Order Accepted Partially Shipping."";break;
         case ""AB"": return ""Order Accepted All Backordered."";break;
         case ""AC"": return ""Order Accepted With Changes."";break;
         default: return ""Unknown Status Code "" + statcode; break;
      }
}


public string StringTrimRight(string str)
{
	if (str == null)
	{
		return """";
	}
	return str.TrimEnd(null);
}


///*Uncomment the following code for a sample Inline C# function
//that concatenates two inputs. Change the number of parameters of
//this function to be equal to the number of inputs connected to this functoid.*/

public string AnalyzeSKUI(string inSKUID)
{ 
   if (inSKUID.Substring(0,3) == ""978"")
      return inSKUID;
   else if (inSKUID.Substring(0,3) == ""979"")
      return inSKUID;
   else
      return """";
}


///*Uncomment the following code for a sample Inline C# function
//that concatenates two inputs. Change the number of parameters of
//this function to be equal to the number of inputs connected to this functoid.*/

public string AnalyzeSKUU(string inSKUID)
{ 
   if (inSKUID.Substring(0,3) == ""978"")
      return """";
    else if (inSKUID.Substring(0,3) == ""979"")
      return """";
    else
      return inSKUID;
}


public string StringUpperCase(string str)
{
	if (str == null)
	{
		return """";
	}
	return str.ToUpper(System.Globalization.CultureInfo.InvariantCulture);
}


public string StringConcat(string param0, string param1)
{
   return param0 + param1;
}


///*Uncomment the following code for a sample Inline C# function
//that concatenates two inputs. Change the number of parameters of
//this function to be equal to the number of inputs connected to this functoid.*/

public string ParseOSN(string OSNOLNLAN)
{
   if(OSNOLNLAN.Length != 18) return """";
   return OSNOLNLAN.Substring(0,10);
}


///*Uncomment the following code for a sample Inline C# function
//that concatenates two inputs. Change the number of parameters of
//this function to be equal to the number of inputs connected to this functoid.*/

public string ParseOLN(string OSNOLNLAN)
{
   if(OSNOLNLAN.Length != 18) return """";
   return OSNOLNLAN.Substring(10,5);
}


///*Uncomment the following code for a sample Inline C# function
//that concatenates two inputs. Change the number of parameters of
//this function to be equal to the number of inputs connected to this functoid.*/

public string ParseLAN(string OSNOLNLAN)
{
   if(OSNOLNLAN.Length != 18) return """";
   return OSNOLNLAN.Substring(15,3);
}


public string StringConcat(string param0)
{
   return param0;
}



]]></msxsl:script>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"BTNextGen.BizTalk.ERPOrders.Schemas.POAck855";
        
        private const global::BTNextGen.BizTalk.ERPOrders.Schemas.POAck855 _srcSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"BTNextGen.BizTalk.ERPOrders.Schemas.POAck855";
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
