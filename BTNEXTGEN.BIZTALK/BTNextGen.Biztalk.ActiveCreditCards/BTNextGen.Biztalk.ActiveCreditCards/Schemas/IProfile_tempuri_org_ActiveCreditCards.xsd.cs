namespace BTNextGen.Biztalk.ActiveCreditCards.Schemas {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"GetCreditCardDetails", @"GetCreditCardDetailsResponse"})]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.Biztalk.ActiveCreditCards.Schemas.IProfile_schemas_datacontract_org_2004_07_IProfiles", typeof(global::BTNextGen.Biztalk.ActiveCreditCards.Schemas.IProfile_schemas_datacontract_org_2004_07_IProfiles))]
    public sealed class IProfile_tempuri_org_ActiveCreditCards : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" xmlns:tns=""http://tempuri.org/"" elementFormDefault=""qualified"" targetNamespace=""http://tempuri.org/"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:import schemaLocation=""BTNextGen.Biztalk.ActiveCreditCards.Schemas.IProfile_schemas_datacontract_org_2004_07_IProfiles"" namespace=""http://schemas.datacontract.org/2004/07/IProfiles"" />
  <xs:annotation>
    <xs:appinfo>
      <b:references>
        <b:reference targetNamespace=""http://schemas.datacontract.org/2004/07/IProfiles"" />
      </b:references>
    </xs:appinfo>
  </xs:annotation>
  <xs:element name=""GetCreditCardDetails"">
    <xs:complexType>
      <xs:sequence>
        <xs:element minOccurs=""0"" name=""NoOfCards"" type=""xs:int"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
  <xs:element name=""GetCreditCardDetailsResponse"">
    <xs:complexType>
      <xs:sequence>
        <xs:element xmlns:q1=""http://schemas.datacontract.org/2004/07/IProfiles"" minOccurs=""0"" name=""GetCreditCardDetailsResult"" nillable=""true"" type=""q1:ArrayOfActiveCreditCards"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>";
        
        public IProfile_tempuri_org_ActiveCreditCards() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [2];
                _RootElements[0] = "GetCreditCardDetails";
                _RootElements[1] = "GetCreditCardDetailsResponse";
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
        
        [Schema(@"http://tempuri.org/",@"GetCreditCardDetails")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"GetCreditCardDetails"})]
        public sealed class GetCreditCardDetails : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public GetCreditCardDetails() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "GetCreditCardDetails";
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
        
        [Schema(@"http://tempuri.org/",@"GetCreditCardDetailsResponse")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"GetCreditCardDetailsResponse"})]
        public sealed class GetCreditCardDetailsResponse : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public GetCreditCardDetailsResponse() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "GetCreditCardDetailsResponse";
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
