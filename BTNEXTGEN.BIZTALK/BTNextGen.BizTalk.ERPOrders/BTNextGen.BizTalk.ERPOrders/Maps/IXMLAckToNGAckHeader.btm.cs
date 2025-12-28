namespace BTNextGen.BizTalk.ERPOrders.Maps {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.BizTalk.ERPOrders.Schemas.IntermediateXMLAck", typeof(global::BTNextGen.BizTalk.ERPOrders.Schemas.IntermediateXMLAck))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.BizTalk.ERPOrders.Schemas.NGAckFormUpTypedProcedure_dbo+procOrderAcknowledgementSetBasketOrderForm", typeof(global::BTNextGen.BizTalk.ERPOrders.Schemas.NGAckFormUpTypedProcedure_dbo.procOrderAcknowledgementSetBasketOrderForm))]
    public sealed class IXMLAckToNGAckHeader : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0"" version=""1.0"" xmlns:s0=""BTNextGen.BizTalk.ERPOrders.Schemas.IntermediateXMLAck"" xmlns:ns0=""http://schemas.microsoft.com/Sql/2008/05/TypedProcedures/dbo"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s0:POAck"" />
  </xsl:template>
  <xsl:template match=""/s0:POAck"">
    <ns0:procOrderAcknowledgementSetBasketOrderForm>
      <ns0:TransmissionNumber>
        <xsl:value-of select=""POAckHeader/TransactionID/text()"" />
      </ns0:TransmissionNumber>
      <ns0:OrderNumber>
        <xsl:value-of select=""POAckHeader/ERPOrderNumber/text()"" />
      </ns0:OrderNumber>
      <ns0:ConfirmationMessage>
        <xsl:value-of select=""POAckHeader/ERPStatusMessage/text()"" />
      </ns0:ConfirmationMessage>
      <ns0:BTTargetERP>
        <xsl:value-of select=""POAckHeader/TargetERP/text()"" />
      </ns0:BTTargetERP>
    </ns0:procOrderAcknowledgementSetBasketOrderForm>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"BTNextGen.BizTalk.ERPOrders.Schemas.IntermediateXMLAck";
        
        private const global::BTNextGen.BizTalk.ERPOrders.Schemas.IntermediateXMLAck _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"BTNextGen.BizTalk.ERPOrders.Schemas.NGAckFormUpTypedProcedure_dbo+procOrderAcknowledgementSetBasketOrderForm";
        
        private const global::BTNextGen.BizTalk.ERPOrders.Schemas.NGAckFormUpTypedProcedure_dbo.procOrderAcknowledgementSetBasketOrderForm _trgSchemaTypeReference0 = null;
        
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
                _TrgSchemas[0] = @"BTNextGen.BizTalk.ERPOrders.Schemas.NGAckFormUpTypedProcedure_dbo+procOrderAcknowledgementSetBasketOrderForm";
                return _TrgSchemas;
            }
        }
    }
}
