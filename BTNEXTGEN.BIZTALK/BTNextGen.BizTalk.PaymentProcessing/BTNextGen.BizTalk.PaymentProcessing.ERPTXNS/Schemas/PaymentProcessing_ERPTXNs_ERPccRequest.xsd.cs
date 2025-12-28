namespace BTNextGen.BizTalk.PaymentProcessing {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"BTNextGen.BizTalk.PaymentProcessing.ERPTXNs.ERPccRequest",@"ERPRequest")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"ERPRequest"})]
    public sealed class PaymentProcessing_n_x2eBizTalk_x2ePaymentProcessing_x2eERPTXNs_x2eERPccRequest : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns:tns=""BTNextGen.BizTalk.PaymentProcessing.ERPTXNs.ERPccRequest"" xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" elementFormDefault=""qualified"" targetNamespace=""BTNextGen.BizTalk.PaymentProcessing.ERPTXNs.ERPccRequest"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:element name=""ERPRequest"">
    <xs:complexType>
      <xs:sequence>
        <xs:element minOccurs=""0"" maxOccurs=""1"" form=""unqualified"" name=""ccAuthService"">
          <xs:complexType>
            <xs:sequence>
              <xs:element minOccurs=""0"" maxOccurs=""1"" form=""unqualified"" name=""ccAuthService_Value"" type=""xs:string"" />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element minOccurs=""0"" maxOccurs=""1"" form=""unqualified"" name=""ccCaptureService"">
          <xs:complexType>
            <xs:sequence>
              <xs:element minOccurs=""0"" maxOccurs=""1"" form=""unqualified"" name=""ccCaptureService_Value"" type=""xs:string"" />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element minOccurs=""0"" maxOccurs=""1"" form=""unqualified"" name=""ccAuthReversalService"">
          <xs:complexType>
            <xs:sequence>
              <xs:element minOccurs=""0"" maxOccurs=""1"" form=""unqualified"" name=""ccAuthReversalService_Value"" type=""xs:string"" />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element minOccurs=""0"" maxOccurs=""1"" form=""unqualified"" name=""ccCreditService"">
          <xs:complexType>
            <xs:sequence>
              <xs:element minOccurs=""0"" maxOccurs=""1"" form=""unqualified"" name=""ccCreditService_Value"" type=""xs:string"" />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element minOccurs=""0"" maxOccurs=""1"" form=""unqualified"" name=""merchantID"">
          <xs:complexType>
            <xs:sequence>
              <xs:element minOccurs=""0"" maxOccurs=""1"" form=""unqualified"" name=""merchantID_Value"" type=""xs:string"" />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element minOccurs=""0"" maxOccurs=""1"" form=""unqualified"" name=""merchantReferenceCode"">
          <xs:complexType>
            <xs:sequence>
              <xs:element minOccurs=""0"" maxOccurs=""1"" form=""unqualified"" name=""merchantReferenceCode_Value"" type=""xs:string"" />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element minOccurs=""0"" maxOccurs=""1"" form=""unqualified"" name=""subscriptionID"">
          <xs:complexType>
            <xs:sequence>
              <xs:element minOccurs=""0"" maxOccurs=""1"" form=""unqualified"" name=""subscriptionID_Value"" type=""xs:string"" />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element minOccurs=""0"" maxOccurs=""1"" form=""unqualified"" name=""requestID"">
          <xs:complexType>
            <xs:sequence>
              <xs:element minOccurs=""0"" maxOccurs=""1"" form=""unqualified"" name=""requestID_Value"" type=""xs:string"" />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element minOccurs=""0"" maxOccurs=""1"" form=""unqualified"" name=""requestToken"">
          <xs:complexType>
            <xs:sequence>
              <xs:element minOccurs=""0"" maxOccurs=""1"" form=""unqualified"" name=""requestToken_Value"" type=""xs:string"" />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element minOccurs=""0"" maxOccurs=""1"" form=""unqualified"" name=""item_0_unitprice"">
          <xs:complexType>
            <xs:sequence>
              <xs:element minOccurs=""0"" maxOccurs=""1"" form=""unqualified"" name=""item_0_unitprice_Value"" type=""xs:string"" />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element minOccurs=""0"" maxOccurs=""1"" form=""unqualified"" name=""item_0_quantity"">
          <xs:complexType>
            <xs:sequence>
              <xs:element minOccurs=""0"" maxOccurs=""1"" form=""unqualified"" name=""item_0_quantity_Value"" type=""xs:string"" />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element minOccurs=""0"" maxOccurs=""1"" form=""unqualified"" name=""item_0_taxamount"">
          <xs:complexType>
            <xs:sequence>
              <xs:element minOccurs=""0"" maxOccurs=""1"" form=""unqualified"" name=""item_0_taxamount_Value"" type=""xs:string"" />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element minOccurs=""0"" maxOccurs=""1"" form=""unqualified"" name=""purchaseTotals_currency"">
          <xs:complexType>
            <xs:sequence>
              <xs:element minOccurs=""0"" maxOccurs=""1"" form=""unqualified"" name=""purchaseTotals_currency_Value"" type=""xs:string"" />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element minOccurs=""0"" maxOccurs=""1"" form=""unqualified"" name=""invoiceHeader_userPO"">
          <xs:complexType>
            <xs:sequence>
              <xs:element minOccurs=""0"" maxOccurs=""1"" form=""unqualified"" name=""invoiceHeader_userPO_Value"" type=""xs:string"" />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element minOccurs=""0"" maxOccurs=""1"" form=""unqualified"" name=""invoiceHeader_amexDataTAA1"">
          <xs:complexType>
            <xs:sequence>
              <xs:element minOccurs=""0"" maxOccurs=""1"" form=""unqualified"" name=""invoiceHeader_amexDataTAA1_Value"" type=""xs:string"" />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element minOccurs=""0"" maxOccurs=""1"" form=""unqualified"" name=""invoiceHeader_merchantDescriptor"">
          <xs:complexType>
            <xs:sequence>
              <xs:element minOccurs=""0"" maxOccurs=""1"" form=""unqualified"" name=""invoiceHeader_merchantDescriptor_Value"" type=""xs:string"" />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element minOccurs=""0"" maxOccurs=""1"" form=""unqualified"" name=""invoiceHeader_merchantDescriptorContact"">
          <xs:complexType>
            <xs:sequence>
              <xs:element minOccurs=""0"" maxOccurs=""1"" form=""unqualified"" name=""invoiceHeader_merchantDescriptorContact_Value"" type=""xs:string"" />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element minOccurs=""0"" maxOccurs=""1"" form=""unqualified"" name=""notificationEmail"">
          <xs:complexType>
            <xs:sequence>
              <xs:element minOccurs=""0"" maxOccurs=""1"" form=""unqualified"" name=""notificationEmail_Value"" type=""xs:string"" />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>";
        
        public PaymentProcessing_n_x2eBizTalk_x2ePaymentProcessing_x2eERPTXNs_x2eERPccRequest() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [1];
                _RootElements[0] = "ERPRequest";
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
