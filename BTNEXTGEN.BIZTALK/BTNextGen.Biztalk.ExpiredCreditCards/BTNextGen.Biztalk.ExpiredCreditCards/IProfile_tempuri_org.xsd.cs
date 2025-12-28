namespace BTNextGen.Biztalk.ExpiredCreditCards {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"GetExpiredCards", @"GetExpiredCardsResponse"})]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.Biztalk.ExpiredCreditCards.IProfile_schemas_datacontract_org_2004_07_IProfiles", typeof(global::BTNextGen.Biztalk.ExpiredCreditCards.IProfile_schemas_datacontract_org_2004_07_IProfiles))]
    public sealed class IProfile_tempuri_org : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" xmlns:tns=""http://tempuri.org/"" elementFormDefault=""qualified"" targetNamespace=""http://tempuri.org/"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:import schemaLocation=""BTNextGen.Biztalk.ExpiredCreditCards.IProfile_schemas_datacontract_org_2004_07_IProfiles"" namespace=""http://schemas.datacontract.org/2004/07/IProfiles"" />
  <xs:annotation>
    <xs:appinfo>
      <b:references>
        <b:reference targetNamespace=""http://schemas.datacontract.org/2004/07/IProfiles"" />
      </b:references>
    </xs:appinfo>
  </xs:annotation>
  <xs:element name=""GetExpiredCards"">
    <xs:complexType>
      <xs:sequence>
        <xs:element minOccurs=""0"" name=""currentYear"" type=""xs:int"" />
        <xs:element minOccurs=""0"" name=""month"" type=""xs:int"" />
        <xs:element minOccurs=""0"" name=""isPrimaryCard"" type=""xs:boolean"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
  <xs:element name=""GetExpiredCardsResponse"">
    <xs:complexType>
      <xs:sequence>
        <xs:element xmlns:q2=""http://schemas.datacontract.org/2004/07/IProfiles"" minOccurs=""0"" name=""GetExpiredCardsResult"" nillable=""true"" type=""q2:ArrayOfExpiredCreditCards"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>";
        
        public IProfile_tempuri_org() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [2];
                _RootElements[0] = "GetExpiredCards";
                _RootElements[1] = "GetExpiredCardsResponse";
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
        
        [Schema(@"http://tempuri.org/",@"GetExpiredCards")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"GetExpiredCards"})]
        public sealed class GetExpiredCards : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public GetExpiredCards() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "GetExpiredCards";
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
        
        [Schema(@"http://tempuri.org/",@"GetExpiredCardsResponse")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"GetExpiredCardsResponse"})]
        public sealed class GetExpiredCardsResponse : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public GetExpiredCardsResponse() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "GetExpiredCardsResponse";
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
