namespace BTNextGen.Biztalk.BAMAlerts {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"http://BTNextGen.Biztalk.BAMAlerts.BamAlertTrigger",@"BAMAlertTrigger")]
    [Microsoft.XLANGs.BaseTypes.DistinguishedFieldAttribute(typeof(System.String), "InterfaceName", XPath = @"/*[local-name()='BAMAlertTrigger' and namespace-uri()='http://BTNextGen.Biztalk.BAMAlerts.BamAlertTrigger']/*[local-name()='InterfaceName' and namespace-uri()='']", XsdType = @"string")]
    [Microsoft.XLANGs.BaseTypes.DistinguishedFieldAttribute(typeof(System.String), "Duration", XPath = @"/*[local-name()='BAMAlertTrigger' and namespace-uri()='http://BTNextGen.Biztalk.BAMAlerts.BamAlertTrigger']/*[local-name()='Duration' and namespace-uri()='']", XsdType = @"string")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"BAMAlertTrigger"})]
    public sealed class BamAlertTrigger : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns=""http://BTNextGen.Biztalk.BAMAlerts.BamAlertTrigger"" xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" targetNamespace=""http://BTNextGen.Biztalk.BAMAlerts.BamAlertTrigger"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:element name=""BAMAlertTrigger"">
    <xs:annotation>
      <xs:appinfo>
        <b:properties>
          <b:property distinguished=""true"" xpath=""/*[local-name()='BAMAlertTrigger' and namespace-uri()='http://BTNextGen.Biztalk.BAMAlerts.BamAlertTrigger']/*[local-name()='InterfaceName' and namespace-uri()='']"" />
          <b:property distinguished=""true"" xpath=""/*[local-name()='BAMAlertTrigger' and namespace-uri()='http://BTNextGen.Biztalk.BAMAlerts.BamAlertTrigger']/*[local-name()='Duration' and namespace-uri()='']"" />
        </b:properties>
      </xs:appinfo>
    </xs:annotation>
    <xs:complexType>
      <xs:sequence>
        <xs:element name=""InterfaceName"" type=""xs:string"" />
        <xs:element name=""Duration"" type=""xs:string"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>";
        
        public BamAlertTrigger() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [1];
                _RootElements[0] = "BAMAlertTrigger";
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
