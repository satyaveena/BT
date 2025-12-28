namespace BTNextGen.BizTalk.ERPOrders.Schemas {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"BTNextGen.BizTalk.ERPOrders.Schemas.IntermediateXMLAck",@"POAck")]
    [Microsoft.XLANGs.BaseTypes.DistinguishedFieldAttribute(typeof(System.String), "POAckHeader.TransactionID", XPath = @"/*[local-name()='POAck' and namespace-uri()='BTNextGen.BizTalk.ERPOrders.Schemas.IntermediateXMLAck']/*[local-name()='POAckHeader' and namespace-uri()='']/*[local-name()='TransactionID' and namespace-uri()='']", XsdType = @"string")]
    [Microsoft.XLANGs.BaseTypes.DistinguishedFieldAttribute(typeof(System.String), "POAckHeader.TransNoAndERP", XPath = @"/*[local-name()='POAck' and namespace-uri()='BTNextGen.BizTalk.ERPOrders.Schemas.IntermediateXMLAck']/*[local-name()='POAckHeader' and namespace-uri()='']/*[local-name()='TransNoAndERP' and namespace-uri()='']", XsdType = @"string")]
    [Microsoft.XLANGs.BaseTypes.DistinguishedFieldAttribute(typeof(System.Boolean), "POAckHeader.IsCPQAck", XPath = @"/*[local-name()='POAck' and namespace-uri()='BTNextGen.BizTalk.ERPOrders.Schemas.IntermediateXMLAck']/*[local-name()='POAckHeader' and namespace-uri()='']/*[local-name()='IsCPQAck' and namespace-uri()='']", XsdType = @"boolean")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"POAck"})]
    public sealed class IntermediateXMLAck : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns=""BTNextGen.BizTalk.ERPOrders.Schemas.IntermediateXMLAck"" xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" targetNamespace=""BTNextGen.BizTalk.ERPOrders.Schemas.IntermediateXMLAck"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:element name=""POAck"">
    <xs:annotation>
      <xs:appinfo>
        <b:properties>
          <b:property distinguished=""true"" xpath=""/*[local-name()='POAck' and namespace-uri()='BTNextGen.BizTalk.ERPOrders.Schemas.IntermediateXMLAck']/*[local-name()='POAckHeader' and namespace-uri()='']/*[local-name()='TransactionID' and namespace-uri()='']"" />
          <b:property distinguished=""true"" xpath=""/*[local-name()='POAck' and namespace-uri()='BTNextGen.BizTalk.ERPOrders.Schemas.IntermediateXMLAck']/*[local-name()='POAckHeader' and namespace-uri()='']/*[local-name()='TransNoAndERP' and namespace-uri()='']"" />
          <b:property distinguished=""true"" xpath=""/*[local-name()='POAck' and namespace-uri()='BTNextGen.BizTalk.ERPOrders.Schemas.IntermediateXMLAck']/*[local-name()='POAckHeader' and namespace-uri()='']/*[local-name()='IsCPQAck' and namespace-uri()='']"" />
        </b:properties>
      </xs:appinfo>
    </xs:annotation>
    <xs:complexType>
      <xs:sequence>
        <xs:element name=""POAckHeader"">
          <xs:complexType>
            <xs:sequence>
              <xs:element name=""TransactionID"" type=""xs:string"" />
              <xs:element name=""TargetERP"" type=""xs:string"" />
              <xs:element name=""SourceSan"" type=""xs:string"" />
              <xs:element name=""AccountID"" type=""xs:string"" />
              <xs:element name=""PONumber"" type=""xs:string"" />
              <xs:element name=""ERPStatusMessage"" type=""xs:string"" />
              <xs:element name=""AckDate"" type=""xs:string"" />
              <xs:element name=""ERPOrderNumber"" type=""xs:string"" />
              <xs:element name=""TransNoAndERP"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""BasketVAS"">
                <xs:complexType>
                  <xs:sequence>
                    <xs:element maxOccurs=""unbounded"" name=""VASRecord"">
                      <xs:complexType>
                        <xs:sequence>
                          <xs:element name=""TransmissionNumber"" type=""xs:string"" />
                          <xs:element name=""VASQuantity"" type=""xs:string"" />
                          <xs:element name=""VASUnitPrice"" type=""xs:string"" />
                          <xs:element name=""VASDescription"" type=""xs:string"" />
                          <xs:element name=""VASBTKey"" type=""xs:string"" />
                        </xs:sequence>
                      </xs:complexType>
                    </xs:element>
                  </xs:sequence>
                </xs:complexType>
              </xs:element>
              <xs:element name=""IsCPQAck"" type=""xs:boolean"" />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element maxOccurs=""unbounded"" name=""POAckDetails"">
          <xs:complexType>
            <xs:sequence>
              <xs:element minOccurs=""0"" name=""ISBN"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""BTKey"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""GTIN"" type=""xs:string"" />
              <xs:element name=""ReplacementBTKey"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""Description"" type=""xs:string"" />
              <xs:element name=""OrderQTY"" type=""xs:int"" />
              <xs:element name=""ShippableQTY"" type=""xs:int"" />
              <xs:element name=""CancelQTY"" type=""xs:int"" />
              <xs:element name=""BackOrderQTY"" type=""xs:int"" />
              <xs:element minOccurs=""0"" name=""StatusCode"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""StatusDescription"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""StatusWHS"" type=""xs:string"" />
              <xs:element name=""LineNumber"" type=""xs:string"" />
              <xs:element name=""OSN"" type=""xs:string"" />
              <xs:element name=""OLN"" type=""xs:string"" />
              <xs:element name=""LAN"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""VASAvailable"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""SurchargeItem"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""ExtendedWithVAS"" type=""xs:string"" />
              <xs:element name=""NetPrice"" type=""xs:string"" />
              <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""SplitShipWHS"">
                <xs:complexType>
                  <xs:sequence>
                    <xs:element minOccurs=""0"" default=""0"" name=""QTY"" type=""xs:string"" />
                    <xs:element minOccurs=""0"" default=""   "" name=""WHSCOD"" type=""xs:string"" />
                  </xs:sequence>
                </xs:complexType>
              </xs:element>
            </xs:sequence>
          </xs:complexType>
        </xs:element>
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>";
        
        public IntermediateXMLAck() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [1];
                _RootElements[0] = "POAck";
                return _RootElements;
            }
        }
        
        protected override object RawSchema {
            get {
                return _rawSchema;
            }
            set {
                _rawSchema = value;
            }
        }
    }
}
