namespace BTNextGen.Biztalk.ExpiredCreditCards {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"http://BTNextGen.Biztalk.ExpiredCreditCards.DummyQueryCreditCard",@"DummyQueryCreditCard")]
    [Microsoft.XLANGs.BaseTypes.DistinguishedFieldAttribute(typeof(System.String), "DummyClause.DummyProperty.Value", XPath = @"/*[local-name()='DummyQueryCreditCard' and namespace-uri()='http://BTNextGen.Biztalk.ExpiredCreditCards.DummyQueryCreditCard']/*[local-name()='DummyClause' and namespace-uri()='']/*[local-name()='DummyProperty' and namespace-uri()='']/*[local-name()='Value' and namespace-uri()='']", XsdType = @"string")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"DummyQueryCreditCard"})]
    public sealed class DummyQueryCreditCard : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns=""http://BTNextGen.Biztalk.ExpiredCreditCards.DummyQueryCreditCard"" xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" targetNamespace=""http://BTNextGen.Biztalk.ExpiredCreditCards.DummyQueryCreditCard"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:element name=""DummyQueryCreditCard"">
    <xs:annotation>
      <xs:appinfo>
        <b:properties>
          <b:property distinguished=""true"" xpath=""/*[local-name()='DummyQueryCreditCard' and namespace-uri()='http://BTNextGen.Biztalk.ExpiredCreditCards.DummyQueryCreditCard']/*[local-name()='DummyClause' and namespace-uri()='']/*[local-name()='DummyProperty' and namespace-uri()='']/*[local-name()='Value' and namespace-uri()='']"" />
        </b:properties>
      </xs:appinfo>
    </xs:annotation>
    <xs:complexType>
      <xs:sequence>
        <xs:element name=""DummyClause"">
          <xs:complexType>
            <xs:sequence>
              <xs:element name=""DummyProperty"">
                <xs:complexType>
                  <xs:sequence>
                    <xs:element name=""Value"" type=""xs:string"" />
                  </xs:sequence>
                </xs:complexType>
              </xs:element>
            </xs:sequence>
          </xs:complexType>
        </xs:element>
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>";
        
        public DummyQueryCreditCard() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [1];
                _RootElements[0] = "DummyQueryCreditCard";
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
