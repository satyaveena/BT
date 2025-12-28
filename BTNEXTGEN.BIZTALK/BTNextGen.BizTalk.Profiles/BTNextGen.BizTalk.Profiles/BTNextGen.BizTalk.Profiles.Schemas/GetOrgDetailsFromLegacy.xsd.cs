namespace BTNextGen.BizTalk.Profiles.Schemas {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Microsoft.XLANGs.BaseTypes.DistinguishedFieldAttribute(typeof(System.Int32), "pintRowCount", XPath = @"/*[local-name()='procGetOrganization' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/TypedProcedures/dbo']/*[local-name()='pintRowCount' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/TypedProcedures/dbo']", XsdType = @"int")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"procGetOrganization", @"procGetOrganizationResponse"})]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.BizTalk.Profiles.Schemas.ProcedureResultSet_dbo_procGetOrganization", typeof(global::BTNextGen.BizTalk.Profiles.Schemas.ProcedureResultSet_dbo_procGetOrganization))]
    public sealed class GetOrgDetailsFromLegacy : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns=""http://schemas.microsoft.com/Sql/2008/05/TypedProcedures/dbo"" xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" xmlns:ns3=""http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procGetOrganization"" elementFormDefault=""qualified"" targetNamespace=""http://schemas.microsoft.com/Sql/2008/05/TypedProcedures/dbo"" version=""1.0"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:import schemaLocation=""BTNextGen.BizTalk.Profiles.Schemas.ProcedureResultSet_dbo_procGetOrganization"" namespace=""http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procGetOrganization"" />
  <xs:annotation>
    <xs:appinfo>
      <fileNameHint xmlns=""http://schemas.microsoft.com/servicemodel/adapters/metadata/xsd"">TypedProcedure.dbo</fileNameHint>
      <schemaInfo is_envelope=""yes"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
      <b:references>
        <b:reference targetNamespace=""http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procGetOrganization"" />
      </b:references>
    </xs:appinfo>
  </xs:annotation>
  <xs:element name=""procGetOrganization"">
    <xs:annotation>
      <xs:documentation>
        <doc:action xmlns:doc=""http://schemas.microsoft.com/servicemodel/adapters/metadata/documentation"">TypedProcedure/dbo/procGetOrganization</doc:action>
      </xs:documentation>
      <xs:appinfo>
        <b:recordInfo body_xpath=""/*[local-name()='procGetOrganization' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/TypedProcedures/dbo']"" />
        <b:properties>
          <b:property distinguished=""true"" xpath=""/*[local-name()='procGetOrganization' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/TypedProcedures/dbo']/*[local-name()='pintRowCount' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/TypedProcedures/dbo']"" />
        </b:properties>
      </xs:appinfo>
    </xs:annotation>
    <xs:complexType>
      <xs:sequence>
        <xs:element name=""pintRowCount"" type=""xs:int"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
  <xs:element name=""procGetOrganizationResponse"">
    <xs:annotation>
      <xs:documentation>
        <doc:action xmlns:doc=""http://schemas.microsoft.com/servicemodel/adapters/metadata/documentation"">TypedProcedure/dbo/procGetOrganization/response</doc:action>
      </xs:documentation>
      <xs:appinfo>
        <recordInfo body_xpath=""/*[local-name()='procGetOrganizationResponse' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/TypedProcedures/dbo']/*[local-name()='StoredProcedureResultSet0' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/TypedProcedures/dbo']"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
      </xs:appinfo>
    </xs:annotation>
    <xs:complexType>
      <xs:sequence>
        <xs:element minOccurs=""0"" maxOccurs=""1"" name=""StoredProcedureResultSet0"" nillable=""true"" type=""ns3:ArrayOfStoredProcedureResultSet0_eeba28"" />
        <xs:element minOccurs=""1"" maxOccurs=""1"" name=""ReturnValue"" type=""xs:int"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>";
        
        public GetOrgDetailsFromLegacy() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [2];
                _RootElements[0] = "procGetOrganization";
                _RootElements[1] = "procGetOrganizationResponse";
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
        
        [Schema(@"http://schemas.microsoft.com/Sql/2008/05/TypedProcedures/dbo",@"procGetOrganization")]
        [BodyXPath(@"/*[local-name()='procGetOrganization' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/TypedProcedures/dbo']")]
        [Microsoft.XLANGs.BaseTypes.DistinguishedFieldAttribute(typeof(System.Int32), "pintRowCount", XPath = @"/*[local-name()='procGetOrganization' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/TypedProcedures/dbo']/*[local-name()='pintRowCount' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/TypedProcedures/dbo']", XsdType = @"int")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"procGetOrganization"})]
        public sealed class procGetOrganization : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public procGetOrganization() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "procGetOrganization";
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
        
        [Schema(@"http://schemas.microsoft.com/Sql/2008/05/TypedProcedures/dbo",@"procGetOrganizationResponse")]
        [BodyXPath(@"/*[local-name()='procGetOrganizationResponse' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/TypedProcedures/dbo']/*[local-name()='StoredProcedureResultSet0' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/TypedProcedures/dbo']")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"procGetOrganizationResponse"})]
        public sealed class procGetOrganizationResponse : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public procGetOrganizationResponse() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "procGetOrganizationResponse";
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
