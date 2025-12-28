namespace BTNextGen.Biztalk.BAMAlerts {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"Insert", @"InsertResponse"})]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.Biztalk.BAMAlerts.Table_dbo", typeof(global::BTNextGen.Biztalk.BAMAlerts.Table_dbo))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.Biztalk.BAMAlerts.SimpleTypeArray", typeof(global::BTNextGen.Biztalk.BAMAlerts.SimpleTypeArray))]
    public sealed class TableOperation_dbo_BT_BAMAlertHistory : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns:array=""http://schemas.microsoft.com/2003/10/Serialization/Arrays"" xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" xmlns:ns3=""http://schemas.microsoft.com/Sql/2008/05/Types/Tables/dbo"" elementFormDefault=""qualified"" targetNamespace=""http://schemas.microsoft.com/Sql/2008/05/TableOp/dbo/BT_BAMAlertHistory"" version=""1.0"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:import schemaLocation=""BTNextGen.Biztalk.BAMAlerts.Table_dbo"" namespace=""http://schemas.microsoft.com/Sql/2008/05/Types/Tables/dbo"" />
  <xs:import schemaLocation=""BTNextGen.Biztalk.BAMAlerts.SimpleTypeArray"" namespace=""http://schemas.microsoft.com/2003/10/Serialization/Arrays"" />
  <xs:annotation>
    <xs:appinfo>
      <fileNameHint xmlns=""http://schemas.microsoft.com/servicemodel/adapters/metadata/xsd"">TableOperation.dbo.BT_BAMAlertHistory</fileNameHint>
      <references xmlns=""http://schemas.microsoft.com/BizTalk/2003"">
        <reference targetNamespace=""http://schemas.microsoft.com/2003/10/Serialization/Arrays"" />
        <reference targetNamespace=""http://schemas.microsoft.com/Sql/2008/05/Types/Tables/dbo"" />
      </references>
    </xs:appinfo>
  </xs:annotation>
  <xs:element name=""Insert"">
    <xs:annotation>
      <xs:documentation>
        <doc:action xmlns:doc=""http://schemas.microsoft.com/servicemodel/adapters/metadata/documentation"">TableOp/Insert/dbo/BT_BAMAlertHistory</doc:action>
      </xs:documentation>
    </xs:annotation>
    <xs:complexType>
      <xs:sequence>
        <xs:element minOccurs=""0"" maxOccurs=""1"" name=""Rows"" nillable=""true"" type=""ns3:ArrayOfBT_BAMAlertHistory"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
  <xs:element name=""InsertResponse"">
    <xs:annotation>
      <xs:documentation>
        <doc:action xmlns:doc=""http://schemas.microsoft.com/servicemodel/adapters/metadata/documentation"">TableOp/Insert/dbo/BT_BAMAlertHistory/response</doc:action>
      </xs:documentation>
    </xs:annotation>
    <xs:complexType>
      <xs:sequence>
        <xs:element minOccurs=""0"" maxOccurs=""1"" name=""InsertResult"" nillable=""true"" type=""array:ArrayOflong"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>";
        
        public TableOperation_dbo_BT_BAMAlertHistory() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [2];
                _RootElements[0] = "Insert";
                _RootElements[1] = "InsertResponse";
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
        
        [Schema(@"http://schemas.microsoft.com/Sql/2008/05/TableOp/dbo/BT_BAMAlertHistory",@"Insert")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"Insert"})]
        public sealed class Insert : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public Insert() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "Insert";
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
        
        [Schema(@"http://schemas.microsoft.com/Sql/2008/05/TableOp/dbo/BT_BAMAlertHistory",@"InsertResponse")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"InsertResponse"})]
        public sealed class InsertResponse : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public InsertResponse() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "InsertResponse";
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
