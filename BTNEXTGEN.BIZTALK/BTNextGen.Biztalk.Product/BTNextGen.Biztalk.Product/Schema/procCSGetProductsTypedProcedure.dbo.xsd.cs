namespace BTNextGen.Biztalk.Product.Schema {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Microsoft.XLANGs.BaseTypes.DistinguishedFieldAttribute(typeof(System.Int32), "MaxProducts", XPath = @"/*[local-name()='procCSGetProducts' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/TypedProcedures/dbo']/*[local-name()='MaxProducts' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/TypedProcedures/dbo']", XsdType = @"int")]
    [Microsoft.XLANGs.BaseTypes.DistinguishedFieldAttribute(typeof(System.String), "CSStatus", XPath = @"/*[local-name()='procCSGetProducts' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/TypedProcedures/dbo']/*[local-name()='CSStatus' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/TypedProcedures/dbo']", XsdType = @"string")]
    [Microsoft.XLANGs.BaseTypes.DistinguishedFieldAttribute(typeof(System.String), "CatalogName", XPath = @"/*[local-name()='procCSGetProducts' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/TypedProcedures/dbo']/*[local-name()='CatalogName' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/TypedProcedures/dbo']", XsdType = @"string")]
    [Microsoft.XLANGs.BaseTypes.DistinguishedFieldAttribute(typeof(System.Int32), "StoredProcedureResultSet1.StoredProcedureResultSet1.ProductCount", XPath = @"/*[local-name()='procCSGetProductsResponse' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/TypedProcedures/dbo']/*[local-name()='StoredProcedureResultSet1' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/TypedProcedures/dbo']/*[local-name()='StoredProcedureResultSet1' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procCSGetProducts']/*[local-name()='ProductCount' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procCSGetProducts']", XsdType = @"int")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"procCSGetProducts", @"procCSGetProductsResponse"})]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.Biztalk.Product.procCSGetProductsProcedureResultSet_dbo_procCSGetProducts", typeof(global::BTNextGen.Biztalk.Product.procCSGetProductsProcedureResultSet_dbo_procCSGetProducts))]
    public sealed class procCSGetProductsTypedProcedure_dbo : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns=""http://schemas.microsoft.com/Sql/2008/05/TypedProcedures/dbo"" xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" xmlns:ns3=""http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procCSGetProducts"" elementFormDefault=""qualified"" targetNamespace=""http://schemas.microsoft.com/Sql/2008/05/TypedProcedures/dbo"" version=""1.0"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:import schemaLocation=""BTNextGen.Biztalk.Product.procCSGetProductsProcedureResultSet_dbo_procCSGetProducts"" namespace=""http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procCSGetProducts"" />
  <xs:annotation>
    <xs:appinfo>
      <fileNameHint xmlns=""http://schemas.microsoft.com/servicemodel/adapters/metadata/xsd"">TypedProcedure.dbo</fileNameHint>
      <b:references>
        <b:reference targetNamespace=""http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procCSGetProducts"" />
      </b:references>
    </xs:appinfo>
  </xs:annotation>
  <xs:element name=""procCSGetProducts"">
    <xs:annotation>
      <xs:documentation>
        <doc:action xmlns:doc=""http://schemas.microsoft.com/servicemodel/adapters/metadata/documentation"">TypedProcedure/dbo/procCSGetProducts</doc:action>
      </xs:documentation>
      <xs:appinfo>
        <b:properties>
          <b:property distinguished=""true"" xpath=""/*[local-name()='procCSGetProducts' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/TypedProcedures/dbo']/*[local-name()='MaxProducts' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/TypedProcedures/dbo']"" />
          <b:property distinguished=""true"" xpath=""/*[local-name()='procCSGetProducts' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/TypedProcedures/dbo']/*[local-name()='CSStatus' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/TypedProcedures/dbo']"" />
          <b:property distinguished=""true"" xpath=""/*[local-name()='procCSGetProducts' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/TypedProcedures/dbo']/*[local-name()='CatalogName' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/TypedProcedures/dbo']"" />
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
        <xs:element minOccurs=""0"" maxOccurs=""1"" name=""CSStatus"" nillable=""true"">
          <xs:simpleType>
            <xs:restriction base=""xs:string"">
              <xs:maxLength value=""20"" />
            </xs:restriction>
          </xs:simpleType>
        </xs:element>
      </xs:sequence>
    </xs:complexType>
  </xs:element>
  <xs:element name=""procCSGetProductsResponse"">
    <xs:annotation>
      <xs:documentation>
        <doc:action xmlns:doc=""http://schemas.microsoft.com/servicemodel/adapters/metadata/documentation"">TypedProcedure/dbo/procCSGetProducts/response</doc:action>
      </xs:documentation>
      <xs:appinfo>
        <b:properties>
          <b:property distinguished=""true"" xpath=""/*[local-name()='procCSGetProductsResponse' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/TypedProcedures/dbo']/*[local-name()='StoredProcedureResultSet1' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/TypedProcedures/dbo']/*[local-name()='StoredProcedureResultSet1' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procCSGetProducts']/*[local-name()='ProductCount' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procCSGetProducts']"" />
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
        
        public procCSGetProductsTypedProcedure_dbo() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [2];
                _RootElements[0] = "procCSGetProducts";
                _RootElements[1] = "procCSGetProductsResponse";
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
        
        [Schema(@"http://schemas.microsoft.com/Sql/2008/05/TypedProcedures/dbo",@"procCSGetProducts")]
        [Microsoft.XLANGs.BaseTypes.DistinguishedFieldAttribute(typeof(System.Int32), "MaxProducts", XPath = @"/*[local-name()='procCSGetProducts' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/TypedProcedures/dbo']/*[local-name()='MaxProducts' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/TypedProcedures/dbo']", XsdType = @"int")]
        [Microsoft.XLANGs.BaseTypes.DistinguishedFieldAttribute(typeof(System.String), "CSStatus", XPath = @"/*[local-name()='procCSGetProducts' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/TypedProcedures/dbo']/*[local-name()='CSStatus' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/TypedProcedures/dbo']", XsdType = @"string")]
        [Microsoft.XLANGs.BaseTypes.DistinguishedFieldAttribute(typeof(System.String), "CatalogName", XPath = @"/*[local-name()='procCSGetProducts' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/TypedProcedures/dbo']/*[local-name()='CatalogName' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/TypedProcedures/dbo']", XsdType = @"string")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"procCSGetProducts"})]
        public sealed class procCSGetProducts : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public procCSGetProducts() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "procCSGetProducts";
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
        
        [Schema(@"http://schemas.microsoft.com/Sql/2008/05/TypedProcedures/dbo",@"procCSGetProductsResponse")]
        [Microsoft.XLANGs.BaseTypes.DistinguishedFieldAttribute(typeof(System.Int32), "StoredProcedureResultSet1.StoredProcedureResultSet1.ProductCount", XPath = @"/*[local-name()='procCSGetProductsResponse' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/TypedProcedures/dbo']/*[local-name()='StoredProcedureResultSet1' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/TypedProcedures/dbo']/*[local-name()='StoredProcedureResultSet1' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procCSGetProducts']/*[local-name()='ProductCount' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procCSGetProducts']", XsdType = @"int")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"procCSGetProductsResponse"})]
        public sealed class procCSGetProductsResponse : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public procCSGetProductsResponse() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "procCSGetProductsResponse";
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
