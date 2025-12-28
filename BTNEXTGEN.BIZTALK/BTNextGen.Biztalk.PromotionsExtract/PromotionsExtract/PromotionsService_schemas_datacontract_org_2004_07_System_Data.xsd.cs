namespace PromotionsExtract {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"BTNextGen.BizTalk.PromotionsExtract.System.Data",@"DataTable")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"DataTable"})]
    public sealed class PromotionsService_schemas_datacontract_org_2004_07_System_Data : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" xmlns:tns=""BTNextGen.BizTalk.PromotionsExtract.System.Data"" elementFormDefault=""qualified"" targetNamespace=""BTNextGen.BizTalk.PromotionsExtract.System.Data"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:element name=""DataTable"" nillable=""true"">
    <xs:complexType>
      <xs:annotation>
        <xs:appinfo>
          <ActualType Name=""DataTable"" Namespace=""http://schemas.datacontract.org/2004/07/System.Data"" xmlns=""http://schemas.microsoft.com/2003/10/Serialization/"" />
        </xs:appinfo>
      </xs:annotation>
      <xs:sequence>
        <xs:any minOccurs=""0"" maxOccurs=""unbounded"" namespace=""http://www.w3.org/2001/XMLSchema"" processContents=""lax"" />
        <xs:any minOccurs=""1"" namespace=""urn:schemas-microsoft-com:xml-diffgram-v1"" processContents=""lax"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>";
        
        public PromotionsService_schemas_datacontract_org_2004_07_System_Data() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [1];
                _RootElements[0] = "DataTable";
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
