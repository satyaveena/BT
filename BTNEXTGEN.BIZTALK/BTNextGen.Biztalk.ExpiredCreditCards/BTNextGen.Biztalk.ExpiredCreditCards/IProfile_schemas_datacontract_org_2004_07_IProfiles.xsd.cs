namespace BTNextGen.Biztalk.ExpiredCreditCards {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"ArrayOfExpiredCreditCards", @"ExpiredCreditCards"})]
    public sealed class IProfile_schemas_datacontract_org_2004_07_IProfiles : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" xmlns:tns=""http://schemas.datacontract.org/2004/07/IProfiles"" elementFormDefault=""qualified"" targetNamespace=""http://schemas.datacontract.org/2004/07/IProfiles"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:complexType name=""ArrayOfExpiredCreditCards"">
    <xs:sequence>
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""ExpiredCreditCards"" nillable=""true"" type=""tns:ExpiredCreditCards"" />
    </xs:sequence>
  </xs:complexType>
  <xs:element name=""ArrayOfExpiredCreditCards"" nillable=""true"" type=""tns:ArrayOfExpiredCreditCards"" />
  <xs:complexType name=""ExpiredCreditCards"">
    <xs:sequence>
      <xs:element minOccurs=""0"" name=""alias_name"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""card_id"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""contact_user"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""email_address"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""expiration_month"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""expiration_year"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""first_name"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""last_4_digits"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""last_name"" nillable=""true"" type=""xs:string"" />
    </xs:sequence>
  </xs:complexType>
  <xs:element name=""ExpiredCreditCards"" nillable=""true"" type=""tns:ExpiredCreditCards"" />
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
                _RootElements[0] = "ArrayOfExpiredCreditCards";
                _RootElements[1] = "ExpiredCreditCards";
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
        
        [Schema(@"http://schemas.datacontract.org/2004/07/IProfiles",@"ArrayOfExpiredCreditCards")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"ArrayOfExpiredCreditCards"})]
        public sealed class ArrayOfExpiredCreditCards : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public ArrayOfExpiredCreditCards() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "ArrayOfExpiredCreditCards";
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
        
        [Schema(@"http://schemas.datacontract.org/2004/07/IProfiles",@"ExpiredCreditCards")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"ExpiredCreditCards"})]
        public sealed class ExpiredCreditCards : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public ExpiredCreditCards() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "ExpiredCreditCards";
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
