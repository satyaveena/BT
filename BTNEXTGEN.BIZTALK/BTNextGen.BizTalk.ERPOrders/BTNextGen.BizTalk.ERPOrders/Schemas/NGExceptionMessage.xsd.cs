namespace BTNextGen.BizTalk.ERPOrders.Schemas {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"BTNextGen.BizTalk.ERPOrders.Schemas.NGExceptionMessage",@"Exception")]
    [Microsoft.XLANGs.BaseTypes.DistinguishedFieldAttribute(typeof(System.String), "ErrorDetail.ErrDetail", XPath = @"/*[local-name()='Exception' and namespace-uri()='BTNextGen.BizTalk.ERPOrders.Schemas.NGExceptionMessage']/*[local-name()='ErrorDetail' and namespace-uri()='']/*[local-name()='ErrDetail' and namespace-uri()='']", XsdType = @"string")]
    [Microsoft.XLANGs.BaseTypes.DistinguishedFieldAttribute(typeof(System.String), "ErrorDetail.InnerException", XPath = @"/*[local-name()='Exception' and namespace-uri()='BTNextGen.BizTalk.ERPOrders.Schemas.NGExceptionMessage']/*[local-name()='ErrorDetail' and namespace-uri()='']/*[local-name()='InnerException' and namespace-uri()='']", XsdType = @"string")]
    [Microsoft.XLANGs.BaseTypes.DistinguishedFieldAttribute(typeof(System.String), "ErrorDetail.Message", XPath = @"/*[local-name()='Exception' and namespace-uri()='BTNextGen.BizTalk.ERPOrders.Schemas.NGExceptionMessage']/*[local-name()='ErrorDetail' and namespace-uri()='']/*[local-name()='Message' and namespace-uri()='']", XsdType = @"anyURI")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"Exception"})]
    public sealed class NGExceptionMessage : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns=""BTNextGen.BizTalk.ERPOrders.Schemas.NGExceptionMessage"" xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" targetNamespace=""BTNextGen.BizTalk.ERPOrders.Schemas.NGExceptionMessage"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:element name=""Exception"">
    <xs:annotation>
      <xs:appinfo>
        <b:properties>
          <b:property distinguished=""true"" xpath=""/*[local-name()='Exception' and namespace-uri()='BTNextGen.BizTalk.ERPOrders.Schemas.NGExceptionMessage']/*[local-name()='ErrorDetail' and namespace-uri()='']/*[local-name()='ErrDetail' and namespace-uri()='']"" />
          <b:property distinguished=""true"" xpath=""/*[local-name()='Exception' and namespace-uri()='BTNextGen.BizTalk.ERPOrders.Schemas.NGExceptionMessage']/*[local-name()='ErrorDetail' and namespace-uri()='']/*[local-name()='InnerException' and namespace-uri()='']"" />
          <b:property distinguished=""true"" xpath=""/*[local-name()='Exception' and namespace-uri()='BTNextGen.BizTalk.ERPOrders.Schemas.NGExceptionMessage']/*[local-name()='ErrorDetail' and namespace-uri()='']/*[local-name()='Message' and namespace-uri()='']"" />
        </b:properties>
      </xs:appinfo>
    </xs:annotation>
    <xs:complexType>
      <xs:sequence>
        <xs:element name=""ErrorDetail"">
          <xs:complexType>
            <xs:sequence>
              <xs:element name=""ErrDetail"" type=""xs:string"" />
              <xs:element name=""InnerException"" type=""xs:string"" />
              <xs:element name=""Message"" type=""xs:anyURI"" />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element name=""RefMessage"">
          <xs:complexType>
            <xs:sequence>
              <xs:any />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>";
        
        public NGExceptionMessage() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [1];
                _RootElements[0] = "Exception";
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
