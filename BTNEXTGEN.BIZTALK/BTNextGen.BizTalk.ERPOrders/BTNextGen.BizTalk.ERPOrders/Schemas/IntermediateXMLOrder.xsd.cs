namespace BTNextGen.BizTalk.ERPOrders.Schemas {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"http://BTNextGen.BizTalk.ERPOrders.Schemas.IntermediateXMLOrder",@"PO")]
    [Microsoft.XLANGs.BaseTypes.DistinguishedFieldAttribute(typeof(System.String), "Header.Order.TargetERP", XPath = @"/*[local-name()='PO' and namespace-uri()='http://BTNextGen.BizTalk.ERPOrders.Schemas.IntermediateXMLOrder']/*[local-name()='Header' and namespace-uri()='']/*[local-name()='Order' and namespace-uri()='']/*[local-name()='TargetERP' and namespace-uri()='']", XsdType = @"string")]
    [Microsoft.XLANGs.BaseTypes.DistinguishedFieldAttribute(typeof(System.String), "Header.Order.TransmissionNumber", XPath = @"/*[local-name()='PO' and namespace-uri()='http://BTNextGen.BizTalk.ERPOrders.Schemas.IntermediateXMLOrder']/*[local-name()='Header' and namespace-uri()='']/*[local-name()='Order' and namespace-uri()='']/*[local-name()='TransmissionNumber' and namespace-uri()='']", XsdType = @"string")]
    [Microsoft.XLANGs.BaseTypes.DistinguishedFieldAttribute(typeof(System.String), "Header.Order.ISHomeDelivery", XPath = @"/*[local-name()='PO' and namespace-uri()='http://BTNextGen.BizTalk.ERPOrders.Schemas.IntermediateXMLOrder']/*[local-name()='Header' and namespace-uri()='']/*[local-name()='Order' and namespace-uri()='']/*[local-name()='ISHomeDelivery' and namespace-uri()='']", XsdType = @"string")]
    [Microsoft.XLANGs.BaseTypes.DistinguishedFieldAttribute(typeof(System.String), "Header.Order.TransNoAndERP", XPath = @"/*[local-name()='PO' and namespace-uri()='http://BTNextGen.BizTalk.ERPOrders.Schemas.IntermediateXMLOrder']/*[local-name()='Header' and namespace-uri()='']/*[local-name()='Order' and namespace-uri()='']/*[local-name()='TransNoAndERP' and namespace-uri()='']", XsdType = @"string")]
    [Microsoft.XLANGs.BaseTypes.DistinguishedFieldAttribute(typeof(System.String), "Header.Order.IsGridCart", XPath = @"/*[local-name()='PO' and namespace-uri()='http://BTNextGen.BizTalk.ERPOrders.Schemas.IntermediateXMLOrder']/*[local-name()='Header' and namespace-uri()='']/*[local-name()='Order' and namespace-uri()='']/*[local-name()='IsGridCart' and namespace-uri()='']", XsdType = @"string")]
    [Microsoft.XLANGs.BaseTypes.DistinguishedFieldAttribute(typeof(System.Boolean), "Header.Order.IsCPQRequest", XPath = @"/*[local-name()='PO' and namespace-uri()='http://BTNextGen.BizTalk.ERPOrders.Schemas.IntermediateXMLOrder']/*[local-name()='Header' and namespace-uri()='']/*[local-name()='Order' and namespace-uri()='']/*[local-name()='IsCPQRequest' and namespace-uri()='']", XsdType = @"boolean")]
    [Microsoft.XLANGs.BaseTypes.DistinguishedFieldAttribute(typeof(System.Boolean), "Header.Order.IsCPQFirmOrder", XPath = @"/*[local-name()='PO' and namespace-uri()='http://BTNextGen.BizTalk.ERPOrders.Schemas.IntermediateXMLOrder']/*[local-name()='Header' and namespace-uri()='']/*[local-name()='Order' and namespace-uri()='']/*[local-name()='IsCPQFirmOrder' and namespace-uri()='']", XsdType = @"boolean")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"PO"})]
    public sealed class IntermediateXMLOrder : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns=""http://BTNextGen.BizTalk.ERPOrders.Schemas.IntermediateXMLOrder"" xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" targetNamespace=""http://BTNextGen.BizTalk.ERPOrders.Schemas.IntermediateXMLOrder"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:element name=""PO"">
    <xs:annotation>
      <xs:appinfo>
        <b:properties>
          <b:property distinguished=""true"" xpath=""/*[local-name()='PO' and namespace-uri()='http://BTNextGen.BizTalk.ERPOrders.Schemas.IntermediateXMLOrder']/*[local-name()='Header' and namespace-uri()='']/*[local-name()='Order' and namespace-uri()='']/*[local-name()='TargetERP' and namespace-uri()='']"" />
          <b:property distinguished=""true"" xpath=""/*[local-name()='PO' and namespace-uri()='http://BTNextGen.BizTalk.ERPOrders.Schemas.IntermediateXMLOrder']/*[local-name()='Header' and namespace-uri()='']/*[local-name()='Order' and namespace-uri()='']/*[local-name()='TransmissionNumber' and namespace-uri()='']"" />
          <b:property distinguished=""true"" xpath=""/*[local-name()='PO' and namespace-uri()='http://BTNextGen.BizTalk.ERPOrders.Schemas.IntermediateXMLOrder']/*[local-name()='Header' and namespace-uri()='']/*[local-name()='Order' and namespace-uri()='']/*[local-name()='ISHomeDelivery' and namespace-uri()='']"" />
          <b:property distinguished=""true"" xpath=""/*[local-name()='PO' and namespace-uri()='http://BTNextGen.BizTalk.ERPOrders.Schemas.IntermediateXMLOrder']/*[local-name()='Header' and namespace-uri()='']/*[local-name()='Order' and namespace-uri()='']/*[local-name()='TransNoAndERP' and namespace-uri()='']"" />
          <b:property distinguished=""true"" xpath=""/*[local-name()='PO' and namespace-uri()='http://BTNextGen.BizTalk.ERPOrders.Schemas.IntermediateXMLOrder']/*[local-name()='Header' and namespace-uri()='']/*[local-name()='Order' and namespace-uri()='']/*[local-name()='IsGridCart' and namespace-uri()='']"" />
          <b:property distinguished=""true"" xpath=""/*[local-name()='PO' and namespace-uri()='http://BTNextGen.BizTalk.ERPOrders.Schemas.IntermediateXMLOrder']/*[local-name()='Header' and namespace-uri()='']/*[local-name()='Order' and namespace-uri()='']/*[local-name()='IsCPQRequest' and namespace-uri()='']"" />
          <b:property distinguished=""true"" xpath=""/*[local-name()='PO' and namespace-uri()='http://BTNextGen.BizTalk.ERPOrders.Schemas.IntermediateXMLOrder']/*[local-name()='Header' and namespace-uri()='']/*[local-name()='Order' and namespace-uri()='']/*[local-name()='IsCPQFirmOrder' and namespace-uri()='']"" />
        </b:properties>
      </xs:appinfo>
    </xs:annotation>
    <xs:complexType>
      <xs:sequence>
        <xs:element name=""Header"">
          <xs:complexType>
            <xs:sequence>
              <xs:element name=""Customer"">
                <xs:complexType>
                  <xs:sequence>
                    <xs:element name=""AccountNumber"" type=""xs:string"" />
                    <xs:element minOccurs=""0"" name=""ShipComplete"" type=""xs:string"" />
                    <xs:element minOccurs=""0"" name=""ShipMethod"" type=""xs:string"" />
                  </xs:sequence>
                  <xs:attribute name=""BO"" type=""xs:string"" />
                </xs:complexType>
              </xs:element>
              <xs:element name=""Order"">
                <xs:complexType>
                  <xs:sequence>
                    <xs:element name=""TransmissionNumber"" type=""xs:string"" />
                    <xs:element default=""Not Applicable"" name=""OrderGroupID"" type=""xs:string"" />
                    <xs:element name=""TargetERP"" type=""xs:string"" />
                    <xs:element name=""PONumber"" type=""xs:string"" />
                    <xs:element name=""ISHomeDelivery"" type=""xs:string"" />
                    <xs:element name=""TransNoAndERP"" type=""xs:string"" />
                    <xs:element minOccurs=""0"" name=""SpecialInstructions"" type=""xs:string"" />
                    <xs:element name=""IsGridCart"" type=""xs:string"" />
                    <xs:element default=""false"" name=""IsCPQRequest"" type=""xs:boolean"" />
                    <xs:element default=""false"" name=""IsCPQFirmOrder"" type=""xs:boolean"" />
                    <xs:element name=""QuotedDate"" type=""xs:string"" />
                  </xs:sequence>
                </xs:complexType>
              </xs:element>
              <xs:element name=""Address"">
                <xs:complexType>
                  <xs:sequence>
                    <xs:element name=""Address1"" type=""xs:string"" />
                    <xs:element name=""Address3"" type=""xs:string"" />
                    <xs:element name=""Address4"" type=""xs:string"" />
                    <xs:element name=""Name"" type=""xs:string"" />
                    <xs:element name=""City"" type=""xs:string"" />
                    <xs:element name=""State"" type=""xs:string"" />
                    <xs:element name=""Country"" type=""xs:string"" />
                    <xs:element name=""Zip"" type=""xs:string"" />
                    <xs:element minOccurs=""0"" name=""Phone"" type=""xs:string"" />
                  </xs:sequence>
                  <xs:attribute name=""AddressChangeFlag"" type=""xs:string"" />
                  <xs:attribute name=""Address2"" type=""xs:string"" />
                </xs:complexType>
              </xs:element>
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element name=""Detail"">
          <xs:complexType>
            <xs:sequence>
              <xs:element minOccurs=""1"" maxOccurs=""unbounded"" name=""LineItem"">
                <xs:complexType mixed=""true"">
                  <xs:sequence>
                    <xs:element name=""Quantity"" type=""xs:string"" />
                    <xs:element name=""Promocode"" type=""xs:string"" />
                    <xs:element name=""OverridePrice"" type=""xs:string"" />
                    <xs:element name=""ListPrice"" type=""xs:string"" />
                    <xs:element name=""OverrideDiscount"" type=""xs:string"" />
                    <xs:element name=""LineNumber"" type=""xs:string"" />
                    <xs:element name=""ISBN"" type=""xs:string"" />
                    <xs:element name=""UPC"" type=""xs:string"" />
                    <xs:element minOccurs=""0"" name=""BibNumber"" type=""xs:string"" />
                    <xs:element minOccurs=""0"" name=""POPerLine"" type=""xs:string"" />
                    <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""GridRecords"">
                      <xs:complexType>
                        <xs:sequence>
                          <xs:element minOccurs=""0"" name=""BranchMemberCode"" type=""xs:string"" />
                          <xs:element minOccurs=""0"" name=""CollectionCode"" type=""xs:string"" />
                          <xs:element minOccurs=""0"" name=""BranchQuantity"" type=""xs:string"" />
                          <xs:element minOccurs=""0"" name=""ItemTypeCode"" type=""xs:string"" />
                          <xs:element minOccurs=""0"" name=""CallNumber"" type=""xs:string"" />
                        </xs:sequence>
                      </xs:complexType>
                    </xs:element>
                  </xs:sequence>
                  <xs:attribute name=""BTKey"" type=""xs:string"" />
                  <xs:attribute name=""GTIN"" type=""xs:string"" />
                  <xs:attribute name=""BO"" type=""xs:string"" />
                </xs:complexType>
              </xs:element>
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element name=""Footer"">
          <xs:complexType>
            <xs:sequence>
              <xs:element minOccurs=""0"" name=""GiftWrapCode"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""GiftWrapMsg"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""GiftWrapMsg2"" type=""xs:string"" />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>";
        
        public IntermediateXMLOrder() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [1];
                _RootElements[0] = "PO";
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
