namespace BTNextGen.Biztalk.BAMAlerts {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"BT_BAMAlertHistory", @"ArrayOfBT_BAMAlertHistory"})]
    public sealed class Table_dbo : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" xmlns:ns3=""http://schemas.microsoft.com/Sql/2008/05/Types/Tables/dbo"" elementFormDefault=""qualified"" targetNamespace=""http://schemas.microsoft.com/Sql/2008/05/Types/Tables/dbo"" version=""1.0"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:annotation>
    <xs:appinfo>
      <fileNameHint xmlns=""http://schemas.microsoft.com/servicemodel/adapters/metadata/xsd"">Table.dbo</fileNameHint>
    </xs:appinfo>
  </xs:annotation>
  <xs:complexType name=""BT_BAMAlertHistory"">
    <xs:sequence>
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""Interface_x0020_Name"" nillable=""true"">
        <xs:simpleType>
          <xs:restriction base=""xs:string"">
            <xs:maxLength value=""50"" />
          </xs:restriction>
        </xs:simpleType>
      </xs:element>
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""ActivityID"" nillable=""true"">
        <xs:simpleType>
          <xs:restriction base=""xs:string"">
            <xs:maxLength value=""100"" />
          </xs:restriction>
        </xs:simpleType>
      </xs:element>
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""RetryCount"" nillable=""true"" type=""xs:int"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""AlertExpired"" nillable=""true"" type=""xs:boolean"" />
    </xs:sequence>
  </xs:complexType>
  <xs:element name=""BT_BAMAlertHistory"" nillable=""true"" type=""ns3:BT_BAMAlertHistory"" />
  <xs:complexType name=""ArrayOfBT_BAMAlertHistory"">
    <xs:sequence>
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""BT_BAMAlertHistory"" type=""ns3:BT_BAMAlertHistory"" />
    </xs:sequence>
  </xs:complexType>
  <xs:element name=""ArrayOfBT_BAMAlertHistory"" nillable=""true"" type=""ns3:ArrayOfBT_BAMAlertHistory"" />
</xs:schema>";
        
        public Table_dbo() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [2];
                _RootElements[0] = "BT_BAMAlertHistory";
                _RootElements[1] = "ArrayOfBT_BAMAlertHistory";
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
        
        [Schema(@"http://schemas.microsoft.com/Sql/2008/05/Types/Tables/dbo",@"BT_BAMAlertHistory")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"BT_BAMAlertHistory"})]
        public sealed class BT_BAMAlertHistory : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public BT_BAMAlertHistory() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "BT_BAMAlertHistory";
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
        
        [Schema(@"http://schemas.microsoft.com/Sql/2008/05/Types/Tables/dbo",@"ArrayOfBT_BAMAlertHistory")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"ArrayOfBT_BAMAlertHistory"})]
        public sealed class ArrayOfBT_BAMAlertHistory : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public ArrayOfBT_BAMAlertHistory() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "ArrayOfBT_BAMAlertHistory";
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
