namespace BTNextGen.BizTalk.ERPOrders.Maps {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.BizTalk.ERPOrders.Schemas.IntermediateXMLAck", typeof(global::BTNextGen.BizTalk.ERPOrders.Schemas.IntermediateXMLAck))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.BizTalk.ERPOrders.POAckLinesWHSProcedure_dbo+procOrderAcknowledgementSetBasketLineItems", typeof(global::BTNextGen.BizTalk.ERPOrders.POAckLinesWHSProcedure_dbo.procOrderAcknowledgementSetBasketLineItems))]
    public sealed class IXMLAckToNGAckLinesWHS : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0"" version=""1.0"" xmlns:ns0=""http://schemas.microsoft.com/Sql/2008/05/Procedures/dbo"" xmlns:ns4=""http://schemas.datacontract.org/2004/07/System.Data"" xmlns:s0=""BTNextGen.BizTalk.ERPOrders.Schemas.IntermediateXMLAck"" xmlns:ns3=""http://schemas.microsoft.com/Sql/2008/05/Types/TableTypes/dbo"">
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
            <ns3:BTShippedQuantity>
              <xsl:value-of select=""ShippableQTY/text()"" />
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
            <ns3:ETOrderSequenceNumber>
              <xsl:value-of select=""OSN/text()"" />
            </ns3:ETOrderSequenceNumber>
            <ns3:ETOrderLineNumber>
              <xsl:value-of select=""OLN/text()"" />
            </ns3:ETOrderLineNumber>
            <ns3:ETOrderLineActNumber>
              <xsl:value-of select=""LAN/text()"" />
            </ns3:ETOrderLineActNumber>
          </ns3:utblOrderAcknowledgementBasketLineItems>
        </xsl:for-each>
      </ns0:BasketLineItems>
      <ns0:BasketLineShippedItems>
        <xsl:for-each select=""POAckDetails"">
          <xsl:for-each select=""SplitShipWHS"">
            <ns3:utblOrderAcknowledgementBasketLineShippedQuantities>
              <ns3:TransmissionNumber>
                <xsl:value-of select=""../../POAckHeader/TransactionID/text()"" />
              </ns3:TransmissionNumber>
              <ns3:LineNumber>
                <xsl:value-of select=""../LineNumber/text()"" />
              </ns3:LineNumber>
              <xsl:if test=""WHSCOD"">
                <ns3:WarehouseCode>
                  <xsl:value-of select=""WHSCOD/text()"" />
                </ns3:WarehouseCode>
              </xsl:if>
              <xsl:if test=""QTY"">
                <ns3:Quantity>
                  <xsl:value-of select=""QTY/text()"" />
                </ns3:Quantity>
              </xsl:if>
            </ns3:utblOrderAcknowledgementBasketLineShippedQuantities>
          </xsl:for-each>
        </xsl:for-each>
      </ns0:BasketLineShippedItems>
    </ns0:procOrderAcknowledgementSetBasketLineItems>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"BTNextGen.BizTalk.ERPOrders.Schemas.IntermediateXMLAck";
        
        private const global::BTNextGen.BizTalk.ERPOrders.Schemas.IntermediateXMLAck _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"BTNextGen.BizTalk.ERPOrders.POAckLinesWHSProcedure_dbo+procOrderAcknowledgementSetBasketLineItems";
        
        private const global::BTNextGen.BizTalk.ERPOrders.POAckLinesWHSProcedure_dbo.procOrderAcknowledgementSetBasketLineItems _trgSchemaTypeReference0 = null;
        
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
                _TrgSchemas[0] = @"BTNextGen.BizTalk.ERPOrders.POAckLinesWHSProcedure_dbo+procOrderAcknowledgementSetBasketLineItems";
                return _TrgSchemas;
            }
        }
    }
}
