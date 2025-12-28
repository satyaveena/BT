namespace BTNextGen.BizTalk.ERPOrders {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"utblOrderAcknowledgementBasketLineItems", @"ArrayOfutblOrderAcknowledgementBasketLineItems", @"utblOrderAcknowledgementBasketLineShippedQuantities", @"ArrayOfutblOrderAcknowledgementBasketLineShippedQuantities"})]
    public sealed class POAckLinesWHSTableType_dbo : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns:ns3=""http://schemas.microsoft.com/Sql/2008/05/Types/TableTypes/dbo"" xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" elementFormDefault=""qualified"" targetNamespace=""http://schemas.microsoft.com/Sql/2008/05/Types/TableTypes/dbo"" version=""1.0"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:annotation>
    <xs:appinfo>
      <fileNameHint xmlns=""http://schemas.microsoft.com/servicemodel/adapters/metadata/xsd"">TableType.dbo</fileNameHint>
    </xs:appinfo>
  </xs:annotation>
  <xs:complexType name=""utblOrderAcknowledgementBasketLineItems"">
    <xs:sequence>
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""TransmissionNumber"" nillable=""true"">
        <xs:simpleType>
          <xs:restriction base=""xs:string"">
            <xs:maxLength value=""20"" />
          </xs:restriction>
        </xs:simpleType>
      </xs:element>
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""LineNumber"" nillable=""true"">
        <xs:simpleType>
          <xs:restriction base=""xs:string"">
            <xs:maxLength value=""10"" />
          </xs:restriction>
        </xs:simpleType>
      </xs:element>
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""BTShippedQuantity"" nillable=""true"" type=""xs:int"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""OrderedQuantity"" nillable=""true"" type=""xs:int"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""Status"" nillable=""true"">
        <xs:simpleType>
          <xs:restriction base=""xs:string"">
            <xs:maxLength value=""64"" />
          </xs:restriction>
        </xs:simpleType>
      </xs:element>
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""BTUPC"" nillable=""true"">
        <xs:simpleType>
          <xs:restriction base=""xs:string"">
            <xs:maxLength value=""14"" />
          </xs:restriction>
        </xs:simpleType>
      </xs:element>
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""BTKey"" nillable=""true"">
        <xs:simpleType>
          <xs:restriction base=""xs:string"">
            <xs:maxLength value=""10"" />
          </xs:restriction>
        </xs:simpleType>
      </xs:element>
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""BTISBN"" nillable=""true"">
        <xs:simpleType>
          <xs:restriction base=""xs:string"">
            <xs:maxLength value=""13"" />
          </xs:restriction>
        </xs:simpleType>
      </xs:element>
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""ReplacementBTKey"" nillable=""true"">
        <xs:simpleType>
          <xs:restriction base=""xs:string"">
            <xs:maxLength value=""10"" />
          </xs:restriction>
        </xs:simpleType>
      </xs:element>
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""BTWarehouse"" nillable=""true"">
        <xs:simpleType>
          <xs:restriction base=""xs:string"">
            <xs:maxLength value=""3"" />
          </xs:restriction>
        </xs:simpleType>
      </xs:element>
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""BackorderQuantity"" nillable=""true"" type=""xs:int"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""BTCancelledQuantity"" nillable=""true"" type=""xs:int"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""ETOrderSequenceNumber"" nillable=""true"">
        <xs:simpleType>
          <xs:restriction base=""xs:string"">
            <xs:maxLength value=""10"" />
          </xs:restriction>
        </xs:simpleType>
      </xs:element>
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""ETOrderLineNumber"" nillable=""true"">
        <xs:simpleType>
          <xs:restriction base=""xs:string"">
            <xs:maxLength value=""5"" />
          </xs:restriction>
        </xs:simpleType>
      </xs:element>
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""ETOrderLineActNumber"" nillable=""true"">
        <xs:simpleType>
          <xs:restriction base=""xs:string"">
            <xs:maxLength value=""3"" />
          </xs:restriction>
        </xs:simpleType>
      </xs:element>
    </xs:sequence>
  </xs:complexType>
  <xs:element name=""utblOrderAcknowledgementBasketLineItems"" nillable=""true"" type=""ns3:utblOrderAcknowledgementBasketLineItems"" />
  <xs:complexType name=""ArrayOfutblOrderAcknowledgementBasketLineItems"">
    <xs:sequence>
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""utblOrderAcknowledgementBasketLineItems"" type=""ns3:utblOrderAcknowledgementBasketLineItems"" />
    </xs:sequence>
  </xs:complexType>
  <xs:element name=""ArrayOfutblOrderAcknowledgementBasketLineItems"" nillable=""true"" type=""ns3:ArrayOfutblOrderAcknowledgementBasketLineItems"" />
  <xs:complexType name=""utblOrderAcknowledgementBasketLineShippedQuantities"">
    <xs:sequence>
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""TransmissionNumber"" nillable=""true"">
        <xs:simpleType>
          <xs:restriction base=""xs:string"">
            <xs:maxLength value=""20"" />
          </xs:restriction>
        </xs:simpleType>
      </xs:element>
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""LineNumber"" nillable=""true"">
        <xs:simpleType>
          <xs:restriction base=""xs:string"">
            <xs:maxLength value=""10"" />
          </xs:restriction>
        </xs:simpleType>
      </xs:element>
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""WarehouseCode"" nillable=""true"">
        <xs:simpleType>
          <xs:restriction base=""xs:string"">
            <xs:maxLength value=""3"" />
          </xs:restriction>
        </xs:simpleType>
      </xs:element>
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""Quantity"" nillable=""true"" type=""xs:int"" />
    </xs:sequence>
  </xs:complexType>
  <xs:element name=""utblOrderAcknowledgementBasketLineShippedQuantities"" nillable=""true"" type=""ns3:utblOrderAcknowledgementBasketLineShippedQuantities"" />
  <xs:complexType name=""ArrayOfutblOrderAcknowledgementBasketLineShippedQuantities"">
    <xs:sequence>
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""utblOrderAcknowledgementBasketLineShippedQuantities"" type=""ns3:utblOrderAcknowledgementBasketLineShippedQuantities"" />
    </xs:sequence>
  </xs:complexType>
  <xs:element name=""ArrayOfutblOrderAcknowledgementBasketLineShippedQuantities"" nillable=""true"" type=""ns3:ArrayOfutblOrderAcknowledgementBasketLineShippedQuantities"" />
