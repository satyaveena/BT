namespace CartMigration {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"BasketDetails", @"ArrayOfLineItemDetails", @"LineItemDetails", @"BasketDetailsResponse", @"BasketDetailsCollection", @"BasketDetailsResponseCollection"})]
    public sealed class IOrder_schemas_datacontract_org_2004_07_BTNextGen_Services_IOrders : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" xmlns:tns=""http://schemas.datacontract.org/2004/07/BTNextGen.Services.IOrders"" elementFormDefault=""qualified"" targetNamespace=""http://schemas.datacontract.org/2004/07/BTNextGen.Services.IOrders"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:complexType name=""BasketDetails"">
    <xs:sequence>
      <xs:element minOccurs=""0"" name=""AccountInventoryReserveNumber"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""AccountInventoryType"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""AccountWarehouseList"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""BasketCreatedDate"" type=""xs:dateTime"" />
      <xs:element minOccurs=""0"" name=""BasketIsBTCart"" type=""xs:boolean"" />
      <xs:element minOccurs=""0"" name=""BasketIsTolas"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""BasketModifiedDate"" type=""xs:dateTime"" />
      <xs:element minOccurs=""0"" name=""BasketNote"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""BasketPONumber"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""BasketSpecialInstructions"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""BasketStatus"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""BasketStoreGiftWrapFee"" type=""xs:decimal"" />
      <xs:element minOccurs=""0"" name=""BasketStoreOrderFee"" type=""xs:decimal"" />
      <xs:element minOccurs=""0"" name=""BasketStoreProcessingFee"" type=""xs:decimal"" />
      <xs:element minOccurs=""0"" name=""BasketStoreShippingFee"" type=""xs:decimal"" />
      <xs:element minOccurs=""0"" name=""BasketTotal"" type=""xs:decimal"" />
      <xs:element minOccurs=""0"" name=""CSBookAccountId"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""CSEntertainmentAccountId"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""CSUserId"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""HomeDeliveryAccountId"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""HomeDeliveryAddressType"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""IsHomeDeliveryIndicator"" type=""xs:boolean"" />
      <xs:element minOccurs=""0"" name=""LegacyBasketId"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""LegacyBasketName"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""LegacySourceSystem"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""LineItemList"" nillable=""true"" type=""tns:ArrayOfLineItemDetails"" />
    </xs:sequence>
  </xs:complexType>
  <xs:element name=""BasketDetails"" nillable=""true"" type=""tns:BasketDetails"" />
  <xs:complexType name=""ArrayOfLineItemDetails"">
    <xs:sequence>
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""LineItemDetails"" nillable=""true"" type=""tns:LineItemDetails"" />
    </xs:sequence>
  </xs:complexType>
  <xs:element name=""ArrayOfLineItemDetails"" nillable=""true"" type=""tns:ArrayOfLineItemDetails"" />
  <xs:complexType name=""LineItemDetails"">
    <xs:sequence>
      <xs:element minOccurs=""0"" name=""BTBibNumber"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""BTGiftWrapCode"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""BTGiftWrapMessage"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""BTItemType"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""BTKey"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""BTTitle"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""BTTitleEditionVersion"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""BTVolumeSet"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""LegacyBasketId"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""LegacyCreatedDate"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""LegacyLineItemId"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""LegacyModifiedDate"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""LinePONumber"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""ListPrice"" type=""xs:decimal"" />
      <xs:element minOccurs=""0"" name=""PlacedPrice"" type=""xs:decimal"" />
      <xs:element minOccurs=""0"" name=""ProductCatalog"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""ProductType"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""Quantity"" type=""xs:decimal"" />
    </xs:sequence>
  </xs:complexType>
  <xs:element name=""LineItemDetails"" nillable=""true"" type=""tns:LineItemDetails"" />
  <xs:complexType name=""BasketDetailsResponse"">
    <xs:sequence>
      <xs:element minOccurs=""0"" name=""ErrorMessage"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""LegacyBasketId"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""LegacySourceSystem"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""LoadStatus"" nillable=""true"" type=""xs:string"" />
    </xs:sequence>
  </xs:complexType>
  <xs:element name=""BasketDetailsResponse"" nillable=""true"" type=""tns:BasketDetailsResponse"" />
  <xs:complexType name=""BasketDetailsCollection"">
    <xs:sequence>
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""BasketDetails"" nillable=""true"" type=""tns:BasketDetails"" />
    </xs:sequence>
  </xs:complexType>
  <xs:element name=""BasketDetailsCollection"" nillable=""true"" type=""tns:BasketDetailsCollection"" />
  <xs:complexType name=""BasketDetailsResponseCollection"">
    <xs:sequence>
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""BasketDetailsResponse"" nillable=""true"" type=""tns:BasketDetailsResponse"" />
    </xs:sequence>
  </xs:complexType>
  <xs:element name=""BasketDetailsResponseCollection"" nillable=""true"" type=""tns:BasketDetailsResponseCollection"" />
</xs:schema>";
        
        public IOrder_schemas_datacontract_org_2004_07_BTNextGen_Services_IOrders() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [6];
                _RootElements[0] = "BasketDetails";
                _RootElements[1] = "ArrayOfLineItemDetails";
                _RootElements[2] = "LineItemDetails";
                _RootElements[3] = "BasketDetailsResponse";
                _RootElements[4] = "BasketDetailsCollection";
                _RootElements[5] = "BasketDetailsResponseCollection";
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
        
        [Schema(@"http://schemas.datacontract.org/2004/07/BTNextGen.Services.IOrders",@"BasketDetails")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"BasketDetails"})]
        public sealed class BasketDetails : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public BasketDetails() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "BasketDetails";
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
        
        [Schema(@"http://schemas.datacontract.org/2004/07/BTNextGen.Services.IOrders",@"ArrayOfLineItemDetails")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"ArrayOfLineItemDetails"})]
        public sealed class ArrayOfLineItemDetails : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public ArrayOfLineItemDetails() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "ArrayOfLineItemDetails";
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
        
        [Schema(@"http://schemas.datacontract.org/2004/07/BTNextGen.Services.IOrders",@"LineItemDetails")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"LineItemDetails"})]
        public sealed class LineItemDetails : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public LineItemDetails() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "LineItemDetails";
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
        
        [Schema(@"http://schemas.datacontract.org/2004/07/BTNextGen.Services.IOrders",@"BasketDetailsResponse")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"BasketDetailsResponse"})]
        public sealed class BasketDetailsResponse : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public BasketDetailsResponse() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "BasketDetailsResponse";
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
        
        [Schema(@"http://schemas.datacontract.org/2004/07/BTNextGen.Services.IOrders",@"BasketDetailsCollection")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"BasketDetailsCollection"})]
        public sealed class BasketDetailsCollection : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public BasketDetailsCollection() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "BasketDetailsCollection";
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
        
        [Schema(@"http://schemas.datacontract.org/2004/07/BTNextGen.Services.IOrders",@"BasketDetailsResponseCollection")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"BasketDetailsResponseCollection"})]
        public sealed class BasketDetailsResponseCollection : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public BasketDetailsResponseCollection() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "BasketDetailsResponseCollection";
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
