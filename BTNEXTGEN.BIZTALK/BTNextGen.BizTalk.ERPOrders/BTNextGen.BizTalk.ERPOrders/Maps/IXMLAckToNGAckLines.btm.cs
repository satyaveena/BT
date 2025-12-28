namespace BTNextGen.BizTalk.ERPOrders.Maps {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.BizTalk.ERPOrders.Schemas.IntermediateXMLAck", typeof(global::BTNextGen.BizTalk.ERPOrders.Schemas.IntermediateXMLAck))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.BizTalk.ERPOrders.Schemas.NGAckLinesUpProc_dbo+procOrderAcknowledgementSetBasketLineItems", typeof(global::BTNextGen.BizTalk.ERPOrders.Schemas.NGAckLinesUpProc_dbo.procOrderAcknowledgementSetBasketLineItems))]
    public sealed class IXMLAckToNGAckLines : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0 userCSharp"" version=""1.0"" xmlns:s0=""BTNextGen.BizTalk.ERPOrders.Schemas.IntermediateXMLAck"" xmlns:ns0=""http://schemas.microsoft.com/Sql/2008/05/TypedProcedures/dbo"" xmlns:ns3=""http://schemas.microsoft.com/Sql/2008/05/Types/TableTypes/dbo"" xmlns:userCSharp=""http://schemas.microsoft.com/BizTalk/2003/userCSharp"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s0:POAck"" />
  </xsl:template>
  <xsl:template match=""/s0:POAck"">
    <ns0:procOrderAcknowledgementSetBasketLineItems>
      <ns0:BasketLineItems>
        <xsl:for-each select=""POAckDetails"">
          <ns3:utblOrderAcknowledgementBasketLineItems>
            <ns3:TransmissionNumber>
              <xsl:value-of select=""../POAckHeader/TransactionID/text()"" />
            </ns3:TransmissionNumber>
            <ns3:LineNumber>
              <xsl:value-of select=""LineNumber/text()"" />
            </ns3:LineNumber>
            <xsl:variable name=""var:v1"" select=""userCSharp:ChkShipQty(string(OrderQTY/text()) , string(ShippableQTY/text()) , string(CancelQTY/text()) , string(BackOrderQTY/text()))"" />
            <ns3:BTShippedQuantity>
              <xsl:value-of select=""$var:v1"" />
            </ns3:BTShippedQuantity>
            <ns3:OrderedQuantity>
              <xsl:value-of select=""OrderQTY/text()"" />
            </ns3:OrderedQuantity>
            <xsl:if test=""StatusDescription"">
              <ns3:Status>
                <xsl:value-of select=""StatusDescription/text()"" />
              </ns3:Status>
            </xsl:if>
            <xsl:if test=""GTIN"">
              <ns3:BTUPC>
                <xsl:value-of select=""GTIN/text()"" />
              </ns3:BTUPC>
            </xsl:if>
            <xsl:if test=""BTKey"">
              <ns3:BTKey>
                <xsl:value-of select=""BTKey/text()"" />
              </ns3:BTKey>
            </xsl:if>
            <xsl:if test=""ISBN"">
              <ns3:BTISBN>
                <xsl:value-of select=""ISBN/text()"" />
              </ns3:BTISBN>
            </xsl:if>
            <ns3:ReplacementBTKey>
              <xsl:value-of select=""ReplacementBTKey/text()"" />
            </ns3:ReplacementBTKey>
            <xsl:if test=""StatusWHS"">
              <ns3:BTWarehouse>
                <xsl:value-of select=""StatusWHS/text()"" />
              </ns3:BTWarehouse>
            </xsl:if>
            <ns3:BackorderQuantity>
              <xsl:value-of select=""BackOrderQTY/text()"" />
            </ns3:BackorderQuantity>
            <ns3:BTCancelledQuantity>
              <xsl:value-of select=""CancelQTY/text()"" />
            </ns3:BTCancelledQuantity>
          </ns3:utblOrderAcknowledgementBasketLineItems>
        </xsl:for-each>
      </ns0:BasketLineItems>
    </ns0:procOrderAcknowledgementSetBasketLineItems>
  </xsl:template>
  <msxsl:script language=""C#"" implements-prefix=""userCSharp""><![CDATA[
///*Uncomment the following code for a sample Inline C# function
//that concatenates two inputs. Change the number of parameters of
//this function to be equal to the number of inputs connected to this functoid.*/

public int ChkShipQty(int ordered, int shipping, int cancelled, int backordered)
{
   if(shipping == (ordered - (cancelled + backordered)))
      return shipping;
   else
      return (ordered - (cancelled + backordered));
}



]]></msxsl:script>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"BTNextGen.BizTalk.ERPOrders.Schemas.IntermediateXMLAck";
        
        private const global::BTNextGen.BizTalk.ERPOrders.Schemas.IntermediateXMLAck _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"BTNextGen.BizTalk.ERPOrders.Schemas.NGAckLinesUpProc_dbo+procOrderAcknowledgementSetBasketLineItems";
        
        private const global::BTNextGen.BizTalk.ERPOrders.Schemas.NGAckLinesUpProc_dbo.procOrderAcknowledgementSetBasketLineItems _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"BTNextGen.BizTalk.ERPOrders.Schemas.IntermediateXMLAck";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"BTNextGen.BizTalk.ERPOrders.Schemas.NGAckLinesUpProc_dbo+procOrderAcknowledgementSetBasketLineItems";
                return _TrgSchemas;
            }
        }
    }
}
