namespace BTNextGen.BizTalk.PaymentProcessing {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"CallCyberSourceByParameters", @"CallCyberSourceByParametersResponse", @"CallCyberSource", @"CallCyberSourceResponse"})]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.BizTalk.PaymentProcessing.PaymentProcessing__x2eBizTalk_x2ePaymentProcessing_x2eERPTXNs_x2eERPccResponse", typeof(global::BTNextGen.BizTalk.PaymentProcessing.PaymentProcessing__x2eBizTalk_x2ePaymentProcessing_x2eERPTXNs_x2eERPccResponse))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.BizTalk.PaymentProcessing.PaymentProcessing_n_x2eBizTalk_x2ePaymentProcessing_x2eERPTXNs_x2eERPccRequest", typeof(global::BTNextGen.BizTalk.PaymentProcessing.PaymentProcessing_n_x2eBizTalk_x2ePaymentProcessing_x2eERPTXNs_x2eERPccRequest))]
    public sealed class PaymentProcessing_edi_btol_com : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" xmlns:tns=""http://edi.btol.com/"" elementFormDefault=""qualified"" targetNamespace=""http://edi.btol.com/"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:import schemaLocation=""BTNextGen.BizTalk.PaymentProcessing.PaymentProcessing__x2eBizTalk_x2ePaymentProcessing_x2eERPTXNs_x2eERPccResponse"" namespace=""BTNextGen.BizTalk.PaymentProcessing.ERPTXNs.ERPccResponse"" />
  <xs:import schemaLocation=""BTNextGen.BizTalk.PaymentProcessing.PaymentProcessing_n_x2eBizTalk_x2ePaymentProcessing_x2eERPTXNs_x2eERPccRequest"" namespace=""BTNextGen.BizTalk.PaymentProcessing.ERPTXNs.ERPccRequest"" />
  <xs:annotation>
    <xs:appinfo>
      <references xmlns=""http://schemas.microsoft.com/BizTalk/2003"">
        <reference targetNamespace=""BTNextGen.BizTalk.PaymentProcessing.ERPTXNs.ERPccResponse"" />
        <reference targetNamespace=""BTNextGen.BizTalk.PaymentProcessing.ERPTXNs.ERPccRequest"" />
      </references>
    </xs:appinfo>
  </xs:annotation>
  <xs:element name=""CallCyberSourceByParameters"">
    <xs:complexType>
      <xs:sequence>
        <xs:element minOccurs=""0"" maxOccurs=""1"" name=""AuthService"" type=""xs:string"" />
        <xs:element minOccurs=""0"" maxOccurs=""1"" name=""CaptureService"" type=""xs:string"" />
        <xs:element minOccurs=""0"" maxOccurs=""1"" name=""AuthReversalService"" type=""xs:string"" />
        <xs:element minOccurs=""0"" maxOccurs=""1"" name=""CreditService"" type=""xs:string"" />
        <xs:element minOccurs=""0"" maxOccurs=""1"" name=""purchasingLevel"" type=""xs:string"" />
        <xs:element minOccurs=""0"" maxOccurs=""1"" name=""merchantID"" type=""xs:string"" />
        <xs:element minOccurs=""0"" maxOccurs=""1"" name=""merchantReferenceCode"" type=""xs:string"" />
        <xs:element minOccurs=""0"" maxOccurs=""1"" name=""subscriptionID"" type=""xs:string"" />
        <xs:element minOccurs=""0"" maxOccurs=""1"" name=""requestID"" type=""xs:string"" />
        <xs:element minOccurs=""0"" maxOccurs=""1"" name=""requestToken"" type=""xs:string"" />
        <xs:element minOccurs=""0"" maxOccurs=""1"" name=""shipFromPostal"" type=""xs:string"" />
        <xs:element minOccurs=""0"" maxOccurs=""1"" name=""shipToCountry"" type=""xs:string"" />
        <xs:element minOccurs=""0"" maxOccurs=""1"" name=""shipToPostal"" type=""xs:string"" />
        <xs:element minOccurs=""0"" maxOccurs=""1"" name=""shipToState"" type=""xs:string"" />
        <xs:element minOccurs=""0"" maxOccurs=""1"" name=""item_0_unitprice"" type=""xs:string"" />
        <xs:element minOccurs=""0"" maxOccurs=""1"" name=""item_0_quantity"" type=""xs:string"" />
        <xs:element minOccurs=""0"" maxOccurs=""1"" name=""item_0_taxamount"" type=""xs:string"" />
        <xs:element minOccurs=""0"" maxOccurs=""1"" name=""item_0_taxrate"" type=""xs:string"" />
        <xs:element minOccurs=""0"" maxOccurs=""1"" name=""purchaseTotals_currency"" type=""xs:string"" />
        <xs:element minOccurs=""0"" maxOccurs=""1"" name=""invoiceHeader_userPO"" type=""xs:string"" />
        <xs:element minOccurs=""0"" maxOccurs=""1"" name=""invoiceHeader_amexDataTAA1"" type=""xs:string"" />
        <xs:element minOccurs=""0"" maxOccurs=""1"" name=""invoiceHeader_merchantDescriptor"" type=""xs:string"" />
        <xs:element minOccurs=""0"" maxOccurs=""1"" name=""invoiceHeader_merchantDescriptorContact"" type=""xs:string"" />
        <xs:element minOccurs=""0"" maxOccurs=""1"" name=""notificationEmail"" type=""xs:string"" />
        <xs:element minOccurs=""1"" maxOccurs=""1"" name=""SimpleAuth"" type=""xs:boolean"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
  <xs:element name=""CallCyberSourceByParametersResponse"">
    <xs:complexType>
      <xs:sequence>
        <xs:element xmlns:q1=""BTNextGen.BizTalk.PaymentProcessing.ERPTXNs.ERPccResponse"" minOccurs=""0"" maxOccurs=""1"" ref=""q1:CallCyberSourceByParametersResult"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
  <xs:element name=""CallCyberSource"">
    <xs:complexType>
      <xs:sequence>
        <xs:element xmlns:q2=""BTNextGen.BizTalk.PaymentProcessing.ERPTXNs.ERPccRequest"" minOccurs=""0"" maxOccurs=""1"" ref=""q2:ERPRequest"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
  <xs:element name=""CallCyberSourceResponse"">
    <xs:complexType>
      <xs:sequence>
        <xs:element xmlns:q3=""BTNextGen.BizTalk.PaymentProcessing.ERPTXNs.ERPccResponse"" minOccurs=""0"" maxOccurs=""1"" ref=""q3:CallCyberSourceResult"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>";
        
        public PaymentProcessing_edi_btol_com() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [4];
                _RootElements[0] = "CallCyberSourceByParameters";
                _RootElements[1] = "CallCyberSourceByParametersResponse";
                _RootElements[2] = "CallCyberSource";
                _RootElements[3] = "CallCyberSourceResponse";
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
        
        [Schema(@"http://edi.btol.com/",@"CallCyberSourceByParameters")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"CallCyberSourceByParameters"})]
        public sealed class CallCyberSourceByParameters : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public CallCyberSourceByParameters() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "CallCyberSourceByParameters";
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
        
        [Schema(@"http://edi.btol.com/",@"CallCyberSourceByParametersResponse")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"CallCyberSourceByParametersResponse"})]
        public sealed class CallCyberSourceByParametersResponse : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public CallCyberSourceByParametersResponse() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "CallCyberSourceByParametersResponse";
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
        
        [Schema(@"http://edi.btol.com/",@"CallCyberSource")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"CallCyberSource"})]
        public sealed class CallCyberSource : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public CallCyberSource() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "CallCyberSource";
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
        
        [Schema(@"http://edi.btol.com/",@"CallCyberSourceResponse")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"CallCyberSourceResponse"})]
        public sealed class CallCyberSourceResponse : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public CallCyberSourceResponse() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "CallCyberSourceResponse";
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
}
