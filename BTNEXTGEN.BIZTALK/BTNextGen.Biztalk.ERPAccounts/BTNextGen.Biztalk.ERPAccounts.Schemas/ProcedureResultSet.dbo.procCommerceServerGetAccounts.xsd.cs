namespace BTNextGen.Biztalk.ERPAccounts.Schemas {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Microsoft.XLANGs.BaseTypes.DistinguishedFieldAttribute(typeof(System.String), "u_bt_account_id", XPath = @"/*[local-name()='StoredProcedureResultSet0' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procCommerceServerGetAccounts']/*[local-name()='u_bt_account_id' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procCommerceServerGetAccounts']", XsdType = @"string")]
    [Microsoft.XLANGs.BaseTypes.DistinguishedFieldAttribute(typeof(System.String), "LoadStatus", XPath = @"/*[local-name()='StoredProcedureResultSet0' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procCommerceServerGetAccounts']/*[local-name()='LoadStatus' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procCommerceServerGetAccounts']", XsdType = @"string")]
    [Microsoft.XLANGs.BaseTypes.DistinguishedFieldAttribute(typeof(System.String), "CompanyCode", XPath = @"/*[local-name()='StoredProcedureResultSet0' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procCommerceServerGetAccounts']/*[local-name()='CompanyCode' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procCommerceServerGetAccounts']", XsdType = @"string")]
    [Microsoft.XLANGs.BaseTypes.DistinguishedFieldAttribute(typeof(System.String), "b_is_shipping_account", XPath = @"/*[local-name()='StoredProcedureResultSet0' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procCommerceServerGetAccounts']/*[local-name()='b_is_shipping_account' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procCommerceServerGetAccounts']", XsdType = @"string")]
    [Microsoft.XLANGs.BaseTypes.DistinguishedFieldAttribute(typeof(System.String), "u_bill_to_account_number", XPath = @"/*[local-name()='StoredProcedureResultSet0' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procCommerceServerGetAccounts']/*[local-name()='u_bill_to_account_number' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procCommerceServerGetAccounts']", XsdType = @"string")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"StoredProcedureResultSet0", @"ArrayOfStoredProcedureResultSet0"})]
    public sealed class ProcedureResultSet_dbo_procCommerceServerGetAccounts : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" xmlns:ns3=""http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procCommerceServerGetAccounts"" elementFormDefault=""qualified"" targetNamespace=""http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procCommerceServerGetAccounts"" version=""1.0"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:annotation>
    <xs:appinfo>
      <fileNameHint xmlns=""http://schemas.microsoft.com/servicemodel/adapters/metadata/xsd"">ProcedureResultSet.dbo.procCommerceServerGetAccounts</fileNameHint>
    </xs:appinfo>
  </xs:annotation>
  <xs:complexType name=""StoredProcedureResultSet0_042022"">
    <xs:sequence>
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""u_bt_account_id"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""u_account_type"" nillable=""true"" type=""xs:int"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""b_is_billing_account"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""b_is_shipping_account"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""u_shipping_address"" nillable=""true"" type=""xs:int"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""u_billing_address"" nillable=""true"" type=""xs:int"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""b_is_order_permitted"" nillable=""true"" type=""xs:int"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""b_is_home_delivery"" nillable=""true"" type=""xs:int"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""u_primary_warehouse"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""u_warehouses"" nillable=""true"" type=""xs:int"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""u_primary_payment_option"" nillable=""true"" type=""xs:int"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""u_payment_options"" nillable=""true"" type=""xs:int"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""i_max_copy_per_line"" nillable=""true"" type=""xs:int"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""u_san_suffix"" nillable=""true"" type=""xs:int"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""u_secondary_warehouse"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""u_account_name"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""u_bill_to_account_number"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""b_is_TOLAS"" nillable=""true"" type=""xs:int"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""u_erp_account_number"" nillable=""true"" type=""xs:int"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""u_exclusive_item_group"" nillable=""true"" type=""xs:int"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""u_inventory_type"" nillable=""true"" type=""xs:int"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""u_reserve_inventory_number"" nillable=""true"" type=""xs:int"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""u_sop_price_plan_list"" nillable=""true"" type=""xs:int"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""b_check_reserve_flag"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""i_minimum_margin"" nillable=""true"" type=""xs:int"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""u_accounts"" nillable=""true"" type=""xs:int"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""LoadStatus"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""CompanyCode"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""u_product_type"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""u_account8_id"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""CheckLEReserve"" nillable=""true"" type=""xs:string"" />
      <xs:element name=""eSupplierCode"" type=""xs:string"" />
      <xs:element name=""eMarket"" type=""xs:string"" />
      <xs:element name=""eTier"" type=""xs:string"" />
      <xs:element name=""b_is_library_system_account"" type=""xs:string"" />
      <xs:element name=""b_is_online_bill_pay_enabled"" type=""xs:string"" />
    </xs:sequence>
  </xs:complexType>
  <xs:element name=""StoredProcedureResultSet0"" nillable=""true"" type=""ns3:StoredProcedureResultSet0_042022"">
    <xs:annotation>
      <xs:appinfo>
        <b:properties>
          <b:property distinguished=""true"" xpath=""/*[local-name()='StoredProcedureResultSet0' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procCommerceServerGetAccounts']/*[local-name()='u_bt_account_id' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procCommerceServerGetAccounts']"" />
          <b:property distinguished=""true"" xpath=""/*[local-name()='StoredProcedureResultSet0' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procCommerceServerGetAccounts']/*[local-name()='LoadStatus' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procCommerceServerGetAccounts']"" />
          <b:property distinguished=""true"" xpath=""/*[local-name()='StoredProcedureResultSet0' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procCommerceServerGetAccounts']/*[local-name()='CompanyCode' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procCommerceServerGetAccounts']"" />
          <b:property distinguished=""true"" xpath=""/*[local-name()='StoredProcedureResultSet0' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procCommerceServerGetAccounts']/*[local-name()='b_is_shipping_account' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procCommerceServerGetAccounts']"" />
          <b:property distinguished=""true"" xpath=""/*[local-name()='StoredProcedureResultSet0' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procCommerceServerGetAccounts']/*[local-name()='u_bill_to_account_number' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procCommerceServerGetAccounts']"" />
        </b:properties>
      </xs:appinfo>
    </xs:annotation>
  </xs:element>
  <xs:complexType name=""ArrayOfStoredProcedureResultSet0_042022"">
    <xs:sequence>
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""StoredProcedureResultSet0"" type=""ns3:StoredProcedureResultSet0_042022"" />
    </xs:sequence>
  </xs:complexType>
  <xs:element name=""ArrayOfStoredProcedureResultSet0"" nillable=""true"" type=""ns3:ArrayOfStoredProcedureResultSet0_042022"" />
</xs:schema>";
        
        public ProcedureResultSet_dbo_procCommerceServerGetAccounts() {
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
        
        [Schema(@"http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procCommerceServerGetAccounts",@"StoredProcedureResultSet0")]
        [Microsoft.XLANGs.BaseTypes.DistinguishedFieldAttribute(typeof(System.String), "u_bt_account_id", XPath = @"/*[local-name()='StoredProcedureResultSet0' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procCommerceServerGetAccounts']/*[local-name()='u_bt_account_id' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procCommerceServerGetAccounts']", XsdType = @"string")]
        [Microsoft.XLANGs.BaseTypes.DistinguishedFieldAttribute(typeof(System.String), "LoadStatus", XPath = @"/*[local-name()='StoredProcedureResultSet0' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procCommerceServerGetAccounts']/*[local-name()='LoadStatus' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procCommerceServerGetAccounts']", XsdType = @"string")]
        [Microsoft.XLANGs.BaseTypes.DistinguishedFieldAttribute(typeof(System.String), "CompanyCode", XPath = @"/*[local-name()='StoredProcedureResultSet0' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procCommerceServerGetAccounts']/*[local-name()='CompanyCode' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procCommerceServerGetAccounts']", XsdType = @"string")]
        [Microsoft.XLANGs.BaseTypes.DistinguishedFieldAttribute(typeof(System.String), "b_is_shipping_account", XPath = @"/*[local-name()='StoredProcedureResultSet0' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procCommerceServerGetAccounts']/*[local-name()='b_is_shipping_account' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procCommerceServerGetAccounts']", XsdType = @"string")]
        [Microsoft.XLANGs.BaseTypes.DistinguishedFieldAttribute(typeof(System.String), "u_bill_to_account_number", XPath = @"/*[local-name()='StoredProcedureResultSet0' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procCommerceServerGetAccounts']/*[local-name()='u_bill_to_account_number' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procCommerceServerGetAccounts']", XsdType = @"string")]
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
        
        [Schema(@"http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procCommerceServerGetAccounts",@"ArrayOfStoredProcedureResultSet0")]
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
