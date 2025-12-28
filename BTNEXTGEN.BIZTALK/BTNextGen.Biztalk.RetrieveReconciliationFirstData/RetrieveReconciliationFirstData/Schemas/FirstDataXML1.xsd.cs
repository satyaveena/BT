namespace RetrieveReconciliationFirstData.Schemas {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"",@"FirstDataReconNode")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"FirstDataReconNode"})]
    public sealed class FirstDataXML1 : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" attributeFormDefault=""unqualified"" elementFormDefault=""qualified"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:element name=""FirstDataReconNode"">
    <xs:complexType>
      <xs:sequence>
        <xs:element name=""F1"" type=""xs:string"" />
        <xs:element minOccurs=""0"" name=""F2"" type=""xs:string"" />
        <xs:element name=""F3"" type=""xs:string"" />
        <xs:element name=""F4"" type=""xs:string"" />
        <xs:element name=""F5"" type=""xs:string"" />
        <xs:element name=""F6"" type=""xs:string"" />
        <xs:element name=""F7"" type=""xs:string"" />
        <xs:element name=""F8"" type=""xs:string"" />
        <xs:element name=""F9"" type=""xs:string"" />
        <xs:element name=""F10"" type=""xs:string"" />
        <xs:element name=""F11"" type=""xs:string"" />
        <xs:element name=""F12"" type=""xs:string"" />
        <xs:element name=""F13"" type=""xs:string"" />
        <xs:element name=""F14"" type=""xs:string"" />
        <xs:element name=""F15"" type=""xs:string"" />
        <xs:element name=""F16"" type=""xs:string"" />
        <xs:element name=""F17"" type=""xs:string"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>";
        
        public FirstDataXML1() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [1];
                _RootElements[0] = "FirstDataReconNode";
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
