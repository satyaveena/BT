namespace PromotionsExtract {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"http://PromotionsExtract.GetPromotions",@"GetPromotions")]
    [Microsoft.XLANGs.BaseTypes.PropertyAttribute(typeof(global::PromotionsExtract.PropertySchema.GetPromotionsField), XPath = @"/*[local-name()='GetPromotions' and namespace-uri()='http://PromotionsExtract.GetPromotions']/*[local-name()='GetPromotionsField' and namespace-uri()='']", XsdType = @"string")]
    [Microsoft.XLANGs.BaseTypes.DistinguishedFieldAttribute(typeof(System.String), "GetPromotionsField", XPath = @"/*[local-name()='GetPromotions' and namespace-uri()='http://PromotionsExtract.GetPromotions']/*[local-name()='GetPromotionsField' and namespace-uri()='']", XsdType = @"string")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"GetPromotions"})]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"PromotionsExtract.PropertySchema.PropertySchema", typeof(global::PromotionsExtract.PropertySchema.PropertySchema))]
    public sealed class GetPromotions : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns=""http://PromotionsExtract.GetPromotions"" xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" xmlns:ns0=""https://PromotionsExtract.PropertySchema"" targetNamespace=""http://PromotionsExtract.GetPromotions"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:annotation>
    <xs:appinfo>
      <b:imports>
        <b:namespace prefix=""ns0"" uri=""https://PromotionsExtract.PropertySchema"" location=""PromotionsExtract.PropertySchema.PropertySchema"" />
      </b:imports>
    </xs:appinfo>
  </xs:annotation>
  <xs:element name=""GetPromotions"">
    <xs:annotation>
      <xs:appinfo>
        <b:properties>
          <b:property name=""ns0:GetPromotionsField"" xpath=""/*[local-name()='GetPromotions' and namespace-uri()='http://PromotionsExtract.GetPromotions']/*[local-name()='GetPromotionsField' and namespace-uri()='']"" />
          <b:property distinguished=""true"" xpath=""/*[local-name()='GetPromotions' and namespace-uri()='http://PromotionsExtract.GetPromotions']/*[local-name()='GetPromotionsField' and namespace-uri()='']"" />
        </b:properties>
      </xs:appinfo>
    </xs:annotation>
    <xs:complexType>
      <xs:sequence>
        <xs:element name=""GetPromotionsField"" type=""xs:string"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>";
        
        public GetPromotions() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [1];
                _RootElements[0] = "GetPromotions";
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
