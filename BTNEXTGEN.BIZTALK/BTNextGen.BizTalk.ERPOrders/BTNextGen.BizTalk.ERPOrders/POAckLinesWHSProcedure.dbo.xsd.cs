namespace BTNextGen.BizTalk.ERPOrders {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"procOrderAcknowledgementSetBasketLineItems", @"procOrderAcknowledgementSetBasketLineItemsResponse"})]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.BizTalk.ERPOrders.POAckLinesWHSTableType_dbo", typeof(global::BTNextGen.BizTalk.ERPOrders.POAckLinesWHSTableType_dbo))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.BizTalk.ERPOrders.POAckLinesWHSDataSetSchema", typeof(global::BTNextGen.BizTalk.ERPOrders.POAckLinesWHSDataSetSchema))]
    public sealed class POAckLinesWHSProcedure_dbo : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns:ns3=""http://schemas.microsoft.com/Sql/2008/05/Types/TableTypes/dbo"" xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" xmlns:ns4=""http://schemas.datacontract.org/2004/07/System.Data"" elementFormDefault=""qualified"" targetNamespace=""http://schemas.microsoft.com/Sql/2008/05/Procedures/dbo"" version=""1.0"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:import schemaLocation=""BTNextGen.BizTalk.ERPOrders.POAckLinesWHSTableType_dbo"" namespace=""http://schemas.microsoft.com/Sql/2008/05/Types/TableTypes/dbo"" />
  <xs:import schemaLocation=""BTNextGen.BizTalk.ERPOrders.POAckLinesWHSDataSetSchema"" namespace=""http://schemas.datacontract.org/2004/07/System.Data"" />
  <xs:annotation>
    <xs:appinfo>
      <fileNameHint xmlns=""http://schemas.microsoft.com/servicemodel/adapters/metadata/xsd"">Procedure.dbo</fileNameHint>
      <references xmlns=""http://schemas.microsoft.com/BizTalk/2003"">
        <reference targetNamespace=""http://schemas.microsoft.com/Sql/2008/05/Types/TableTypes/dbo"" />
        <reference targetNamespace=""http://schemas.datacontract.org/2004/07/System.Data"" />
      </references>
    </xs:appinfo>
  </xs:annotation>
  <xs:element name=""procOrderAcknowledgementSetBasketLineItems"">
    <xs:annotation>
      <xs:documentation>
        <doc:action xmlns:doc=""http://schemas.microsoft.com/servicemodel/adapters/metadata/documentation"">Procedure/dbo/procOrderAcknowledgementSetBasketLineItems</doc:action>
      </xs:documentation>
    </xs:annotation>
    <xs:complexType>
      <xs:sequence>
        <xs:element minOccurs=""0"" maxOccurs=""1"" name=""BasketLineItems"" nillable=""true"" type=""ns3:ArrayOfutblOrderAcknowledgementBasketLineItems"" />
        <xs:element minOccurs=""0"" maxOccurs=""1"" name=""BasketLineShippedItems"" nillable=""true"" type=""ns3:ArrayOfutblOrderAcknowledgementBasketLineShippedQuantities"" />
        <xs:element minOccurs=""0"" maxOccurs=""1"" name=""ReturnLiteral"" nillable=""true"" type=""xs:string"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
  <xs:element name=""procOrderAcknowledgementSetBasketLineItemsResponse"">
    <xs:annotation>
      <xs:documentation>
        <doc:action xmlns:doc=""http://schemas.microsoft.com/servicemodel/adapters/metadata/documentation"">Procedure/dbo/procOrderAcknowledgementSetBasketLineItems/response</doc:action>
      </xs:documentation>
    </xs:annotation>
    <xs:complexType>
      <xs:sequence>
        <xs:element minOccurs=""0"" maxOccurs=""1"" name=""procOrderAcknowledgementSetBasketLineItemsResult"" nillable=""true"" type=""ns4:ArrayOfDataSet"" />
        <xs:element minOccurs=""1"" maxOccurs=""1"" name=""ReturnValue"" type=""xs:int"" />
        <xs:element minOccurs=""0"" maxOccurs=""1"" name=""ReturnLiteral"" nillable=""true"" type=""xs:string"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>";
        
        public POAckLinesWHSProcedure_dbo() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [2];
                _RootElements[0] = "procOrderAcknowledgementSetBasketLineItems";
                _RootElements[1] = "procOrderAcknowledgementSetBasketLineItemsResponse";
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
        
        [Schema(@"http://schemas.microsoft.com/Sql/2008/05/Procedures/dbo",@"procOrderAcknowledgementSetBasketLineItems")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"procOrderAcknowledgementSetBasketLineItems"})]
        public sealed class procOrderAcknowledgementSetBasketLineItems : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public procOrderAcknowledgementSetBasketLineItems() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "procOrderAcknowledgementSetBasketLineItems";
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
        
        [Schema(@"http://schemas.microsoft.com/Sql/2008/05/Procedures/dbo",@"procOrderAcknowledgementSetBasketLineItemsResponse")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"procOrderAcknowledgementSetBasketLineItemsResponse"})]
        public sealed class procOrderAcknowledgementSetBasketLineItemsResponse : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public procOrderAcknowledgementSetBasketLineItemsResponse() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "procOrderAcknowledgementSetBasketLineItemsResponse";
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
