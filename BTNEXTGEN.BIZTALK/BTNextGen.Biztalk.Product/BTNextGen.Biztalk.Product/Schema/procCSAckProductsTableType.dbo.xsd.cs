namespace BTNextGen.Biztalk.Product.Schema {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"udtCSAckProducts", @"ArrayOfudtCSAckProducts"})]
    public sealed class procCSAckProductsTableType_dbo : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" xmlns:ns3=""http://schemas.microsoft.com/Sql/2008/05/Types/TableTypes/dbo"" elementFormDefault=""qualified"" targetNamespace=""http://schemas.microsoft.com/Sql/2008/05/Types/TableTypes/dbo"" version=""1.0"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:annotation>
    <xs:appinfo>
      <fileNameHint xmlns=""http://schemas.microsoft.com/servicemodel/adapters/metadata/xsd"">TableType.dbo</fileNameHint>
    </xs:appinfo>
  </xs:annotation>
  <xs:complexType name=""udtCSAckProducts"">
    <xs:sequence>
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""BTKey"" nillable=""true"">
        <xs:simpleType>
          <xs:restriction base=""xs:string"">
            <xs:maxLength value=""10"" />
          </xs:restriction>
        </xs:simpleType>
      </xs:element>
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""CSLoadError"" nillable=""true"" type=""xs:string"" />
    </xs:sequence>
  </xs:complexType>
  <xs:element name=""udtCSAckProducts"" nillable=""true"" type=""ns3:udtCSAckProducts"" />
  <xs:complexType name=""ArrayOfudtCSAckProducts"">
    <xs:sequence>
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""udtCSAckProducts"" type=""ns3:udtCSAckProducts"" />
    </xs:sequence>
  </xs:complexType>
  <xs:element name=""ArrayOfudtCSAckProducts"" nillable=""true"" type=""ns3:ArrayOfudtCSAckProducts"" />
</xs:schema>";
        
        public procCSAckProductsTableType_dbo() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [2];
                _RootElements[0] = "udtCSAckProducts";
                _RootElements[1] = "ArrayOfudtCSAckProducts";
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
        
        [Schema(@"http://schemas.microsoft.com/Sql/2008/05/Types/TableTypes/dbo",@"udtCSAckProducts")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"udtCSAckProducts"})]
        public sealed class udtCSAckProducts : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public udtCSAckProducts() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "udtCSAckProducts";
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
        
        [Schema(@"http://schemas.microsoft.com/Sql/2008/05/Types/TableTypes/dbo",@"ArrayOfudtCSAckProducts")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"ArrayOfudtCSAckProducts"})]
        public sealed class ArrayOfudtCSAckProducts : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public ArrayOfudtCSAckProducts() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "ArrayOfudtCSAckProducts";
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
