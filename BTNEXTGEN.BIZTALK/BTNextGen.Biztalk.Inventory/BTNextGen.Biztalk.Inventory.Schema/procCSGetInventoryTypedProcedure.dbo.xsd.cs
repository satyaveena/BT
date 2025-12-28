namespace BTNextGen.Biztalk.Inventory.Schema {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Microsoft.XLANGs.BaseTypes.DistinguishedFieldAttribute(typeof(System.Int32), "MaxProducts", XPath = @"/*[local-name()='procCSGetInventory' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/TypedProcedures/dbo']/*[local-name()='MaxProducts' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/TypedProcedures/dbo']", XsdType = @"int")]
    [Microsoft.XLANGs.BaseTypes.DistinguishedFieldAttribute(typeof(System.Int32), "StoredProcedureResultSet1.StoredProcedureResultSet1.ProductCount", XPath = @"/*[local-name()='procCSGetInventoryResponse' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/TypedProcedures/dbo']/*[local-name()='StoredProcedureResultSet1' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/TypedProcedures/dbo']/*[local-name()='StoredProcedureResultSet1' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procCSGetInventory']/*[local-name()='ProductCount' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procCSGetInventory']", XsdType = @"int")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"procCSGetInventory", @"procCSGetInventoryResponse"})]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.Biztalk.Inventory.Schema.procCSGetInventoryProcedureResultSet_dbo_procCSGetInventory", typeof(global::BTNextGen.Biztalk.Inventory.Schema.procCSGetInventoryProcedureResultSet_dbo_procCSGetInventory))]
    public sealed class procCSGetInventoryTypedProcedure_dbo : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns=""http://schemas.microsoft.com/Sql/2008/05/TypedProcedures/dbo"" xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" xmlns:ns3=""http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procCSGetInventory"" elementFormDefault=""qualified"" targetNamespace=""http://schemas.microsoft.com/Sql/2008/05/TypedProcedures/dbo"" version=""1.0"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:import schemaLocation=""BTNextGen.Biztalk.Inventory.Schema.procCSGetInventoryProcedureResultSet_dbo_procCSGetInventory"" namespace=""http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procCSGetInventory"" />
  <xs:annotation>
    <xs:appinfo>
      <fileNameHint xmlns=""http://schemas.microsoft.com/servicemodel/adapters/metadata/xsd"">TypedProcedure.dbo</fileNameHint>
      <b:references>
        <b:reference targetNamespace=""http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procCSGetInventory"" />
      </b:references>
    </xs:appinfo>
  </xs:annotation>
  <xs:element name=""procCSGetInventory"">
    <xs:annotation>
      <xs:documentation>
        <doc:action xmlns:doc=""http://schemas.microsoft.com/servicemodel/adapters/metadata/documentation"">TypedProcedure/dbo/procCSGetInventory</doc:action>
      </xs:documentation>
      <xs:appinfo>
        <b:properties>
          <b:property distinguished=""true"" xpath=""/*[local-name()='procCSGetInventory' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/TypedProcedures/dbo']/*[local-name()='MaxProducts' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/TypedProcedures/dbo']"" />
        </b:properties>
      </xs:appinfo>
    </xs:annotation>
    <xs:complexType>
      <xs:sequence>
        <xs:element minOccurs=""0"" maxOccurs=""1"" name=""CatalogName"" nillable=""true"">
          <xs:simpleType>
            <xs:restriction base=""xs:string"">
              <xs:maxLength value=""50"" />
            </xs:restriction>
          </xs:simpleType>
        </xs:element>
        <xs:element minOccurs=""0"" maxOccurs=""1"" name=""MaxProducts"" nillable=""true"" type=""xs:int"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
  <xs:element name=""procCSGetInventoryResponse"">
    <xs:annotation>
      <xs:documentation>
        <doc:action xmlns:doc=""http://schemas.microsoft.com/servicemodel/adapters/metadata/documentation"">TypedProcedure/dbo/procCSGetInventory/response</doc:action>
      </xs:documentation>
      <xs:appinfo>
        <b:properties>
          <b:property distinguished=""true"" xpath=""/*[local-name()='procCSGetInventoryResponse' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/TypedProcedures/dbo']/*[local-name()='StoredProcedureResultSet1' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/TypedProcedures/dbo']/*[local-name()='StoredProcedureResultSet1' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procCSGetInventory']/*[local-name()='ProductCount' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procCSGetInventory']"" />
        </b:properties>
      </xs:appinfo>
    </xs:annotation>
    <xs:complexType>
      <xs:sequence>
        <xs:element minOccurs=""0"" maxOccurs=""1"" name=""StoredProcedureResultSet0"" nillable=""true"" type=""ns3:ArrayOfStoredProcedureResultSet0"" />
        <xs:element minOccurs=""0"" maxOccurs=""1"" name=""StoredProcedureResultSet1"" nillable=""true"" type=""ns3:ArrayOfStoredProcedureResultSet1"" />
        <xs:element minOccurs=""1"" maxOccurs=""1"" name=""ReturnValue"" type=""xs:int"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>";
        
        public procCSGetInventoryTypedProcedure_dbo() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [2];
                _RootElements[0] = "procCSGetInventory";
                _RootElements[1] = "procCSGetInventoryResponse";
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
        
        [Schema(@"http://schemas.microsoft.com/Sql/2008/05/TypedProcedures/dbo",@"procCSGetInventory")]
        [Microsoft.XLANGs.BaseTypes.DistinguishedFieldAttribute(typeof(System.Int32), "MaxProducts", XPath = @"/*[local-name()='procCSGetInventory' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/TypedProcedures/dbo']/*[local-name()='MaxProducts' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/TypedProcedures/dbo']", XsdType = @"int")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"procCSGetInventory"})]
        public sealed class procCSGetInventory : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public procCSGetInventory() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "procCSGetInventory";
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
        
        [Schema(@"http://schemas.microsoft.com/Sql/2008/05/TypedProcedures/dbo",@"procCSGetInventoryResponse")]
        [Microsoft.XLANGs.BaseTypes.DistinguishedFieldAttribute(typeof(System.Int32), "StoredProcedureResultSet1.StoredProcedureResultSet1.ProductCount", XPath = @"/*[local-name()='procCSGetInventoryResponse' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/TypedProcedures/dbo']/*[local-name()='StoredProcedureResultSet1' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/TypedProcedures/dbo']/*[local-name()='StoredProcedureResultSet1' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procCSGetInventory']/*[local-name()='ProductCount' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procCSGetInventory']", XsdType = @"int")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"procCSGetInventoryResponse"})]
        public sealed class procCSGetInventoryResponse : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public procCSGetInventoryResponse() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "procCSGetInventoryResponse";
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
