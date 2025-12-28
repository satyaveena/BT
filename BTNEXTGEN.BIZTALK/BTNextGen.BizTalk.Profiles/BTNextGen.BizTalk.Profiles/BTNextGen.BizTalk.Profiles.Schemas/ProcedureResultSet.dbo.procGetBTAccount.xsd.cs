namespace BTNextGen.BizTalk.Profiles.Schemas {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Microsoft.XLANGs.BaseTypes.DistinguishedFieldAttribute(typeof(System.String), "legacy_source_system", XPath = @"/*[local-name()='StoredProcedureResultSet0' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procGetBTAccount']/*[local-name()='legacy_source_system' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procGetBTAccount']", XsdType = @"string")]
    [Microsoft.XLANGs.BaseTypes.DistinguishedFieldAttribute(typeof(System.Int32), "legacy_org_id", XPath = @"/*[local-name()='StoredProcedureResultSet0' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procGetBTAccount']/*[local-name()='legacy_org_id' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procGetBTAccount']", XsdType = @"int")]
    [Microsoft.XLANGs.BaseTypes.DistinguishedFieldAttribute(typeof(System.Int32), "legacy_account_id", XPath = @"/*[local-name()='StoredProcedureResultSet0' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procGetBTAccount']/*[local-name()='legacy_account_id' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procGetBTAccount']", XsdType = @"int")]
    [Microsoft.XLANGs.BaseTypes.DistinguishedFieldAttribute(typeof(System.String), "ShipToAccountGUID", XPath = @"/*[local-name()='StoredProcedureResultSet0' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procGetBTAccount']/*[local-name()='ShipToAccountGUID' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procGetBTAccount']", XsdType = @"string")]
    [Microsoft.XLANGs.BaseTypes.DistinguishedFieldAttribute(typeof(System.String), "BILLTOAccountGUID", XPath = @"/*[local-name()='StoredProcedureResultSet0' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procGetBTAccount']/*[local-name()='BILLTOAccountGUID' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procGetBTAccount']", XsdType = @"string")]
    [Microsoft.XLANGs.BaseTypes.DistinguishedFieldAttribute(typeof(System.String), "CSOrgGUID", XPath = @"/*[local-name()='StoredProcedureResultSet0' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procGetBTAccount']/*[local-name()='CSOrgGUID' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procGetBTAccount']", XsdType = @"string")]
    [Microsoft.XLANGs.BaseTypes.DistinguishedFieldAttribute(typeof(System.String), "UdtIdentityAccount", XPath = @"/*[local-name()='StoredProcedureResultSet0' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procGetBTAccount']/*[local-name()='UdtIdentityAccount' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procGetBTAccount']", XsdType = @"string")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"StoredProcedureResultSet0", @"ArrayOfStoredProcedureResultSet0"})]
    public sealed class ProcedureResultSet_dbo_procGetBTAccount : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" xmlns:ns3=""http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procGetBTAccount"" elementFormDefault=""qualified"" targetNamespace=""http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procGetBTAccount"" version=""1.0"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:annotation>
    <xs:appinfo>
      <fileNameHint xmlns=""http://schemas.microsoft.com/servicemodel/adapters/metadata/xsd"">ProcedureResultSet.dbo.procGetBTAccount</fileNameHint>
    </xs:appinfo>
  </xs:annotation>
  <xs:complexType name=""StoredProcedureResultSet0_af7784"">
    <xs:sequence>
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""legacy_source_system"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""legacy_org_id"" nillable=""true"" type=""xs:int"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""legacy_account_id"" nillable=""true"" type=""xs:int"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""account_alias"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""is_home_delivery"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""max_copy_per_line"" nillable=""true"" type=""xs:int"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""Legacy_updated_date"" nillable=""true"" type=""xs:dateTime"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""ShipToAccountGUID"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""CSOrgGUID"" nillable=""true"" type=""xs:string"" />
      <xs:element name=""BILLTOAccountGUID"" type=""xs:string"" />
      <xs:element name=""u_users_create_cart"" type=""xs:string"" />
      <xs:element name=""u_users_view_order"" type=""xs:string"" />
      <xs:element name=""UdtIdentityAccount"" type=""xs:string"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""b_is_disabled"" nillable=""true"" type=""xs:boolean"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""dt_disable_date"" nillable=""true"" type=""xs:dateTime"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""u_disable_reason_code"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""u_disable_user_id"" nillable=""true"" type=""xs:string"" />
    </xs:sequence>
  </xs:complexType>
  <xs:element name=""StoredProcedureResultSet0"" nillable=""true"" type=""ns3:StoredProcedureResultSet0_af7784"">
    <xs:annotation>
      <xs:appinfo>
        <b:properties>
          <b:property distinguished=""true"" xpath=""/*[local-name()='StoredProcedureResultSet0' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procGetBTAccount']/*[local-name()='legacy_source_system' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procGetBTAccount']"" />
          <b:property distinguished=""true"" xpath=""/*[local-name()='StoredProcedureResultSet0' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procGetBTAccount']/*[local-name()='legacy_org_id' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procGetBTAccount']"" />
          <b:property distinguished=""true"" xpath=""/*[local-name()='StoredProcedureResultSet0' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procGetBTAccount']/*[local-name()='legacy_account_id' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procGetBTAccount']"" />
          <b:property distinguished=""true"" xpath=""/*[local-name()='StoredProcedureResultSet0' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procGetBTAccount']/*[local-name()='ShipToAccountGUID' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procGetBTAccount']"" />
          <b:property distinguished=""true"" xpath=""/*[local-name()='StoredProcedureResultSet0' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procGetBTAccount']/*[local-name()='BILLTOAccountGUID' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procGetBTAccount']"" />
          <b:property distinguished=""true"" xpath=""/*[local-name()='StoredProcedureResultSet0' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procGetBTAccount']/*[local-name()='CSOrgGUID' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procGetBTAccount']"" />
          <b:property distinguished=""true"" xpath=""/*[local-name()='StoredProcedureResultSet0' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procGetBTAccount']/*[local-name()='UdtIdentityAccount' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procGetBTAccount']"" />
        </b:properties>
      </xs:appinfo>
    </xs:annotation>
  </xs:element>
  <xs:complexType name=""ArrayOfStoredProcedureResultSet0_af7784"">
    <xs:sequence>
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""StoredProcedureResultSet0"" type=""ns3:StoredProcedureResultSet0_af7784"" />
    </xs:sequence>
  </xs:complexType>
  <xs:element name=""ArrayOfStoredProcedureResultSet0"" nillable=""true"" type=""ns3:ArrayOfStoredProcedureResultSet0_af7784"" />
</xs:schema>";
        
        public ProcedureResultSet_dbo_procGetBTAccount() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [2];
                _RootElements[0] = "StoredProcedureResultSet0";
                _RootElements[1] = "ArrayOfStoredProcedureResultSet0";
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
        
        [Schema(@"http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procGetBTAccount",@"StoredProcedureResultSet0")]
        [Microsoft.XLANGs.BaseTypes.DistinguishedFieldAttribute(typeof(System.String), "legacy_source_system", XPath = @"/*[local-name()='StoredProcedureResultSet0' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procGetBTAccount']/*[local-name()='legacy_source_system' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procGetBTAccount']", XsdType = @"string")]
        [Microsoft.XLANGs.BaseTypes.DistinguishedFieldAttribute(typeof(System.Int32), "legacy_org_id", XPath = @"/*[local-name()='StoredProcedureResultSet0' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procGetBTAccount']/*[local-name()='legacy_org_id' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procGetBTAccount']", XsdType = @"int")]
        [Microsoft.XLANGs.BaseTypes.DistinguishedFieldAttribute(typeof(System.Int32), "legacy_account_id", XPath = @"/*[local-name()='StoredProcedureResultSet0' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procGetBTAccount']/*[local-name()='legacy_account_id' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procGetBTAccount']", XsdType = @"int")]
        [Microsoft.XLANGs.BaseTypes.DistinguishedFieldAttribute(typeof(System.String), "ShipToAccountGUID", XPath = @"/*[local-name()='StoredProcedureResultSet0' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procGetBTAccount']/*[local-name()='ShipToAccountGUID' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procGetBTAccount']", XsdType = @"string")]
        [Microsoft.XLANGs.BaseTypes.DistinguishedFieldAttribute(typeof(System.String), "BILLTOAccountGUID", XPath = @"/*[local-name()='StoredProcedureResultSet0' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procGetBTAccount']/*[local-name()='BILLTOAccountGUID' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procGetBTAccount']", XsdType = @"string")]
        [Microsoft.XLANGs.BaseTypes.DistinguishedFieldAttribute(typeof(System.String), "CSOrgGUID", XPath = @"/*[local-name()='StoredProcedureResultSet0' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procGetBTAccount']/*[local-name()='CSOrgGUID' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procGetBTAccount']", XsdType = @"string")]
        [Microsoft.XLANGs.BaseTypes.DistinguishedFieldAttribute(typeof(System.String), "UdtIdentityAccount", XPath = @"/*[local-name()='StoredProcedureResultSet0' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procGetBTAccount']/*[local-name()='UdtIdentityAccount' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procGetBTAccount']", XsdType = @"string")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"StoredProcedureResultSet0"})]
        public sealed class StoredProcedureResultSet0 : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public StoredProcedureResultSet0() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "StoredProcedureResultSet0";
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
        
        [Schema(@"http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procGetBTAccount",@"ArrayOfStoredProcedureResultSet0")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"ArrayOfStoredProcedureResultSet0"})]
        public sealed class ArrayOfStoredProcedureResultSet0 : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public ArrayOfStoredProcedureResultSet0() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "ArrayOfStoredProcedureResultSet0";
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
