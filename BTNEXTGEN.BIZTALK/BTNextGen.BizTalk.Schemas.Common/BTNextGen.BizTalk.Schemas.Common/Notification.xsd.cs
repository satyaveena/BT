namespace BTNextGen.BizTalk.Schemas.Common {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"http://schemas.microsoft.com/Sql/2008/05/Notification/",@"Notification")]
    [Microsoft.XLANGs.BaseTypes.DistinguishedFieldAttribute(typeof(System.String), "Info", XPath = @"/*[local-name()='Notification' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/Notification/']/*[local-name()='Info' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/Notification/']", XsdType = @"string")]
    [Microsoft.XLANGs.BaseTypes.DistinguishedFieldAttribute(typeof(System.String), "Source", XPath = @"/*[local-name()='Notification' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/Notification/']/*[local-name()='Source' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/Notification/']", XsdType = @"string")]
    [Microsoft.XLANGs.BaseTypes.DistinguishedFieldAttribute(typeof(System.String), "Type", XPath = @"/*[local-name()='Notification' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/Notification/']/*[local-name()='Type' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/Notification/']", XsdType = @"string")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"Notification"})]
    public sealed class Notification : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" xmlns:tns=""http://schemas.microsoft.com/Sql/2008/05/Notification/"" elementFormDefault=""qualified"" targetNamespace=""http://schemas.microsoft.com/Sql/2008/05/Notification/"" version=""1.0"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:annotation>
    <xs:appinfo>
      <fileNameHint xmlns=""http://schemas.microsoft.com/servicemodel/adapters/metadata/xsd"">Notification</fileNameHint>
    </xs:appinfo>
  </xs:annotation>
  <xs:element name=""Notification"">
    <xs:annotation>
      <xs:documentation>
        <doc:action xmlns:doc=""http://schemas.microsoft.com/servicemodel/adapters/metadata/documentation"">Notification</doc:action>
      </xs:documentation>
      <xs:appinfo>
        <b:properties>
          <b:property distinguished=""true"" xpath=""/*[local-name()='Notification' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/Notification/']/*[local-name()='Info' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/Notification/']"" />
          <b:property distinguished=""true"" xpath=""/*[local-name()='Notification' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/Notification/']/*[local-name()='Source' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/Notification/']"" />
          <b:property distinguished=""true"" xpath=""/*[local-name()='Notification' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/Notification/']/*[local-name()='Type' and namespace-uri()='http://schemas.microsoft.com/Sql/2008/05/Notification/']"" />
        </b:properties>
      </xs:appinfo>
    </xs:annotation>
    <xs:complexType>
      <xs:sequence>
        <xs:element minOccurs=""1"" maxOccurs=""1"" name=""Info"" nillable=""true"" type=""xs:string"" />
        <xs:element minOccurs=""1"" maxOccurs=""1"" name=""Source"" nillable=""true"" type=""xs:string"" />
        <xs:element minOccurs=""1"" maxOccurs=""1"" name=""Type"" nillable=""true"" type=""xs:string"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>";
        
        public Notification() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [1];
                _RootElements[0] = "Notification";
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
