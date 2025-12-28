namespace BTNextGen.Biztalk.ActiveCreditCards.Schemas {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"",@"SendCreditCardERP")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"SendCreditCardERP"})]
    public sealed class SendCreditCardERPXXX : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:element name=""SendCreditCardERP"">
    <xs:complexType>
      <xs:sequence>
        <xs:element name=""SendCreditCardERPDetails"">
          <xs:complexType>
            <xs:sequence>
              <xs:element maxOccurs=""unbounded"" name=""ActiveCreditCards"">
                <xs:complexType>
                  <xs:sequence>
                    <xs:element name=""alias"" type=""xs:string"" />
                    <xs:element name=""cardID"" type=""xs:string"" />
                    <xs:element name=""card_type"" type=""xs:unsignedByte"" />
                    <xs:element name=""erp_account_id"" type=""xs:string"" />
                    <xs:element name=""expiration_month"" type=""xs:unsignedByte"" />
                    <xs:element name=""expiration_year"" type=""xs:unsignedShort"" />
                    <xs:element name=""is_Tolas"" type=""xs:string"" />
                    <xs:element name=""last_4_digits"" type=""xs:unsignedShort"" />
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
        
        public SendCreditCardERPXXX() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [1];
                _RootElements[0] = "SendCreditCardERP";
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