</xs:schema>";
        
        public POAckLinesWHSTableType_dbo() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [4];
                _RootElements[0] = "utblOrderAcknowledgementBasketLineItems";
                _RootElements[1] = "ArrayOfutblOrderAcknowledgementBasketLineItems";
                _RootElements[2] = "utblOrderAcknowledgementBasketLineShippedQuantities";
                _RootElements[3] = "ArrayOfutblOrderAcknowledgementBasketLineShippedQuantities";
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
        
        [Schema(@"http://schemas.microsoft.com/Sql/2008/05/Types/TableTypes/dbo",@"utblOrderAcknowledgementBasketLineItems")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"utblOrderAcknowledgementBasketLineItems"})]
        public sealed class utblOrderAcknowledgementBasketLineItems : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public utblOrderAcknowledgementBasketLineItems() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "utblOrderAcknowledgementBasketLineItems";
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
        
        [Schema(@"http://schemas.microsoft.com/Sql/2008/05/Types/TableTypes/dbo",@"ArrayOfutblOrderAcknowledgementBasketLineItems")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"ArrayOfutblOrderAcknowledgementBasketLineItems"})]
        public sealed class ArrayOfutblOrderAcknowledgementBasketLineItems : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public ArrayOfutblOrderAcknowledgementBasketLineItems() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "ArrayOfutblOrderAcknowledgementBasketLineItems";
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
        
        [Schema(@"http://schemas.microsoft.com/Sql/2008/05/Types/TableTypes/dbo",@"utblOrderAcknowledgementBasketLineShippedQuantities")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"utblOrderAcknowledgementBasketLineShippedQuantities"})]
        public sealed class utblOrderAcknowledgementBasketLineShippedQuantities : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public utblOrderAcknowledgementBasketLineShippedQuantities() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "utblOrderAcknowledgementBasketLineShippedQuantities";
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
        
        [Schema(@"http://schemas.microsoft.com/Sql/2008/05/Types/TableTypes/dbo",@"ArrayOfutblOrderAcknowledgementBasketLineShippedQuantities")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"ArrayOfutblOrderAcknowledgementBasketLineShippedQuantities"})]
        public sealed class ArrayOfutblOrderAcknowledgementBasketLineShippedQuantities : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public ArrayOfutblOrderAcknowledgementBasketLineShippedQuantities() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "ArrayOfutblOrderAcknowledgementBasketLineShippedQuantities";
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
