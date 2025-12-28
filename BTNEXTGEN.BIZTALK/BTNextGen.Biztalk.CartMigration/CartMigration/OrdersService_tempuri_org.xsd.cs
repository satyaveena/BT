namespace CartMigration {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"CreateLegacyBasket", @"CreateLegacyBasketResponse", @"CreateLegacyBaskets", @"CreateLegacyBasketsResponse"})]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"CartMigration.OrdersService_schemas_datacontract_org_2004_07_BTNextGen_Services_Orders", typeof(global::CartMigration.OrdersService_schemas_datacontract_org_2004_07_BTNextGen_Services_Orders))]
    public sealed class OrdersService_tempuri_org : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" xmlns:tns=""http://tempuri.org/"" elementFormDefault=""qualified"" targetNamespace=""http://tempuri.org/"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:import schemaLocation=""CartMigration.OrdersService_schemas_datacontract_org_2004_07_BTNextGen_Services_Orders"" namespace=""http://schemas.datacontract.org/2004/07/BTNextGen.Services.Orders"" />
  <xs:annotation>
    <xs:appinfo>
      <references xmlns=""http://schemas.microsoft.com/BizTalk/2003"">
        <reference targetNamespace=""http://schemas.datacontract.org/2004/07/BTNextGen.Services.Orders"" />
      </references>
    </xs:appinfo>
  </xs:annotation>
  <xs:element name=""CreateLegacyBasket"">
    <xs:complexType>
      <xs:sequence>
        <xs:element xmlns:q1=""http://schemas.datacontract.org/2004/07/BTNextGen.Services.Orders"" minOccurs=""0"" name=""basketList"" nillable=""true"" type=""q1:BasketDetails"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
  <xs:element name=""CreateLegacyBasketResponse"">
    <xs:complexType>
      <xs:sequence>
        <xs:element xmlns:q2=""http://schemas.datacontract.org/2004/07/BTNextGen.Services.Orders"" minOccurs=""0"" name=""CreateLegacyBasketResult"" nillable=""true"" type=""q2:BasketDetailsResponse"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
  <xs:element name=""CreateLegacyBaskets"">
    <xs:complexType>
      <xs:sequence>
        <xs:element xmlns:q3=""http://schemas.datacontract.org/2004/07/BTNextGen.Services.Orders"" minOccurs=""0"" name=""basketList"" nillable=""true"" type=""q3:BasketDetailsCollection"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
  <xs:element name=""CreateLegacyBasketsResponse"">
    <xs:complexType>
      <xs:sequence>
        <xs:element xmlns:q4=""http://schemas.datacontract.org/2004/07/BTNextGen.Services.Orders"" minOccurs=""0"" name=""CreateLegacyBasketsResult"" nillable=""true"" type=""q4:BasketDetailsResponseCollection"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>";
        
        public OrdersService_tempuri_org() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [4];
                _RootElements[0] = "CreateLegacyBasket";
                _RootElements[1] = "CreateLegacyBasketResponse";
                _RootElements[2] = "CreateLegacyBaskets";
                _RootElements[3] = "CreateLegacyBasketsResponse";
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
        
        [Schema(@"http://tempuri.org/",@"CreateLegacyBasket")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"CreateLegacyBasket"})]
        public sealed class CreateLegacyBasket : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public CreateLegacyBasket() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "CreateLegacyBasket";
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
        
        [Schema(@"http://tempuri.org/",@"CreateLegacyBasketResponse")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"CreateLegacyBasketResponse"})]
        public sealed class CreateLegacyBasketResponse : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public CreateLegacyBasketResponse() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "CreateLegacyBasketResponse";
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
        
        [Schema(@"http://tempuri.org/",@"CreateLegacyBaskets")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"CreateLegacyBaskets"})]
        public sealed class CreateLegacyBaskets : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public CreateLegacyBaskets() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "CreateLegacyBaskets";
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
        
        [Schema(@"http://tempuri.org/",@"CreateLegacyBasketsResponse")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"CreateLegacyBasketsResponse"})]
        public sealed class CreateLegacyBasketsResponse : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public CreateLegacyBasketsResponse() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "CreateLegacyBasketsResponse";
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
