namespace BTNextGen.Biztalk.ActiveCreditCards.Schemas {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"ArrayOfActiveCreditCards", @"ActiveCreditCards"})]
    public sealed class IProfile_schemas_datacontract_org_2004_07_IProfiles : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" xmlns:tns=""http://schemas.datacontract.org/2004/07/IProfiles"" elementFormDefault=""qualified"" targetNamespace=""http://schemas.datacontract.org/2004/07/IProfiles"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:complexType name=""ArrayOfActiveCreditCards"">
    <xs:sequence>
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""ActiveCreditCards"" nillable=""true"" type=""tns:ActiveCreditCards"" />
    </xs:sequence>
  </xs:complexType>
  <xs:element name=""ArrayOfActiveCreditCards"" nillable=""true"" type=""tns:ArrayOfActiveCreditCards"" />
  <xs:complexType name=""ActiveCreditCards"">
    <xs:sequence>
      <xs:element minOccurs=""0"" name=""alias"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""cardID"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""card_type"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""erp_account_id"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""expiration_month"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""expiration_year"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""is_Tolas"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""last_4_digits"" nillable=""true"" type=""xs:string"" />
    </xs:sequence>
  </xs:complexType>
  <xs:element name=""ActiveCreditCards"" nillable=""true"" type=""tns:ActiveCreditCards"" />
</xs:schema>";
        
        public IProfile_schemas_datacontract_org_2004_07_IProfiles() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [2];
                _RootElements[0] = "ArrayOfActiveCreditCards";
                _RootElements[1] = "ActiveCreditCards";
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
        
        [Schema(@"http://schemas.datacontract.org/2004/07/IProfiles",@"ArrayOfActiveCreditCards")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"ArrayOfActiveCreditCards"})]
        public sealed class ArrayOfActiveCreditCards : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public ArrayOfActiveCreditCards() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "ArrayOfActiveCreditCards";
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
        
        [Schema(@"http://schemas.datacontract.org/2004/07/IProfiles",@"ActiveCreditCards")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"ActiveCreditCards"})]
        public sealed class ActiveCreditCards : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public ActiveCreditCards() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "ActiveCreditCards";
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
